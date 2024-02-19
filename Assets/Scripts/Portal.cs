//                   Jacob McGowan - The Dev Atlas - Feb 17th, 2024                  \\
// This Script Allows The Input System To Add An Unknown Device: The Portal Of Power \\
//                                                                                   \\

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;

public struct PortalState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('H', 'I', 'D');

    [InputControl(name = "ByteOne", layout = "Integer")]
    public int ByteOne;

    [InputControl(name = "ByteTwo", layout = "Integer")]
    public int ByteTwo;

    // Add additional bytes here
    [InputControl(name = "ByteThree", layout = "Integer")]
    public int ByteThree;

    [InputControl(name = "ByteFour", layout = "Integer")]
    public int ByteFour;

    // Repeat for bytes up to ByteSixteen
    [InputControl(name = "ByteFive", layout = "Integer")]
    public int ByteFive;
    // Continue defining up to ByteSixteen
}

[InputControlLayout(stateType = typeof(PortalState))]
[InitializeOnLoad]
public class Portal : InputDevice
{
    [InputControl(displayName = "ByteOne")]
    public IntegerControl ByteOne { get; private set; }

    [InputControl(displayName = "ByteTwo")]
    public IntegerControl ByteTwo { get; private set; }

    // Add additional byte controls here
    [InputControl(displayName = "ByteThree")]
    public IntegerControl ByteThree { get; private set; }

    [InputControl(displayName = "ByteFour")]
    public IntegerControl ByteFour { get; private set; }

    // Repeat for bytes up to ByteSixteen
    [InputControl(displayName = "ByteFive")]
    public IntegerControl ByteFive { get; private set; }
    // Continue adding controls up to ByteSixteen

    protected override void FinishSetup()
    {
        base.FinishSetup();

        ByteOne = GetChildControl<IntegerControl>("ByteOne");
        ByteTwo = GetChildControl<IntegerControl>("ByteTwo");

        // Setup additional bytes
        ByteThree = GetChildControl<IntegerControl>("ByteThree");
        ByteFour = GetChildControl<IntegerControl>("ByteFour");
        // Continue setup for additional bytes up to ByteSixteen
    }

    static Portal()
    {
        InputSystem.RegisterLayout<Portal>(
            matches: new InputDeviceMatcher()
            .WithInterface("HID")
            .WithCapability("vendorId", 0x1430)  // Look into diff portals + products
            .WithCapability("productId", 0x0150) // look into potential diff codes for consoles ect.
        );

        if (!InputSystem.devices.Any(x => x is Portal))
        {
            InputSystem.AddDevice<Portal>();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    [MenuItem("Tools/Add Portal Device")]
    public static void Initialize()
    {
    }
}