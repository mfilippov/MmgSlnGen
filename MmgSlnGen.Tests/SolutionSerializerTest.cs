using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MmgSlnGen.Tests
{
    public class SolutionSerializerTest : BaseTestWithGold
    {
        [Fact]
        public void SerializeSdkSolution()
        {
            var sln = new Solution("TestSolution", new List<Project>
            {
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                    "TestProject",
                    1,
                    new List<Project>())
            });
            sln.SerializeTo(TempDir, Mode.Sdk);
            ExecuteWithGold("SerializeSdkSolution.gold", wrt =>
            {
                wrt.WriteLine("TestSolution.sln =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestSolution.sln")));
                wrt.WriteLine("TestProject.csproj =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestProject", "TestProject.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestProject", "Class001.cs")));
            });
        }
        
        [Fact]
        public void SerializeNonSdkSolution()
        {
            var sln = new Solution("TestSolution", new List<Project>
            {
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                    "TestProject",
                    1,
                    new List<Project>())
            });
            sln.SerializeTo(TempDir, Mode.NonSdk);
            ExecuteWithGold("SerializeNonSdkSolution.gold", wrt =>
            {
                wrt.WriteLine("TestSolution.sln =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestSolution.sln")));
                wrt.WriteLine("TestProject.csproj =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestProject", "TestProject.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestSolution", "TestProject", "Class001.cs")));
            });
        }
    }
}