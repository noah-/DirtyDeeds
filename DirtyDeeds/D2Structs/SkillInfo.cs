﻿using System.Runtime.InteropServices;

namespace DD.D2Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SkillInfo
    {
        public ushort wSkillId;         // 0x00
    }
}
