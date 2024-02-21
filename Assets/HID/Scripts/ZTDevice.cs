using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

// The input system stores a chunk of memory for each device. What that
// memory looks like we can determine ourselves. The easiest way is to just describe
// it as a struct.
//
// Each chunk of memory is tagged with a "format" identifier in the form
// of a "FourCC" (a 32-bit code comprised of four characters). Using
// IInputStateTypeInfo we allow the system to get to the FourCC specific
// to our struct.



[StructLayout(LayoutKind.Explicit, Size = 19)]
public struct ZTGameDeviceState : IInputStateTypeInfo
{
    // We use "HID" here as our custom format code. It can be anything really.
    // Should be sufficiently unique to identify our memory format, though.
    public FourCC format => new FourCC('H', 'I', 'D');

    // Next we just define fields that store the state for our input device.
    // The only thing really interesting here is the [InputControl] attributes.
    // These automatically attach InputControls to the various memory bits that
    // we define.
    //
    // To get started, let's say that our device has a bitfield of buttons. Each
    // bit indicates whether a certain button is pressed or not. For the sake of
    // demonstration, let's say our device has 16 possible buttons. So, we define
    // a ushort field that contains the state of each possible button on the
    // device.
    //
    // On top of that, we need to tell the input system about each button. Both
    // what to call it and where to find it. The "name" property tells the input system
    // what to call the control; the "layout" property tells it what type of control
    // to create ("Button" in our case); and the "bit" property tells it which bit
    // in the bitfield corresponds to the button.
    //

    [FieldOffset(0)] public byte reportId;

    [InputControl(name = "dpad", format = "BIT", layout = "Dpad", sizeInBits = 4, defaultState = 8)]
    [InputControl(name = "dpad/up", format = "BIT", layout = "DiscreteButton", parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/right", format = "BIT", layout = "DiscreteButton", parameters = "minValue=1,maxValue=3", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/down", format = "BIT", layout = "DiscreteButton", parameters = "minValue=3,maxValue=5", bit = 0, sizeInBits = 4)]
    [InputControl(name = "dpad/left", format = "BIT", layout = "DiscreteButton", parameters = "minValue=5, maxValue=7", bit = 0, sizeInBits = 4)]
    [InputControl(name = "buttonWest", layout = "Button",displayName = "Square", bit = 4)]
    [InputControl(name = "buttonNorth", layout = "Button", displayName = "Triangle", bit = 5)]
    [InputControl(name = "buttonSouth", layout = "Button", displayName = "Cross", bit = 6)]
    [InputControl(name = "buttonEast", layout = "Button", displayName = "Circle", bit = 7)]
    
    [FieldOffset(1)] public byte buttons1;

    // We also tell the input system about "display names" here. These are names
    // that get displayed in the UI and such.
    [InputControl(name = "leftShoulder", layout = "Button", bit = 0)]
    [InputControl(name = "rightShoulder", layout = "Button", bit = 1)]
    [InputControl(name = "leftTriggerButton", layout = "Button", bit = 2)]
    [InputControl(name = "rightTriggerButton", layout = "Button", bit = 3)]
    [InputControl(name = "leftStickPress", layout = "Button", bit = 4)]
    [InputControl(name = "rightStickPress", layout = "Button", bit = 5)]
    [InputControl(name = "select", layout = "Button", displayName = "Share", bit = 6)]
    [InputControl(name = "start", layout = "Button", displayName = "Options", bit = 7)]

    [FieldOffset(2)] public byte buttons2;

