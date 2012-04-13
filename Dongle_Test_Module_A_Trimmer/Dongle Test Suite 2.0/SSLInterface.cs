using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dongle_Test_Suite_2._1
{
    class SSLInterface
    {
        enum ENGCommandHeader 
        { 
            engReadReq = 0x01, 
            engReadResp = 0x02, 
            engWriteReq = 0x03, 
            engCommitReq = 0x04, 
            engEraseReq = 0x05, 
            engCmdCnf = 0xF0,
            engTrimReq_c = 0x20
        };
        enum ENGCmdStatus 
        {
          gEngValidReq_c = 0x00,
          gEngInvalidReq_c,
          gEngSuccessOp_c,
          gEngWriteError_c,
          gEngReadError_c,
          gEngCRCError_c,
          gEngCommError_c,
          gEngExecError_c,
          gEngNoConfirm_c
        };
        enum ENGSecureOption
        {
          engSecured_c    = 0xC3,
          engUnsecured_c  = 0x3C
        }

        byte SSL_SOF = 0x55; //ssl's start of frame byte
        Parameters parameters;
        FTDIdevice thisUSB;

        public SSLInterface(Parameters pars, FTDIdevice USB)
        {
            parameters = pars;
            thisUSB = USB;
        }
        public void SSL_WRITE(uint writeaddr, byte[] chunktowrite, UInt16 chunklength)
        {
            //Ensure we have a valid length and source data
            if ((chunklength > Parameters.SSL_ENG_BUFFER_SIZE) || (chunktowrite == null)) throw new Exception_Yellow("Problem with the chunk handed to the SSL_WRITE function");
            
            //send the command
            byte[] packet = BuildWritePacket(writeaddr, chunktowrite, chunklength);
            thisUSB.WriteByte(packet);
            
            //Wait for confirm. Return to the caller the respective status.
            ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);

            //temp for debugging error
            //status = ENGCmdStatus.gEngExecError_c;
            if (status != ENGCmdStatus.gEngSuccessOp_c)
                SSLErrorHandler(status);
        }

        //original for safekeeping:
        //public void SSL_WRITE(uint writeaddr, byte[] chunktowrite, UInt16 chunklength)
        //{
        //    //Ensure we have a valid length and source data
        //    if ((chunklength > Parameters.SSL_ENG_BUFFER_SIZE) || (chunktowrite == null)) throw new Exception_Yellow("Problem with the chunk handed to the SSL_WRITE function");

        //    //send the command
        //    byte[] packet = BuildWritePacket(writeaddr, chunktowrite, chunklength);
        //    thisUSB.WriteByte(packet);

        //    //Wait for confirm. Return to the caller the respective status.
        //    ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);

        //    //temp for debugging error
        //    //status = ENGCmdStatus.gEngExecError_c;

        //    if (status != ENGCmdStatus.gEngSuccessOp_c)
        //        SSLErrorHandler(status);
        //}
        private byte[] BuildWritePacket(uint writeaddr, byte[] chunktowrite, UInt16 chunklength)
        {
            //packet structure dictated by the SSL, show with bytes separated by '|':  
            //   SOF|cmdlength1|cmdlength2|cmd.ID|cmd.writeaddr1|cmd.writeaddr2|cmd.writeaddr3|cmd.writeaddr4|cmd.chunklength1|cmd.chunklength2|cmd.(byte[] chunktowrite)|CRC
            UInt16 commandlength = (UInt16)(chunklength + 7);
            int packetlength = commandlength + 4;
            byte[] packet = new byte[packetlength]; // 1 byte StartOfFrame + 2bytes cmdlength + 1 byte ID + 4 bytes Addr + 2 byte lengthvalue + chunklength + 1byte CRC

            //packet prefixes
            packet[0] = SSL_SOF;
            packet[2] = Convert.ToByte((commandlength & 0xFF00)>>8);  //split 32-bit length into 2 bytes, little-Endian
            packet[1] = Convert.ToByte(commandlength & 0x00FF);

            //command itself, within packet (writereq ID + write addr + chunklength + chunk array
            packet[3] = Convert.ToByte(ENGCommandHeader.engWriteReq);

            packet[7] = Convert.ToByte((writeaddr & 0xFF000000) >> 24);  //split 32-bit address into 4 bytes, little-Endian
            packet[6] = Convert.ToByte((writeaddr & 0x00FF0000)>>16);
            packet[5] = Convert.ToByte((writeaddr & 0x0000FF00)>>8);
            packet[4] = Convert.ToByte(writeaddr & 0x000000FF);

            packet[9] = Convert.ToByte((chunklength & 0xFF00) >> 8);  //split 16-bit length into 2 bytes, little-Endian
            packet[8] = Convert.ToByte(chunklength & 0x00FF);
            
            Buffer.BlockCopy(chunktowrite, 0, packet, 10, chunklength);
            
            //tack on the CRC
            packet[packet.Length-1] = getCRC(packet);

            return packet;
        }
        private byte getCRC(byte[] packet)
        {
            //we only want the CRC of the command within the packet, which is from index 3 onward till the second to last byte (leaving room for the CRC to be the actual last byte)
            byte computedCRC = 0;
            for(int i = 3; i < packet.Length-1; i++)
            {
                computedCRC+= packet[i];        
            }
            return computedCRC;
        }
        public byte WaitForConfirm(int timeout)
        {
            //cnf packet structure, with bytes separated by |: SOF|cmdlen1|cmdlen2|cmd.cnfID|cmd.cnfstatus|CRC
            byte[] confirm = ListenForConfirm(timeout);
            if (confirm.Length < 6)  //was previously 4
                throw new Exception("full confirm not received");
            if (confirm[3] != (byte)ENGCommandHeader.engCmdCnf)
                throw new Exception_Yellow("write or commit confirm header not formatted as expected; USB may have been unplugged too early.");
            return confirm[4];
        }
        private byte[] ListenForConfirm(int timeout)
        {
            Queue<byte> cnfpacketqueue = new Queue<byte>();
            //byte[] cnfpacket = new byte[1];// = new byte[6];  //really this shouldn't be defined yet
            byte[] onebyte;
            //int i;
                //keep polling till we see a SOF, then read 2 bytes to get the length, then read however many bytes the length says, then read the CRC and check it, then break
            for(int i = timeout; i > 0; i--)
            {
                onebyte = thisUSB.ReadOneByte();
                if (onebyte != null)
                {
                    if (onebyte[0] == SSL_SOF)
                    {
                        cnfpacketqueue.Enqueue(onebyte[0]);//cnfpacket[0] = onebyte[0];
                        onebyte = thisUSB.ReadOneByte();
                        if (onebyte == null)
                        {  //early retry, just wait a bit then re-listen for this byte.  There are 4 of these early retry if statements; they're 
                            //  written out repeatedly instead of being wrapped into readonebyte so that 1) we can still get a null response when 
                            //  we need it (outer loop) and 2) we can differentiate where exactly the missing byte happens.
                            System.Threading.Thread.Sleep(30);
                            onebyte = thisUSB.ReadOneByte();
                            if (onebyte == null) throw new Exception_Yellow("Packet cut off before length bytes finished, retry failed."); //main retry, one time, resends command
                        }
                        byte[] lengthbytes = new byte[2];
                        cnfpacketqueue.Enqueue(onebyte[0]);//cnfpacket[1] = onebyte[0]; //length byte 1
                        lengthbytes[0] = onebyte[0];

                        onebyte = thisUSB.ReadOneByte();
                        if (onebyte == null)
                        {  //early retry, just wait a bit then re-listen for this byte
                            System.Threading.Thread.Sleep(30);
                            onebyte = thisUSB.ReadOneByte();
                            if (onebyte == null) throw new Exception_Yellow("Packet cut off before length bytes finished, retry failed."); //main retry, one time, resends command
                        }
                        cnfpacketqueue.Enqueue(onebyte[0]);//cnfpacket[2] = onebyte[0]; //length byte 2
                        lengthbytes[1] = onebyte[0];
                        int length = lengthbytes[0] + (lengthbytes[1] << 8) ;
                        byte computedCRC = 0;
                        for (int j = 0; j < length; j++)
                        {
                            onebyte = thisUSB.ReadOneByte();
                            if (onebyte == null) 
                            {  //early retry, just wait a bit then re-listen for this byte
                                System.Threading.Thread.Sleep(30);
                                onebyte = thisUSB.ReadOneByte();
                                if (onebyte == null) throw new Exception_Yellow("incomplete confirm packet after SOF was read, retry failed."); //main retry, one time, resends command
                            }
                            cnfpacketqueue.Enqueue(onebyte[0]);//cnfpacket[3 + j] = onebyte[0];
                            computedCRC += onebyte[0];
                        }
                        onebyte = thisUSB.ReadOneByte();
                        if(onebyte == null)
                        {  //early retry, just wait a bit then re-listen for this byte
                            System.Threading.Thread.Sleep(30);
                            onebyte = thisUSB.ReadOneByte();
                            if (onebyte == null) throw new Exception_Yellow("packet incomplete (missing CRC), retry failed."); //main retry, one time, resends command
                        }
                        cnfpacketqueue.Enqueue(onebyte[0]);//cnfpacket[3 + length] = onebyte[0];  //CRC
                        byte[] cnfpacket = cnfpacketqueue.ToArray();
                        if (computedCRC != cnfpacket[3 + length]) throw new Exception_Yellow("bad CRC on write confirm, retry failed.");  //main retry.  "retry failed" error will be caught to lead to one re-send of the packet being written.
                        return cnfpacket;
                    }
                }
                System.Threading.Thread.Sleep(1);
            }
            if (cnfpacketqueue.Count < 6) throw new Exception_Yellow("no complete ENG_WRITE confirm recieved, retry failed.");  //will this catch if some other weird packet comes back?  i think not
            else throw new Exception_Yellow("Listen loop was escaped with full-sized packet intact, retry failed.");
        }

        //stored for safekeeping:
        //public byte WaitForConfirm(int timeout)
        //{
        //    //cnf packet structure, with bytes separated by |: SOF|cmdlen1|cmdlen2|cmd.cnfID|cmd.cnfstatus|CRC
        //    byte[] confirm = ListenForConfirm(timeout);
        //    if (confirm.Length < 6)  //was previously 4
        //        throw new Exception("full confirm not received");
        //    if (confirm[3] != (byte)ENGCommandHeader.engCmdCnf)
        //        throw new Exception_Yellow("write or commit confirm header not formatted as expected; USB may have been unplugged too early.");
        //    return confirm[4];
        //}
        //private byte[] ListenForConfirm(int timeout)
        //{
        //    byte[] cnfpacket = new byte[6];  //really this shouldn't be defined yet
        //    byte[] onebyte;
        //    int i;
        //    //keep polling till we see a SOF, then read 2 bytes to get the length, then read however many bytes the length says, then read the CRC and check it, then break
        //    for (i = timeout; i > 0; i--)
        //    {
        //        onebyte = thisUSB.ReadOneByte();
        //        if (onebyte != null)
        //        {
        //            if (onebyte[0] == SSL_SOF)
        //            {
        //                cnfpacket[0] = onebyte[0];
        //                onebyte = thisUSB.ReadOneByte();
        //                if (onebyte == null)
        //                {  //early retry, just wait a bit then re-listen for this byte
        //                    System.Threading.Thread.Sleep(30);
        //                    onebyte = thisUSB.ReadOneByte();
        //                    if (onebyte == null) throw new Exception_Yellow("Packet cut off before length bytes finished, retry failed."); //main retry, one time, resends command
        //                }
        //                cnfpacket[1] = onebyte[0]; //length byte 1
        //                onebyte = thisUSB.ReadOneByte();
        //                if (onebyte == null)
        //                {  //early retry, just wait a bit then re-listen for this byte
        //                    System.Threading.Thread.Sleep(30);
        //                    onebyte = thisUSB.ReadOneByte();
        //                    if (onebyte == null) throw new Exception_Yellow("Packet cut off before length bytes finished, retry failed."); //main retry, one time, resends command
        //                }
        //                cnfpacket[2] = onebyte[0]; //length byte 2
        //                int length = cnfpacket[1] + (cnfpacket[2] << 8);
        //                byte computedCRC = 0;
        //                for (int j = 0; j < length; j++)
        //                {
        //                    onebyte = thisUSB.ReadOneByte();
        //                    if (onebyte == null)
        //                    {  //early retry, just wait a bit then re-listen for this byte
        //                        System.Threading.Thread.Sleep(30);
        //                        onebyte = thisUSB.ReadOneByte();
        //                        if (onebyte == null) throw new Exception_Yellow("incomplete confirm packet after SOF was read, retry failed."); //main retry, one time, resends command
        //                    }
        //                    cnfpacket[3 + j] = onebyte[0];
        //                    computedCRC += onebyte[0];
        //                }
        //                onebyte = thisUSB.ReadOneByte();
        //                cnfpacket[3 + length] = onebyte[0];  //CRC
        //                if (computedCRC != cnfpacket[3 + length]) throw new Exception_Yellow("bad CRC on write confirm, retry failed.");
        //                return cnfpacket;
        //            }
        //        }
        //        System.Threading.Thread.Sleep(1);
        //    }
        //    if (i == 0 && cnfpacket == null) throw new Exception_Yellow("no ENG_WRITE confirm recieved, retry failed.");  //will this catch if some other weird packet comes back?  i think not
        //    else return cnfpacket;
        //}
        public void SSL_COMMIT(uint filelength)
        {
            //send the command
            thisUSB.WriteByte(BuildCommitPacket(filelength));

            //Wait for confirm. Return to the caller the respective status.
            ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);
            if (status != ENGCmdStatus.gEngSuccessOp_c)
                SSLErrorHandler(status);
        }
        public void SSL_FULLERASE()
        {
            //send the command
            thisUSB.WriteByte(BuildErasePacket(0xFFFFFFFF));

            //Wait for confirm. Return to the caller the respective status.
            ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);
            if (status != ENGCmdStatus.gEngSuccessOp_c)
                SSLErrorHandler(status);
        }
        public void SSL_ERASE_Sector(uint eraseAddr)
        {
            //send the command
            thisUSB.WriteByte(BuildErasePacket(eraseAddr));

            //Wait for confirm. Return to the caller the respective status.
            ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);
            if (status != ENGCmdStatus.gEngSuccessOp_c)
                SSLErrorHandler(status);
        }
        private byte[] BuildCommitPacket(uint filelength)
        {
            //packet structure, bytes separated by '|':  SOF|cmdlength1|cmdlength2|cmd.ID|cmd.filelength1|cmd.filelength2|cmd.filelength3|cmd.filelength4|cmd.securedoption|CRC
            UInt16 commandlength = 6;
            int packetlength = commandlength + 4;
            byte[] packet = new byte[packetlength];

            //packet prefixes
            packet[0] = SSL_SOF;
            packet[2] = Convert.ToByte((commandlength & 0xFF00) >> 8);
            packet[1] = Convert.ToByte(commandlength & 0x00FF);

            //command itself, within packet 
            packet[3] = Convert.ToByte(ENGCommandHeader.engCommitReq);

            packet[7] = Convert.ToByte((filelength & 0xFF000000) >> 24);
            packet[6] = Convert.ToByte((filelength & 0x00FF0000) >> 16);
            packet[5] = Convert.ToByte((filelength & 0x0000FF00) >> 8);
            packet[4] = Convert.ToByte(filelength & 0x000000FF);

            packet[8] = Convert.ToByte(ENGSecureOption.engUnsecured_c);  //hard-coded here to be unsecured.

            //tack on the CRC
            packet[packet.Length - 1] = getCRC(packet);

            return packet;
        }
        private byte[] BuildErasePacket(uint eraseaddr)
        {
            //packet structure, bytes separated by '|':  SOF|cmdlength1|cmdlength2|cmd.ID|cmd.filelength1|cmd.filelength2|cmd.filelength3|cmd.filelength4|cmd.securedoption|CRC
            UInt16 commandlength = 5;
            int packetlength = commandlength + 4;
            byte[] packet = new byte[packetlength];

            //packet prefixes
            packet[0] = SSL_SOF;
            packet[2] = Convert.ToByte((commandlength & 0xFF00) >> 8);
            packet[1] = Convert.ToByte(commandlength & 0x00FF);

            //command itself, within packet 
            packet[3] = Convert.ToByte(ENGCommandHeader.engEraseReq);

            packet[7] = Convert.ToByte((eraseaddr & 0xFF000000) >> 24);
            packet[6] = Convert.ToByte((eraseaddr & 0x00FF0000) >> 16);
            packet[5] = Convert.ToByte((eraseaddr & 0x0000FF00) >> 8);
            packet[4] = Convert.ToByte(eraseaddr & 0x000000FF);

            //tack on the CRC
            packet[packet.Length - 1] = getCRC(packet);

            return packet;
        }
        private void SSLErrorHandler(ENGCmdStatus status)
        {
            string errortype = "";
            if (status == ENGCmdStatus.gEngValidReq_c) errortype = "ValidReq";
            else if (status == ENGCmdStatus.gEngInvalidReq_c) errortype = "InvalidReq";
            else if (status == ENGCmdStatus.gEngWriteError_c) errortype = "Write Error";
            else if (status == ENGCmdStatus.gEngReadError_c) errortype = "Read Error";
            else if (status == ENGCmdStatus.gEngCRCError_c) errortype = "CRC Error";
            else if (status == ENGCmdStatus.gEngCommError_c) errortype = "Comm Error";
            else if (status == ENGCmdStatus.gEngExecError_c) errortype = "Exec Error";
            else if (status == ENGCmdStatus.gEngNoConfirm_c) errortype = "No Confirm";
            throw new Exception_Yellow("Error: SSL has returned a " + errortype + " status.");
        }


        public void SSL_SetTrims(byte finetrim, byte coarsetrim)
        {
            thisUSB.WriteByte(BuildTrimsPacket(finetrim, coarsetrim));

            //Wait for confirm. Return to the caller the respective status.
            ENGCmdStatus status = (ENGCmdStatus)WaitForConfirm(Parameters.SSL_write_timeout);
            if (status != ENGCmdStatus.gEngSuccessOp_c)
                SSLErrorHandler(status);
        }
        private byte[] BuildTrimsPacket(byte fine, byte coarse)
        {
            //packet structure, bytes separated by '|':  SOF|cmdlength1|cmdlength2|cmd.ID|cmd.finetrim|cmd.coarsetrim|CRC
            UInt16 commandlength = 3;
            int packetlength = commandlength + 4;
            byte[] packet = new byte[packetlength];

            //packet prefixes
            packet[0] = SSL_SOF;
            packet[2] = Convert.ToByte((commandlength & 0xFF00) >> 8);
            packet[1] = Convert.ToByte(commandlength & 0x00FF);

            //command itself, within packet 
            packet[3] = Convert.ToByte(ENGCommandHeader.engTrimReq_c);


            packet[4] = fine;
            packet[5] = coarse;

            //tack on the CRC
            packet[packet.Length - 1] = getCRC(packet);

            return packet;
        }

    }
}


////pack all structures
////#pragma pack(1)
//struct ENGReadReq
//{
//  UInt32        startAddress;  //start address in FLASH for the read operation
//  UInt16        length;        //number of bytes to read
//};

//struct ENGReadResp
//{
//  uint         cmdStatus;     //Should be ENGCmdStatus_t(enum),but that is int in VC++
//  UInt16        length;        //valid data bytes in the buffer
//  uint[]         buffer;//[SSL_ENG_BUFFER_SIZE];
//};

//struct ENGWriteReq
//{
//  UInt32        startAddress;  //start address for the write operation
//  UInt16        length;        //number of bytes to be written from the command buffer
//  uint[]         buffer;//[ENG_BUFFER_SIZE];
//};

//struct ENGCommitReq
//{
//  UInt32        binLength;     //length of the binary commited to FLASH
//  uint         secured;       //secured option
//};

//struct ENGEraseReq
//{
//  UInt32        address;       //the bits from [AMS-A12] are used to determine the sector to be erased. For all F's, all FLASH will be erased.
//};

//struct ENGCmdCnf
//{
//  uint  cmdStatus;     //Should be ENGCmdStatus_t(enum),but that is int in VC++
//} ;

