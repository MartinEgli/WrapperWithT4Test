using System;

namespace TestLibrary
{
    [Flags, Serializable]
    public enum Enum
    {
        [Obsolete("Hello")]
        Enum1,
        Enum2,
        Enum3,
        EnumX = 99
    }
}