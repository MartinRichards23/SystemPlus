using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SystemPlus.IO
{
    /// <summary>
    /// Helpers for the file system
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Enumerates all files in a directory that match the given regex
        /// </summary>
        public static IEnumerable<string> GetFiles(string dir, Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (string file in Directory.GetFiles(dir, "*", searchOption))
            {
                string name = Path.GetFileName(file);

                if (regex.IsMatch(name))
                    yield return file;
            }
        }

        public static IEnumerable<string> GetFiles(string dir, string regex, RegexOptions options = RegexOptions.IgnoreCase, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return GetFiles(dir, new Regex(regex, options), searchOption);
        }

        public static void CopyDirectory(string source, string target)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(source);
            DirectoryInfo targetDir = new DirectoryInfo(target);

            CopyDirectory(sourceDir, targetDir);
        }

        /// <summary>
        /// Copies an entire directory, with all the sub directories and files
        /// </summary>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static long GetDirSize(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            return GetDirSize(dir);
        }

        public static long GetDirSize(DirectoryInfo dir)
        {
            long size = 0;
            DirectoryInfo[] dirs = dir.GetDirectories();
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo fi in files)
            {
                size += fi.Length;
            }

            foreach (DirectoryInfo di in dirs)
            {
                size += GetDirSize(di);
            }

            return size;
        }

        /// <summary>
        /// Generates a unique filename by adding a number 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string MakeNewFileName(string name, string directory, string extension)
        {
            if (!extension.StartsWith(".", StringComparison.Ordinal))
                extension = "." + extension;

            if (!directory.EndsWith("\\", StringComparison.Ordinal))
                directory += "\\";

            string tempName = name;

            //add numbers until it is unique
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (i > 1)
                    tempName = name + " (" + i.ToString(CultureInfo.InvariantCulture) + ")";
                else
                    tempName = name;

                string testPath = directory + tempName + extension;

                //does basic name exist?
                if (!File.Exists(testPath))
                {
                    return tempName;
                }
            }

            return tempName;
        }

        /// <summary>
        /// Removes invalid characters from file name
        /// </summary>
        public static string CleanFileName(string name, char replaceChar = '_', int maxLength = 100)
        {
            char[] illegals = Path.GetInvalidPathChars();

            foreach (char c in illegals)
            {
                name = name.Replace(c, replaceChar);
            }

            name = name.Replace(':', replaceChar);
            name = name.Replace('/', replaceChar);
            name = name.Replace('?', replaceChar);

            if (name.Length > maxLength)
                name = name.Substring(0, maxLength);

            return name;
        }

        public static bool ValidateFileName(string name, out string? invalidChars)
        {
            Match m = Regex.Match(name, "[\\/:\"*?<>|]");

            if (m.Success)
            {
                invalidChars = m.Value;
                return false;
            }

            invalidChars = null;
            return true;
        }

        public static void EnsureDirExists(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// Folder that the assembly exe is located in
        /// </summary>
        public static string? CurrentFolder
        {
            get
            {
                string? s = Assembly.GetEntryAssembly()?.Location;                
                s = Path.GetDirectoryName(s);
                return s;
            }
        }

        /// <summary>
        /// Enumerates all FileInfos in a directory (including sub directories)
        /// </summary>
        public static IEnumerable<FileInfo> EnumerateAllFiles(string dir)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(dir);

            while (queue.Count > 0)
            {
                dir = queue.Dequeue();

                foreach (string subDir in Directory.GetDirectories(dir))
                {
                    queue.Enqueue(subDir);
                }

                string[] files = Directory.GetFiles(dir);

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo info = new FileInfo(files[i]);

                    yield return info;
                }
            }
        }

        /// <summary>
        /// Gets the normalised extension of the file name e.g. "txt"
        /// </summary>
        public static string GetExtensionNormalised(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant().TrimStart('.');
        }

        /// <summary>
        /// Makes a file path with a random name and the given extension
        /// </summary>
        public static string GetTempFilePathWithExtension(string extension)
        {
            string path = Path.GetTempPath();
            string fileName = Guid.NewGuid() + extension;
            return Path.Combine(path, fileName);
        }

    }
}