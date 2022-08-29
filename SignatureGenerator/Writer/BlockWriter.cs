using SignatureGenerator.Extensions;
using SignatureGenerator.Models;

namespace SignatureGenerator.Writer;

public sealed class BlockWriter : IBlockWriter
{
    private readonly string?[] _buffer;
    private int StartNumber { get; set; }
    private int EndNumber => StartNumber + _buffer.Length;

    
    public BlockWriter(EnvironmentParameters parameters)
    {
        _buffer = new string[parameters.BackgroundThreadCount];
    }
    
    public void WriteBlock(HashedBlock block)
    {
        lock (_buffer)
        {
            while (block.Number >= EndNumber)
            {
                Monitor.Wait(_buffer);
            }

            var blockIndex = block.Number - StartNumber;
            
            _buffer[blockIndex] = block.ToString();

            if (blockIndex != 0) return;
            
            var readyIndex = _buffer.IndexOfOrDefault(null, _buffer.Length);

            for (var i = 0; i < _buffer.Length; i++)
            {
                if (i < readyIndex)
                {
                    Console.WriteLine(_buffer[i]);
                }
                else
                {
                    _buffer[i - readyIndex] = _buffer[i];
                }
                    
                _buffer[i] = null;
            }
                
            StartNumber += readyIndex;
                
            Monitor.PulseAll(_buffer);
        }
    }
}