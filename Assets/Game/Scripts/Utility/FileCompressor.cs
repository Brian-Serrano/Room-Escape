using System.IO.Compression;
using System.IO;
using System;
using UnityEngine;

public class FileCompressor
{
    public static byte[] CompressFilesAndFolders(params string[] itemsToCompress)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var item in itemsToCompress)
            {
                if (File.Exists(item))
                {
                    zip.CreateEntryFromFile(item, Path.GetFileName(item));
                }
                else if (Directory.Exists(item))
                {
                    AddFolderToZip(zip, item, Path.GetFileName(item));
                }
            }
        };

        return memoryStream.GetBuffer();
    }

    public static void ExtractFilesAndFolders(byte[] compressedData, string destDirectory)
    {
        using MemoryStream memoryStream = new MemoryStream(compressedData);
        using ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Update);

        foreach (ZipArchiveEntry entry in zip.Entries)
        {
            string path = Path.Combine(destDirectory, entry.FullName);

            // create the directory
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // move the zip entry to the file
            entry.ExtractToFile(path, true);
        }
    }

    private static void AddFolderToZip(ZipArchive zip, string folderPath, string folderNameInZip)
    {
        foreach (var file in Directory.GetFiles(folderPath))
        {
            string entryName = Path.Combine(folderNameInZip, Path.GetFileName(file));
            zip.CreateEntryFromFile(file, entryName);
        }

        foreach (var directory in Directory.GetDirectories(folderPath))
        {
            string directoryNameInZip = Path.Combine(folderNameInZip, Path.GetFileName(directory));
            AddFolderToZip(zip, directory, directoryNameInZip);
        }
    }
}
