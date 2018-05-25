using System.IO;

namespace Stratis.CoinmasterClient.Utilities
{
    public static class FileSystem
    {
        public static long GetDirectorySize(this DirectoryInfo directory)
        {
            long totalDirectorySize = 0;
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files) totalDirectorySize += file.Length;

            DirectoryInfo[] subDirectories = directory.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories) totalDirectorySize += GetDirectorySize(subDirectory);
            return totalDirectorySize;
        }
    }
}
