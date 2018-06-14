using System;

namespace ConsoleRunner
{
    class Runner
    {
        public static void run(Test test)
        {
            test.TestMethod.Invoke(Activator.CreateInstance(test.TestMethod.DeclaringType), null);

        }
    }
}
