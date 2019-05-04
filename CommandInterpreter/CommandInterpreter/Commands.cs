using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;

namespace CommandInterpreter
{
    class Commands
    {
        private string[] _textExtensions = { ".txt", ".pdf", ".doc", ".docx" };
        public string CurrentFolder { get; private set; }

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
                Console.WriteLine($"{Properties.Resources.InvalidDirname}");
            else
                Console.WriteLine($"{Properties.Resources.IncorrectPath}");
        }

        public void Color(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Clear();
        }

        public void FileCompare(string path1, string path2, Dictionary<string, long> options)
        {
            Console.WriteLine($"{Properties.Resources.Comparing} {path1} {Properties.Resources.And} {path2}");

            if (!File.Exists(path1))
            {
                Console.WriteLine($"{Properties.Resources.IncorrectFile} {path1}");
                return;
            }
            if (!File.Exists(path2))
            {
                Console.WriteLine($"{Properties.Resources.IncorrectFile} {path2}");
                return;
            }

            FileInfo file1 = new FileInfo(path1), file2 = new FileInfo(path2);
            if (file1.Length != file2.Length)
            {
                Console.WriteLine($"{Properties.Resources.DiffrentSizes}");
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
                            Console.WriteLine($"{Properties.Resources.ErrorCompareOffset} {i}");
                            Console.WriteLine($"file1 = {(char)file1Bytes[i]}");
                            Console.WriteLine($"file2 = {(char)file2Bytes[i]}");
                            break;
                        }

                if (options["d"] == 1)
                    for (int i = 0; i < file1Bytes.Length; ++i)
                        if (file1Bytes[i] != file2Bytes[i] && options["c"] != 1)
                        {
                            DisplayOffset(file1Bytes[i], file2Bytes[i], i);
                            break;
                        }

                if (options["l"] == 1 || options["n"] != -1)
                {
                    long n = file1Lines.Length;
                    if (options["n"] != -1 && options["n"] < n)
                        n = options["n"];

                    for (int i = 0; i < n; ++i)
                        if (!file1Lines[i].SequenceEqual(file2Lines[i]))
                        {
                            Console.WriteLine($"{Properties.Resources.ErrorCompareLine} {i + 1}");
                            break;
                        }
                }

                if (options["a"] != 1)
                    for (int i = 0; i < file1Bytes.Length; ++i)
                        if (file1Bytes[i] != file2Bytes[i] && options["c"] != 1)
                        {
                            DisplayOffset(file1Bytes[i], file2Bytes[i], i, true);
                            break;
                        }
                        else if (((char)file1Bytes[i]).ToString().ToLower() != ((char)file2Bytes[i]).ToString().ToLower() && options["c"] == 1)
                        {
                            DisplayOffset(file1Bytes[i], file2Bytes[i], i, true);
                            break;
                        }
            }

