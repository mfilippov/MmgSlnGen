using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MmgSlnGen
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 
                || args.Length != 5
                || !Enum.TryParse<Mode>(args[0], out var mode)
                || !Enum.TryParse<ProjectReferenceMode>(args[4], out var projectReferenceMode))
            {
                Console.WriteLine(
                    "Usage: MmgSlnGen [Mode(NonSdk|Sdk)] [DestinationFolder] [ProjectCount] [ClassesPerProject] [WithProjectReferences|WithoutProjectReferences]");
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
            var classContent = "\tpublic void MyMethod()\n" +
                               "{\n" +
                               "\t\tint a0, b0, c0, d0, e0, f0, g0, h0, i0, j0, k0, l0, m0, n0, o0, p0, q0, r0, s0, t0, u0, v0, w0, x0, y0, z0;\n" +
                               "\t\tint a1, b1, c1, d1, e1, f1, g1, h1, i1, j1, k1, l1, m1, n1, o1, p1, q1, r1, s1, t1, u1, v1, w1, x1, y1, z1;\n" +
                               "\t\tint a2, b2, c2, d2, e2, f2, g2, h2, i2, j2, k2, l2, m2, n2, o2, p2, q2, r2, s2, t2, u2, v2, w2, x2, y2, z2;\n" +
                               "\t\tint a3, b3, c3, d3, e3, f3, g3, h3, i3, j3, k3, l3, m3, n3, o3, p3, q3, r3, s3, t3, u3, v3, w3, x3, y3, z3;\n" +
                               "\t\tint a4, b4, c4, d4, e4, f4, g4, h4, i4, j4, k4, l4, m4, n4, o4, p4, q4, r4, s4, t4, u4, v4, w4, x4, y4, z4;\n" +
                               "\t\tint a5, b5, c5, d5, e5, f5, g5, h5, i5, j5, k5, l5, m5, n5, o5, p5, q5, r5, s5, t5, u5, v5, w5, x5, y5, z5;\n" +
                               "\t\tint a6, b6, c6, d6, e6, f6, g6, h6, i6, j6, k6, l6, m6, n6, o6, p6, q6, r6, s6, t6, u6, v6, w6, x6, y6, z6;\n" +
                               "\t\tint a7, b7, c7, d7, e7, f7, g7, h7, i7, j7, k7, l7, m7, n7, o7, p7, q7, r7, s7, t7, u7, v7, w7, x7, y7, z7;\n" +
                               "\t\tint a8, b8, c8, d8, e8, f8, g8, h8, i8, j8, k8, l8, m8, n8, o8, p8, q8, r8, s8, t8, u8, v8, w8, x8, y8, z8;\n" +
                               "\t\tint a9, b9, c9, d9, e9, f9, g9, h9, i9, j9, k9, l9, m9, n9, o9, p9, q9, r9, s9, t9, u9, v9, w9, x9, y9, z9;\n" +
                               "\t}\n";
            for (var i = 1; i <= projectCount; i++)
            {
                var projectName = $"ClassLibrary{i:D3}";
                projects.Add(new Project(Guid.NewGuid(), projectName, classesPerProject, 
                    new List<Project>(), classContent));
            }

            var solution = new Solution(solutionName, projects, projectReferenceMode);
            solution.SerializeTo(destinationDir, mode);
        }
    }
}