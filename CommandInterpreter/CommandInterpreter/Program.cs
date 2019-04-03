using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInterpreter
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.GetEncoding(1251);
            Console.Title = "Pretty command";

            bool exit = false;
            var commands = new Commands(Environment.CurrentDirectory);
            var parser = new Parser(commands);

            while (!exit)
            {
                Console.Write(commands.CurrentFolder + "> ");

                var commandargs = Console.ReadLine().Split(' ');

                var command = commandargs[0].ToString();
                List<string> args = new List<string>();
                for (int i = 1; i < commandargs.Length; ++i)
                    args.Add(commandargs[i].ToString());

                exit = parser.Parse(command, args.ToArray());
                Console.WriteLine();
            }
            Environment.Exit(0);
        }
    }
}
