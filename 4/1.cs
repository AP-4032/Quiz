using System;
using System.Collections.Generic;

struct File
{
    public string Name { get; set; }
    public int Size { get; set; }
    public string Content { get; set; }
    public Directory? Parent { get; set; }
    public DateTime CreationDate { get; set; }

    public File(string Name, string Content)
    {
        this.Name = Name;
        this.Content = Content;
        this.Size = Content.Length;
        this.CreationDate = DateTime.Now;
        Parent = null;
    }
}

class Directory
{
    public string Name { get; set; }
    public List<File> Files { get; set; }
    public List<Directory> Directories { get; set; }
    public Directory Parent { get; set; }
    public DateTime CreationDate { get; set; }

    public Directory(string Name, Directory Parent)
    {
        this.Name = Name;
        this.Parent = Parent;
        this.Files = new List<File>();
        this.Directories = new List<Directory>();
        this.CreationDate = DateTime.Now;
    }
}

class FileSystem
{
    public Directory Root;
    public Directory CurrentDirectory;

    public int TotalFiles
    {
        get
        {
            return CountFiles(Root);
        }
    }

    public int TotalDirectories
    {
        get
        {
            return CountDirectories(Root);
        }
    }

    public string CurrentPath
    {
        get
        {
            return GetPath(CurrentDirectory);
        }
    }

    public FileSystem()
    {
        this.Root = new Directory("Root", null);
        this.CurrentDirectory = this.Root;
    }

    private int CountFiles(Directory directory)
    {
        int count = directory.Files.Count;
        foreach (var subDirectory in directory.Directories)
        {
            count += CountFiles(subDirectory);
        }
        return count;
    }

    private int CountDirectories(Directory directory)
    {
        int count = directory.Directories.Count;
        foreach (var subDirectory in directory.Directories)
        {
            count += CountDirectories(subDirectory);
        }
        return count;
    }

    private string GetPath(Directory directory)
    {
        if (directory.Parent == null)
        {
            return "/";
        }
        return $"{GetPath(directory.Parent)}/{directory.Name}";
    }

    public void CreateFile(string name, string content)
    {
        File newFile = new File(name, content);
        newFile.Parent = this.CurrentDirectory;
        this.CurrentDirectory.Files.Add(newFile);
        Console.WriteLine($"File '{name}' created. Total files: {TotalFiles}");
    }

    public void CreateDirectory(string Name)
    {
        Directory newDirectory = new Directory(Name, this.CurrentDirectory);
        this.CurrentDirectory.Directories.Add(newDirectory);
        Console.WriteLine($"Directory '{Name}' created. Total directories: {TotalDirectories}");
    }

    public void ChangeDirectory(string path)
    {
        if (path == "/")
        {
            this.CurrentDirectory = this.Root;
            return;
        }

        if (path.StartsWith("/"))
        {
            string[] directories = path.Split('/');
            Directory tempDirectory = this.Root;

            for (int i = 1; i < directories.Length; i++)
            {
                string dir = directories[i];
                bool directoryFound = false;
                foreach (var d in tempDirectory.Directories)
                {
                    if (d.Name == dir)
                    {
                        tempDirectory = d;
                        directoryFound = true;
                        break;
                    }
                }
                if (!directoryFound)
                {
                    Console.WriteLine("Directory not found.");
                    return;
                }
            }

            this.CurrentDirectory = tempDirectory;
        }
        else
        {
            Console.WriteLine("Only absolute paths are supported.");
        }
    }

    public void List()
    {
        Console.WriteLine($"Contents of directory '{CurrentDirectory.Name}' (Path: {CurrentPath}):");

        foreach (var directory in CurrentDirectory.Directories)
        {
            Console.WriteLine($"Directory: {directory.Name}");
        }

        foreach (var file in CurrentDirectory.Files)
        {
            Console.WriteLine($"File: {file.Name}");
        }
    }

    public void DeleteFile(string name)
    {
        File? fileToRemove = null;
        foreach (var file in CurrentDirectory.Files)
        {
            if (file.Name == name)
            {
                fileToRemove = file;
                break;
            }
        }

        if (fileToRemove != null)
        {
            File toRm = (File)fileToRemove;
            CurrentDirectory.Files.Remove(toRm);
            Console.WriteLine($"File '{name}' has been deleted.");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    public void DeleteDirectory(string name)
    {
        Directory? directoryToRemove = null;
        foreach (var directory in CurrentDirectory.Directories)
        {
            if (directory.Name == name)
            {
                directoryToRemove = directory;
                break;
            }
        }

        if (directoryToRemove != null)
        {
            Console.WriteLine($"Directorie '{directoryToRemove.Name}' has been deleted.");
            CurrentDirectory.Directories.Remove(directoryToRemove);
        }
        else
        {
            Console.WriteLine("Directory not found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        FileSystem fileSystem = new FileSystem();

        while (true)
        {
            Console.Write($"{fileSystem.CurrentPath} (Files: {fileSystem.TotalFiles}, Directories: {fileSystem.TotalDirectories})> ");
            string input = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            string[] parts = input.Split(' ');
            string command = parts[0];

            switch (command)
            {
                case "mkfile":
                    if (parts.Length >= 3)
                    {
                        string name = parts[1];
                        string content = string.Join(" ", parts, 2, parts.Length - 2);
                        fileSystem.CreateFile(name, content);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command format. Usage: mkfile <name> <content>");
                    }
                    break;

                case "ls":
                    fileSystem.List();
                    break;

                case "rm":
                    if (parts.Length == 2)
                    {
                        string name = parts[1];
                        fileSystem.DeleteFile(name);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command format. Usage: rm <name>");
                    }
                    break;

                case "rmdir":
                    if (parts.Length == 2)
                    {
                        string name = parts[1];
                        fileSystem.DeleteDirectory(name);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command format. Usage: rmdir <name>");
                    }
                    break;

                case "mkdir":
                    if (parts.Length == 2)
                    {
                        string name = parts[1];
                        fileSystem.CreateDirectory(name);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command format. Usage: mkdir <name>");
                    }
                    break;

                case "pwd":
                    Console.WriteLine($"Current directory path: {fileSystem.CurrentPath}");
                    break;

                case "cd":
                    if (parts.Length == 2)
                    {
                        string path = parts[1];
                        fileSystem.ChangeDirectory(path);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command format. Usage: cd <path>");
                    }
                    break;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
