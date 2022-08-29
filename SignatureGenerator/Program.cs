using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SignatureGenerator.Buffer;
using SignatureGenerator.Extensions;
using SignatureGenerator.Hasher;
using SignatureGenerator.Queue;
using SignatureGenerator.Reader;
using SignatureGenerator.Writer;

namespace SignatureGenerator;

public static class Program
{
	public static int Main(string[] args)
	{
		AppDomain.CurrentDomain.UnhandledException += ConsoleLogException;

		var host = CreateHostBuilder(args).Build();
		var generator = host?.Services?.GetService<SignatureGenerator>();
		generator?.ComputeSignature();

		return 0;
	}

	private static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureServices((_, services) => services
				.AddCommandLineArguments(args)
				.AddScoped<IBufferPool, BufferPool>()
				.AddScoped<IBlockReader, BlockReader>()
				.AddScoped<IBlockHasher, BlockHasher>()
				.AddScoped<IBlockWriter, BlockWriter>()
				.AddScoped<IBackgroundQueue, BackgroundQueue>()
				.AddScoped<SignatureGenerator, SignatureGenerator>());
	
	private static void ConsoleLogException(object sender, UnhandledExceptionEventArgs e)
	{
		var exception = (Exception) e.ExceptionObject;
		Console.WriteLine($"Message: {exception.Message}\nStack trace:\n{exception.StackTrace}");
		Environment.Exit(1);
	}
}