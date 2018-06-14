using System.Collections.Generic;

namespace ConsoleRunner
{
    interface IParseTests
    {
        IList<Test> GetTests(string testAssemblyPath);
        bool HasTests(string testAssemblyPath);
    }
}
