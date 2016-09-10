namespace BRKGA.Interface
{
    public interface IMutation<T> : IChromosomeOperator
    {
        void Mutate(IChromosome<T> chromosome, float probability);
    }
}
