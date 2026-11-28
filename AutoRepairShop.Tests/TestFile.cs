using System;
using System.IO;
using System.Threading;

namespace AutoRepairShop.Tests
{
    internal static class TestFile
    {
        private const int Retries = 5;

        public static string CreatePath(string prefix) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, prefix + "-" + Guid.NewGuid().ToString("N") + ".txt");

        public static void Run(Action action)
        {
            for (int attempt = 0; ; attempt++)
            {
                try
                {
                    action();

                    return;
                }
                catch (IOException) when (attempt < Retries)
                {
                    Thread.Sleep(50);
                }
            }
        }

        public static void Write(string path, string text) => Run(() => File.WriteAllText(path, text));

        public static void Delete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (IOException)
            {
            }
        }
    }
}
