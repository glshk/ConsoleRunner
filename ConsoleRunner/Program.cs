using System.IO;
using System.Linq;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //specify test solution file
            var slnPath = Path.GetFullPath(
                "C:\\Users\\Admin\\Documents\\Visual Studio 2017\\Projects\\testFrameworkV0\\testFrameworkV0.sln");
            var basePath = Directory.GetParent(slnPath).FullName;

            Builder.PrepareSources(basePath);

            var discoveredTests = Discoverer.DiscoverTests(Directory.GetCurrentDirectory());
            var testToTest = discoveredTests.First(test => test.TestMethod.Name.Equals("Test1"));
            Runner.run(testToTest);

            //foreach (var test in Discoverer.DiscoverTests(path))
            //{
            //    Runner.run(test);
            //}
        }
    }
}
