using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRunner
{
    class Runner
    {
        public static void run(Test test)
        {
            var instance = Activator.CreateInstance(test.TestMethod.DeclaringType);
            new List<MethodInfo>
            {
                test.Before,
                test.TestMethod,
                test.After
            }.ForEach(method => method.Invoke(instance, null));
        }
    }
}
