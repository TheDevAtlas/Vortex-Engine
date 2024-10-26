using System;
using System.Runtime.InteropServices;

// this is getting the functions from the hidapi, we just need to declare them //

[StructLayout(LayoutKind.Sequential)]
public struct hidDeviceInfo
{
    /** Platform-specific device path */
    public string path;
    /** Device Vendor ID */
    public ushort vendor_id;
    /** Device Product ID */
    public ushort product_id;
    /** Serial Number */
    public string serial_number;
    /** Device Release Number in binary-coded decimal,
        also known as Device Version Number */
    public ushort release_number;
    /** Manufacturer String */
    public string manufacturer_string;
    /** Product string */
    public string product_string;
    /** Usage Page for this Device/Interface
        (Windows/Mac/hidraw only) */
    public ushort usage_page;
    /** Usage for this Device/Interface
        (Windows/Mac/hidraw only) */
    public ushort usage;
    /** The USB interface which this logical device
        represents.

        Valid only if the device is a USB HID device.
        Set to -1 in all other cases.
    */
    public int interface_number;

    /** Pointer to the next device */
    public IntPtr next;

    /** Underlying bus type
		Since version 0.13.0, @ref HID_API_VERSION >= HID_API_MAKE_VERSION(0, 13, 0)
	*/
    public int bus_type;
}

[StructLayout(LayoutKind.Sequential)]
internal struct HidDevice
{
    public IntPtr device_handle;
};

public class hidapi
{
    const string dllFileName = "hidapi";

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_open_path([In] string path);

    // from Kernal 32
    [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool DeviceIoControl(
            SafeObjectHandle hDevice,
            int dwIoControlCode,
            IntPtr inBuffer,
            int nInBufferSize,
            IntPtr outBuffer,
            int nOutBufferSize,
            out int pBytesReturned,
            IntPtr lpOverlapped);

    // from hidapi.dl
    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_init();

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_enumerate(ushort vendor_id, ushort product_id);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void hid_free_enumeration(IntPtr devs);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_write(IntPtr device, [In] byte[] data, uint length);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int hid_read(IntPtr device, [Out] byte[] buf_data, uint length);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public extern static void hid_close(IntPtr device);

    [DllImport(dllFileName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr hid_open(ushort vendor_id, ushort product_id, [In] string? serial_number);

}

/// <summary>
/// Represents a Win32 handle that can be closed with <see cref="CloseHandle"/>.
/// </summary>
public class SafeObjectHandle : SafeHandle
{
    /// <summary>
    /// An invalid handle that may be used in place of <see cref="INVALID_HANDLE_VALUE"/>.
    /// </summary>
    public static readonly SafeObjectHandle Invalid = new SafeObjectHandle();

    /// <summary>
    /// A handle that may be used in place of <see cref="IntPtr.Zero"/>.
    /// </summary>
    public static readonly SafeObjectHandle Null = new SafeObjectHandle(IntPtr.Zero, false);

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeObjectHandle"/> class.
    /// </summary>
    public SafeObjectHandle()
        : base(IntPtr.Zero, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeObjectHandle"/> class.
    /// </summary>
    /// <param name="preexistingHandle">An object that represents the pre-existing handle to use.</param>
    /// <param name="ownsHandle">
    ///     <see langword="true" /> to have the native handle released when this safe handle is disposed or finalized;
    ///     <see langword="false" /> otherwise.
    /// </param>
    public SafeObjectHandle(IntPtr preexistingHandle, bool ownsHandle = true)
        : base(IntPtr.Zero, ownsHandle)
    {
        this.SetHandle(preexistingHandle);
    }

    /// <inheritdoc />
    public override bool IsInvalid => this.handle == IntPtr.Zero || this.handle == IntPtr.Zero;

    /// <inheritdoc />
    protected override bool ReleaseHandle()
    {
        return true;
    }
}