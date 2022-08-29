namespace SignatureGenerator.Hasher;

public interface IBlockHasher
{
    string ComputeHash(byte[] block);
}