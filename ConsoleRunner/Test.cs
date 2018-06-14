using System.Reflection;

namespace ConsoleRunner
{
    public class Test
    {
        internal MethodInfo TestMethod;

        public string Name { get; set; }

        public string PathToAssembly { get; set; }

        public string ClassName { get; set; }

        public TestTypes TestType { get; set; }

        public bool IsIgnored { get; set; }
    }

    public enum TestTypes
    {
        MsTest,
        NUnit
    }
}
