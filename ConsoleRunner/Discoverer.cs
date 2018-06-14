using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleRunner
{
    static class Discoverer
    {
        private static readonly List<Test> _empty = new List<Test>();

        public static List<Test> DiscoverTests(string path, string testCaseFilter = null)
        {
            Console.WriteLine("\r\n---Starting test discovery...---\r\n");

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Tests assemblies directory '{path}' doesn't exist");
                return _empty;
            }

            var parser = new TestParser();
            var assemblies = parser.GetTestAssemblyPaths(path);

            if (assemblies.Count == 0)
            {
                Console.WriteLine($"No assemblies with tests were found at {path} directory");
                return _empty;
            }

            List<Test> tests = parser.GetTests(assemblies);
            List<Test> uniqueTests = tests.GroupBy(e => e.Name).Select(x => x.First()).ToList();

            //string ignoredTests = BuildProperties.Current.TestRunProperties.IgnoredTests;
            //string[] ignoredTestsValues = ignoredTests.Split(',').Select(e => e.Trim()).ToArray();

            //foreach (var test in uniqueTests)
            //{
            //    bool isIgnored = test.Categories.Any(e => ignoredTestsValues.Contains(e));
            //    if (isIgnored)
            //    {
            //        test.IsIgnored = true;
            //    }
            //}

            if (tests.Count == 0)
            {
                Console.WriteLine($"No tests were found inside of '{path}' directory");
                return _empty;
            }

            var testAssemblies = tests.Select(e => e.PathToAssembly).Distinct().Select(e => e);
            Console.WriteLine($"Test Assemblies:\r\n{string.Join("\r\n", testAssemblies)}\r\n");

            //if (string.IsNullOrWhiteSpace(testCaseFilter))
            //{
            //    return uniqueTests;
            //}

            //var filter = new TestCaseFilter();
            //var unescapedFilter = testCaseFilter.Replace(@"\", string.Empty);

            //if (unescapedFilter.StartsWith("\"") && unescapedFilter.EndsWith("\""))
            //{
            //    // remove quotes, if user enters testfilter with quotes
            //    unescapedFilter = unescapedFilter.Remove(0, 1).Remove(unescapedFilter.Length - 2, 1);
            //}

            //var filteredTests = filter.TestsToRun(uniqueTests, unescapedFilter);
            //if (!filteredTests.Any())
            //{
            //    Log.Error($"No tests found after applying '{unescapedFilter}' filter. Total discovered tests count: {uniqueTests.Count}");
            //    return _empty;
            //}

            //Log.Info($"Tests found '{filteredTests.Count}' after applying '{unescapedFilter}' filter. Total discovered tests without filter count: {uniqueTests.Count}");
            //return filteredTests;

            Console.WriteLine($"\r\n---Total discovered tests without filter count: {uniqueTests.Count}---\r\n");
            return uniqueTests;
        }
    }
}
