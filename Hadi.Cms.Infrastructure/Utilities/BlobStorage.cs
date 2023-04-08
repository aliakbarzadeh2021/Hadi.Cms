using Hadi.Cms.Configuration;
using System;
using System.IO;

namespace Hadi.Cms.Infrastructure.Utilities
{
    public static class BlobStorage
    {
        private static readonly object lockObject = new object();

        public static void AddFile(Guid id, byte[] content)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString());

                //if (File.Exists(path))
                //throw new InvalidOperationException();

                File.WriteAllBytes(path, content);
            }
        }

        public static string AddFile(Guid id, byte[] content, bool returnPath = false)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString());

                File.WriteAllBytes(path, content);

                return path;
            }
        }

        public static string AddFile(Guid id, byte[] content, string extention, bool returnPath = false)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString() + extention);

                File.WriteAllBytes(path, content);

                return path;
            }
        }

        public static string AddFileToPhysicalStorage(Guid id, byte[] content, string extention, bool returnPath = false)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.PhysicalStoragePath, id.ToString() + extention);

                
                    File.WriteAllBytes(path, content);

                    return path;
            }
        }

        public static void RemoveFile(Guid id)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString());

                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        public static bool Exists(Guid id)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString());

                return File.Exists(path);
            }
        }

        public static byte[] GetFileContent(Guid id)
        {
            lock (lockObject)
            {
                string path = null;
                try
                {
                    path = Path.Combine(Settings.BlobStoragePath, id.ToString());
                    return File.ReadAllBytes(path);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public static FileStream GetFileStream(Guid id)
        {
            lock (lockObject)
            {
                string path = Path.Combine(Settings.BlobStoragePath, id.ToString());

                return new FileStream(path, FileMode.Open);
            }
        }
    }
}
