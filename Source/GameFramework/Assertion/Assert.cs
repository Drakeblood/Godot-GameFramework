using System.Collections.Generic;
using System.Diagnostics;

namespace GodotGameFramework.GameFramework.Assertion
{
    public static class Assert
    {
        private static void Fail(string message, string userMessage)
        {
            throw new AssertionException(message, userMessage);
        }

        [Conditional("DEBUG")]
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                Fail(AssertionMessages.BooleanFailureMessage(expected: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition) Fail(AssertionMessages.BooleanFailureMessage(expected: false), message);
        }

        [Conditional("DEBUG")]
        public static void AreEqual(object expected, object actual, string message = "")
        {
            if (actual != expected)
            {
                Fail(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (!comparer.Equals(actual, expected))
            {
                Fail(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreNotEqual(object expected, object actual, string message = "")
        {
            if (actual == expected)
            {
                Fail(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (comparer.Equals(actual, expected))
            {
                Fail(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNull<T>(T value, string message = "") where T : class
        {
            if (value != null)
            {
                Fail(AssertionMessages.NullFailureMessage(value, expectNull: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotNull<T>(T value, string message = "") where T : class
        {
            if (value == null)
            {
                Fail(AssertionMessages.NullFailureMessage(value, expectNull: false), message);
            }
        }
    }
}