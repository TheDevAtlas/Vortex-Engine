//                   Jacob McGowan - The Dev Atlas - Feb 17th, 2024                  \\
// This Script Allows The Input System To Add An Unknown Device: The Portal Of Power \\
//                                                                                   \\

using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

[StructLayout(LayoutKind.Explicit, Size = 33)]
public struct PortalState : IInputStateTypeInfo
{
    // Portal State Is What Unity Reads From The Portal \\

    public FourCC format => new FourCC('H', 'I', 'D');

    [InputControl(name = "_00Byte", format = "BYTE", layout = "button")]
    [FieldOffset(0)]
    public byte _00Byte;
    [InputControl(name = "_01Byte", format = "BYTE", layout = "button")]
    [FieldOffset(1)]
    public byte _01Byte;
    [InputControl(name = "_02Byte", format = "BYTE", layout = "button")]
    [FieldOffset(2)]
    public byte _02Byte;
    [InputControl(name = "_03Byte", format = "BYTE", layout = "button")]
    [FieldOffset(3)]
    public byte _03Byte;
    [InputControl(name = "_04Byte", format = "BYTE", layout = "button")]
    [FieldOffset(4)]
    public byte _04Byte;
    [InputControl(name = "_05Byte", format = "BYTE", layout = "button")]
    [FieldOffset(5)]
    public byte _05Byte;
    [InputControl(name = "_06Byte", format = "BYTE", layout = "button")]
    [FieldOffset(6)]
    public byte _06Byte;
    [InputControl(name = "_07Byte", format = "BYTE", layout = "button")]
    [FieldOffset(7)]
    public byte _07Byte;
    [InputControl(name = "_08Byte", format = "BYTE", layout = "button")]
    [FieldOffset(8)]
    public byte _08Byte;
    [InputControl(name = "_09Byte", format = "BYTE", layout = "button")]
    [FieldOffset(9)]
    public byte _09Byte;
    [InputControl(name = "_0AByte", format = "BYTE", layout = "button")]
    [FieldOffset(10)]
    public byte _0AByte;
    [InputControl(name = "_0BByte", format = "BYTE", layout = "button")]
    [FieldOffset(11)]
    public byte _0BByte;
    [InputControl(name = "_0CByte", format = "BYTE", layout = "button")]
    [FieldOffset(12)]
    public byte _0CByte;
    [InputControl(name = "_0DByte", format = "BYTE", layout = "button")]
    [FieldOffset(13)]
    public byte _0DByte;
    [InputControl(name = "_0EByte", format = "BYTE", layout = "button")]
    [FieldOffset(14)]
    public byte _0EByte;
    [InputControl(name = "_0FByte", format = "BYTE", layout = "button")]
    [FieldOffset(15)]
    public byte _0FByte;
    [InputControl(name = "_10Byte", format = "BYTE", layout = "button")]
    [FieldOffset(16)]
    public byte _10Byte;
    [InputControl(name = "_11Byte", format = "BYTE", layout = "button")]
    [FieldOffset(17)]
    public byte _11Byte;
    [InputControl(name = "_12Byte", format = "BYTE", layout = "button")]
    [FieldOffset(18)]
    public byte _12Byte;
    [InputControl(name = "_13Byte", format = "BYTE", layout = "button")]
    [FieldOffset(19)]
    public byte _13Byte;
    [InputControl(name = "_14Byte", format = "BYTE", layout = "button")]
    [FieldOffset(20)]
    public byte _14Byte;
    [InputControl(name = "_15Byte", format = "BYTE", layout = "button")]
    [FieldOffset(21)]
    public byte _15Byte;
    [InputControl(name = "_16Byte", format = "BYTE", layout = "button")]
    [FieldOffset(22)]
    public byte _16Byte;
    [InputControl(name = "_17Byte", format = "BYTE", layout = "button")]
    [FieldOffset(23)]
    public byte _17Byte;
    [InputControl(name = "_18Byte", format = "BYTE", layout = "button")]
    [FieldOffset(24)]
    public byte _18Byte;
    [InputControl(name = "_19Byte", format = "BYTE", layout = "button")]
    [FieldOffset(25)]
    public byte _19Byte;
    [InputControl(name = "_1AByte", format = "BYTE", layout = "button")]
    [FieldOffset(26)]
    public byte _1AByte;
    [InputControl(name = "_1BByte", format = "BYTE", layout = "button")]
    [FieldOffset(27)]
    public byte _1BByte;
    [InputControl(name = "_1CByte", format = "BYTE", layout = "button")]
    [FieldOffset(28)]
    public byte _1CByte;
    [InputControl(name = "_1DByte", format = "BYTE", layout = "button")]
    [FieldOffset(29)]
    public byte _1DByte;
    [InputControl(name = "_1EByte", format = "BYTE", layout = "button")]
    [FieldOffset(30)]
    public byte _1EByte;
    [InputControl(name = "_1FByte", format = "BYTE", layout = "button")]
    [FieldOffset(31)]
    public byte _1FByte;
    [InputControl(name = "_20Byte", format = "BYTE", layout = "button")]
    [FieldOffset(32)]
    public byte _20Byte;

}

