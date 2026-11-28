using System;
using Xunit;
using Xunit.Sdk;

namespace AutoRepairShop.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestAttribute : FactAttribute
    {
    }

    internal static class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            if (condition == false)
            {
                throw new XunitException(message);
            }
        }

        public static void IsFalse(bool condition, string message) => IsTrue(condition == false, message);

        public static void AreEqual<T>(T expected, T actual, string message)
        {
            if (Equals(expected, actual) == false)
            {
                throw new XunitException(
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
                throw new XunitException(
                    message + " (получено другое исключение: " + exception.GetType().Name + ")");
            }

            throw new XunitException(message + " (исключение не выброшено)");
        }
    }
}
