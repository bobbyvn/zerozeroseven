using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Exception
{
    public sealed class MutationException<T> : System.Exception
    {
        public MutationException(IMutation<T> mutation, string message)
            : base("{0}: {1}".With(mutation != null ? mutation.GetType().Name : String.Empty, message))
        {
            Mutation = mutation;
        }

        public MutationException(IMutation<T> mutation, string message, System.Exception innerException)
            : base("{0}: {1}".With(mutation != null ? mutation.GetType().Name : String.Empty, message), innerException)
        {
            Mutation = mutation;
        }

        public MutationException()
        {
        }

        public MutationException(string message)
            : base(message)
        {
        }

        public MutationException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        private MutationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IMutation<T> Mutation { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Mutation", Mutation);
        }
    }
}
