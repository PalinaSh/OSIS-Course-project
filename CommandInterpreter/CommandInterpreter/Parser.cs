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
                case "chat":
                    ParseChat(args);
                    break;
                case "color":
                    ParseColor(args);
                    break;
                case "clear":
                    _commands.Clear();
                    break;
                case "compact":
                    ParseCompact(args);
                    break;
                case "copy":
                    ParseCopy(args);
                    break;
                case "create":
                    ParseCreate(args);
                    break;
                case "createdir":
                    ParseCreateDir(args);
                    break;
                case "date":
                    _commands.Date();
                    break;
                case "decompact":
                    ParseDecompact(args);
                    break;
                case "delete":
                    ParseDelete(args);
                    break;
                case "fc":
                    ParseFileCompare(args);
                    break;
                case "goto":
                    ParseGoto(args);
                    break;
                case "move":
                    ParseMove(args);
                    break;
                case "removedir":
                    ParseRemoveDir(args);
                    break;
                case "title":
                    ParseTitle(args);
                    break;
                case "show":
                    ParseShow(args);
                    break;
                case "whoami":
                    _commands.WhoAmI();
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
            if (string.IsNullOrEmpty(args[0]))
                return "";
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
            string path1 = "", path2 = "";
            Dictionary<string, long> options = new Dictionary<string, long>() { { "a", 0 }, { "l", 0 }, { "n", -1 }, { "c", 0 }, { "d", 0 } };

            if (args.Length == 0)
            {
                Console.Write("Name of first file to compare: ");
                path1 = GetPath(new string[] { Console.ReadLine() });
            }

            if (args.Length == 1 || args.Length == 0)
            {
                Console.Write("Name of second file to compare: ");
                path2 = GetPath(new string[] { Console.ReadLine() });
                if (args.Length != 0)
                    path1 = GetPath(new string[] { args[0] });
            }

            if (args.Length == 2)
            {
                path1 = GetPath(new string[] { args[0] });
                path2 = GetPath(new string[] { args[1] });
            }

            if (args.Length > 2)
            {
                Console.WriteLine("Bad command line syntax");
                return;
            }

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

        private void ParseCopy(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("The file cannot be copied onto itself.\n\t0 file(s) copied.");
                return;
            }

            if (args.Length > 2 || args.Length < 1)
            {
                Console.WriteLine("The syntax of the command is incorrect.");
                return;
            }

            _commands.Copy(GetPath(new string[] {args[0]}), GetPath(new string[] {args[1]}));
        }

        private List<string> ParseRemovePaths(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("The syntax of the command is incorrect.");
                return null;
            }

            var paths = new List<string>();
            foreach (var path in args)
                paths.Add(GetPath(new string[] { path }));

            return paths;
        }

        private void ParseDelete(string[] args)
        {
            var paths = ParseRemovePaths(args);

            if (paths == null)
                return;

            _commands.Delete(paths);
        }

        private void ParseRemoveDir(string[] args)
        {
            bool recursive = false;
            var p = new OptionSet()
            {
                {"r|recursive", v => recursive = v != null },
            };
            p.Parse(new List<string> { args[0] });

            var paths = new List<string>();
            if (!recursive)
                paths = ParseRemovePaths(args);
            else
                paths = ParseRemovePaths(args.Skip(1).Take(args.Length - 1).ToArray());

            if (paths == null)
                return;

            _commands.RemoveDir(paths, recursive);
        }

        private void ParseMove(string[] args)
        {
            if (args.Length != 2)
                Console.WriteLine("The syntax of the command is incorrect.");

            List<string> paths = new List<string>();
            foreach (var path in args)
                paths.Add(GetPath(new string[] { path }));

            var srcPaths = paths.Take(paths.Count - 1);
            var dstPath = paths.Last();

            _commands.Move(srcPaths.ToArray(), dstPath);
        }

        private void ParseShow(string[] args)
        {
            string path = "";
            if (args.Length == 0)
                path = _commands.CurrentFolder;
            else
                if (args[0].StartsWith("/") || args[0].StartsWith("-"))
                    path = _commands.CurrentFolder;
                else
                    path = GetPath(new string[] { args[0] });

            var options = new Dictionary<string, bool> { { "s", false }, { "c", false } };
            var attributes = "";
            var p = new OptionSet()
            {
                { "s|size", v => options["s"] = v != null },
                { "c|create|created", v => options["c"] = v != null },
                { "a|attributes=", v => attributes = v }
            };
            p.Parse(args);

            List<string> attr = attributes.Split('&').ToList();

            _commands.Show(path, options, attr);
        }

        private void ParseCreate(string[] args)
        {
            if (args.Length == 0)
                Console.WriteLine("The syntax of the command is incorrect.");

            var paths = new List<string>();
            foreach(var arg in args)
                paths.Add(GetPath(new string[] { arg }));

            _commands.Create(paths.ToArray());
        }

        private void ParseCreateDir(string[] args)
        {
            if (args.Length == 0)
                Console.WriteLine("The syntax of the command is incorrect.");

            var paths = new List<string>();
            foreach (var arg in args)
                paths.Add(GetPath(new string[] { arg }));

            _commands.CreateDir(paths.ToArray());
        }

        private void ParseChat(string[] args)
        {
            string name = Environment.UserName;
            int localPort = -1, remotePort = -1;

            var p = new OptionSet()
            {
                { "n|name=", v => name = v },
                { "l|local|localport=", v => localPort = int.Parse(v) },
                { "r|remote|remoteport=", v => remotePort = int.Parse(v) }
            };

            try
            {
                p.Parse(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Port is too big");
                return;
            }

            if (localPort == -1)
            {
                Console.WriteLine("Local port is required argument");
                return;
            }
            if (remotePort == -1)
            {
                Console.WriteLine("Remote port is required argument");
                return;
            }

            if (localPort < 0 || localPort > 65535)
            {
                Console.WriteLine("Local port is incorrect");
                return;
            }
            if (remotePort < 0 || remotePort > 65535)
            {
                Console.WriteLine("Remote port is incorrect");
                return;
            }

            _commands.Chat(name, localPort, remotePort);
        }

        private void ParseCompact(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("The syntax of the command is incorrect.");
                return;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Name of destination directory is required");
                return;
            }

            var srcPaths = new List<string>();
            foreach (var arg in args.Take(args.Length - 1))
                srcPaths.Add(GetPath(new string[] { arg }));

            var dstPath = GetPath(new string[] { args.Last() });

            _commands.Compact(srcPaths.ToArray(), dstPath);
        }

        private void ParseDecompact(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("The syntax of the command is incorrect.");
                return;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Name of destination directory is required");
                return;
            }

            var srcPath = GetPath(new string[] { args.First() });
            var dstPath = GetPath(new string[] { args.Last() });

            _commands.Decompact(srcPath, dstPath);
        }
    }
}
