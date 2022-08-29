using Microsoft.Extensions.DependencyInjection;
using SignatureGenerator.Models;

namespace SignatureGenerator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandLineArguments(this IServiceCollection services, string[] args)
    {
        var fileNameArg = args is { Length: >= 1 } ? args[0] : null;
        var blockSizeArg = args is { Length: >= 2 } ? args[1] : null;
        
        if (fileNameArg == null) 
            throw new ArgumentNullException(nameof(InputParameters.FileName));
        
        var fileInfo = new FileInfo(fileNameArg);
        
        if (!fileInfo.Exists) 
            throw new FileNotFoundException($"File '{fileNameArg}' not found");
        
        if (new FileInfo(fileNameArg).Length == 0) 
            throw new ArgumentException($"File '{fileNameArg}' is empty");
        
        if (blockSizeArg == null) 
            throw new ArgumentNullException(nameof(InputParameters.BlockSize));
        
        if (!int.TryParse(blockSizeArg, out var blockSize))
            throw new ArgumentException($"Invalid input value of parameter {nameof(InputParameters.BlockSize)}");

        if (blockSize <= 0) 
            throw new ArgumentOutOfRangeException(nameof(InputParameters.BlockSize), "Value cannot be negative");
        
        if (blockSize > fileInfo.Length)
            throw new ArgumentOutOfRangeException(nameof(InputParameters.BlockSize), $"Value can not be greater than file length {fileInfo.Length}");
        
        var generator = new InputParameters(fileNameArg, blockSize);

        var processCount = Environment.ProcessorCount;
        var availableBackgroundProcessCount = processCount != 1 ? processCount - 1 : 1;
        var blockCount = (long) Math.Ceiling((double) fileInfo.Length / blockSize);
        var backgroundProcessCount = availableBackgroundProcessCount > blockCount ? (int) blockCount : availableBackgroundProcessCount;
        var environment = new EnvironmentParameters(backgroundProcessCount);
        
        return services.AddSingleton(generator).AddSingleton(environment);
    }
}