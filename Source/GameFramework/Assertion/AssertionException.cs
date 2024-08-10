using System;

namespace GameFramework.Assertion
{
    public class AssertionException : Exception
    {
        private string userMessage;

        public override string Message
        {
            get
            {
                string text = base.Message;
                if (userMessage != null)
                {
                    text = userMessage + "\n" + text;
                }

                return text;
            }
        }

        public AssertionException(string message, string userMessage) 
            : base(message)
        {
            this.userMessage = userMessage;
        }
    }
}