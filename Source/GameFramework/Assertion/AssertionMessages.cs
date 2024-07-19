using System;

namespace GodotGameFramework.GameFramework.Assertion
{
    internal class AssertionMessages
    {
        private const string expected = "Expected:";

        private const string assertionFailed = "Assertion failure.";

        public static string GetMessage(string failureMessage)
        {
            return string.Format("{0} {1}", "Assertion failure:", failureMessage);
        }

        public static string GetMessage(string failureMessage, string expected)
        {
            return GetMessage(string.Format("{0} {1} {2}", failureMessage, "Expected:", expected));
        }

        public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
        {
            return GetMessage(string.Format("Values are {0}equal.", expectEqual ? "not " : ""), string.Format("{0} {2} {1}", actual, expected, expectEqual ? "==" : "!="));
        }

        public static string NullFailureMessage(object value, bool expectNull)
        {
            return GetMessage(string.Format("Value was {0}Null", expectNull ? "not " : ""), string.Format("Value was {0}Null", expectNull ? "" : "not "));
        }

        public static string BooleanFailureMessage(bool expected)
        {
            return GetMessage("Value was " + !expected, expected.ToString());
        }
    }
}