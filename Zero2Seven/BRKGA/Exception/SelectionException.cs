using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Exception
{
    [Serializable]
    public sealed class SelectionException<T> : System.Exception
    {
        public SelectionException(ISelection<T> selection, string message)
            : base("{0}: {1}".With(selection != null ? selection.GetType().Name : String.Empty, message))
        {
            Selection = selection;
        }

        public SelectionException(ISelection<T> selection, string message, System.Exception innerException)
            : base("{0}: {1}".With(selection != null ? selection.GetType().Name : String.Empty, message), innerException)
        {
            Selection = selection;
        }

        public SelectionException()
        {
        }

        public SelectionException(string message)
            : base(message)
        {
        }

        public SelectionException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        private SelectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ISelection<T> Selection { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Selection", Selection);
        }
    }
}
