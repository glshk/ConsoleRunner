using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        internal static void PrepareSources(string path)
        {
            //foreach (var process in new[] { "IEDriverServer", "geckodriver", "chromedriver" })
            //    Process.GetProcessesByName(process).ToList()
            //        .ForEach(proc => proc.Kill());

            var slnName = Path.GetFileName(path);
            Build(Path.ChangeExtension(Path.Combine(path, slnName), "sln"));

            string currentFile(string fileToCopy) 
                => Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(fileToCopy));

            foreach (var file in Directory.GetFiles(Path.Combine(path, slnName, "bin\\Debug")))
                //if (!File.Exists(currentFile(file)))
                    File.Copy(file, currentFile(file), true);
        }
    }
}