    // Let's say our device also has a stick. However, the stick isn't stored
    // simply as two floats but as two unsigned bytes with the midpoint of each
    // axis located at value 127. We can simply define two consecutive byte
    // fields to represent the stick and annotate them like so.
    //
    // First, let's introduce stick control itself. This one is simple. We don't
    // yet worry about X and Y individually as the stick as whole will itself read the
    // component values from those controls.
    //
    // We need to set "format" here too as InputControlLayout will otherwise try to
    // infer the memory format from the field. As we put this attribute on "X", that
    // would come out as "BYTE" -- which we don't want. So we set it to "VC2B" (a Vector2
    // of bytes).
    [InputControl(name = "leftStick", format = "VC2B", layout = "Stick", displayName = "Main Stick")]
    // So that's what we need next. By default, both X and Y on "Stick" are floating-point
    // controls so here we need to individually configure them the way they work for our
    // stick.
    //
    // NOTE: We don't mention things as "layout" and such here. The reason is that we are
    //       modifying a control already defined by "Stick". This means that we only need
    //       to set the values that are different from what "Stick" stick itself already
    //       configures. And since "Stick" configures both "X" and "Y" to be "Axis" controls,
    //       we don't need to worry about that here.
    //
    // Using "format", we tell the controls how their data is stored. As bytes in our case
    // so we use "BYTE" (check the documentation for InputStateBlock for details on that).
    //
    // NOTE: We don't use "SBYT" (signed byte) here. Our values are not signed. They are
    //       unsigned. It's just that our "resting" (i.e. mid) point is at 127 and not at 0.
    //
    // Also, we use "defaultState" to tell the system that in our case, setting the
    // memory to all zeroes will *NOT* result in a default value. Instead, if both x and y
    // are set to zero, the result will be Vector2(-1,-1).
    //
    // And then, using the various "normalize" parameters, we tell the input system how to
    // deal with the fact that our midpoint is located smack in the middle of our value range.
    // Using "normalize" (which is equivalent to "normalize=true") we instruct the control
    // to normalize values. Using "normalizeZero=0.5", we tell it that our midpoint is located
    // at 0.5 (AxisControl will convert the BYTE value to a [0..1] floating-point value with
    // 0=0 and 255=1) and that our lower limit is "normalizeMin=0" and our upper limit is
    // "normalizeMax=1". Put another way, it will map [0..1] to [-1..1].
    //
    // Finally, we also set "offset" here as this is already set by StickControl.X and
    // StickControl.Y -- which we inherit. Note that because we're looking at child controls
    // of the stick, the offset is relative to the stick, not relative to the beginning
    // of the state struct.
    [InputControl(name = "leftStick/x", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/left", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/right", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
    [InputControl(name = "leftStick/y", offset = 1, format = "BYTE",
        parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/up", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/down", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
    // The stick up/down/left/right buttons automatically use the state set up for X
    // and Y but they have their own parameters. Thus we need to also sync them to
    // the parameter settings we need for our BYTE setup.
    // NOTE: This is a shortcoming in the current layout system that cannot yet correctly
    //       merge parameters. Will be fixed in a future version.

    [FieldOffset(3)] public byte leftStickX;
    [FieldOffset(4)] public byte leftStickY;

    [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
    [InputControl(name = "rightStick/x", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "rightStick/left", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/right", offset = 0, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
    [InputControl(name = "rightStick/y", offset = 1, format = "BYTE", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "rightStick/up", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "rightStick/down", offset = 1, format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
    [FieldOffset(5)] public byte rightStickX;
    [FieldOffset(6)] public byte rightStickY;


    [InputControl(name = "leftTrigger", format = "BYTE")]
    [FieldOffset(7)] public byte leftTrigger;
    [InputControl(name = "rightTrigger", format = "BYTE")]
    [FieldOffset(8)] public byte rightTrigger;

    //Vendor defined data,length:1+1+4+4 = 10
    [InputControl(name = "dCMD", layout = "Integer", format = "BYTE")]
    [FieldOffset(9)] public byte Data0;

    [InputControl(name = "dState", layout = "Integer", format = "BYTE")]
    [FieldOffset(10)] public byte Data1;

    [InputControl(name = "dData0", layout = "Integer", format = "UINT")]
    [FieldOffset(11)] public UInt32 Data2;

    [InputControl(name = "dData1", layout = "Integer", format = "UINT")]
    [FieldOffset(15)] public UInt32 Data3;
    //You can add any data here, but need to match the HID input report

}


#if UNITY_EDITOR
[InitializeOnLoad] // Call static class constructor in editor.
#endif
[InputControlLayout(stateType = typeof(ZTGameDeviceState))]
//public class CustomDevice : InputDevice, IInputUpdateCallbackReceiver
public class ZTGameCtrl : Gamepad
//IInputUpdateCallbackReceiver
{

    // [InitializeOnLoad] will ensure this gets called on every domain (re)load
    // in the editor.
#if UNITY_EDITOR
    static ZTGameCtrl()
    {
        // Trigger our RegisterLayout code in the editor.
        //InputSystem.RegisterLayout<Gamepad>();
        Initialize();
    }

#endif

    // In the player, [RuntimeInitializeOnLoadMethod] will make sure our
    // initialization code gets called during startup.
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        //// Alternatively, you can also match by PID and VID, which is generally
        //// more reliable for HIDs.
        InputSystem.RegisterLayout<ZTGameCtrl>(
            matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x5E04) // 
                .WithCapability("productId", 0x8E02)); //
    }

    public ButtonControl leftTriggerButton  { get; private set; }
    public ButtonControl rightTriggerButton { get; private set; }

    public IntegerControl dCMD { get; private set; }
    public IntegerControl dState { get; private set; }
    public IntegerControl dData0 { get; private set; }
    public IntegerControl dData1 { get; private set; }

    public new static ZTGameCtrl current { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();

        leftTriggerButton = GetChildControl<ButtonControl>("leftTriggerButton");
        rightTriggerButton = GetChildControl<ButtonControl>("rightTriggerButton");

        dCMD = GetChildControl<IntegerControl>("dCMD");
        dState = GetChildControl<IntegerControl>("dState");
        dData0 = GetChildControl<IntegerControl>("dData0");
        dData1 = GetChildControl<IntegerControl>("dData1");

    }

    public override void MakeCurrent()
    {
        base.MakeCurrent();
        current = this;
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();
        if (current == this)
            current = null;
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Custom Device Sample/Create Device")]
    private static void CreateDevice()
    {
        // This is the code that you would normally run at the point where
        // you discover devices of your custom type.
        InputSystem.AddDevice(new InputDeviceDescription
        {
            interfaceName = "Custom",
            product = "Sample Product"
        });
    }
    // For completeness sake, let's also add code to remove one instance of our
    // custom device. Note that you can also manually remove the device from
    // the input debugger by right-clicking in and selecting "Remove Device".
    [MenuItem("Tools/Custom Device Sample/Remove Device")]
    private static void RemoveDevice()
    {
        var customDevice = InputSystem.devices.FirstOrDefault(x => x is ZTGameCtrl);
        if (customDevice != null)
            InputSystem.RemoveDevice(customDevice);
    }
#endif


}
