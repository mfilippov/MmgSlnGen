using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MmgSlnGen.Tests
{
    public class ProjectGraphTests : BaseTestWithGold
    {
        [Fact]
        public void ShouldBuildCorrectlyProjectGraph()
        {
            var sln = new Solution("TestSolution", new List<Project>
            {
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838001}"),
                    "Project1",
                    1,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838002}"),
                    "Project2",
                    2,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838003}"),
                    "Project3",
                    3,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838004}"),
                    "Project4",
                    3,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838005}"),
                    "Project5",
                    3,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838006}"),
                    "Project6",
                    3,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838007}"),
                    "Project7",
                    3,
                    new List<Project>()),
                new Project(
                    new Guid("{2D895D67-050F-494B-B4C9-3ED7BD838008}"),
                    "Project8",
                    3,
                    new List<Project>())
            });
            sln.SerializeTo(TempDir, Mode.Sdk);
            ExecuteWithGold("ShouldBuildCorrectlyProjectGraph.gold", wrt =>
            {
                wrt.WriteLine("Project Graph =>");
                foreach (var project in sln.Projects)
                {
                    Expand(wrt, project, 0);
                }
            });
        }

        private static void Expand(TextWriter wrt, Project project, int level)
        {
            for (var i = 0; i < level; i++)
            {
                wrt.Write("\t");
            }
            wrt.WriteLine(project.Name);
            foreach (var reference in project.ProjectReferences)
            {
                Expand(wrt, reference, level + 1);
            }
        }
    }
}