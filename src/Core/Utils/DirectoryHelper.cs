using System.IO;
using Ardalis.GuardClauses;

namespace X.Core.Utils
{
    /// <summary>
    /// A helper class for Directory operations.
    /// </summary>
    public static class DirectoryHelper
    {
        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        public static void DeleteIfExists(string directory)
        {
            if (Directory.Exists(directory)) Directory.Delete(directory);
        }

        public static void DeleteIfExists(string directory, bool recursive)
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, recursive);
        }

        public static void CreateIfNotExists(DirectoryInfo directory)
        {
            if (!directory.Exists) directory.Create();
        }

        public static bool IsSubDirectoryOf(string parentDirectoryPath, string childDirectoryPath)
        {
            Guard.Against.Null(parentDirectoryPath, nameof(parentDirectoryPath));
            Guard.Against.Null(childDirectoryPath,  nameof(childDirectoryPath));

            return IsSubDirectoryOf(
                new DirectoryInfo(parentDirectoryPath),
                new DirectoryInfo(childDirectoryPath)
            );
        }

        public static bool IsSubDirectoryOf(DirectoryInfo parentDirectory, DirectoryInfo childDirectory)
        {
            Guard.Against.Null(parentDirectory, nameof(parentDirectory));
            Guard.Against.Null(childDirectory,  nameof(childDirectory));

            if (parentDirectory.FullName == childDirectory.FullName) return true;

            var parentOfChild = childDirectory.Parent;

            return parentOfChild is not null && IsSubDirectoryOf(parentDirectory, parentOfChild);
        }
    }
}
