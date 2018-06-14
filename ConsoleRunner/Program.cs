using System.IO;
using System.Linq;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = "C:\\Users\\Admin\\Documents\\Visual Studio 2017\\Projects\\testFrameworkV0";

            var path = Path.Combine(basePath, "testFrameworkV0\\bin\\Debug");
            var slnPath = Path.Combine(basePath, "testFrameworkV0\\testFrameworkV0.csproj");

            Builder.Build(slnPath);

            Runner.run(Discoverer.DiscoverTests(path).First(test => test.TestMethod.Name.Equals("TestFull")));

            foreach (var test in Discoverer.DiscoverTests(path))
            {
                Runner.run(test);
            }
        }
    }
}