[StructLayout(LayoutKind.Explicit, Size = kSize)]
internal struct PortalCommand : IInputDeviceCommandInfo
{
    // Portal Command Allows For Unity To Send A Command \\

    public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
    internal const int kSize = InputDeviceCommand.BaseCommandSize + 33;

    [FieldOffset(0)]
    public InputDeviceCommand baseCommand;

    // output bytes for a total of 33 bytes
    [FieldOffset(InputDeviceCommand.BaseCommandSize)]
    public byte _00Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 1)]
    public byte _01Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 2)]
    public byte _02Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 3)]
    public byte _03Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 4)]
    public byte _04Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 5)]
    public byte _05Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 6)]
    public byte _06Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 7)]
    public byte _07Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 8)]
    public byte _08Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 9)]
    public byte _09Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 10)]
    public byte _0AByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 11)]
    public byte _0BByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 12)]
    public byte _0CByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 13)]
    public byte _0DByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 14)]
    public byte _0EByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 15)]
    public byte _0FByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 16)]
    public byte _10Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 17)]
    public byte _11Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 18)]
    public byte _12Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 19)]
    public byte _13Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 20)]
    public byte _14Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 21)]
    public byte _15Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 22)]
    public byte _16Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 23)]
    public byte _17Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 24)]
    public byte _18Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 25)]
    public byte _19Byte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 26)]
    public byte _1AByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 27)]
    public byte _1BByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 28)]
    public byte _1CByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 29)]
    public byte _1DByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 30)]
    public byte _1EByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 31)]
    public byte _1FByte;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 32)]
    public byte _20Byte;

    public FourCC typeStatic
    {
        get { return Type; }
    }

    // Creates The Struct For Use In Other Scripts \\
    public static PortalCommand Create(byte[] data)
    {
        return new PortalCommand
        {
            // You Need To Describe How Many Bytes You Are Sending (kSize) \\
            baseCommand = new InputDeviceCommand(Type, kSize),
            _00Byte = data[0],
            _01Byte = data[1],
            _02Byte = data[2],
            _03Byte = data[3],
            _04Byte = data[4],
            _05Byte = data[5],
            _06Byte = data[6],
            _07Byte = data[7],
            _08Byte = data[8],
            _09Byte = data[9],
            _0AByte = data[10],
            _0BByte = data[11],
            _0CByte = data[12],
            _0DByte = data[13],
            _0EByte = data[14],
            _0FByte = data[15],
            _10Byte = data[16],
            _11Byte = data[17],
            _12Byte = data[18],
            _13Byte = data[19],
            _14Byte = data[20],
            _15Byte = data[21],
            _16Byte = data[22],
            _17Byte = data[23],
            _18Byte = data[24],
            _19Byte = data[25],
            _1AByte = data[26],
            _1BByte = data[27],
            _1CByte = data[28],
            _1DByte = data[29],
            _1EByte = data[30],
            _1FByte = data[31],
            _20Byte = data[32]
        };
    }
}
/// <summary>
/// Testing PS4 Way Of Handling Input
/// </summary>
/// 
[StructLayout(LayoutKind.Explicit, Size = 33)]
internal struct PortalDualSenseHIDOutputReportPayload
{
    // output bytes for a total of 33 bytes
    [FieldOffset(0)] public byte _00Byte;
    [FieldOffset(1)] public byte _01Byte;
    [FieldOffset(2)] public byte _02Byte;
    [FieldOffset(3)] public byte _03Byte;
    [FieldOffset(4)] public byte _04Byte;
    [FieldOffset(5)] public byte _05Byte;
    [FieldOffset(6)] public byte _06Byte;
    [FieldOffset(7)] public byte _07Byte;
    [FieldOffset(8)] public byte _08Byte;
    [FieldOffset(9)] public byte _09Byte;
    [FieldOffset(10)] public byte _0AByte;
    [FieldOffset(11)] public byte _0BByte;
    [FieldOffset(12)] public byte _0CByte;
    [FieldOffset(13)] public byte _0DByte;
    [FieldOffset(14)] public byte _0EByte;
    [FieldOffset(15)] public byte _0FByte;
    [FieldOffset(16)] public byte _10Byte;
    [FieldOffset(17)] public byte _11Byte;
    [FieldOffset(18)] public byte _12Byte;
    [FieldOffset(19)] public byte _13Byte;
    [FieldOffset(20)] public byte _14Byte;
    [FieldOffset(21)] public byte _15Byte;
    [FieldOffset(22)] public byte _16Byte;
    [FieldOffset(23)] public byte _17Byte;
    [FieldOffset(24)] public byte _18Byte;
    [FieldOffset(25)] public byte _19Byte;
    [FieldOffset(26)] public byte _1AByte;
    [FieldOffset(27)] public byte _1BByte;
    [FieldOffset(28)] public byte _1CByte;
    [FieldOffset(29)] public byte _1DByte;
    [FieldOffset(30)] public byte _1EByte;
    [FieldOffset(31)] public byte _1FByte;
    [FieldOffset(32)] public byte _20Byte;
}

