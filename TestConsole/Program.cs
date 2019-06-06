using System;
using System.Linq;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
        }

        readonly struct VectorStruct
        {
            public int X { get; }
            public int Y { get; }
        }

        private void Test()
        {
            Span<VectorStruct> vectors = stackalloc VectorStruct[5];

            foreach(var v in vectors)
            {
            }
        }
    }



}
