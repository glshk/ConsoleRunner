using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ConsoleRunner
{
    class NUnitTestParser : IParseTests
    {
        public NUnitTestParser()
        {
            //new NUnitCategory();
        }

        public IList<Test> GetTests(string testAssemblyPath)
        {
            return GetTests(testAssemblyPath, typeof(TestFixtureAttribute), typeof(TestAttribute));
        }

        public bool HasTests(string testAssemblyPath)
        {
            return HasTests(testAssemblyPath, typeof(TestFixtureAttribute), typeof(TestAttribute));
        }

        IList<Test> GetTests(string testAssemblyPath, Type testFixtureAttr, Type testAttr)
        {
            List<MethodInfo> testMethods = GetTestMethods(testAssemblyPath, testFixtureAttr, testAttr).ToList();
            return GetTestsFromMethodInfo(testMethods, testAssemblyPath, testFixtureAttr);
        }

        List<Test> GetTestsFromMethodInfo(List<MethodInfo> testMethods, string testAssemblyPath, Type testFixtureAttr, bool isParametrized = false)
        {
            return testMethods.Select(m =>
                new Test
                {
                    TestMethod = m,
                    Name = GetMethodFullName(m),
                    PathToAssembly = testAssemblyPath,
                    ClassName = m.DeclaringType.FullName,
                    TestType = GetTestType(testFixtureAttr),
                    IsIgnored = IsTestMethodIgnored(m)
                }
            ).ToList();
        }

        bool IsTestMethodIgnored(MethodInfo methodInfo)
        {
            return methodInfo.CustomAttributes.Any(e => e.AttributeType.Name.Equals("IgnoreAttribute"));
        }

        TestTypes GetTestType(Type attr)
        {
            //if (attr == typeof(TestClassAttribute))
            //{
            //    return TestTypes.MsTest;
            //}

            if (attr == typeof(TestFixtureAttribute))
            {
                return TestTypes.NUnit;
            }

            throw new NotSupportedException(string.Format("Attribute {0} is not a valid unit test type attribute.", attr.FullName));
        }

        string GetMethodFullName(MethodInfo m)
        {
            return string.Format("{0}.{1}", m.DeclaringType.FullName, m.Name);
        }

        IEnumerable<MethodInfo> GetTestMethods(string testAssemblyPath, Type testFixtureAttr, Type testAttr)
        {
            IEnumerable<Type> testFixtures = GetTestFixtures(testAssemblyPath, testFixtureAttr);
            IEnumerable<MethodInfo> methods = testFixtures.SelectMany(f => f.GetMethods());
            return methods.Where(method => method.GetCustomAttributesData().Any(attribute => attribute.AttributeType.FullName == testAttr.FullName));
        }

        IEnumerable<Type> GetTestFixtures(string testAssemblyPath, Type testFixtureAttr)
        {
            Assembly assembly = LoadAssembly(testAssemblyPath);
            if (assembly == null)
            {
                return new List<Type>();
            }
            return GetTypes(testFixtureAttr, assembly);
        }

        Assembly LoadAssembly(string testAssemblyPath)
        {
            var testParser = new TestParser();
            //List<string> assembliesFullPath = testParser.GetTestAssemblyPaths(testAssemblyPath);
            List<string> assembliesFullPath = testParser.GetTestAssemblyPaths(Directory.GetParent(testAssemblyPath).FullName);

            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(testAssemblyPath);

                foreach (var assemblyName in assembly.GetReferencedAssemblies())
                {
                    try
                    {
                        var pathToReferencedAssembly = assembliesFullPath.First(e => e.EndsWith("\\" + assemblyName.Name + ".dll"));
                        Assembly.LoadFrom(pathToReferencedAssembly);
                    }
                    catch (FileNotFoundException)
                    {
                        // must be a system assembly if not in the TestAssemblies folder
                        try
                        {
                            Assembly.ReflectionOnlyLoad(assemblyName.FullName);
                        }
                        catch (FileNotFoundException)
                        {
                            // could be a missing assembly or we are in too deep
                        }
                    }
                    catch (FileLoadException)
                    {
                        // attempted to reload an existing dll
                    }
                    catch (BadImageFormatException)
                    {
                        // unmanaged assemblies can throw this exception
                    }
                    catch (InvalidOperationException)
                    {
                        // when can not find referenced assembly in found assemblies
                    }
                }
            }
            catch (FileLoadException)
            {
                // attempted to reload an existing dll
            }
            catch (BadImageFormatException)
            {
                // unmanaged assemblies can throw this exception
            }
            return assembly;
        }

        IEnumerable<Type> GetTypes(Type testFixtureAttr, Assembly assembly)
        {
            IList<Type> types = new List<Type>();

            Type[] assemblyTypes = { };
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exc)
            {
               
            }
            foreach (var type in assemblyTypes)
            {
                try
                {
                    if (type.GetCustomAttributesData().Any(attribute => attribute.AttributeType.FullName == testFixtureAttr.FullName))
                    {
                        types.Add(type);
                    }
                }
                catch (TypeLoadException exc)
                {

                }
                catch (FileNotFoundException exc)
                {

                }
            }

            return types;
        }

        bool HasTests(string testAssemblyPath, Type testFixtureAttr, Type testAttr)
        {
            return GetTestFixtures(testAssemblyPath, testFixtureAttr).Any(fixture => fixture.GetMethods().Any(method => method.GetCustomAttributesData().Any(attribute => attribute.AttributeType.FullName == testAttr.FullName)));
        }

    }
}
