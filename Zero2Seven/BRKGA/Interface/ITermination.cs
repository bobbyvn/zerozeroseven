namespace BRKGA.Interface
{
    public interface ITermination<T>
    {
        bool HasReached(IGeneticAlgorithm<T> geneticAlgorithm);
    }
}
