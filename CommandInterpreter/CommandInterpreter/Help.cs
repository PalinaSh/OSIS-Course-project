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
            "whoami"};

        public static void Helper()
        {
            Console.WriteLine("For more information on a specific command, type HELP command-name\n");
            Console.WriteLine(GetCommandsDescription(_commands, true));
        }

        public static void HelperComands(string command)
        {
            Console.WriteLine(GetCommandsDescription(new List<string> { command }));
            Console.WriteLine($"{GetSyntax(command)}\n");
        }

        private static string GetCommandsDescription(List<string> commands, bool name = false)
        {
            StringBuilder sb = new StringBuilder();
            if (commands.Contains("chat"))
            {
                if (name)
                    sb.Append($"CHAT\t\t");
                sb.Append($"Creates UDP-chat for your secure communication\n");
            }
            if (commands.Contains("clear"))
            {
                if (name)
                    sb.Append($"CLEAR\t\t");
                sb.Append($"Clears the screen\n");
            }
            if (commands.Contains("color"))
            {
                if (name)
                    sb.Append($"COLOR\t\t");
                sb.Append($"Sets the default console foreground and background colors\n");
            }
            if (commands.Contains("compact"))
            {
                if (name)
                    sb.Append($"COMPACT\t\t");
                sb.Append($"Creates .zip archive of files\n");
            }
            if (commands.Contains("copy"))
            {
                if (name)
                    sb.Append($"COPY\t\t");
                sb.Append($"Copies one file to another location\n");
            }
            if (commands.Contains("create"))
            {
                if (name)
                    sb.Append($"CREATE\t\t");
                sb.Append($"Creates files\n");
            }
            if (commands.Contains("createdir"))
            {
                if (name)
                    sb.Append($"CREATEDIR\t");
                sb.Append($"Creates directories\n");
            }
            if (commands.Contains("date"))
            {
                if (name)
                    sb.Append($"DATE\t\t");
                sb.Append($"Displays the date\n");
            }
            if (commands.Contains("decompact"))
            {
                if (name)
                    sb.Append($"DECOMPACT\t");
                sb.Append($"Extract .zip file to directory\n");
            }
            if (commands.Contains("delete"))
            {
                if (name)
                    sb.Append($"DELETE\t\t");
                sb.Append($"Deletes one or more files\n");
            }
            if (commands.Contains("exit"))
            {
                if (name)
                    sb.Append($"EXIT\t\t");
                sb.Append($"Quits the CommandInterpreter.EXE program (or chat)\n");
            }
            if (commands.Contains("fc"))
            {
                if (name)
                    sb.Append($"FC\t\t");
                sb.Append("Compares the contents of two files\n");
            }
            if (commands.Contains("goto"))
            {
                if (name)
                    sb.Append($"GOTO\t\t");
                sb.Append($"Displays the name of or changes the current directory\n");
            }
            if (commands.Contains("help"))
            {
                if (name)
                    sb.Append($"HELP\t\t");
                sb.Append($"Provides Help information for Windows commands\n");
            }
            if (commands.Contains("move"))
            {
                if (name)
                    sb.Append($"MOVE\t\t");
                sb.Append($"Moves one or more files from one directory to another directory\n");
            }
            if (commands.Contains("removedir"))
            {
                if (name)
                    sb.Append($"REMOVEDIR\t");
                sb.Append($"Removes a directory\n");
            }
            if (commands.Contains("show"))
            {
                if (name)
                    sb.Append($"SHOW\t\t");
                sb.Append($"Displays a list of files and subdirectories in a directory\n");
            }
            if (commands.Contains("title"))
            {
                if (name)
                    sb.Append($"TITLE\t\t");
                sb.Append($"Sets the window title for a CommandInterpreter.EXE session\n");
            }
            if (commands.Contains("whoami"))
            {
                if (name)
                    sb.Append($"WHOAMI\t\t");
                sb.Append("Displays current user's name\n");
            }

            return sb.ToString();
        }

        private static string GetSyntax(string command)
        {
            switch(command.ToLower().Trim())
            {
                case "chat":
                    return $"CHAT -l=localPort -r=remotePort [-n=name]";
                case "color":
                    return $"COLOR [-b=background] [-f=foreground]";
                case "clear":
                    return $"CLEAR";
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
                    return $"FC [-a] [-l] [-n=number] [-c] [-d] filename1 filename2";
                case "goto":
                    return $"GOTO directory";
                case "help":
                    return $"HELP [command]";
                case "move":
                    return $"MOVE filename [...] dirname";
                case "removedir":
                    return $"REMOVEDIR [-r] directory";
                case "title":
                    return $"TITLE title";
                case "show":
                    return $"SHOW [-s] [-c] [-a=attributes[,]] [dirname]";
                case "whoami":
                    return $"WHOAMI";
                case "exit":
                    return $"EXIT";
                default:
                    if (!string.IsNullOrEmpty(command))
                    Console.WriteLine($"'{command}' is not recognized as an internal or external command, operable program or batch file.");
                    return "";
            }
        }
    }
}
