using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInterpreter
{
    class Help
    {
        private static List<string> _commands = new List<string>() {"chat", "color", "copy", "clear", "compact", "create",
            "createdir", "date", "exit", "decompact", "delete", "fc", "goto", "help", "move", "removedir", "title", "show",
            "whoami", "lang"};

        public static void Helper()
        {
            Console.WriteLine($"{Properties.Resources.ForMoreInfo}\n");
            Console.WriteLine(GetCommandsSmallDescription(_commands, true));
        }

        public static void HelperComands(string command)
        {
            Console.WriteLine(GetCommandsSmallDescription(new List<string> { command }));
            Console.WriteLine($"{GetSyntax(command)}\n");
            Console.WriteLine($"{GetAttributes(command)}");
            Console.Write($"{GetCommandDescription(command)}");
        }

        private static string GetCommandsSmallDescription(List<string> commands, bool name = false)
        {
            StringBuilder sb = new StringBuilder();
            if (commands.Contains("chat"))
            {
                if (name)
                    sb.Append($"CHAT\t\t");
                sb.Append($"{Properties.Resources.Chat}\n");
            }
            if (commands.Contains("clear"))
            {
                if (name)
                    sb.Append($"CLEAR\t\t");
                sb.Append($"{Properties.Resources.Clear}\n");
            }
            if (commands.Contains("color"))
            {
                if (name)
                    sb.Append($"COLOR\t\t");
                sb.Append($"{Properties.Resources.Color}\n");
            }
            if (commands.Contains("compact"))
            {
                if (name)
                    sb.Append($"COMPACT\t\t");
                sb.Append($"{Properties.Resources.Compact}\n");
            }
            if (commands.Contains("copy"))
            {
                if (name)
                    sb.Append($"COPY\t\t");
                sb.Append($"{Properties.Resources.Copy}\n");
            }
            if (commands.Contains("create"))
            {
                if (name)
                    sb.Append($"CREATE\t\t");
                sb.Append($"{Properties.Resources.Create}\n");
            }
            if (commands.Contains("createdir"))
            {
                if (name)
                    sb.Append($"CREATEDIR\t");
                sb.Append($"{Properties.Resources.Createdir}\n");
            }
            if (commands.Contains("date"))
            {
                if (name)
                    sb.Append($"DATE\t\t");
                sb.Append($"{Properties.Resources.Date}\n");
            }
            if (commands.Contains("decompact"))
            {
                if (name)
                    sb.Append($"DECOMPACT\t");
                sb.Append($"{Properties.Resources.Decompact}\n");
            }
            if (commands.Contains("delete"))
            {
                if (name)
                    sb.Append($"DELETE\t\t");
                sb.Append($"{Properties.Resources.Delete}\n");
            }
            if (commands.Contains("exit"))
            {
                if (name)
                    sb.Append($"EXIT\t\t");
                sb.Append($"{Properties.Resources.Exit}\n");
            }
            if (commands.Contains("fc"))
            {
                if (name)
                    sb.Append($"FC\t\t");
                sb.Append($"{Properties.Resources.Fc}\n");
            }
            if (commands.Contains("goto"))
            {
                if (name)
                    sb.Append($"GOTO\t\t");
                sb.Append($"{Properties.Resources.Goto}\n");
            }
            if (commands.Contains("help"))
            {
                if (name)
                    sb.Append($"HELP\t\t");
                sb.Append($"{Properties.Resources.Help}\n");
            }
            if (commands.Contains("lang"))
            {
                if (name)
                    sb.Append($"LANG\t\t");
                sb.Append($"{Properties.Resources.Lang}\n");
            }
            if (commands.Contains("move"))
            {
                if (name)
                    sb.Append($"MOVE\t\t");
                sb.Append($"{Properties.Resources.Move}\n");
            }
            if (commands.Contains("removedir"))
            {
                if (name)
                    sb.Append($"REMOVEDIR\t");
                sb.Append($"{Properties.Resources.Removedir}\n");
            }
            if (commands.Contains("show"))
            {
                if (name)
                    sb.Append($"SHOW\t\t");
                sb.Append($"{Properties.Resources.Show}\n");
            }
            if (commands.Contains("title"))
            {
                if (name)
                    sb.Append($"TITLE\t\t");
                sb.Append($"{Properties.Resources.Title}\n");
            }
            if (commands.Contains("whoami"))
            {
                if (name)
                    sb.Append($"WHOAMI\t\t");
                sb.Append($"{Properties.Resources.Whoami}\n");
            }

            return sb.ToString();
        }

        private static string GetSyntax(string command)
        {
            switch(command.ToLower().Trim())
            {
                case "chat":
                    return $"CHAT [-l|local|localport]=localPort [-r|remote|remoteport]=remotePort [[-n|name]=name]";
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
            StringBuilder sb = new StringBuilder();
            switch (command.ToLower().Trim())
            {
                case "chat":
                    sb.Append($"{Properties.Resources.ChatDescr}\n");
                    break;
                case "color":
                    sb.Append($"{Properties.Resources.ColorDescr}\n");
                    sb.Append($"\tblack\tblue\n");
                    sb.Append($"\tgreen\tred\n");
                    sb.Append($"\twhite\tyellow\n");
                    break;
                case "compact":
                    sb.Append($"{Properties.Resources.CompactDescr} (file1 file2 file3)\n");
                    break;
                case "copy":
                    sb.Append($"{Properties.Resources.CopyDescr}\n");
                    break;
                case "create":
                    sb.Append($"{Properties.Resources.CompactDescr} (file1 file2 file3)\n");
                    break;
                case "createdir":
                    sb.Append($"{Properties.Resources.CompactDescr} (dir1 dir2 dir3)\n");
                    break;
                case "date":
                    sb.Append($"{Properties.Resources.DateDescr}\n");
                    break;
                case "decompact":
                    sb.Append($"{Properties.Resources.DecompactDescr}\n");
                    break;
                case "delete":
                    sb.Append($"{Properties.Resources.CompactDescr} (file1 file2 file3)\n");
                    break;
                case "fc":
                    sb.Append($"{Properties.Resources.FcDescr}\n");
                    break;
                case "goto":
                    sb.Append($"{Properties.Resources.GotoDescr}\n");
                    break;
                case "move":
                    sb.Append($"{Properties.Resources.CompactDescr} (file1 file2 file3)\n");
                    break;
                case "removedir":
                    sb.Append($"{Properties.Resources.RemovedirDescr}\n");
                    break;
                case "show":
                    sb.Append($"{Properties.Resources.ShowDescr}\n");
                    break;
            }

            return sb.ToString();
        }
    }
}
