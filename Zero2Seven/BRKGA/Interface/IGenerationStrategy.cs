namespace BRKGA.Interface
{
    public interface IGenerationStrategy<T>
    {
        void RegisterNewGeneration(IPopulation<T> population);
    }
}
