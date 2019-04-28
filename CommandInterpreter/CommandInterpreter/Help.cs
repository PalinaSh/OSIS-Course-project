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
                    sb.Append($"lacalport\tPort from which messages are sent\n");
                    sb.Append($"remoteport\tPort on which messages come\n");
                    sb.Append($"name\t\tYour name in the chat\n");
                    break;
                case "color":
                    sb.Append($"background\tSpecifies color of console background\n");
                    sb.Append($"foreground\tSpecifies color of console foreground\n");
                    break;
                case "compact":
                    sb.Append($"filename\t\tPaths to files to be added to the archive\n");
                    sb.Append($"destinationDirectory\tPath to the directory which will be the archive\n");
                    break;
                case "copy":
                    sb.Append($"sourceFilename\t\tPath to file to be copied\n");
                    sb.Append($"destinatioFilename\tPath to the new file\n");
                    break;
                case "create":
                    sb.Append($"filename\tPaths to new files to be added\n");
                    break;
                case "createdir":
                    sb.Append($"dirname\t\tPaths to directories to be added\n");
                    break;
                case "decompact":
                    sb.Append($"sourceFilename\t\tPaths to .zip file to be extarct\n");
                    sb.Append($"destinationDirname\tPath to the directory in which extract files\n");
                    break;
                case "delete":
                    sb.Append($"filename\tPaths to files to be removed\n");
                    break;
                case "fc":
                    sb.Append($"ascii\t\tDisplays differences in ASCII characters\n");
                    sb.Append($"lines\t\tDisplays line numbers for differences\n");
                    sb.Append($"number\t\tCompares only the first specified number of lines in each file\n");
                    sb.Append($"case\t\tDisregards case of ASCII letters when comparing files\n");
                    sb.Append($"decimal\t\tDisplays differences in decimal format\n");
                    sb.Append($"filename1\tSpecifies location and name of first file to compare\n");
                    sb.Append($"filename2\tSpecifies location and name of second file to compare\n");
                    break;
                case "goto":
                    sb.Append($"directory\tCanges current directory\n");
                    break;
                case "help":
                    sb.Append($"command\t\tDisplays help information on that command\n");
                    break;
                case "move":
                    sb.Append($"filename\tPaths to files to be moved\n");
                    sb.Append($"dirname\t\tPath to the directory in which move files\n");
                    break;
                case "removedir":
                    sb.Append($"recursive\tRemoves all directories and files in the specified directory in addition to the directory itself\n");
                    sb.Append($"directory\tPath to the directory\n");
                    break;
                case "title":
                    sb.Append($"title\t\tSpecifies the title for the command prompt window\n");
                    break;
                case "show":
                    sb.Append($"size\t\tDisplays files' sizes\n");
                    sb.Append($"created\t\tDisplays date of file and directories created\n");
                    sb.Append($"attributes\tDisplays files with specified attributes\n");
                    sb.Append($"dirname\t\tPath to the directory\n");
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
                    sb.Append($"Local chat created using a UDP connection. Join yourself and call your friends.\n");
                    break;
                case "color":
                    sb.Append($"Console support colors:\n");
                    sb.Append($"\tblack\tblue\n");
                    sb.Append($"\tgreen\tred\n");
                    sb.Append($"\twhite\tyellow\n");
                    break;
                case "compact":
                    sb.Append($"All parameters is required. For multiple files used syntax (file1 file2 file3)\n");
                    break;
                case "copy":
                    sb.Append($"All parameters is required\n");
                    break;
                case "create":
                    sb.Append($"All parameters is required. For multiple files used syntax (file1 file2 file3)\n");
                    break;
                case "createdir":
                    sb.Append($"All parameters is required. For multiple directories used syntax (dir1 dir2 dir3)\n");
                    break;
                case "date":
                    sb.Append($"Type DATE to display the current date.\n");
                    break;
                case "decompact":
                    sb.Append($"All parameters is required. sourceFilename must ends with '.zip'\n");
                    break;
                case "delete":
                    sb.Append($"All parameters is required. For multiple files used syntax (file1 file2 file3)\n");
                    break;
                case "fc":
                    sb.Append($"To compare sets of files, use wildcards in filename1 and filename2 parameters. Files must be in text-format.\n");
                    break;
                case "goto":
                    sb.Append($"Type GOTO without parameters to display the current drive and directory.\n");
                    sb.Append($"Use the /D switch to change current drive in addition to changing current directory for a drive.\n");
                    sb.Append($"   ..    Specifies that you want to change to the parent directory.\n");
                    break;
                case "move":
                    sb.Append($"All parameters is required. For multiple files used syntax (file1 file2 file3)\n");
                    break;
                case "removedir":
                    sb.Append($"If directory is not empty and you want to delete it use 'recursive'-deleting.\n");
                    break;
                case "show":
                    sb.Append($"You can use attributes (attr1,attr2,attr3)\n");
                    sb.Append($"\tD  Directories\n");
                    sb.Append($"\tH  Hidden files and directories\n");
                    sb.Append($"\tR  Readonly files and directories\n");
                    sb.Append($"\tS  System files and directories\n");
                    break;
            }

            return sb.ToString();
        }
    }
}
