**Tool for generation boilerplate solution to performance investigations.**

Usage:
```
dotnet run [Mode(NonSdk|Sdk)] [DestinationFolder] [ProjectCount] [ClassesPerProject]
```
where:
* `Mode` defines if SDK-style projects will be used (see https://docs.microsoft.com/en-us/dotnet/standard/frameworks),
* `DestinationFolder` sets output directory, where `TestSolution` output folder
will be created
* `ProjectCount` defines number of generated projects in the solution
* `ClassesPerProject` defines number of classes in each project
