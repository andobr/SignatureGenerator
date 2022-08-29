using SignatureGenerator.Models;

namespace SignatureGenerator.Reader;

public interface IBlockReader
{
    IEnumerable<Block> ReadBlocks();
    void ReturnBuffer(byte[] buffer);
}