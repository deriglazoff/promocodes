using System;
using System.Runtime.Serialization;

namespace Otus.Teaching.PromoCodeFactory.Core.Exceptions
{
    [Serializable]
    public sealed class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }

        private BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
    }
}
