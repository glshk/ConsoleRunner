using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;


namespace ConsoleRunner
{
    public static class Builder
    {
        internal static void Build(string path)
        {
            //new ProjectCollection().LoadProject(path).Build();
            //var p = new Project(path);
            //p.Build();

            var globalProperty = new Dictionary<string, string> {{"Configuration", "Debug"}, {"Platform", "Any CPU"}};

            BuildManager.DefaultBuildManager.Build(new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger> { new ConsoleLogger() } },
                new BuildRequestData(path, new Dictionary<string, string> { { "Configuration", "Debug" }, { "Platform", "Any CPU" } }, 
                    "4.0", new[] {"Build"}, null));
        }
    }
}
