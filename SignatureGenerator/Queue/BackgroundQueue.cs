using SignatureGenerator.Models;

namespace SignatureGenerator.Queue;

public sealed class BackgroundQueue : IBackgroundQueue
{
	private readonly Queue<Action?> _actionQueue = new();
	private readonly List<Thread> _threads = new();

	public BackgroundQueue(EnvironmentParameters parameters)
	{
		for (var i = 0; i < parameters.BackgroundThreadCount; i++)
		{
			var thread = new Thread(Consume) { IsBackground = true };
			thread.Start();
			_threads.Add(thread);
		}
	}

	public void Enqueue(Action? action)
	{
		lock (_actionQueue)
		{
			_actionQueue.Enqueue(action);
			Monitor.Pulse(_actionQueue);
		}
	}
	
	private void Consume()
	{
		while (true)
		{
			Action? work;
			lock (_actionQueue)
			{
				while (_actionQueue.Count == 0)
				{
					Monitor.Wait(_actionQueue);
				}
				work = _actionQueue.Dequeue();
			}
			if (work == null) return;

			try
			{
				work();
			}
			catch (Exception)
			{
				Shutdown(false);
				throw;
			}
		}
	}

	public void Shutdown(bool waitForWorkers)
	{
		foreach (var worker in _threads) 
			Enqueue(null);
        
		if (waitForWorkers) 
			foreach (var worker in _threads) 
				worker.Join();
	}
}