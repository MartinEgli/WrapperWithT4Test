using System;

namespace TestLibrary
{
    public class Class : AbstractClass , Interface1
    {

        public class NestetClass
        {
            public void Method1()
            {
                throw new NotImplementedException();
            }

            public void Method2(string str)
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler Event1;

        public static void StaticMethod1()
        {
        }

        public string pubicField1 = "";
        public const string PublicConst1 = "";

        public delegate void publicDelegate1();
        public delegate void publicDelegate2(string s);
        public delegate string publicDelegate3(string s);

        protected string protectedField1 = "";
        protected const string ProtectedConst1 = "";


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

        public void PublicDelegateMethod1(publicDelegate1 del)
        {
            throw new NotImplementedException();
        }

        public publicDelegate1 PublicDelegateMethod2()
        {
            throw new NotImplementedException();
        }

        public publicDelegate1 PublicDelegateMethod3(publicDelegate2 del)
        {
            throw new NotImplementedException();
        }

        public publicDelegate1 PublicDelegateMethod4(publicDelegate2 del1, publicDelegate3 del2)
        {
            throw new NotImplementedException();
        }

        public string ReadWrite { get; set; }
        public string Read { get; }

        public string this[int i, int j] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string this[int i] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}