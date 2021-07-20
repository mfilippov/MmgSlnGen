using System;
using System.IO;
using System.Text;

namespace MmgSlnGen
{
    public static class ProjectSerializer
    {
        public static void SerializeTo(this Project project, string dir, Mode mode)
        {
            switch (mode)
            {
                case Mode.Sdk:
                    SdkMode(project, dir);
                    break;
                case Mode.NonSdk:
                    NonSdkMode(project, dir);
                    break;
                default:
                    throw new InvalidOperationException("Unknown serialization mode");
            }
        }

        private static void SdkMode(Project project, string dir)
        {
            var projectDir = Path.Combine(dir, project.Name);
            var projectFileContent = new StringBuilder("<Project Sdk=\"Microsoft.NET.Sdk.Web\">\n" +
                                                       "\t<PropertyGroup>\n" +
                                                       "\t\t<TargetFramework>net5.0</TargetFramework>\n" +
                                                       "\t</PropertyGroup>\n" +
                                                       "\t<ItemGroup>\n");
            foreach (var projectReference in project.ProjectReferences)
            {
                projectFileContent.Append(
                    $"\t\t<ProjectReference Include = \"..\\{projectReference.Name}\\{projectReference.Name}.csproj\" />\n");
            }

            projectFileContent.Append("\t</ItemGroup>\n" +
                                      "</Project>\n");

            Directory.CreateDirectory(projectDir);
            File.WriteAllText(Path.Combine(dir, project.ProjectPath), projectFileContent.ToString());
            File.WriteAllText(Path.Combine(projectDir, $"Program.cs"), $"namespace {project.Name}\n" +
                                                                       "{\n" +
                                                                       "\t public static class Program\n" +
                                                                       "\t{\n" +
                                                                       "\t\tpublic static void Main()\n" +
                                                                       "\t\t{\n" +
                                                                       "\t\t\tSystem.Console.WriteLine(\"Hello world\");\n" +
                                                                       "\t\t}\n" +
                                                                       "\t}\n" +
                                                                       "}\n");
            for (var i = 1; i <= project.ClassesPerProject; i++)
            {
                var classContent = new StringBuilder($"using Microsoft.AspNetCore.Mvc;\n\n" +
                                                     $"namespace {project.Name}\n" +
                                                     "{\n" +
                                                     "\t[Route(\"[controller]/[action]\")]\n" +
                                                     $"\tpublic class Test{i:D3}Controller : ControllerBase {{\n");
                for (var j = 1; j <= 100; j++)
                {
                    classContent.Append("\t[HttpGet(\"{id:int}\")]\n" +
                                        $"\tpublic string MyMethod{j}(int id)\n" +
                                        "\t{\n" +
                                        $"\t\treturn $\"Hello{j}_Id=${{id}}\";\n" +
                                        "\t}\n");
                }
                classContent.Append("\t}\n" +
                                    "}\n");
                File.WriteAllText(Path.Combine(projectDir, $"Class{i:D3}.cs"), classContent.ToString());
            }
        }

        private static void NonSdkMode(Project project, string dir)
        {
            var projectDir = Path.Combine(dir, project.Name);
            var projectFileContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                                       "<Project ToolsVersion=\"4.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\n" +
                                                       "\t<Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />\n" +
                                                       "\t<PropertyGroup>\n" +
                                                       "\t\t<Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>\n" +
                                                       "\t\t<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>\n" +
                                                       $"\t\t<ProjectGuid>{{{project.Guid.ToString().ToUpperInvariant()}}}</ProjectGuid>\n" +
                                                       "\t\t<OutputType>Library</OutputType>\n" +
                                                       $"\t\t<RootNamespace>{project.Name}</RootNamespace>\n" +
                                                       $"\t\t<AssemblyName>{project.Name}</AssemblyName>\n" +
                                                       "\t\t<TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>\n" +
                                                       "\t\t<FileAlignment>512</FileAlignment>\n" +
                                                       "\t</PropertyGroup>\n" +
                                                       "\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">\n" +
                                                       "\t\t<PlatformTarget>AnyCPU</PlatformTarget>\n" +
                                                       "\t\t<DebugSymbols>true</DebugSymbols>\n" +
                                                       "\t\t<DebugType>portable</DebugType>\n" +
                                                       "\t\t<Optimize>false</Optimize>\n" +
                                                       "\t\t<OutputPath>bin\\Debug\\</OutputPath>\n" +
                                                       "\t\t<DefineConstants>DEBUG;TRACE</DefineConstants>\n" +
                                                       "\t\t<ErrorReport>prompt</ErrorReport>\n" +
                                                       "\t\t<WarningLevel>4</WarningLevel>\n" +
                                                       "\t</PropertyGroup>\n " +
                                                       "\t<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">\n" +
                                                       "\t\t<PlatformTarget>AnyCPU</PlatformTarget>\n" +
                                                       "\t\t<DebugType>pdbonly</DebugType>\n" +
                                                       "\t\t<Optimize>true</Optimize>\n" +
                                                       "\t\t<OutputPath>bin\\Release\\</OutputPath>\n" +
                                                       "\t\t<DefineConstants>TRACE</DefineConstants>\n" +
                                                       "\t\t<ErrorReport>prompt</ErrorReport>\n" +
                                                       "\t\t<WarningLevel>4</WarningLevel>\n" +
                                                       "\t</PropertyGroup>\n" +
                                                       "\t<ItemGroup>\n" +
                                                       "\t\t<Reference Include=\"System\" />\n" +
                                                       "\t\t<Reference Include=\"System.Core\" />\n" +
                                                       "\t</ItemGroup>\n" +
                                                       "\t<ItemGroup>\n");
            for (var i = 1; i <= project.ClassesPerProject; i++)
            {
                projectFileContent.Append($"\t\t<Compile Include=\"Class{i:D3}.cs\" />\n");
            }

            projectFileContent.Append("\t</ItemGroup>\n" +
                                      "\t<ItemGroup>\n");
            foreach (var projectReference in project.ProjectReferences)
            {
                projectFileContent.Append(
                    $"\t\t<ProjectReference Include=\"..\\{projectReference.Name}\\{projectReference.Name}.csproj\">\n");
                projectFileContent.Append(
                    $"\t\t\t<Project>{{{projectReference.Guid.ToString().ToUpperInvariant()}}}</Project>\n");
                projectFileContent.Append("\t\t\t<Name>ClassLibrary4</Name>\n");
                projectFileContent.Append("\t\t</ProjectReference>\n");
            }

            projectFileContent.Append("\t</ItemGroup>\n" +
                                      "\t<Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />\n" +
                                      "</Project>\n");
            Directory.CreateDirectory(projectDir);
            File.WriteAllText(Path.Combine(dir, project.ProjectPath), projectFileContent.ToString());
            for (var i = 1; i <= project.ClassesPerProject; i++)
            {
                File.WriteAllText(Path.Combine(projectDir, $"Class{i:D3}.cs"), $"namespace {project.Name}\n" +
                                                                               "{\n" +
                                                                               $"\tpublic class Class{i:D3} {{}}\n" +
                                                                               "}\n");
            }
        }
    }
}