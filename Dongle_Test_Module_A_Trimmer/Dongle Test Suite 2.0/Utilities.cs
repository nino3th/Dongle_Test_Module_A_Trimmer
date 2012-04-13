using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dongle_Test_Suite_2._1
{
    class Utils
    {
        public static void SendZTCCommand(FTDIdevice thisdevice, string commandhex)
        {
            thisdevice.WriteByte(MakePacket(ConvertHexStringToByteArray(commandhex)));
        }
        public static byte[] MakePacket(byte[] prelimPacket)
        {
            byte[] completePacket = new byte[prelimPacket.Length + 2];
            byte CRC = 0;

            //calculate CRC
            for (int i = 0; i < prelimPacket.Length; i++)
            {
                CRC = Convert.ToByte(CRC ^ prelimPacket[i]);
                completePacket[i + 1] = prelimPacket[i];
            }

            //add header (0x02) and CRC
            completePacket[completePacket.Length - 1] = CRC;
            completePacket[0] = 2;
            return completePacket;
        }
        public static void WaitForZTCResponse(FTDIdevice thisdevice, Parameters parameters)
        {
            byte[] readResult;
            int PLengthInt;
            int i = 1;
            bool responseA = false;
            string cache = string.Empty;
            string readOut = string.Empty;
            string packet = string.Empty;
            char[] charcache;
            byte[] PLength = new Byte[1];


            while (i <parameters.listenlooptimeout && responseA == false)
            {
                readResult = thisdevice.poll();
                readOut = ConvertByteArrayToSpacedHexString(readResult);
                cache = cache + readOut;
                charcache = cache.ToCharArray(0, cache.Length);

                if (cache.Length > 11)
                {
                    PLength = ConvertHexStringToByteArray(cache.Substring(9, 2));
                    byte[] PLengthB = new byte[2];
                    PLengthB[0] = PLength[0];
                    PLengthInt = BitConverter.ToInt16(PLengthB, 0);
                    if (cache.Length > 12 + PLengthInt * 3)
                    {
                        packet = cache.Substring(0, 14 + PLengthInt * 3);
                        cache = cache.Remove(0, 14 + PLengthInt * 3);
                        packet = packet.Remove(0, 2);
                        packet = packet.Remove(packet.Length - 2);
                        //Console.WriteLine("From Port 1: {0}", packet);
                        responseA = true;
                    }
                }

                System.Threading.Thread.Sleep(20);
                i++;
                if (i == parameters.listenlooptimeout)
                    throw new Exception_Yellow("No response received during listen.");
            }
        }
        public static bool WaitForZTCResponseAndIndication(FTDIdevice EndDevice, FTDIdevice Coordinator, string packetsent)
        {
            //StartTestingTimer3();
            //StartTestingTimer2(); string time2 = GetTestingTimestamp2();
            //string TxTime = GetTimestamp();
            //string RxTime = string.Empty;
            //int deltaT = 0;
            int success = 0;

            byte[] readResult;
            int PLengthInt;
            int i = 1;
            bool responseA = false;
            string cache = string.Empty;
            string readOut = string.Empty;
            string packet = string.Empty;
            char[] charcache;
            byte[] PLength = new Byte[1];

            byte[] readResult2;
            int PLengthInt2;
            bool responseB = false;
            string cache2 = string.Empty;
            string readOut2 = string.Empty;
            string packet2 = string.Empty;
            char[] charcache2;
            byte[] PLength2 = new Byte[1];

            while (i < 40 && (responseA == false || responseB == false))
            {
                //time2 = GetTestingTimestamp2();
                if (responseA == false)
                {
                    readResult = EndDevice.poll();                             //read from port of device A
                    readOut = ConvertByteArrayToSpacedHexString(readResult);
                    cache = cache + readOut;                                //add most recent byte from port A to cache
                    charcache = cache.ToCharArray(0, cache.Length);

                    if (cache.Length > 11)                                  //if the cache is long enough to hold a full transmission (at least 4 bytes in string format)
                    {
                        PLength = ConvertHexStringToByteArray(cache.Substring(9, 2));  // packet length = the length stored in the length byte of the transmission
                        byte[] PLengthB = new byte[2];
                        PLengthB[0] = PLength[0];
                        PLengthInt = BitConverter.ToInt16(PLengthB, 0);
                        if (cache.Length > 12 + PLengthInt * 3)             //if transmission is complete (i.e. if the number of bytes specified by the length byte have all been received)
                        {
                            packet = cache.Substring(0, 14 + PLengthInt * 3);
                            cache = cache.Remove(0, 14 + PLengthInt * 3);
                            packet = packet.Remove(0, 2);
                            packet = packet.Remove(packet.Length - 2);      //removing extraneous bytes, like parity byte?
                            //Console.WriteLine("From Port 1: {0}", packet);
                            responseA = true;                               //done listening
                        }
                    }
                }
                //time2 = GetTestingTimestamp2();

                if (responseB == false)
                {
                    readResult2 = Coordinator.poll();
                    readOut2 = ConvertByteArrayToSpacedHexString(readResult2);
                    cache2 = cache2 + readOut2;
                    charcache2 = cache2.ToCharArray(0, cache2.Length);

                    if (cache2.Length > 11)
                    {
                        PLength2 = ConvertHexStringToByteArray(cache2.Substring(9, 2));
                        byte[] PLengthC = new byte[2];
                        PLengthC[0] = PLength2[0];
                        PLengthInt2 = BitConverter.ToInt16(PLengthC, 0);
                        if (cache2.Length > 12 + PLengthInt2 * 3)
                        {
                            packet2 = cache2.Substring(0, 14 + PLengthInt2 * 3);
                            cache2 = cache2.Remove(0, 14 + PLengthInt2 * 3);
                            packet2 = packet2.Remove(0, 2);
                            packet2 = packet2.Remove(packet2.Length - 2);
                            //Console.WriteLine("From Port 2: {0}", packet2);
                            cache2 = string.Empty;

                            //Packet comparing
                            packet2 = packet2.Replace(" ", "");
                            packetsent = packetsent.Replace(" ", "");
                            if (packet2.Length == packetsent.Length + 2)
                            {
                                if (string.Compare(packet2.Substring(packet2.Length - 8, 8), packetsent.Substring(packetsent.Length - 8, 8), true) == 0)
                                {
                                    responseB = true;
                                    success = 1;
                                    //RxTime = GetTimestamp();
                                    //string RxTimeMillisec = RxTime.Substring(12, 6);
                                    //string TxTimeMillisec = TxTime.Substring(12, 6);
                                    //deltaT = int.Parse(RxTimeMillisec) - int.Parse(TxTimeMillisec);
                                }

                            }
                        }
                    }
                }
                //time2 = GetTestingTimestamp2();
                System.Threading.Thread.Sleep(1);
                i++;
                //if (i == 40) timeoutcounter++;
            }
            //if (deltaT <= 0)
            //    deltaT = 0;

            packetsent = packetsent.Replace(" ", "");

            //if (responseA == false)
            //Console.WriteLine("Port 1: No Reply");
            //if (responseB == false)
            //Console.WriteLine("Port 2: No Reply");

            //string time = GetTestingTimestamp3();

            if (responseA == true && responseB == true) 
                return true;
            else return false;

        }
        public static byte[] ConvertHexStringToByteArray(string stringofhexbytes)
        {
            stringofhexbytes = stringofhexbytes.Replace(" ", "");                                                 //remove any spaces from the string
            byte[] bytearray = new byte[(stringofhexbytes.Length / 2)];
            //  (one byte is 2 characters in length)
            for (int i = 0; i < stringofhexbytes.Length; i += 2)
            {
                bytearray[(i / 2)] = (byte)Convert.ToByte(stringofhexbytes.Substring(i, 2), 16);       //convert each set of 2 characters to a byte and add to the array    
            }
            return bytearray;
        }
        public static string ConvertByteArrayToSpacedHexString(byte[] bytearray)
        {
            StringBuilder builder = new StringBuilder(bytearray.Length * 3);
            foreach (byte thisbyte in bytearray)
                builder.Append(Convert.ToString(thisbyte, 16).PadLeft(2, '0').PadRight(3, ' '));    //convert the byte to a string, add a 0 in front of it if it's only 1 character long (a nibble),
            return builder.ToString().ToUpper();                                                    // then add a space to the end, and finally append it to our growing string
        }


        public byte[] AddtoReceiveList_ListenForNewFWResponse(FTDIdevice thisdevice, Parameters parameters)
        {
            Queue<byte> incomingbytequeue = new Queue<byte>();
            byte ZTCHeader = 0x02;
            int initialqueuelen = incomingbytequeue.Count;
            int indexofZTCheaderinqueue = 0;
            byte[] incomingdata = thisdevice.poll();
            int arraylen = incomingdata.Length;
            for (int i = 0; i < arraylen; i++)
                incomingbytequeue.Enqueue(incomingdata[i]);
            int queuelen = incomingbytequeue.Count;
            byte temp;
            bool packetstarted = false;
            for (int j = 0; j < queuelen; j++)
            {
                temp = incomingbytequeue.Dequeue();
                if (temp == ZTCHeader && incomingbytequeue.Count > 3)
                {
                    packetstarted = true;
                    break;
                }
                else
                {
                    //incomingbytequeue.Enqueue(temp); //this is a problem. we should throw this out.
                    indexofZTCheaderinqueue++;
                }
            }
            if (packetstarted)
            {
                byte tempheader = incomingbytequeue.Dequeue();
                byte tempopcode = incomingbytequeue.Dequeue();
                int payloadlen = incomingbytequeue.Dequeue();
                if (incomingbytequeue.Count < payloadlen + 1)  //if the packet hasn't fully arrived yet, just put the packets we've read so far back on the queue and then exit, to come back and read again later
                {
                    int count = incomingbytequeue.Count;
                    incomingbytequeue.Enqueue(ZTCHeader);
                    incomingbytequeue.Enqueue(tempheader);
                    incomingbytequeue.Enqueue(tempopcode);
                    incomingbytequeue.Enqueue((byte)payloadlen);
                    for (int k = 0; k < count; k++)
                        incomingbytequeue.Enqueue(incomingbytequeue.Dequeue());
                    return null;
                    //throw new Exception_Yellow("incomplete packet received.");
                }
                byte[] packet = new byte[payloadlen + 5];
                packet[0] = ZTCHeader;
                packet[1] = tempheader;
                packet[2] = tempopcode;
                packet[3] = (byte)payloadlen;
                for (int k = 0; k < payloadlen; k++)
                    packet[4 + k] = incomingbytequeue.Dequeue();
                packet[payloadlen + 4] = incomingbytequeue.Dequeue(); //CRC
                //if (packet[payloadlen + 4] == 0) packet[0] = 2; //dummy just to have breakpoint to test theory that 0 is returned sometimes

                return packet;
                //RxPacket rx = new RxPacket();
                //rx.completepacket = packet;
                //RxFIFO.Enqueue(rx);
                //AllRxPacketQueueDebugging.Enqueue(rx);
                ////now figure out what to do with the rest of the packets that are hanging out there.  re-enqueue them probably
            }
            else return null;
            //if(incomingpacket!=null && incomingpacket.Length!=0)
            //RxFIFO.Enqueue(incomingpacket);
        }
        //public static byte[] MakePacket_NewFW(byte[] prelimPacket)
        //{
        //    byte[] completePacket = new byte[prelimPacket.Length + 2];
        //    byte CRC = 0;

        //    //calculate CRC
        //    for (int i = 0; i < prelimPacket.Length; i++)
        //    {
        //        CRC = Convert.ToByte(CRC + prelimPacket[i]);
        //        completePacket[i + 1] = prelimPacket[i];
        //    }

        //    //add header (0x02) and CRC
        //    completePacket[completePacket.Length - 1] = CRC;
        //    completePacket[0] = 2;
        //    return completePacket;
        //}

        //public static void WaitForZTCResponse_NewFW(FTDIdevice thisdevice, Parameters parameters)
        //{
        //    byte[] readResult;
        //    int PLengthInt;
        //    int i = 1;
        //    bool responseA = false;
        //    string cache = string.Empty;
        //    string readOut = string.Empty;
        //    string packet = string.Empty;
        //    char[] charcache;
        //    byte[] PLength = new Byte[1];


        //    while (i < parameters.listenlooptimeout && responseA == false)
        //    {
        //        readResult = thisdevice.poll();
        //        readOut = ConvertByteArrayToSpacedHexString(readResult);
        //        cache = cache + readOut;
        //        charcache = cache.ToCharArray(0, cache.Length);

        //        if (cache.Length > 11)
        //        {
        //            PLength = ConvertHexStringToByteArray(cache.Substring(9, 2));
        //            byte[] PLengthB = new byte[2];
        //            PLengthB[0] = PLength[0];
        //            PLengthInt = BitConverter.ToInt16(PLengthB, 0);
        //            if (cache.Length > 12 + PLengthInt * 3)
        //            {
        //                packet = cache.Substring(0, 14 + PLengthInt * 3);
        //                cache = cache.Remove(0, 14 + PLengthInt * 3);
        //                packet = packet.Remove(0, 2);
        //                packet = packet.Remove(packet.Length - 2);
        //                //Console.WriteLine("From Port 1: {0}", packet);
        //                responseA = true;
        //            }
        //        }

        //        System.Threading.Thread.Sleep(20);
        //        i++;
        //        if (i == parameters.listenlooptimeout)
        //            throw new Exception_Yellow("No response received during listen.");
        //    }
        //}


    }
}