[StructLayout(LayoutKind.Explicit, Size = kSize)]
internal struct PortalDualSenseHIDUSBOutputReport : IInputDeviceCommandInfo
{
    public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
    public FourCC typeStatic => Type;

    internal const int kSize = InputDeviceCommand.BaseCommandSize + 33;

    [FieldOffset(0)] public InputDeviceCommand baseCommand;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)] public byte reportId;
    [FieldOffset(InputDeviceCommand.BaseCommandSize + 1)] public PortalDualSenseHIDOutputReportPayload payload;

    public static PortalDualSenseHIDUSBOutputReport Create(PortalDualSenseHIDOutputReportPayload payload)
    {
        return new PortalDualSenseHIDUSBOutputReport
        {
            baseCommand = new InputDeviceCommand(Type, kSize),
            reportId = 0,
            payload = payload
        };
    }
}

/// <summary>
/// //////////////////////////////////////////////////
/// </summary>

// Create The Portal Device Itself / Setup Reading \\

[InputControlLayout(stateType = typeof(PortalState))]
#if (UNITY_EDITOR)
[InitializeOnLoad]
#endif
public class PortalDevice : InputDevice
{
    // Create The Bytes For Reading / What Byte It Is \\
    // This Shows Up In The Input Debugger For Use    \\

    [InputControl(displayName = "_00Byte")]
    public ButtonControl _00Byte { get; private set; }
    [InputControl(displayName = "_01Byte")]
    public ButtonControl _01Byte { get; private set; }
    [InputControl(displayName = "_02Byte")]
    public ButtonControl _02Byte { get; private set; }
    [InputControl(displayName = "_03Byte")]
    public ButtonControl _03Byte { get; private set; }
    [InputControl(displayName = "_04Byte")]
    public ButtonControl _04Byte { get; private set; }
    [InputControl(displayName = "_05Byte")]
    public ButtonControl _05Byte { get; private set; }
    [InputControl(displayName = "_06Byte")]
    public ButtonControl _06Byte { get; private set; }
    [InputControl(displayName = "_07Byte")]
    public ButtonControl _07Byte { get; private set; }
    [InputControl(displayName = "_08Byte")]
    public ButtonControl _08Byte { get; private set; }
    [InputControl(displayName = "_09Byte")]
    public ButtonControl _09Byte { get; private set; }
    [InputControl(displayName = "_0AByte")]
    public ButtonControl _0AByte { get; private set; }
    [InputControl(displayName = "_0BByte")]
    public ButtonControl _0BByte { get; private set; }
    [InputControl(displayName = "_0CByte")]
    public ButtonControl _0CByte { get; private set; }
    [InputControl(displayName = "_0DByte")]
    public ButtonControl _0DByte { get; private set; }
    [InputControl(displayName = "_0EByte")]
    public ButtonControl _0EByte { get; private set; }
    [InputControl(displayName = "_0FByte")]
    public ButtonControl _0FByte { get; private set; }
    [InputControl(displayName = "_10Byte")]
    public ButtonControl _10Byte { get; private set; }
    [InputControl(displayName = "_11Byte")]
    public ButtonControl _11Byte { get; private set; }
    [InputControl(displayName = "_12Byte")]
    public ButtonControl _12Byte { get; private set; }
    [InputControl(displayName = "_13Byte")]
    public ButtonControl _13Byte { get; private set; }
    [InputControl(displayName = "_14Byte")]
    public ButtonControl _14Byte { get; private set; }
    [InputControl(displayName = "_15Byte")]
    public ButtonControl _15Byte { get; private set; }
    [InputControl(displayName = "_16Byte")]
    public ButtonControl _16Byte { get; private set; }
    [InputControl(displayName = "_17Byte")]
    public ButtonControl _17Byte { get; private set; }
    [InputControl(displayName = "_18Byte")]
    public ButtonControl _18Byte { get; private set; }
    [InputControl(displayName = "_19Byte")]
    public ButtonControl _19Byte { get; private set; }
    [InputControl(displayName = "_1AByte")]
    public ButtonControl _1AByte { get; private set; }
    [InputControl(displayName = "_1BByte")]
    public ButtonControl _1BByte { get; private set; }
    [InputControl(displayName = "_1CByte")]
    public ButtonControl _1CByte { get; private set; }
    [InputControl(displayName = "_1DByte")]
    public ButtonControl _1DByte { get; private set; }
    [InputControl(displayName = "_1EByte")]
    public ButtonControl _1EByte { get; private set; }
    [InputControl(displayName = "_1FByte")]
    public ButtonControl _1FByte { get; private set; }
    [InputControl(displayName = "_20Byte")]
    public ButtonControl _20Byte { get; private set; }

