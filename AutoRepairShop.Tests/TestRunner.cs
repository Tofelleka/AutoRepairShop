using System;
using System.Linq;
using System.Reflection;

namespace AutoRepairShop.Tests
{
    internal static class TestRunner
    {
        public static int RunAll()
        {
            int passed = 0;
            int failed = 0;

            var tests = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.Name.EndsWith("Tests"))
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(method => method.GetCustomAttribute<TestAttribute>() != null)
                .OrderBy(method => method.DeclaringType.Name)
                .ThenBy(method => method.Name);

            foreach (MethodInfo method in tests)
            {
                string name = method.DeclaringType.Name + "." + method.Name;

                try
                {
                    method.Invoke(null, null);

                    passed++;
                    Console.WriteLine("[ok]   " + name);
                }
                catch (TargetInvocationException exception)
                {
                    failed++;
                    Console.WriteLine("[FAIL] " + name + ": " + exception.InnerException.Message);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Пройдено: " + passed + ", провалено: " + failed);

            return failed;
        }
    }

    internal static class Program
    {
        private static int Main()
        {
            return TestRunner.RunAll();
        }
    }
}