            if (File.ReadLines(path1).SequenceEqual(File.ReadLines(path2)))
            {
                Console.WriteLine($"{Properties.Resources.OkCompare}");
                return;
            }
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
                Console.WriteLine($"{Properties.Resources.IncorrectPath}");
                return;
            }

            int successCopied = 0;

            if (!Directory.Exists(path2) && !File.Exists(path2))
                Console.WriteLine($"{Properties.Resources.AccessDenied}");

            if (Directory.Exists(path2))
            {
                path2 = Path.Combine(path2, Path.GetFileName(path1));
                var file = File.Create(path2);
                file.Close();
            }

            if (File.Exists(path2))
            {
                bool ok = true;
                Console.Write($"{Properties.Resources.Overwrite} {path2}? (Yes/No): ");
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

            Console.WriteLine($"\t{successCopied} {Properties.Resources.CountCopiedFiles}");
        }

        public void Date()
        {
            Console.WriteLine($"{Properties.Resources.CurrentDate} {DateTime.Now}");
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
                    Console.Write($"{path}\\*, {Properties.Resources.Sure} (Y/N)? ");
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
                    Console.WriteLine($"{Properties.Resources.CouldNotFind} {path}");
                    return;
                }
            }

            foreach (var file in files)
                File.Delete(file);
            foreach (var directory in directories)
                RemoveDirectory(directory);
        }

        public void RemoveDir(List<string> paths, bool recursive)
        {
            foreach (var path in paths) {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"{Properties.Resources.InvalidDirname}");
                    continue;
                }

                if (Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length > 0 && !recursive)
                {
                    Console.WriteLine($"{Properties.Resources.DirectoryNotEmpty}");
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
                    Console.WriteLine($"{Properties.Resources.IncorrectPath} {path}");
                    return;
                }

            int movedFiles = 0;
            if (File.Exists(dstPath))
            {
                if (srcPaths.Length > 1)
                {
                    Console.WriteLine($"{Properties.Resources.BadSyntax}");
                    return;
                }

                bool ok = true;
                Console.Write($"{Properties.Resources.Overwrite} {dstPath} (Yes/No): ");
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
                            Console.WriteLine($"\t{movedFiles} {Properties.Resources.CountMovedFiles}");
                            return;
                    }
            }

            foreach (var path in srcPaths)
                if (Directory.Exists(dstPath))
                {
                    File.Move(path, Path.Combine(dstPath, Path.GetFileName(path)));
                    movedFiles++;
                }

            Console.WriteLine($"\t{movedFiles} {Properties.Resources.CountMovedFiles}");
        }

        public void Show(string path, Dictionary<string, bool> options, List<string> attributes)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"{path} {Properties.Resources.IncorrectPath.ToLower()}");
                return;
            }

            Console.WriteLine($"\n{Properties.Resources.Directory} {path}\n");
            List<string> points = new List<string>() { ".", ".." };
            int filesCount = 0, directoriesCount = 0;

            if (string.IsNullOrEmpty(attributes[0]) || attributes.Contains("D"))
                foreach (var point in points)
                {
                    if (options["c"])
                        Console.Write("\t\t\t");
                    Console.Write($"DIR\t");
                    if (options["s"])
                        Console.Write("\t");
                    Console.WriteLine(point);
                    directoriesCount++;
                }

            var files = Directory.GetFileSystemEntries(path);
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
                    Console.WriteLine($"{Properties.Resources.DidNotFindPart} {path}");
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

        public void Chat(string name, int localPort, int remotePort)
        {
            var chat = new Chat(localPort, remotePort, name);

            ThreadPool.QueueUserWorkItem(chat.ReceiveMessage);

            chat.SendMessage();
            chat.IsReceive = false;
        }

        public void Compact(string[] paths, string dstPath)
        {
            foreach(var path in paths)
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    Console.WriteLine($"{path} {Properties.Resources.IncorrectPath.ToLower()}");
                    return;
                }

            dstPath += (Path.GetExtension(dstPath) == ".zip") ? "" : ".zip";
            
            if (File.Exists(dstPath))
            {
                bool ok = true;
                Console.WriteLine($"{Properties.Resources.Overwrite} {dstPath} (Yes/No): ");
                while (ok)
                    switch (Console.ReadLine().ToLower())
                    {
                        case "y":
                        case "yes":
                            ok = false;
                            File.Delete(dstPath);
                            break;
                        case "n":
                        case "no":
                            return;
                    }
            }

            Random rnd = new Random();
            string newPath = Path.Combine(CurrentFolder, ((char)rnd.Next(97, 121)).ToString());
            while (true)
            {
                if (Directory.Exists(newPath))
                {
                    newPath += (char)rnd.Next(97, 121);
                    continue;
                }

                break;
            }

            Directory.CreateDirectory(newPath);
            foreach(var path in paths.Distinct())
                File.Copy(path, Path.Combine(newPath, Path.GetFileName(path)));

            Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
            ZipFile.CreateFromDirectory(newPath, dstPath);
            Directory.Delete(newPath, true);
        }

        public void Decompact(string srcFile, string dstDirectory)
        {
            if (!File.Exists(srcFile))
            {
                Console.WriteLine($"{srcFile} {Properties.Resources.IncorrectPath.ToLower()}");
                return;
            }

            if (Path.GetExtension(srcFile) != ".zip")
            {
                Console.WriteLine($"{srcFile} {Properties.Resources.NotArchive}");
                return;
            }

            if (!Directory.Exists(dstDirectory))
                Directory.CreateDirectory(dstDirectory);

            try
            {
                ZipFile.ExtractToDirectory(srcFile, dstDirectory);
            }
            catch
            {
                Console.WriteLine($"{Properties.Resources.YetExists}");
            }
        }

        public void Lang()
        {
            if (Thread.CurrentThread.CurrentUICulture == System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            else
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");

            Help.UpdateDictionary();
        }

        private void DisplayOffset(byte file1, byte file2, int i, bool toHex = false)
        {
            var file1Bytes = (toHex) ? Convert.ToString(file1, 16) : file1.ToString();
            var file2Bytes = (toHex) ? Convert.ToString(file2, 16) : file2.ToString();

            Console.WriteLine($"{Properties.Resources.ErrorCompareOffset} {i}");
            Console.WriteLine($"file1 = {file1Bytes}");
            Console.WriteLine($"file2 = {file2Bytes}");
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

        private void RemoveDirectory(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            foreach (FileInfo fileInfo in dir.GetFiles())
                fileInfo.Delete();
            foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
                directoryInfo.Delete(true);
            dir.Delete();
        }
    }
}
