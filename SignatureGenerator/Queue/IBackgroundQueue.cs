namespace SignatureGenerator.Queue;

public interface IBackgroundQueue
{
    void Enqueue(Action? action);
    void Shutdown(bool waitForWorkers);
}