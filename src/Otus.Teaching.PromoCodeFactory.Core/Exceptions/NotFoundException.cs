using System;
using System.Runtime.Serialization;

namespace Otus.Teaching.PromoCodeFactory.Core.Exceptions
{
    [Serializable]
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        private NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);
    }
}