    // Sets up the device and allows us to connect the data from \\
    // The usb devices to our portal state                       \\
    protected override void FinishSetup()
    {
        base.FinishSetup();

        _00Byte = GetChildControl<ButtonControl>("_00Byte");
        _01Byte = GetChildControl<ButtonControl>("_01Byte");
        _02Byte = GetChildControl<ButtonControl>("_02Byte");
        _03Byte = GetChildControl<ButtonControl>("_03Byte");
        _04Byte = GetChildControl<ButtonControl>("_04Byte");
        _05Byte = GetChildControl<ButtonControl>("_05Byte");
        _06Byte = GetChildControl<ButtonControl>("_06Byte");
        _07Byte = GetChildControl<ButtonControl>("_07Byte");
        _08Byte = GetChildControl<ButtonControl>("_08Byte");
        _09Byte = GetChildControl<ButtonControl>("_09Byte");
        _0AByte = GetChildControl<ButtonControl>("_0AByte");
        _0BByte = GetChildControl<ButtonControl>("_0BByte");
        _0CByte = GetChildControl<ButtonControl>("_0CByte");
        _0DByte = GetChildControl<ButtonControl>("_0DByte");
        _0EByte = GetChildControl<ButtonControl>("_0EByte");
        _0FByte = GetChildControl<ButtonControl>("_0FByte");

        _10Byte = GetChildControl<ButtonControl>("_10Byte");
        _11Byte = GetChildControl<ButtonControl>("_11Byte");
        _12Byte = GetChildControl<ButtonControl>("_12Byte");
        _13Byte = GetChildControl<ButtonControl>("_13Byte");
        _14Byte = GetChildControl<ButtonControl>("_14Byte");
        _15Byte = GetChildControl<ButtonControl>("_15Byte");
        _16Byte = GetChildControl<ButtonControl>("_16Byte");
        _17Byte = GetChildControl<ButtonControl>("_17Byte");
        _18Byte = GetChildControl<ButtonControl>("_18Byte");
        _19Byte = GetChildControl<ButtonControl>("_19Byte");
        _1AByte = GetChildControl<ButtonControl>("_1AByte");
        _1BByte = GetChildControl<ButtonControl>("_1BByte");
        _1CByte = GetChildControl<ButtonControl>("_1CByte");
        _1DByte = GetChildControl<ButtonControl>("_1DByte");
        _1EByte = GetChildControl<ButtonControl>("_1EByte");
        _1FByte = GetChildControl<ButtonControl>("_1FByte");

        _20Byte = GetChildControl<ButtonControl>("_20Byte");

    }

    // What defines our device against others, basically is this the \\
    // portal or another device                                      \\
    static PortalDevice()
    {
        InputSystem.RegisterLayout<PortalDevice>(
            matches: new InputDeviceMatcher()
            .WithInterface("HID")
            .WithCapability("vendorId", 0x1430)  // Activision ID \\
            .WithCapability("productId", 0x0150) // Portal ID     \\
        );

        if (!InputSystem.devices.Any(x => x is PortalDevice))
        {
            InputSystem.AddDevice<PortalDevice>();
        }
    }

    [RuntimeInitializeOnLoadMethod]
#if (UNITY_EDITOR)
    [MenuItem("Tools/Add Portal Device")]
#endif
    public static void Initialize()
    {
    }

    public PortalState GetNewState()
    {
        InputSystem.QueueStateEvent(this, new PortalState());
        return new PortalState();
    }
}
