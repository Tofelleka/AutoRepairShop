using System;

namespace AutoRepairShop.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class TestAttribute : Attribute
    {
    }

    internal sealed class AssertException : Exception
    {
        public AssertException(string message) : base(message)
        {
        }
    }

    internal static class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            if (condition == false)
            {
                throw new AssertException(message);
            }
        }

        public static void AreEqual<T>(T expected, T actual, string message)
        {
            if (Equals(expected, actual) == false)
            {
                throw new AssertException(
                    message + " (ожидалось: " + expected + ", получено: " + actual + ")");
            }
        }

        public static void Throws<TException>(Action action, string message) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception exception)
            {
                throw new AssertException(
                    message + " (получено другое исключение: " + exception.GetType().Name + ")");
            }

            throw new AssertException(message + " (исключение не выброшено)");
        }
    }
}
