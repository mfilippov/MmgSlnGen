using System;
using System.Collections.Generic;
using System.IO;

namespace MmgSlnGen
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length != 4 || !Enum.TryParse<Mode>(args[0], out var mode))
            {
                Console.WriteLine("Usage: MmgSlnGen [Mode(NonSdk|Sdk)] [DestinationFolder] [ProjectCount] [ClassesPerProject]");
                return;
            }

            var destinationDir = args[1];
            var projectCount = int.Parse(args[2]);
            var classesPerProject = int.Parse(args[3]);

            const string solutionName = "TestSolution";
            var solutionDir = Path.Combine(destinationDir, solutionName);
            if (Directory.Exists(solutionDir))
            {
                Console.WriteLine("Cleanup old folder.");
                Directory.Delete(solutionDir, true);
            }

            var projects = new List<Project>();
            for (var i = 1; i <= projectCount; i++)
            {
                var projectName = $"ClassLibrary{i:D3}";
                projects.Add(new Project(Guid.NewGuid(), projectName, classesPerProject, new List<Project>()));
            }

            var solution = new Solution(solutionName, projects);
            solution.SerializeTo(destinationDir, mode);
        }
    }
}