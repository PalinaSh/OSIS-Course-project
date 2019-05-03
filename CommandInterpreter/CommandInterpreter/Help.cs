using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInterpreter
{
    static class Help
    {
        private static Dictionary<string, string> _commands = new Dictionary<string, string>
        {
            { "chat", Properties.Resources.Chat },
            { "color" , Properties.Resources.Color},
            { "copy",Properties.Resources.Copy },
            { "clear", Properties.Resources.Clear },
            { "compact" , Properties.Resources.Compact},
            { "create", Properties.Resources.Create },
            { "createdir", Properties.Resources.Createdir },
            { "date", Properties.Resources.Date },
            { "exit", Properties.Resources.Exit },
            { "decompact", Properties.Resources.Decompact },
            { "delete", Properties.Resources.Delete },
            { "fc", Properties.Resources.Fc },
            { "goto", Properties.Resources.Goto },
            { "help", Properties.Resources.Help },
            { "move", Properties.Resources.Move },
            { "removedir", Properties.Resources.Removedir },
            { "title", Properties.Resources.Title },
            { "show", Properties.Resources.Show },
            { "whoami", Properties.Resources.Whoami },
            { "lang", Properties.Resources.Lang }
        };

        private static Dictionary<string, string> _commandsDescr = new Dictionary<string, string>
        {
            { "chat", $"{Properties.Resources.ChatDescr}\n" },
            { "color", $"{Properties.Resources.ColorDescr}\n\tblack\tblue\n\tgreen\tred\n\twhite\tyellow\n"},
            { "compact", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
            { "copy", $"{Properties.Resources.CopyDescr}\n" },
            { "create", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
            { "createdir", $"{Properties.Resources.CompactDescr} (dir1 dir2 dir3)\n" },
            { "date", $"{Properties.Resources.DateDescr}\n" },
            { "decompact", $"{Properties.Resources.DecompactDescr}\n" },
            { "delete", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
            { "fc", $"{Properties.Resources.FcDescr}\n" },
            { "goto", $"{Properties.Resources.GotoDescr}\n{Properties.Resources.ChangeDrive}\n{Properties.Resources.Parrent}\n" },
            { "move", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
            { "removedir", $"{Properties.Resources.RemovedirDescr}\n" },
            { "show", $"{Properties.Resources.ShowDescr}\n\tD  {Properties.Resources.Directories}\n\tH  {Properties.Resources.Hidden}\n\tR  {Properties.Resources.Readonly}\n\tS  {Properties.Resources.System}\n" }
        };

        private static List<string> _commandsWithoutAttr = new List<string> { "clear", "date", "whoami", "lang", "exit" };
        private static List<string> _bigCommands = new List<string> { "createdir", "decompact", "removedir" };

        public static void Helper()
        {
            Console.WriteLine($"{Properties.Resources.ForMoreInfo}{Environment.NewLine}");
            Console.WriteLine(GetCommandsSmallDescription(_commands, true));
        }

        public static void HelperComands(string command)
        {
            string str = string.Empty;

            str += GetCommandsSmallDescription(_commands.Where(s => s.Key == command)
                .ToDictionary(s => s.Key, s => s.Value)) + Environment.NewLine;
            str += GetSyntax(command) + Environment.NewLine + 
                (_commandsWithoutAttr.Contains(command) ? "" : Environment.NewLine);
            str += GetAttributes(command) + Environment.NewLine;
            str += GetCommandDescription(command);

            Console.Write(str);
        }

        public static void UpdateDictionary()
        {
            _commands = new Dictionary<string, string>
            {
                { "chat", Properties.Resources.Chat },
                { "color" , Properties.Resources.Color},
                { "copy",Properties.Resources.Copy },
                { "clear", Properties.Resources.Clear },
                { "compact" , Properties.Resources.Compact},
                { "create", Properties.Resources.Create },
                { "createdir", Properties.Resources.Createdir },
                { "date", Properties.Resources.Date },
                { "exit", Properties.Resources.Exit },
                { "decompact", Properties.Resources.Decompact },
                { "delete", Properties.Resources.Delete },
                { "fc", Properties.Resources.Fc },
                { "goto", Properties.Resources.Goto },
                { "help", Properties.Resources.Help },
                { "move", Properties.Resources.Move },
                { "removedir", Properties.Resources.Removedir },
                { "title", Properties.Resources.Title },
                { "show", Properties.Resources.Show },
                { "whoami", Properties.Resources.Whoami },
                { "lang", Properties.Resources.Lang }
            };

            _commandsDescr = new Dictionary<string, string>
            {
                { "chat", $"{Properties.Resources.ChatDescr}\n" },
                { "color", $"{Properties.Resources.ColorDescr}\n\tblack\tblue\n\tgreen\tred\n\twhite\tyellow\n"},
                { "compact", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
                { "copy", $"{Properties.Resources.CopyDescr}\n" },
                { "create", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
                { "createdir", $"{Properties.Resources.CompactDescr} (dir1 dir2 dir3)\n" },
                { "date", $"{Properties.Resources.DateDescr}\n" },
                { "decompact", $"{Properties.Resources.DecompactDescr}\n" },
                { "delete", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
                { "fc", $"{Properties.Resources.FcDescr}\n" },
                { "goto", $"{Properties.Resources.GotoDescr}\n{Properties.Resources.ChangeDrive}\n{Properties.Resources.Parrent}\n" },
                { "move", $"{Properties.Resources.CompactDescr} (file1 file2 file3)\n" },
                { "removedir", $"{Properties.Resources.RemovedirDescr}\n" },
                { "show", $"{Properties.Resources.ShowDescr}\n\tD  {Properties.Resources.Directories}\n\tH  {Properties.Resources.Hidden}\n\tR  {Properties.Resources.Readonly}\n\tS  {Properties.Resources.System}\n" }
            };
        }

        private static string GetCommandsSmallDescription(Dictionary<string, string> commands, bool name = false)
        {
            string s = string.Empty;

            foreach (var command in commands)
            {
                if (name)
                {
                    s += $"{command.Key.ToUpper()}";
                    s += (_bigCommands.Contains(command.Key)) ? "\t" : "\t\t";
                }
                s += $"{command.Value}{Environment.NewLine}";
            }

            return s;
        }

        private static string GetSyntax(string command)
        {
            switch(command.ToLower().Trim())
            {
                case "chat":
                    return $"CHAT [-l|local|localport]=localPort [-r|remote|remoteport]=remotePort [[-n|name]=name]";
                case "clear":
                    return "CLEAR";
                case "color":
                    return $"COLOR [[-b|background]=background] [[-f|foreground]=foreground]";
                case "compact":
                    return $"COMPACT filename [...] destinationDirectory";
                case "copy":
                    return $"COPY sourceFilename destinationFilename";
                case "create":
                    return $"CREATE filename [...]";
                case "createdir":
                    return $"CREATEDIR dirname [...]";
                case "date":
                    return $"DATE";
                case "decompact":
                    return $"DECOMPACT sourceFilename destinationDirname";
                case "delete":
                    return $"DELETE filename [...]";
                case "fc":
                    return $"FC [-a|ascii] [-l|lines] [[-n|number]=number] [-c|case] [-d|decimal] filename1 filename2";
                case "goto":
                    return $"GOTO directory";
                case "help":
                    return $"HELP [command]";
                case "lang":
                    return "LANG";
                case "move":
                    return $"MOVE filename [...] dirname";
                case "removedir":
                    return $"REMOVEDIR [-r|recurseve] directory";
                case "title":
                    return $"TITLE title";
                case "show":
                    return $"SHOW [-s|size] [-c|create|created] [[-a|attributes]=attributes[,]] [dirname]";
                case "whoami":
                    return $"WHOAMI";
                case "exit":
                    return $"EXIT";
                default:
                    return (new StringBuilder()).ToString();
            }
        }

        private static string GetAttributes(string command)
        {
            StringBuilder sb = new StringBuilder();
            switch (command.Trim().ToLower())
            {
                case "chat":
                    sb.Append($"lacalport\t{Properties.Resources.ChatLocalport}\n");
                    sb.Append($"remoteport\t{Properties.Resources.ChatRemoteport}\n");
                    sb.Append($"name\t\t{Properties.Resources.ChatName}\n");
                    break;
                case "color":
                    sb.Append($"background\t{Properties.Resources.ColorBackground}\n");
                    sb.Append($"foreground\t{Properties.Resources.ColorForeground}\n");
                    break;
                case "compact":
                    sb.Append($"filename\t\t{Properties.Resources.CompactFilename}\n");
                    sb.Append($"destinationDirectory\t{Properties.Resources.CompactDirectory}\n");
                    break;
                case "copy":
                    sb.Append($"sourceFilename\t\t{Properties.Resources.CopySourceFilename}\n");
                    sb.Append($"destinatioFilename\t{Properties.Resources.CopyDstFilename}\n");
                    break;
                case "create":
                    sb.Append($"filename\t{Properties.Resources.CreateFilename}\n");
                    break;
                case "createdir":
                    sb.Append($"dirname\t\t{Properties.Resources.CreateDirname}\n");
                    break;
                case "decompact":
                    sb.Append($"sourceFilename\t\t{Properties.Resources.DecompactFilename}\n");
                    sb.Append($"destinationDirname\t{Properties.Resources.DecompactDirname}\n");
                    break;
                case "delete":
                    sb.Append($"filename\t{Properties.Resources.DeleteFilename}\n");
                    break;
                case "fc":
                    sb.Append($"ascii\t\t{Properties.Resources.FcAscii}\n");
                    sb.Append($"lines\t\t{Properties.Resources.FcLine}\n");
                    sb.Append($"number\t\t{Properties.Resources.FcNumber}\n");
                    sb.Append($"case\t\t{Properties.Resources.FcCase}\n");
                    sb.Append($"decimal\t\t{Properties.Resources.FcDecimal}\n");
                    sb.Append($"filename1\t{Properties.Resources.FcFirst}\n");
                    sb.Append($"filename2\t{Properties.Resources.FcSecond}\n");
                    break;
                case "goto":
                    sb.Append($"directory\t{Properties.Resources.GotoDirectory}\n");
                    break;
                case "help":
                    sb.Append($"command\t\t{Properties.Resources.HelpCommand}\n");
                    break;
                case "move":
                    sb.Append($"filename\t{Properties.Resources.MoveFilename}\n");
                    sb.Append($"dirname\t\t{Properties.Resources.MoveDirname}\n");
                    break;
                case "removedir":
                    sb.Append($"recursive\t{Properties.Resources.RemovedirRecursive}\n");
                    sb.Append($"directory\t{Properties.Resources.RemoveDirname}\n");
                    break;
                case "title":
                    sb.Append($"title\t\t{Properties.Resources.TitleTitle}\n");
                    break;
                case "show":
                    sb.Append($"size\t\t{Properties.Resources.ShowSize}\n");
                    sb.Append($"created\t\t{Properties.Resources.ShowCreated}\n");
                    sb.Append($"attributes\t{Properties.Resources.ShowAttributes}\n");
                    sb.Append($"dirname\t\t{Properties.Resources.ShowDirname}\n");
                    break;
                default:
                    return "";
            }

            return sb.ToString();
        }

        private static string GetCommandDescription(string command)
        {
            if (_commandsDescr.Where(c => c.Key == command).Count() == 1)
                return _commandsDescr.Where(c => c.Key == command).First().Value;

            return "";
        }
    }
}
