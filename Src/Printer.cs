using System.Globalization;
using System.Text.RegularExpressions;

namespace BigFileFinder
{
    public class Printer(SizeFormatter sizeFormatter, int maxDepth, string? fileRegex = null, string? direcotryRegex = null, long? minSize = null)
    {
        private readonly SizeFormatter sizeFormatter = sizeFormatter;
        private readonly int maxDepth = maxDepth;
        private readonly string? fileRegex = fileRegex;
        private readonly string? direcotryRegex = direcotryRegex;
        private readonly long? minSize = minSize;

        //skips inaccesible and hidden files
        private static readonly EnumerationOptions defaultEnumerationOptions = new();

        public void PrintDirectory(DirectoryInfo directoryInfo, int depth = 0)
        {
            var list = PrintDirectoryRec(directoryInfo, depth);
            Console.WriteLine(String.Join(Environment.NewLine, list));
        }

        private List<String> PrintDirectoryRec(DirectoryInfo directoryInfo, int depth = 0)
        {
            var list = new List<string>();
            if (IsFilteredOutByName(directoryInfo.Name, direcotryRegex)) return list;
            if (depth == maxDepth)
            {
                return list;
            }
            else if (depth == 0)
            {
                Console.WriteLine(directoryInfo.FullName);
            }
            else if (depth > 0)
            {
                list.Add(Indent(depth - 1, NameFormatter.DisplayName(directoryInfo)));
            }

            foreach (var file in directoryInfo.GetFiles("*", defaultEnumerationOptions))
            {
                string? printed = PrintFile(file, depth);
                if (printed != null) { list.Add(printed); }
            }

            depth++;
            foreach (var directory in directoryInfo.GetDirectories("*", defaultEnumerationOptions))
            {
                list.AddRange(PrintDirectoryRec(directory, depth));
            }

            return list.Count > 1 ? list : [];
        }

        private string? PrintFile(FileInfo fileInfo, int depth = 0)
        {
            var name = NameFormatter.DisplayName(fileInfo);
            var fileSize = fileInfo.Length;
            if (IsFilteredOutByName(name, fileRegex) || IsFilteredOutBySize(fileSize)) return null;
            return String.Format(CultureInfo.CurrentCulture, "{0,-60}{1,-20}", Indent(depth, name), sizeFormatter.DisplaySize(fileSize));
        }

        private static string Indent(int depth, string name)
        {
            return new String(' ', depth * 2) + '˪' + " " + name;
        }

        public static bool IsFilteredOutByName(string name, string? regex)
        {
            if (regex == null) return false;
            try
            {
                return !Regex.IsMatch(name, regex);
            }
            catch (Exception ex)
            {
                throw new InvalidArgumentsException($"regex '{regex}' is not valid: {ex.Message}", ex);
            }
        }

        private bool IsFilteredOutBySize(long fileSize)
        {
            return fileSize < minSize;
        }

    }
}
