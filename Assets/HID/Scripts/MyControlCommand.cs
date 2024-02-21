using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Utilities;
using System;

namespace UnityEngine.InputSystem.LowLevel
{
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct MyControlCommand : IInputDeviceCommandInfo
    {
        public static FourCC Type { get { return new FourCC('H', 'I', 'D', 'O'); } }
        internal const int id = 0x01;
        internal const int kSize = InputDeviceCommand.BaseCommandSize + 11;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize)]
        public byte reportId;

        [FieldOffset(InputDeviceCommand.BaseCommandSize+1)]
        public byte dataID;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 2)]
        public byte dataFun;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 3)]
        public UInt32 data1;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 7)]
        public UInt32 data2;


        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static MyControlCommand Create(byte indataID, byte indataFun, UInt32 indata1, UInt32 indata2)
        {
            return new MyControlCommand
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
                reportId = id,
                dataID = indataID,
                dataFun = indataFun,
                data1 = indata1,
                data2 = indata2
            };
        }
    }
}
