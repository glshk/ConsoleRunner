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
            => BuildManager.DefaultBuildManager.Build(
                new BuildParameters(new ProjectCollection()) { Loggers = new List<ILogger> { new ConsoleLogger() } },
                new BuildRequestData(
                    path, 
                    new Dictionary<string, string> { { "Configuration", "Debug" }, { "Platform", "Any CPU" } }, 
                    "15.0", 
                    new[] {"Build"},
                    null));
    }
}
