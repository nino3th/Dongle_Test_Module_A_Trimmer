#region Assembly NationalInstruments.NI4882.dll, v2.0.50727
// C:\Program Files\National Instruments\MeasurementStudioVS2008\DotNET\Assemblies\Current\NationalInstruments.NI4882.dll
#endregion

using NationalInstruments;
using System;
using System.ComponentModel;

namespace NationalInstruments.NI4882
{
    // Summary:
    //     Contains all the device functionality of the NI-488.2 driver.
    //
    // Remarks:
    //     In some cases, callbacks and event handlers are executed in a different thread
    //     than the rest of the program. Therefore, you must take special care when
    //     accessing objects that have thread affinity, such as UI controls, from these
    //     callbacks and event handlers. For more information, refer to .
    public class Device : MarshalByRefObject, IDisposable, ISynchronizeCallbacks, ISupportSynchronizationContext
    {
        // Summary:
        //     Opens and initializes a device and configures it according to specified board
        //     number and address.
        //
        // Parameters:
        //   boardNumber:
        //     Index of the access board for the device.
        //
        //   address:
        //     The NationalInstruments.NI4882.Address of the GPIB device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      The address parameter is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     boardNumber is within the range 0-99, but the interface board described by
        //     boardNumber is not installed nor properly configured.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.SetEndOnWrite is set to true, NationalInstruments.NI4882.Device.EndOfStringCharacter
        //     is set to 0, and NationalInstruments.NI4882.Device.IOTimeout is set to NationalInstruments.NI4882.TimeoutValue.T10s
        //     during constructor.
        public Device(int boardNumber, Address address);
        //
        // Summary:
        //     Opens and initializes a device and configures it according to the specified
        //     board number and primary address.
        //
        // Parameters:
        //   boardNumber:
        //     Index of the access board for the device.
        //
        //   primaryAddress:
        //     The primary GPIB address of the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      The primaryAddress parameter is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     boardNumber is within the range 0-99, but the interface board described by
        //     boardNumber is not installed nor properly configured.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.SetEndOnWrite is set to true, NationalInstruments.NI4882.Device.EndOfStringCharacter
        //     is set to 0, NationalInstruments.NI4882.Device.IOTimeout is set to NationalInstruments.NI4882.TimeoutValue.T10s,
        //     and NationalInstruments.NI4882.Device.SecondaryAddress is set to 0 during
        //     constructor.
        public Device(int boardNumber, byte primaryAddress);
        //
        // Summary:
        //     Opens and initializes a device and configures it according to the specified
        //     board number, primary address, and secondary address.
        //
        // Parameters:
        //   boardNumber:
        //     Index of the access board for the device.
        //
        //   primaryAddress:
        //     The primary GPIB address of the device.
        //
        //   secondaryAddress:
        //     The secondary GPIB address of the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      The primaryAddress parameter is invalid.
        //     -or-
        //     The secondaryAddress parameter is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     boardNumber is within the range 0-99, but the interface board described by
        //     boardNumber is not installed nor properly configured.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.SetEndOnWrite is set to true, NationalInstruments.NI4882.Device.EndOfStringCharacter
        //     is set to 0, and NationalInstruments.NI4882.Device.IOTimeout is set to NationalInstruments.NI4882.TimeoutValue.T10s
        //     during constructor.
        public Device(int boardNumber, byte primaryAddress, byte secondaryAddress);
        //
        // Summary:
        //     Opens and initializes a device and configures it according to the specified
        //     board number, primary address, secondary address, and timeout.
        //
        // Parameters:
        //   boardNumber:
        //     Index of the access board for the device.
        //
        //   primaryAddress:
        //     The primary GPIB address of the device.
        //
        //   secondaryAddress:
        //     The secondary GPIB address of the device.
        //
        //   timeoutValue:
        //     The I/O timeout value.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      The primaryAddress parameter is invalid.
        //     -or-
        //     The secondaryAddress parameter is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     boardNumber is within the range 0-99, but the interface board described by
        //     boardNumber is not installed nor properly configured.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.SetEndOnWrite is set to true, and NationalInstruments.NI4882.Device.EndOfStringCharacter
        //     is set to 0 during constructor.
        public Device(int boardNumber, byte primaryAddress, byte secondaryAddress, TimeoutValue timeoutValue);

