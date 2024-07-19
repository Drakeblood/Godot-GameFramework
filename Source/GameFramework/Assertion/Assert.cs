using System.Collections.Generic;
using System.Diagnostics;

namespace GodotGameFramework.GameFramework.Assertion
{
    public static class Assert
    {
        [Conditional("DEBUG")]
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                throw new AssertionException(AssertionMessages.BooleanFailureMessage(expected: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                throw new AssertionException(AssertionMessages.BooleanFailureMessage(expected: false), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreEqual(object expected, object actual, string message = "")
        {
            if (actual != expected)
            {
                throw new AssertionException(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (!comparer.Equals(actual, expected))
            {
                throw new AssertionException(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreNotEqual(object expected, object actual, string message = "")
        {
            if (actual == expected)
            {
                throw new AssertionException(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        [Conditional("DEBUG")]
        public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer)
        {
            if (comparer.Equals(actual, expected))
            {
                throw new AssertionException(AssertionMessages.GetEqualityMessage(actual, expected, expectEqual: false), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNull<T>(T value, string message = "") where T : class
        {
            if (value != null)
            {
                throw new AssertionException(AssertionMessages.NullFailureMessage(value, expectNull: true), message);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotNull<T>(T value, string message = "") where T : class
        {
            if (value == null)
            {
                throw new AssertionException(AssertionMessages.NullFailureMessage(value, expectNull: false), message);
            }
        }
    }
}