using System.Collections.Generic;

namespace MmgSlnGen
{
    public class Solution
    {
        public string Name { get; }
        public List<Project> Projects { get; }

        private void FillProjectGraph()
        {
            if (Projects.Count == 0)
                return;

            for (var i = 0; i < Projects.Count; i++)
            {
                if (2 * i + 1 < Projects.Count)
                {
                    Projects[i].ProjectReferences.Add(Projects[2 * i + 1]);
                }
                else
                {
                    return;
                }
                if (2 * i + 2 < Projects.Count)
                {
                    Projects[i].ProjectReferences.Add(Projects[2 * i + 2]);
                }
            }
        }

        public Solution(string name, List<Project> projects)
        {
            Name = name;
            Projects = projects;
            FillProjectGraph();
        }
    }
}