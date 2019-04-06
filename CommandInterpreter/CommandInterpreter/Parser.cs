using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk.Options;

namespace CommandInterpreter
{
    class Parser
    {
        private Commands _commands;
        public Parser(Commands commands)
        {
            _commands = commands;
        }

        public bool Parse(string command, string[] args)
        {
            bool exit = false;
            switch (command.ToLower().Trim())
            {
                case "color":
                    ParseColor(args);
                    break;
                case "clear":
                    _commands.Clear();
                    break;
                case "fc":
                    ParseFileCompare(args);
                    break;
                case "goto":
                    ParseGoto(args);
                    break;
                case "title":
                    ParseTitle(args);
                    break;
                case "exit":
                    exit = true;
                    break;
                default:
                    if (!string.IsNullOrEmpty(command))
                        Console.WriteLine($"'{command}' is not recognized as an internal or external command, operable program or batch file.");
                    break;
            }
            return exit;
        }

        private void ParseGoto(string[] args)
        {
            var path = GetPath(args);
            _commands.Goto(path);
        }

        private string GetPath(string[] args)
        {
            string path;
            if (args.Length == 0)
                path = _commands.CurrentFolder;
            else if (args[0].Contains(":") || args[0][0] == '\\')
                path = string.Join(" ", args);
            else
                path = Path.Combine(_commands.CurrentFolder, string.Join(" ", args));
            return Path.GetFullPath(path);
        }

        private void ParseColor(string[] args)
        {
            string background = null, foreground = null;
            var p = new OptionSet()
            {
                { "b|background=", v => background = v },
                { "f|foreground=", v => foreground = v }
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try --help for more information");
                return;
            }

            ConsoleColor backgroundColor = ConsoleColor.Black, foregroundColor = ConsoleColor.White;
            bool isBackground = true, isForeground = true;
            if (background != null) 
                switch (background.ToLower().Trim())
                {
                    case null:
                        break;
                    case "red":
                        backgroundColor = ConsoleColor.Red;
                        break;
                    case "black":
                        backgroundColor = ConsoleColor.Black;
                        break;
                    case "white":
                        backgroundColor = ConsoleColor.White;
                        break;
                    case "yellow":
                        backgroundColor = ConsoleColor.Yellow;
                        break;
                    case "green":
                        backgroundColor = ConsoleColor.Green;
                        break;
                    case "blue":
                        backgroundColor = ConsoleColor.Blue;
                        break;
                    default:
                        isBackground = false;
                        break;
                }

            if (foreground != null)
                switch (foreground.ToLower().Trim())
                {
                    case null:
                        break;
                    case "red":
                        foregroundColor = ConsoleColor.Red;
                        break;
                    case "black":
                        foregroundColor = ConsoleColor.Black;
                        break;
                    case "white":
                        foregroundColor = ConsoleColor.White;
                        break;
                    case "yellow":
                        foregroundColor = ConsoleColor.Yellow;
                        break;
                    case "green":
                        foregroundColor = ConsoleColor.Green;
                        break;
                    case "blue":
                        foregroundColor = ConsoleColor.Blue;
                        break;
                    default:
                        isForeground = false;
                        break;
                }

            if (isBackground && isForeground && background != null && foreground != null)
                _commands.Color(backgroundColor, foregroundColor);
            else if (background == null && isForeground && foreground != null)
                _commands.Color(Console.BackgroundColor, foregroundColor);
            else if (foreground == null && isBackground && background != null)
                _commands.Color(backgroundColor, Console.ForegroundColor);
            else
                Console.WriteLine("Colors is not valid");
            
        }

        private void ParseFileCompare(string[] args)
        {
            string path1 = GetPath(new string[] { args[0] }), path2 = GetPath(new string[] { args[1] });
            Dictionary<string, long> options = new Dictionary<string, long>() { { "a", 0 }, { "l", 0 }, { "n", -1 }, { "c", 0 }, { "d", 0 } };

            var p = new OptionSet()
            {
                { "a|ascii", v => options["a"] = v != null ? 1 : 0 },
                { "l|lines", v => options["l"] = v != null ? 1 : 0 },
                { "n|number=", v => options["n"] = long.Parse(v) },
                { "c|case", v => options["c"] = v != null ? 1 : 0 },
                { "d|decimal" , v => options["d"] = v != null ? 1 : 0},
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try --help for more information");
                return;
            }

            _commands.FileCompare(path1, path2, options);
        }

        private void ParseTitle(string[] args)
        {
            var title = string.Join(" ", args);
            _commands.Title(title);
        }
    }
}
