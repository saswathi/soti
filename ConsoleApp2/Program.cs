using System;

namespace Soti_Dict
{
    class Program
    {
        static void Main(string[] args)
        {
            WordTransform cWT = new WordTransform(args[0], args[1]);
            if (args[0].Length == args[1].Length)
            {
                cWT.doTransform();
            }
            else
            {
                Console.WriteLine("Strings' length are not same");
            }
        }
    }
}
