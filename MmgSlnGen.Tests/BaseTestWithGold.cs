using System;
using System.IO;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MmgSlnGen.Tests
{
    public abstract class BaseTestWithGold : IDisposable
    {
        private readonly string _testDataDir = Path.GetFullPath("../../../testData");
        private readonly ITestOutputHelper _testOutputHelper;
        protected string TempDir { get; }

        protected BaseTestWithGold(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            TempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(TempDir);
        }

        protected void ExecuteWithGold(string goldFilePath, Action<TextWriter> action)
        {
            var goldFileAbsolutePath = Path.Combine(_testDataDir, goldFilePath);
            var tempFile = Path.Combine(Path.GetDirectoryName(goldFileAbsolutePath)
                                        ?? throw new InvalidOperationException($"Cannot get directory name from goldFilePath: '{goldFileAbsolutePath}'"),
                $"{Path.GetFileNameWithoutExtension(goldFileAbsolutePath)}.tmp");
            using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                var wrt = new StreamWriter(fs);
                action(wrt);
                wrt.Flush();
                fs.Close();
            }
            var host = Environment.GetEnvironmentVariable("RESHARPER_HOST");
            var goldFileContent = File.ReadAllText(goldFileAbsolutePath).Replace("\r\n", "\n");
            var tempFileContent = File.ReadAllText(tempFile).Replace("\r\n", "\n");
            if (host == "Rider")
            {
                if (goldFileContent != tempFileContent)
                {
                    _testOutputHelper.WriteLine($"Compare(Rider)=\"{ToFileUri(tempFile)}\",\"{ToFileUri(goldFileAbsolutePath)}\"");
                    Assert.Fail("Gold file content is different");
                }
            }
            else
            {
                goldFileContent.ShouldBe(tempFileContent, $"Gold file content is different [{tempFile}]."); 
            }
            File.Delete(tempFile);
        }
        
        private static string ToFileUri(string path) => "file:///" + path.Replace("\\", "/");

        public void Dispose()
        {
            Directory.Delete(TempDir, true);
        }
    }
}