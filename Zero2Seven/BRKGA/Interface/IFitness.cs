namespace BRKGA.Interface
{
    public interface IFitness<T>
    {
        double Evaluate(IChromosome<T> chromosome);
    }
}
