using System;
using System.Security.Cryptography;

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

  

    public abstract class AbstractClass
    {
    }

    public class Class : AbstractClass , Interface1
    {
        /// <summary>
        /// Method1s this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Method1()
        {
            throw new NotImplementedException();
        }

        public void Method2(string str)
        {
            throw new NotImplementedException();
        }

        public void Method3(string str)
        {
            throw new NotImplementedException();
        }
        public void Method4(ref string str)
        {
            throw new NotImplementedException();
        }

        public void Method5([Attribute]out string str)
        {
            throw new NotImplementedException();
        }

        public void Method6(string str, bool isBool = true)
        {
            throw new NotImplementedException();
        }

        public void Method7(string str, bool isBool = true)
        {
            throw new NotImplementedException();
        }

        public string ReadWrite { get; set; }
        public string Read { get; }

        public string this[int i, int j] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string this[int i] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public sealed class SealedClass : Class{}
}