        // Summary:
        //     Gets or sets the default size of read buffers when they are not explicitly
        //     defined as a parameter on read operations.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        public int DefaultBufferSize { get; set; }
        //
        // Summary:
        //     Gets or sets the end-of-string character to use during data transfer.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public byte EndOfStringCharacter { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether to use an 8-bit or 7-bit compare.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public EndOfStringComparison EndOfStringComparison { get; set; }
        //
        // Summary:
        //     Gets access to the NI-488.2 driver device handle that is internally used
        //     with NI-488.2 driver operations.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        // Remarks:
        //     This property is provided only for extensibility only. Use this property
        //     only if the desired NI-488.2 functionality is not a part of this API. If
        //     a future revision of this API adds the unavailable functionality, change
        //     statements that use NationalInstruments.NI4882.Device.Handle to use the new
        //     API members.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IntPtr Handle { get; }
        //
        // Summary:
        //     Gets or sets the timeout period to select the maximum duration allowed for
        //     a synchronous I/O operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        //
        // Remarks:
        //     Examples of synchronous operations are NationalInstruments.NI4882.Board.ReadByteArray(),
        //     NationalInstruments.NI4882.Board.ReadString(), and NationalInstruments.NI4882.Board.Write(System.String).
        //     NationalInstruments.NI4882.Board.IOTimeout is also the timeout value used
        //     for NationalInstruments.NI4882.Board.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     and NationalInstruments.NI4882.Board.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     when a NationalInstruments.NI4882.GpibStatusFlags.Timeout is passed in for
        //     the NationalInstruments.NI4882.GpibStatusFlags parameter. The NationalInstruments.NI4882.TimeoutValue
        //     represent the minimum timeout period. The actual period can be longer.
        public TimeoutValue IOTimeout { get; set; }
        //
        // Summary:
        //     Gets the count of the last call made on the device.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        public int LastCount { get; }
        //
        // Summary:
        //     Gets the last status of the last call made on the NationalInstruments.NI4882.Device
        //     object.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Board.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        public GpibStatusFlags LastStatus { get; }
        //
        // Summary:
        //     Gets or sets the primary address of the NationalInstruments.NI4882.Device
        //     object.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public byte PrimaryAddress { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating if readdressing is performed between read
        //     and write operations.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public bool ReaddressingEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the secondary address of the NationalInstruments.NI4882.Device
        //     object.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public byte SecondaryAddress { get; set; }
        //
        // Summary:
        //     Gets or sets the length of time the driver waits for a serial poll response
        //     byte when polling.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public TimeoutValue SerialPollResponseTimeout { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating if the EOI line is asserted when the EOS
        //     character is sent during write operations. Refer to NationalInstruments.NI4882.Board.SetEndOnWrite.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public bool SetEndOnEndOfString { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether to assert the end or identify (EOI)
        //     line during write operations.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public bool SetEndOnWrite { get; set; }
        //
        // Summary:
        //     Specifies how events and callback delegates are invoked.
        //
        // Remarks:
        //     For more information, refer to .
        public bool SynchronizeCallbacks { get; set; }
        //
        // Summary:
        //     Gets or sets the object used to marshal event-handler and callback calls.
        //
        // Remarks:
        //     When the value of this property is null, event-handler and callback calls
        //     are raised in the default manner. This could mean that the calls happen from
        //     a thread other than the main thread.  Avoid this behavior by setting this
        //     property to an object that implements System.ComponentModel.ISynchronizeInvoke
        //     such as a System.Windows.Forms.Control.  For more information, refer to .
        [Browsable(false)]
        [Obsolete("Use SynchronizeCallbacks to specify that the object marshals callbacks across threads appropriately.")]
        public ISynchronizeInvoke SynchronizingObject { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the end-of-string (EOS) character
        //     is used during read operations.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public bool TerminateReadOnEndOfString { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether to send UNT (Untalk) and UNL (Unlisten)
        //     at the end of read and write operations.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The property is set to an invalid state.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the property.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     Value is not currently supported by the NI-488.2 driver.
        public bool UnaddressingEnabled { get; set; }

        // Summary:
        //     Aborts any asynchronous read or write operation that is in progress on the
        //     device and resynchronizes the application with the driver.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        public void AbortAsynchronousIO();
        //
        // Summary:
        //     Initiates a read to a device asynchronously and reads up to NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes of data.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginRead() addresses the GPIB and begins
        //     an asynchronous read of up to NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes of data from a GPIB device. The operation terminates normally when
        //     NationalInstruments.NI4882.Device.DefaultBufferSize bytes have been received
        //     or END is received.
        //     The asynchronous I/O calls (NationalInstruments.NI4882.Device.BeginRead()
        //     and NationalInstruments.NI4882.Device.BeginWrite(System.String)) are designed
        //     so that applications can perform other non-GPIB operations while the I/O
        //     is in progress. Once the asynchronous I/O has begun, further NI-488.2 calls
        //     are strictly limited. Any calls that interfere with the I/O in progress are
        //     not allowed and return an exception.
        //     You can receive data from NationalInstruments.NI4882.Device.BeginRead() by
        //     calling NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult).
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) Blocks
        //     current program execution until the I/O completes and the driver and application
        //     are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the read, you must always call
        //     NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead() or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.
        public IAsyncResult BeginRead();
        //
        // Summary:
        //     Initiates a read to a device asynchronously and reads up to a specified number
        //     of bytes of data.
        //
        // Parameters:
        //   count:
        //     Number of bytes to read from the GPIB.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     count is negative.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginRead() addresses the GPIB and begins
        //     an asynchronous read of up to count bytes of data from a GPIB device. The
        //     operation terminates normally when NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes have been received or END is received.
        //     The asynchronous I/O calls (NationalInstruments.NI4882.Device.BeginRead()
        //     and NationalInstruments.NI4882.Device.BeginWrite(System.String)) are designed
        //     so that applications can perform other non-GPIB operations while the I/O
        //     is in progress. Once the asynchronous I/O has begun, further NI-488.2 calls
        //     are strictly limited. Any calls that interfere with the I/O in progress are
        //     not allowed and return an exception.
        //     You can receive data from NationalInstruments.NI4882.Device.BeginRead() by
        //     calling NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult).
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) Blocks
        //     current program execution until the I/O completes and the driver and application
        //     are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the read, you must always call
        //     NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead() or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.
        public IAsyncResult BeginRead(int count);
        //
        // Summary:
        //     Initiates a read of NationalInstruments.NI4882.Device.DefaultBufferSize to
        //     a device asynchronously and invokes a callback method when complete.
        //
        // Parameters:
        //   callback:
        //     The System.AsyncCallback that is raised when the read completes.
        //
        //   state:
        //     Object that contains additional user information.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     An invalid value was passed to the method. If thrown due to an invalid NI-488.2
        //     driver argument, the inner exception is set to NationalInstruments.NI4882.GpibException.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginRead() addresses the GPIB and begins
        //     an asynchronous read of up to NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes of data from a GPIB device. The operation terminates normally when
        //     NationalInstruments.NI4882.Device.DefaultBufferSize bytes have been received
        //     or END is received.
        //     The asynchronous I/O calls (NationalInstruments.NI4882.Device.BeginRead()
        //     and NationalInstruments.NI4882.Device.BeginWrite(System.String)) are designed
        //     so that applications can perform other non-GPIB operations while the I/O
        //     is in progress. Once the asynchronous I/O has begun, further NI-488.2 calls
        //     are strictly limited. Any calls that interfere with the I/O in progress are
        //     not allowed and return an exception.
        //     You can receive data from NationalInstruments.NI4882.Device.BeginRead() by
        //     calling NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult).
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) Blocks
        //     current program execution until the I/O completes and the driver and application
        //     are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the read, you must always call
        //     NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead() or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.  For more information, refer to .
        public IAsyncResult BeginRead(AsyncCallback callback, object state);
        //
        // Summary:
        //     Initiates a read of up to a specified number of bytes to a device asynchronously
        //     and invokes a callback method when complete.
        //
        // Parameters:
        //   count:
        //     Number of bytes to read from the GPIB.
        //
        //   callback:
        //     The System.AsyncCallback that is raised when the read completes.
        //
        //   state:
        //     Object that contains additional user information.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     count is negative.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginRead() addresses the GPIB and begins
        //     an asynchronous read of up to count bytes of data from a GPIB device. The
        //     operation terminates normally when NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes have been received or END is received.
        //     The asynchronous I/O calls (NationalInstruments.NI4882.Device.BeginRead()
        //     and NationalInstruments.NI4882.Device.BeginWrite(System.String)) are designed
        //     so that applications can perform other non-GPIB operations while the I/O
        //     is in progress. Once the asynchronous I/O has begun, further NI-488.2 calls
        //     are strictly limited. Any calls that interfere with the I/O in progress are
        //     not allowed and return an exception.
        //     You can receive data from NationalInstruments.NI4882.Device.BeginRead() by
        //     calling NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult).
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult).
        //      Blocks current program execution until the I/O completes and the driver
        //     and application are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the read, you must always call
        //     NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead() or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.  For more information, refer to .
        public IAsyncResult BeginRead(int count, AsyncCallback callback, object state);
        //
        // Summary:
        //     Writes byte array data asynchronously to a device from a user buffer.
        //
        // Parameters:
        //   data:
        //     Address of the buffer that contains the string to write.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String) addresses the
        //     GPIB properly and writes data to the GPIB device. The operation terminates
        //     normally when the data has been sent. The actual number of bytes transferred
        //     can be obtained from NationalInstruments.NI4882.Device.LastCount.
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult)
        //     Blocks current program execution until the I/O completes and the driver and
        //     application are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the write, you must always call
        //     NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult) after calling
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     or in the same thread of execution where NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     was called.
        public IAsyncResult BeginWrite(byte[] data);
        //
        // Summary:
        //     Writes string data asynchronously to a device from a user buffer.
        //
        // Parameters:
        //   data:
        //     Address of the buffer that contains the string to write.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String) addresses the
        //     GPIB properly and writes data to the GPIB device. The operation terminates
        //     normally when the data has been sent. The actual number of bytes transferred
        //     can be obtained from NationalInstruments.NI4882.Device.LastCount.
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult)
        //     Blocks current program execution until the I/O completes and the driver and
        //     application are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the write, you must always call
        //     NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult) after calling
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     or in the same thread of execution where NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     was called.
        public IAsyncResult BeginWrite(string data);
        //
        // Summary:
        //     Writes byte array data asynchronously to a device from a user buffer and
        //     invokes a callback method when complete.
        //
        // Parameters:
        //   data:
        //     Address of the buffer that contains the string to write.
        //
        //   callback:
        //     The System.AsyncCallback that is raised when the write completes.
        //
        //   state:
        //     Object that contains additional user information.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String) addresses the
        //     GPIB properly and writes data to the GPIB device. The operation terminates
        //     normally when the data has been sent. The actual number of bytes transferred
        //     can be obtained from NationalInstruments.NI4882.Device.LastCount.
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult)
        //     Blocks current program execution until the I/O completes and the driver and
        //     application are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the write, you must always call
        //     NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult) after calling
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     or in the same thread of execution where NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     was called.  For more information, refer to .
        public IAsyncResult BeginWrite(byte[] data, AsyncCallback callback, object state);
        //
        // Summary:
        //     Writes string data asynchronously to a device from a user buffer and invokes
        //     a callback method when complete.
        //
        // Parameters:
        //   data:
        //     Address of the buffer that contains the string to write.
        //
        //   callback:
        //     The System.AsyncCallback raised when the write completes.
        //
        //   state:
        //     Object that contains additional user information.
        //
        // Returns:
        //     An asynchronous result that represents this operation.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String) addresses the
        //     GPIB properly and writes data to the GPIB device. The operation terminates
        //     normally when the data has been sent. The actual number of bytes transferred
        //     can be obtained from NationalInstruments.NI4882.Device.LastCount.
        //     Once the I/O is complete, the application must resynchronize with the NI-488.2
        //     driver. Resynchronization is accomplished by using one of the following methods:
        //     MethodResult NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult)
        //     Blocks current program execution until the I/O completes and the driver and
        //     application are resynchronized.  NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes resynchronization.
        //      NationalInstruments.NI4882.Device.Reset() The I/O is canceled, the interface
        //     is reset, and the driver and application are resynchronized.  NationalInstruments.NI4882.Device.AbortAsynchronousIO()
        //     The I/O is canceled, and the driver and application are resynchronized. 
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     Passing NationalInstruments.NI4882.GpibStatusFlags.IOComplete causes the
        //     driver and application to be resynchronized.
        //     In order to free resources associated with the write, you must always call
        //     NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult) after calling
        //     NationalInstruments.NI4882.Device.BeginWrite(System.String), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     or in the same thread of execution where NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     was called.  For more information, refer to .
        public IAsyncResult BeginWrite(string data, AsyncCallback callback, object state);
        //
        // Summary:
        //     Sends the GPIB Selected Device Clear (SDC) message to the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        public void Clear();
        //
        // Summary:
        //     Releases all resources used by the NationalInstruments.NI4882.Device object.
        public void Dispose();
        //
        // Summary:
        //     Releases the resources used by the NationalInstruments.NI4882.Device object.
        //
        // Parameters:
        //   disposing:
        //     true if this method releases managed and unmanaged resources; false if this
        //     method releases only unmanaged resources.
        //
        // Remarks:
        //      The public System.IDisposable.Dispose() method and the finalizer call this
        //     method.The public System.IDisposable.Dispose() invokes the protected NationalInstruments.NI4882.Device.Dispose()(Boolean)
        //     method with disposing set to true. The finalizer invokes the protected NationalInstruments.NI4882.Device.Dispose()(Boolean)
        //     method with disposing set to false.
        //     When you set the disposing parameter to true, this method releases all resources
        //     held by managed objects that this NationalInstruments.NI4882.Device object
        //     references. This method invokes the System.IDisposable.Dispose() method of
        //     each referenced object.
        protected virtual void Dispose(bool disposing);
        //
        // Summary:
        //     Waits indefinitely for a previous NationalInstruments.NI4882.Device.BeginRead()
        //     call to complete.
        //
        // Parameters:
        //   asyncResult:
        //     An asynchronous result that represents the asynchronous NI-488.2 read operation
        //     that you want to end.
        //
        // Returns:
        //     A byte array containing the read data.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      asyncResult is invalid.
        //     -or-
        //     asyncResult is null.
        //
        //   System.InvalidOperationException:
        //      NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) was
        //     called before NationalInstruments.NI4882.Device.BeginRead().
        //     -or-
        //     The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //
        // Remarks:
        //     Call this method to wait for completion of an asynchronous read started with
        //     NationalInstruments.NI4882.Device.BeginRead() and to retrieve the read data.
        //     This method blocks execution of the current thread until the asynchronous
        //     read that you are waiting on completes.  In order to free resources associated
        //     with the read, you must always call NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead(), or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.
        public byte[] EndReadByteArray(IAsyncResult asyncResult);
        //
        // Summary:
        //     Waits indefinitely for a previous NationalInstruments.NI4882.Device.BeginRead()
        //     call to complete.
        //
        // Parameters:
        //   asyncResult:
        //     An asynchronous result that represents the asynchronous NI-488.2 read operation
        //     that you want to end.
        //
        // Returns:
        //     A string containing the read data.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      asyncResult is invalid.
        //     -or-
        //     asyncResult is null.
        //
        //   System.InvalidOperationException:
        //      NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult) was
        //     called before NationalInstruments.NI4882.Device.BeginRead().
        //     -or-
        //     The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //
        // Remarks:
        //     Call this method to wait for completion of an asynchronous read started with
        //     NationalInstruments.NI4882.Device.BeginRead() and to retrieve the read data.
        //     This method blocks execution of the current thread until the asynchronous
        //     read that you are waiting on completes.  In order to free resources associated
        //     with the read, you must always call NationalInstruments.NI4882.Device.EndReadString(System.IAsyncResult)
        //     or NationalInstruments.NI4882.Device.EndReadByteArray(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginRead(), whether in the
        //     callback given to NationalInstruments.NI4882.Device.BeginRead() or in the
        //     same thread of execution where NationalInstruments.NI4882.Device.BeginRead()
        //     was called.
        public string EndReadString(IAsyncResult asyncResult);
        //
        // Summary:
        //     Waits indefinitely for a previous NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     call to complete.
        //
        // Parameters:
        //   asyncResult:
        //     An asynchronous result that represents the asynchronous NI-488.2 write operation
        //     that you want to end.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      asyncResult is invalid.
        //     -or-
        //     asyncResult is null.
        //
        //   System.InvalidOperationException:
        //      NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult) was called
        //     before NationalInstruments.NI4882.Device.BeginWrite(System.String).
        //     -or-
        //     The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //
        // Remarks:
        //     Call this method to wait for an asynchronous write started with NationalInstruments.NI4882.Device.BeginWrite(System.String).
        //     This method blocks execution of the current thread until the asynchronous
        //     write operation that you are waiting on completes.  In order to free resources
        //     associated with the write, you must always call NationalInstruments.NI4882.Device.EndWrite(System.IAsyncResult)
        //     after calling NationalInstruments.NI4882.Device.BeginWrite(System.String),
        //     whether in the callback given to NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     or in the same thread of execution where NationalInstruments.NI4882.Device.BeginWrite(System.String)
        //     was called.
        public void EndWrite(IAsyncResult asyncResult);
        //
        // Summary:
        //     Returns the current value of various configuration parameters for the specified
        //     board or device.
        //
        // Parameters:
        //   configurationOption:
        //     Option to query.
        //
        // Returns:
        //     Value of configurationOption.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     configurationOption is less than 0x0000 or greater than 0x7FFFF.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     configurationOption is not available in the current NI-488.2 driver installed
        //     on the system.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetConfigurationOption(int configurationOption);
        //
        // Summary:
        //     Returns the current status of the GPIB.
        //
        // Returns:
        //     The updated status of GPIB.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Board.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Method requires GPIB interface to be Controller-In-Charge (CIC).
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     SRQ stuck in ON position.
        //     -or-
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     or NationalInstruments.NI4882.Device.GetCurrentStatus() is already in progress
        //     on the interface.
        //
        // Remarks:
        //     This method is performance intensive because it returns the GPIB status at
        //     that point in time.
        public GpibStatusFlags GetCurrentStatus();
        //
        // Summary:
        //     Automatically places the specified device in local mode.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Board.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     NationalInstruments.NI4882.Device.GoToLocal() is used to move devices temporarily
        //     from a remote program mode to a local mode, until the next device method
        //     is executed on that device.
        public void GoToLocal();
        //
        // Summary:
        //     Notifies the user of one or more GPIB events by invoking the user callback.
        //
        // Parameters:
        //   mask:
        //     Bit mask of GPIB events to notice.
        //
        //   callback:
        //     Pointer to the delegate method NationalInstruments.NI4882.NotifyCallback.
        //
        //   userData:
        //     User-defined reference data for the callback.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Board.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      mask is invalid or nonzero.
        //     -or-
        //     callback is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The current NI-488.2 driver cannot perform notification on one or more of
        //     the specified mask bits.
        //
        // Remarks:
        //     If mask is nonzero, NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     monitors the events specified by mask, and when one or more of the events
        //     is true, the callback is invoked. The only valid mask bits are NationalInstruments.NI4882.GpibStatusFlags.IOComplete,
        //     NationalInstruments.NI4882.GpibStatusFlags.Timeout, NationalInstruments.NI4882.GpibStatusFlags.End,
        //     and NationalInstruments.NI4882.GpibStatusFlags.DeviceServiceRequest. If NationalInstruments.NI4882.GpibStatusFlags.Timeout
        //     is set in the notify mask, and one or more of the other specified events
        //     has not already occurred, NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     calls the callback method when the NationalInstruments.NI4882.Board.IOTimeout
        //     period has elapsed. If NationalInstruments.NI4882.GpibStatusFlags.Timeout
        //     is not set in the Notify mask, the callback is not called until one or more
        //     of the specified events occur.
        //     Notification on NationalInstruments.NI4882.GpibStatusFlags.DeviceServiceRequest
        //     is not guaranteed to work if automatic serial polling is disabled. By default,
        //     automatic serial polling is enabled.
        //     A device can have only one outstanding NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     call at any one time. If a current NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     is in effect for the device, it is replaced by a subsequent NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     call. An outstanding NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     call for NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     can be canceled by a subsequent NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     call for a device that has a mask of zero.
        //     If a NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     call is outstanding and one or more of the GPIB events it is waiting on become
        //     true, the callback is invoked.
        //     Notification occurs when the state of one or more of the mask bits is true.
        //     Therefore, if a request is made to be notified when NationalInstruments.NI4882.GpibStatusFlags.IOComplete
        //     is true, and NationalInstruments.NI4882.GpibStatusFlags.IOComplete is currently
        //     true, the callback is invoked immediately.  In Measurement Studio Technology
        //     Preview 1, NationalInstruments.NI4882.NotifyData.SetReenableMask(NationalInstruments.NI4882.GpibStatusFlags)
        //     does not work correctly. To work around this problem, call NationalInstruments.NI4882.Device.Notify(NationalInstruments.NI4882.GpibStatusFlags,NationalInstruments.NI4882.NotifyCallback,System.Object)
        //     every time after the NationalInstruments.NI4882.NotifyCallback returns.
        public void Notify(GpibStatusFlags mask, NotifyCallback callback, object userData);
        //
        // Summary:
        //     Parallel polls devices on the GPIB.
        //
        // Returns:
        //     Parallel poll response byte.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     For more information about parallel polling, refer to the Parallel Polling
        //     Overview in the NI-488.2 User Manual.
        public byte ParallelPoll();
        //
        // Summary:
        //     Configures a device for a parallel poll.
        //
        // Parameters:
        //   parallelPollMessage:
        //     Parallel poll enable/disable value.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      The parallelPollMessage parameter is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     Enables or disables the device from responding to parallel polls. The device
        //     is addressed and sent the appropriate parallel poll message—Parallel Poll
        //     Enable (PPE) or Disable (PPD). Valid parallel poll messages are 96 to 126
        //     (0x60 to 0x7E) or zero to send PPD.
        public void ParallelPollConfigure(int parallelPollMessage);
        //
        // Summary:
        //     Passes control to another GPIB device with Controller capability.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //
        // Remarks:
        //     Passes Controller-in-Charge (CIC) status to the device. The access board
        //     automatically unasserts the ATN line and goes to Controller Idle State (CIDS).
        //     This method assumes that the device has Controller capability.
        public void PassControl();
        //
        // Summary:
        //     Reads byte array data from a device into a user buffer.
        //
        // Returns:
        //     The byte array data read from the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     I/O operation aborted.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     Addresses the GPIB and reads up to NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes of data. The operation terminates normally when NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes have been received or END is received. The operation throws an exception
        //     if the transfer cannot complete within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public byte[] ReadByteArray();
        //
        // Summary:
        //     Addresses the GPIB and reads up to the specified number of bytes of data.
        //
        // Parameters:
        //   count:
        //     Number of bytes to read.
        //
        // Returns:
        //     The byte array data read from the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     count is negative.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     I/O operation aborted.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     Addresses the GPIB and reads up to count bytes of data. The operation terminates
        //     normally when count bytes have been received or END is received. The operation
        //     throws an exception if the transfer cannot complete within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public byte[] ReadByteArray(int count);
        //
        // Summary:
        //     Reads string data from a device into a user buffer.
        //
        // Returns:
        //     The string data read from the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     I/O operation aborted.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     Addresses the GPIB and reads up to NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes of data. The operation terminates normally when NationalInstruments.NI4882.Device.DefaultBufferSize
        //     bytes have been received or END is received. The operation throws an exception
        //     if the transfer cannot complete within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public string ReadString();
        //
        // Summary:
        //     Addresses the GPIB and reads up to the specified number of bytes of data.
        //
        // Parameters:
        //   count:
        //     Number of bytes to read.
        //
        // Returns:
        //     The string data read from the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     count is negative.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     I/O operation aborted.
        //     -or-
        //     GPIB interface not addressed correctly.
        //
        // Remarks:
        //     Addresses the GPIB and reads up to count bytes of data. The operation terminates
        //     normally when count bytes have been received or END is received. The operation
        //     throws an exception if the transfer cannot complete within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public string ReadString(int count);
        //
        // Summary:
        //     Reads data from a device into a file.
        //
        // Parameters:
        //   fileName:
        //     Name of file into which data is read.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     fileName is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     I/0 operation aborted.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     File System Error.
        //
        // Remarks:
        //     Addresses the GPIB, reads data from a GPIB device, and places the data into
        //     the file specified by fileName. The operation terminates normally when END
        //     is received. The operation throws an exception if the transfer cannot complete
        //     within the NationalInstruments.NI4882.Device.IOTimeout period. The actual
        //     number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public void ReadToFile(string fileName);
        //
        // Summary:
        //     Places the device online by putting its software configuration parameters
        //     in their preconfigured state. The device is left operational or online.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     GPIB bus error.
        public void Reset();
        //
        // Summary:
        //     Conducts a serial poll.
        //
        // Returns:
        //     Serial poll response byte.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     The serial poll response could not be read within the serial poll timeout
        //     period.
        //
        // Remarks:
        //     This method returns the serial poll response byte. If bit 6 (hex 40) of the
        //     response is set, the device is requesting service. When the automatic serial
        //     polling feature is enabled, the device might have already been polled. In
        //     this case, NationalInstruments.NI4882.Device.SerialPoll() returns the previously
        //     acquired status byte.
        //     For more information about serial polling, refer to the Serial Polling Overview
        //     in the NI-488.2 User Manual.
        public SerialPollFlags SerialPoll();
        //
        // Summary:
        //     Changes a configuration item to the specified value for the selected device.
        //
        // Parameters:
        //   configurationOption:
        //     A parameter that selects the software configuration item.
        //
        //   configurationOptionValue:
        //     The value to which the selected configuration item is to change.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      configurationOption is valid, but configurationOptionValue is not defined
        //     for it.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     configurationOption is not available in the current NI-488.2 driver installed
        //     on the system.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetConfigurationOption(int configurationOption, int configurationOptionValue);
        //
        // Summary:
        //     Overrides System.Object.ToString().
        //
        // Returns:
        //     Returns a string representation of the object.
        public override string ToString();
        //
        // Summary:
        //     Sends the Group Execute Trigger (GET) GPIB message to the device.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        public void Trigger();
        //
        // Summary:
        //     Monitors the events specified by mask and delays processing until one or
        //     more of the events occurs.
        //
        // Parameters:
        //   mask:
        //     Bit mask of GPIB events to wait for.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //     A bit set in the mask is invalid.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Method requires GPIB interface to be Controller-in-Charge (CIC).
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     SRQ stuck in ON position.
        //     -or-
        //     NationalInstruments.NI4882.Device.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     or NationalInstruments.NI4882.Device.GetCurrentStatus() is already in progress
        //     on the interface.
        //
        // Remarks:
        //     If mask is NationalInstruments.NI4882.GpibStatusFlags.None, NationalInstruments.NI4882.Board.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     returns immediately with the updated status returned in NationalInstruments.NI4882.Board.LastStatus.
        //     This call is identical to NationalInstruments.NI4882.Board.GetCurrentStatus().
        //     If NationalInstruments.NI4882.GpibStatusFlags.Timeout is set in mask, and
        //     one or more of the other specified events have not already occurred, NationalInstruments.NI4882.Board.Wait(NationalInstruments.NI4882.GpibStatusFlags)
        //     returns when the NationalInstruments.NI4882.Board.IOTimeout period has elapsed.
        //     If NationalInstruments.NI4882.GpibStatusFlags.Timeout is not set in mask,
        //     the method waits indefinitely for one or more of the specified events to
        //     occur. You can configure the timeout period using the NationalInstruments.NI4882.Board.IOTimeout
        //     property. The only valid wait mask bits are NationalInstruments.NI4882.GpibStatusFlags.End,
        //     NationalInstruments.NI4882.GpibStatusFlags.DeviceServiceRequest, and NationalInstruments.NI4882.GpibStatusFlags.IOComplete.
        public void Wait(GpibStatusFlags mask);
        //
        // Summary:
        //     Writes byte array data to a device.
        //
        // Parameters:
        //   data:
        //     Data to write.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //     -or-
        //     I/O operation aborted.
        //
        // Remarks:
        //     Addresses the GPIB and writes data to a GPIB device. The operation terminates
        //     normally when all data bytes have been sent. The operation throws an exception
        //     if data cannot be sent within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public void Write(byte[] data);
        //
        // Summary:
        //     Writes string data to a device.
        //
        // Parameters:
        //   data:
        //     Data to write.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      data is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //     -or-
        //     I/O operation aborted.
        //
        // Remarks:
        //     Addresses the GPIB and writes data to a GPIB device. The operation terminates
        //     normally when all data bytes have been sent. The operation throws an exception
        //     if data cannot be sent within the NationalInstruments.NI4882.Device.IOTimeout
        //     period. The actual number of bytes transferred is returned in NationalInstruments.NI4882.Device.LastCount.
        public void Write(string data);
        //
        // Summary:
        //     Writes data to a device from a file.
        //
        // Parameters:
        //   fileName:
        //     Name of file that contains the data to write.
        //
        // Exceptions:
        //   NationalInstruments.NI4882.GpibException:
        //     The NI-488.2 driver returns an error as a result of calling this method.
        //
        //   System.ObjectDisposedException:
        //     This member is called after the NationalInstruments.NI4882.Device.Dispose()
        //     method has been called directly from your code or indirectly through a finalizer.
        //
        //   System.DllNotFoundException:
        //     The NI-488.2 driver library cannot be found.
        //
        //   System.EntryPointNotFoundException:
        //     A required operation in the NI-488.2 driver library cannot be found.
        //
        //   System.ArgumentException:
        //      fileName is null.
        //
        //   System.InvalidOperationException:
        //      The inner exception is set to the NationalInstruments.NI4882.GpibException
        //     due to one of the following conditions:
        //     A different process owns a lock for the interface.
        //     -or-
        //     Nonexistent GPIB interface.
        //     -or-
        //     Asynchronous I/O operation in progress.
        //     -or-
        //     The interface board is not Controller-In-Charge.
        //     -or-
        //     DMA error.
        //     -or-
        //     GPIB bus error.
        //     -or-
        //     GPIB interface not addressed correctly.
        //     -or-
        //     No Listeners on the GPIB.
        //     -or-
        //     I/O operation aborted.
        //
        // Remarks:
        //     Addresses the GPIB and writes all bytes from fileName to a GPIB device. The
        //     operation terminates normally when all bytes have been sent. The operation
        //     terminates and throws an exception if all bytes cannot be sent within the
        //     NationalInstruments.NI4882.Device.IOTimeout. The actual number of bytes transferred
        //     is returned in NationalInstruments.NI4882.Device.LastCount.
        public void WriteFromFile(string fileName);
    }
}
