using System.IO;
using System.Text;

namespace MmgSlnGen
{
    public static class SolutionSerializer
    {
        public static void SerializeTo(this Solution solution, string dir, Mode mode)
        {
            var sb = new StringBuilder("Microsoft Visual Studio Solution File, Format Version 12.00\n");
            foreach (var project in solution.Projects)
            {
                sb.Append(
                    $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{project.Name}\", \"{project.ProjectPath}\", \"{{{project.Guid.ToString().ToUpperInvariant()}}}\"\n");
            }

            sb.Append("EndProject\n" +
                      "Global\n" +
                      "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\n" +
                      "\t\tDebug|Any CPU = Debug|Any CPU\n" +
                      "\t\tRelease|Any CPU = Release|Any CPU\n" +
                      "\tEndGlobalSection\n" +
                      "\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\n");
            foreach (var project in solution.Projects)
            {
                sb.Append(
                    $"\t\t{{{project.Guid.ToString().ToUpperInvariant()}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n");
                sb.Append(
                    $"\t\t{{{project.Guid.ToString().ToUpperInvariant()}}}.Debug|Any CPU.Build.0 = Debug|Any CPU\n");
                sb.Append(
                    $"\t\t{{{project.Guid.ToString().ToUpperInvariant()}}}.Release|Any CPU.ActiveCfg = Release|Any CPU\n");
                sb.Append(
                    $"\t\t{{{project.Guid.ToString().ToUpperInvariant()}}}.Release|Any CPU.Build.0 = Release|Any CPU\n");
            }

            sb.Append("\tEndGlobalSection\n" +
                      "EndGlobal\n");
            var slnDirectory = Path.Combine(dir, solution.Name);
            Directory.CreateDirectory(slnDirectory);
            File.WriteAllText(Path.Combine(slnDirectory, $"{solution.Name}.sln"), sb.ToString());
            foreach (var project in solution.Projects)
            {
                project.SerializeTo(slnDirectory, mode);
            }
        }
    }
}