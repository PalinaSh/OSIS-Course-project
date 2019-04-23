using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandInterpreter
{
    class Commands
    {
        public string CurrentFolder { get; private set; }
        private string[] _textExtensions = { ".txt", ".pdf", ".doc", ".docx" };

        public Commands(string currentFolder)
        {
            CurrentFolder = currentFolder;
        }

        public void Goto(string path)
        {
            if (Directory.Exists(path))
            {
                if (path == CurrentFolder)
                    Console.WriteLine(CurrentFolder);
                CurrentFolder = path;
            }
            else if (File.Exists(path))
                Console.WriteLine("The directory name is invalid.");
            else
                Console.WriteLine("The system cannot find the path specified.");
        }

        public void Color(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Clear();
        }

        public void FileCompare(string path1, string path2, Dictionary<string, long> options)
        {
            Console.WriteLine($"Comparing {path1} and {path2}");
            if (!File.Exists(path1))
            {
                Console.WriteLine($"Can't find/open file: {path1}");
                return;
            }
            if (!File.Exists(path2))
            {
                Console.WriteLine($"Can't find/open file: {path2}");
                return;
            }

            FileInfo file1 = new FileInfo(path1), file2 = new FileInfo(path2);
            if (file1.Length != file2.Length)
            {
                Console.WriteLine("Files are different sizes.");
                return;
            }

            if (_textExtensions.Contains(Path.GetExtension(path1)) && _textExtensions.Contains(Path.GetExtension(path2)))
            {
                byte[] file1Bytes = ReadBytesFromFile(path1), file2Bytes = ReadBytesFromFile(path2);
                string[] file1Lines = File.ReadAllLines(path1), file2Lines = File.ReadAllLines(path2);

                if (options["a"] == 1)
                    for (int i = 0; i < file1Bytes.Length; ++i)
                        if (file1Bytes[i] != file2Bytes[i])
                        {
                            Console.WriteLine($"Compare error at OFFSET {i}");
                            Console.WriteLine($"file1 = {(char)file1Bytes[i]}");
                            Console.WriteLine($"file2 = {(char)file2Bytes[i]}");
                            break;
                        }

                if (options["d"] == 1)
                    for (int i = 0; i < file1Bytes.Length; ++i)
                        if (file1Bytes[i] != file2Bytes[i] && options["c"] != 1)
                        {
                            Console.WriteLine($"Compare error at OFFSET {i}");
                            Console.WriteLine($"file1 = {file1Bytes[i]}");
                            Console.WriteLine($"file2 = {file2Bytes[i]}");
                            break;
                        }

                if (options["l"] == 1)
                    for (int i = 0; i < file1Lines.Length; ++i)
                        if (!file1Lines[i].SequenceEqual(file2Lines[i]))
                        {
                            Console.WriteLine($"Compare error at LINE {i+1}");
                            break;
                        }

                if (options["n"] != -1)
                    for (int i = 0; i < options["n"]; ++i)
                        if (!file1Lines[i].SequenceEqual(file2Lines[i]))
                        {
                            Console.WriteLine($"Compare error at LINE {i+1}");
                            break;
                        }

                if (options["a"] != 1)
                    for (int i = 0; i < file1Bytes.Length; ++i)
                        if (file1Bytes[i] != file2Bytes[i] && options["c"] != 1)
                        {
                            Console.WriteLine($"Compare error at OFFSET {i}");
                            Console.WriteLine($"file1 = {Convert.ToString(file1Bytes[i], 16)}");
                            Console.WriteLine($"file2 = {Convert.ToString(file2Bytes[i], 16)}");
                            break;
                        }
                        else if (((char)file1Bytes[i]).ToString().ToLower() != ((char)file2Bytes[i]).ToString().ToLower() && options["c"] == 1)
                        {
                            Console.WriteLine($"Compare error at OFFSET {i}");
                            Console.WriteLine($"file1 = {Convert.ToString(file1Bytes[i], 16)}");
                            Console.WriteLine($"file2 = {Convert.ToString(file2Bytes[i], 16)}");
                            break;
                        }
            }

            if (File.ReadLines(path1).SequenceEqual(File.ReadLines(path2)))
            {
                Console.WriteLine("Files compare OK.");
                return;
            }
        }

        private byte[] ReadBytesFromFile(string path)
        {
            byte[] fileBytes;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[file.Length];
                long numBytesToRead = file.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = file.Read(fileBytes, numBytesRead, (int)numBytesToRead);
                    if (n == 0)
                        break;
                    numBytesToRead -= n;
                    numBytesRead += n;
                }
            }
            return fileBytes;
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Title(string title)
        {
            Console.Title = title;
        }

        public void Copy(string path1, string path2)
        {
            if (!File.Exists(path1))
            {
                Console.WriteLine("The system cannot find the file specified.");
                return;
            }

            int successCopied = 0;

            if (!Directory.Exists(path2) && !File.Exists(path2))
                Console.WriteLine("Access is denied.");

            if (Directory.Exists(path2))
            {
                path2 = Path.Combine(path2, Path.GetFileName(path1));
                var file = File.Create(path2);
                file.Close();
            }

            if (File.Exists(path2))
            {
                bool ok = true;
                Console.Write($"Overwrite {path2}? (Yes/No): ");
                while (ok)
                    switch (Console.ReadLine().ToLower())
                    {
                        case "y":
                        case "yes":
                            File.Delete(path2);
                            File.Copy(path1, path2);
                            successCopied++;
                            ok = false;
                            break;
                        case "n":
                        case "no":
                            ok = false;
                            break;
                    }
            }

            Console.WriteLine($"\t{successCopied} file(s) copied.");
        }

        public void Date()
        {
            Console.WriteLine($"The current date is: {DateTime.Now}");
        }

        public void Delete(List<string> paths)
        {
            List<string> files = new List<string>();
            List<string> directories = new List<string>();

            foreach (var path in paths.Distinct())
            {
                if (Directory.Exists(path))
                {
                    bool ok = true;
                    Console.Write($"{path}\\*, Are you sure (Y/N)? ");
                    while (ok)
                        switch (Console.ReadLine().ToLower())
                        {
                            case "y":
                            case "yes":
                                directories.Add(path);
                                ok = false;
                                break;
                            case "n":
                            case "no":
                                ok = false;
                                return;
                        }
                }
                else if (File.Exists(path))
                    files.Add(path);
                else
                {
                    Console.WriteLine($"Could Not Find {path}");
                    return;
                }
            }

            foreach (var file in files)
                File.Delete(file);
            foreach (var directory in directories)
                RemoveDirectory(directory);
        }

        private void RemoveDirectory(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            foreach (FileInfo fileInfo in dir.GetFiles())
                fileInfo.Delete();
            foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
                directoryInfo.Delete(true);
            dir.Delete();
        }

        public void RemoveDir(List<string> paths, bool recursive)
        {
            foreach (var path in paths) {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("The directory name is invalid.");
                    continue;
                }

                if (Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length > 0 && !recursive)
                {
                    Console.WriteLine("The directory is not empty.");
                    continue;
                }
                else
                    RemoveDirectory(path);
            }
        }

        public void Move(string[] srcPaths, string dstPath)
        {
            foreach (var path in srcPaths)
                if (!File.Exists(path))
                {
                    Console.WriteLine($"The system cannot find the file {path}");
                    return;
                }

            int movedFiles = 0;
            if (File.Exists(dstPath))
            {
                if (srcPaths.Length > 1)
                {
                    Console.WriteLine("The syntax of the command is incorrect.");
                    return;
                }

                bool ok = true;
                Console.Write($"Overwrite {dstPath} (Yes/No): ");
                while (ok)
                    switch (Console.ReadLine().ToLower())
                    {
                        case "y":
                        case "yes":
                            ok = false;
                            File.Delete(dstPath);
                            File.Move(srcPaths[0], dstPath);
                            movedFiles++;
                            break;
                        case "n":
                        case "no":
                            ok = false;
                            Console.WriteLine($"\t{movedFiles} file(s) moved.");
                            return;
                    }
            }

            foreach (var path in srcPaths)
                if (Directory.Exists(dstPath))
                {
                    File.Move(path, Path.Combine(dstPath, Path.GetFileName(path)));
                    movedFiles++;
                }

            Console.WriteLine($"\t{movedFiles} file(s) moved.");
        }

        public void Show(string path, Dictionary<string, bool> options, List<string> attributes)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Directory {path} not found.");
                return;
            }

            Console.WriteLine($"\nDirectory of {path}\n");
            List<string> points = new List<string>() { ".", ".." };

            if (!string.IsNullOrEmpty(attributes[0]) || attributes.Contains("D"))
                foreach (var point in points)
                {
                    if (options["c"])
                        Console.Write("\t\t\t");
                    Console.Write($"DIR\t");
                    if (options["s"])
                        Console.Write("\t");
                    Console.WriteLine(point);
                }

            var files = Directory.GetFileSystemEntries(path);
            int filesCount = 0, directoriesCount = 2;
            foreach (var file in files)
            {
                FileInfo f = new FileInfo(file);
                bool contin = true;

                if (attributes.Contains("D"))
                    contin &= !((f.Attributes & FileAttributes.Directory) == FileAttributes.Directory);
                if (attributes.Contains("H"))
                    contin &= !((f.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
                if (attributes.Contains("R"))
                    contin &= !((f.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
                if (attributes.Contains("S"))
                    contin &= !((f.Attributes & FileAttributes.System) == FileAttributes.System);

                if (contin && !string.IsNullOrEmpty(attributes[0]))
                    continue;

                if (Directory.Exists(file))
                {
                    DirectoryInfo d = new DirectoryInfo(file);

                    if (options["c"])
                        Console.Write($"{d.CreationTime}\t");

                    Console.Write($"DIR\t");

                    if (options["s"])
                        Console.Write($"\t");

                    Console.WriteLine($"{Path.GetFileName(file)}");
                    directoriesCount++;
                }
                else
                {
                    if (options["c"])
                        Console.Write($"{f.CreationTime}\t");

                    Console.Write($"\t");

                    if (options["s"])
                        Console.Write($"{f.Length}\t");
                    
                    Console.WriteLine($"{Path.GetFileName(file)}");
                    filesCount++;
                }
            }
            Console.WriteLine($"\t\t\t{filesCount} File(s)\n\t\t\t{directoriesCount} Dir(s)");
        }

        public void WhoAmI()
        {
            Console.WriteLine(Environment.UserName);
        }

        public void Create(string[] paths)
        {
            foreach(var path in paths)
            {
                try
                {
                    File.Create(path);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Didn't find part of the path {path}");
                }
            }
        }

        public void CreateDir(string[] paths)
        {
            foreach(var path in paths)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
