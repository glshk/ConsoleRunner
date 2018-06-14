using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleRunner
{
    class TestParser
    {
        private IList<IParseTests> _parsers;

        public TestParser()
        {
            _parsers = new List<IParseTests>
            {
                new NUnitTestParser(),
                //new MsTestParser()
            };
        }

        public List<string> GetTestAssemblyPaths(string testAssemblyDirectory) 
            => Directory.GetFiles(Path.Combine(testAssemblyDirectory), "*.dll", SearchOption.AllDirectories).ToList();

        public List<Test> GetTests(IList<string> testAssemblyPaths) 
            => testAssemblyPaths.SelectMany(path => _parsers.SelectMany(parser => parser.GetTests(path))).ToList();
    }
}
