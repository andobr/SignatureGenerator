using System.Buffers;
using SignatureGenerator.Models;

namespace SignatureGenerator.Buffer;

public class BufferPool : IBufferPool
{
    private readonly ArrayPool<byte> _bufferPool;

    public BufferPool(InputParameters input, EnvironmentParameters environment)
    {
        _bufferPool = ArrayPool<byte>.Create(input.BlockSize, environment.BackgroundThreadCount);
    }

    public byte[] Rent(int minimumLength) => _bufferPool.Rent(minimumLength);

    public void Return(byte[] array) => _bufferPool.Return(array, true);
}