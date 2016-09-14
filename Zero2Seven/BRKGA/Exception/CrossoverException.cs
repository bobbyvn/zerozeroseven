using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Exception
{
    [Serializable]
    public sealed class CrossoverException<T> : System.Exception
    {
        public CrossoverException(ICrossover<T> crossover, string message)
            : base("{0}: {1}".With(crossover != null ? crossover.GetType().Name : String.Empty, message))
        {
            Crossover = crossover;
        }

        public CrossoverException(ICrossover<T> crossover, string message, System.Exception innerException)
            : base("{0}: {1}".With(crossover != null ? crossover.GetType().Name : String.Empty, message), innerException
                )
        {
            Crossover = crossover;
        }

        public CrossoverException()
        {
        }

        public CrossoverException(string message)
            : base(message)
        {
        }

        public CrossoverException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        private CrossoverException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ICrossover<T> Crossover { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Crossover", Crossover);
        }
    }
}
