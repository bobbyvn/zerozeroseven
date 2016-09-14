using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Exception
{
    [Serializable]
    public sealed class ReinsertionException<T> : System.Exception
    {
        public ReinsertionException(IReinsertion<T> reinsertion, string message)
            : base("{0}: {1}".With(reinsertion != null ? reinsertion.GetType().Name : String.Empty, message))
        {
            Reinsertion = reinsertion;
        }

        public ReinsertionException(IReinsertion<T> reinsertion, string message, System.Exception innerException)
            : base("{0}: {1}".With(reinsertion != null ? reinsertion.GetType().Name : String.Empty, message), innerException)
        {
            Reinsertion = reinsertion;
        }

        public ReinsertionException()
        {
        }

        public ReinsertionException(string message)
            : base(message)
        {
        }

        public ReinsertionException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        private ReinsertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IReinsertion<T> Reinsertion { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Reinsertion", Reinsertion);
        }
    }
}
