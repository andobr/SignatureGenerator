using SignatureGenerator.Buffer;
using SignatureGenerator.Models;

namespace SignatureGenerator.Reader;

public sealed class BlockReader : IBlockReader
{
    private readonly InputParameters _parameters;
    private readonly IBufferPool _bufferPool;

    public BlockReader(InputParameters parameters, IBufferPool bufferPool)
    {
        _parameters = parameters;
        _bufferPool = bufferPool;
    }
        
    public IEnumerable<Block> ReadBlocks()
    {
        using var stream = File.OpenRead(_parameters.FileName);

        for (var number = 0; number < _parameters.BlockCount; number++)
        {
            var buffer = _bufferPool.Rent(_parameters.BlockSize);
            var bytesRead = stream.Read(buffer, 0, _parameters.BlockSize);
            if (bytesRead == 0)
            {
                stream.Close();
                yield break;
            }
            yield return new Block(number, buffer);
        }
    }

    public void ReturnBuffer(byte[] buffer) => _bufferPool.Return(buffer);
}