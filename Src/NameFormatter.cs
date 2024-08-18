namespace BigFileFinder
{
    public class NameFormatter
    {
        public static string DisplayName(DirectoryInfo directoryInfo)
        {
            return "\\" + directoryInfo.Name;
        }

        public static string DisplayName(FileInfo fileInfo)
        {
            return fileInfo.Name;
        }
    }
}
