using SignatureGenerator.Models;

namespace SignatureGenerator.Writer;

public interface IBlockWriter
{
    void WriteBlock(HashedBlock block);
}