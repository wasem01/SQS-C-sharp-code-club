using System;
using System.Runtime.Serialization;

namespace SQSBankingApplicationNew.Bank
{
    [Serializable]
    internal class AmountLessEqualsZeroException : Exception
    {
        public AmountLessEqualsZeroException()
        {
        }

        public AmountLessEqualsZeroException(string message) : base(message)
        {
        }

        public AmountLessEqualsZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AmountLessEqualsZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}