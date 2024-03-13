using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace MmgSlnGen.Tests
{
    public class ProjectSerializerTest(ITestOutputHelper testOutputHelper) : BaseTestWithGold(testOutputHelper)
    {
        [Fact]
        public void ShouldSerializeSingleSdkProject()
        {
            var project = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "TestProject",
                1,
                new List<Project>(),
                string.Empty);

            project.SerializeTo(TempDir, Mode.Sdk);
            ExecuteWithGold("ShouldSerializeSingleSdkProject.gold", wrt =>
            {
                wrt.WriteLine("TestProject.csproj =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestProject", "TestProject.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestProject", "Class001.cs")));
            });
        }
        
        [Fact]
        public void ShouldSerializeSingleNonSdkProject()
        {
            var project = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "TestProject",
                1,
                new List<Project>(),
                string.Empty);

            project.SerializeTo(TempDir, Mode.NonSdk);
            ExecuteWithGold("ShouldSerializeSingleNonSdkProject.gold", wrt =>
            {
                wrt.WriteLine("TestProject.csproj =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestProject", "TestProject.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "TestProject", "Class001.cs")));
            });
        }

        [Fact]
        public void ShouldSerializeSdkProjectWithReferences()
        {
            var projectA = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "ProjectA",
                1,
                new List<Project>(),
                string.Empty);

            var projectB = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "ProjectB",
                1,
                new List<Project> {projectA},
                string.Empty);

            projectB.SerializeTo(TempDir, Mode.Sdk);
            ExecuteWithGold("ShouldSerializeSdkProjectWithReferences.gold", wrt =>
            {
                wrt.WriteLine("ProjectB.csproj =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "ProjectB", "ProjectB.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.WriteLine(File.ReadAllText(Path.Combine(TempDir, "ProjectB", "Class001.cs")));
            });
        }
        
        [Fact]
        public void ShouldSerializeNonSdkProjectWithReferences()
        {
            var projectA = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "ProjectA",
                1,
                new List<Project>(),
                string.Empty);

            var projectB = new Project(
                new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838D4C}"),
                "ProjectB",
                1,
                new List<Project> {projectA},
                string.Empty);
            projectB.SerializeTo(TempDir, Mode.NonSdk);
            ExecuteWithGold("ShouldSerializeNonSdkProjectWithReferences.gold", wrt =>
            {
                wrt.WriteLine("ProjectB.csproj =>");
                wrt.Write(File.ReadAllText(Path.Combine(TempDir, "ProjectB", "ProjectB.csproj")));
                wrt.WriteLine("Class1.cs =>");
                wrt.Write(File.ReadAllText(Path.Combine(TempDir, "ProjectB", "Class001.cs")));
            });
        }
    }
}