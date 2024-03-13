using System;
using System.Collections.Generic;
using System.IO;

namespace MmgSlnGen
{
    public class Project
    {
        public int ClassesPerProject { get; }
        public List<Project> ProjectReferences { get; }
        public Guid Guid { get; }
        public string Name { get; }

        public string ProjectPath => Path.Combine(Name, $"{Name}.csproj");

        public string ClassContent { get; set; }

        public Project(Guid guid, string name, int classesPerProject, List<Project> projectReferences, string classContent)
        {
            ClassesPerProject = classesPerProject;
            ProjectReferences = projectReferences;
            Guid = guid;
            Name = name;
            ClassContent = classContent;
        }
    }
}