using System;
using System.IO;
using Shouldly;

namespace MmgSlnGen.Tests
{
    public class BaseTestWithGold : IDisposable
    {
        private const string TestDataDir = "..\\..\\..\\testData";
        protected string TempDir { get; }

        protected BaseTestWithGold()
        {
            TempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(TempDir);
        }

        protected static void ExecuteWithGold(string goldFilePath, Action<TextWriter> action)
        {
            var tempFile = Path.GetTempFileName();
            using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                var wrt = new StreamWriter(fs);
                action(wrt);
                wrt.Flush();
                fs.Close();
            }
            File.ReadAllText(Path.Combine(TestDataDir, goldFilePath)).Replace("\r\n", "\n")
                .ShouldBe(File.ReadAllText(tempFile).Replace("\r\n", "\n"),
                    $"Gold file content is different [{tempFile}].");
            File.Delete(tempFile);
        }

        public void Dispose()
        {
            Directory.Delete(TempDir, true);
        }
    }
}