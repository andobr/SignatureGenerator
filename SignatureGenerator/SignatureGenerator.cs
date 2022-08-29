using SignatureGenerator.Hasher;
using SignatureGenerator.Models;
using SignatureGenerator.Queue;
using SignatureGenerator.Reader;
using SignatureGenerator.Writer;

namespace SignatureGenerator;

public sealed class SignatureGenerator
{
	private readonly IBlockReader _reader;
	private readonly IBlockHasher _hasher;
	private readonly IBlockWriter _writer;
	private readonly IBackgroundQueue _queue;

	private readonly SemaphoreSlim _semaphore;
	
	public SignatureGenerator(
		EnvironmentParameters environment,
		IBlockReader reader, IBlockHasher hasher, 
		IBlockWriter writer, IBackgroundQueue queue)
	{
		_reader = reader;
		_hasher = hasher;
		_writer = writer;
		_queue = queue;
		
		_semaphore = new(
			environment.OverallThreadCount, 
			environment.OverallThreadCount);
	}

	public void ComputeSignature()
	{
		foreach (var block in _reader.ReadBlocks())
		{
			_semaphore.Wait();
			_queue.Enqueue(() => ProcessBlock(block));
		}
		
		_queue.Shutdown(true);
	}

	private void ProcessBlock(Block block)
	{
		var hash = _hasher.ComputeHash(block.Data);
		_reader.ReturnBuffer(block.Data);
		_writer.WriteBlock(new HashedBlock(block.Number, hash));
		_semaphore.Release();
	}
}