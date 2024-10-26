using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PortalConnection
{
    IntPtr device_handle;
    IntPtr windows_handle;

    public PortalConnection(string path)
    {

        device_handle = hidapi.hid_open_path(path);

        Debug.Log(device_handle);

        windows_handle = Marshal.PtrToStructure<HidDevice>(device_handle).device_handle;
    }

    public void Write(byte[] data)
    {
        IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, dataPtr, data.Length);

        // TODO - Change Later // - Sends to the portal
        hidapi.DeviceIoControl(new SafeObjectHandle(windows_handle, false), 721301, dataPtr, 0x21, IntPtr.Zero, 0, out _, new IntPtr());

        Marshal.FreeHGlobal(dataPtr);
    }

    public byte[] Read()
    {
        byte[] input = new byte[0x20];
        hidapi.hid_read(device_handle, input, 0x20);
        return input;
    }
}
