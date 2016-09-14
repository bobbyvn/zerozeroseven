using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BRKGA.Interface;
using HelperSharp;

namespace BRKGA.Exception
{
    [Serializable]
    public sealed class FitnessException<T> : System.Exception
    {
        public FitnessException(IFitness<T> fitness, string message)
            : base("{0}: {1}".With(fitness != null ? fitness.GetType().Name : String.Empty, message))
        {
            Fitness = fitness;
        }

        public FitnessException(IFitness<T> fitness, string message, System.Exception innerException)
            : base("{0}: {1}".With(fitness != null ? fitness.GetType().Name : String.Empty, message), innerException)
        {
            Fitness = fitness;
        }

        public FitnessException()
        {
        }

        public FitnessException(string message)
            : base(message)
        {
        }

        public FitnessException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        private FitnessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IFitness<T> Fitness { get; private set; }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Fitness", Fitness);
        }
    }
}
