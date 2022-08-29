using System.Security.Cryptography;

namespace SignatureGenerator.Hasher;

public sealed class BlockHasher : IBlockHasher
{
    private readonly HashAlgorithms _algorithm;

    public BlockHasher(HashAlgorithms algorithm = HashAlgorithms.SHA256)
    {
        _algorithm = algorithm;
    }

    public string ComputeHash(byte[] block)
    {
        using var hashAlgorithm = CreateHashAlgorithm(_algorithm);
        var hashByte = hashAlgorithm.ComputeHash(block);
        var hash = BitConverter.ToString(hashByte);
        return hash.Replace("-",string.Empty);
    }

    private static HashAlgorithm CreateHashAlgorithm(HashAlgorithms algorithm)
    {
        return algorithm switch
        {
            HashAlgorithms.MD5 => MD5.Create(),
            HashAlgorithms.SHA1 => SHA1.Create(),
            HashAlgorithms.SHA256 => SHA256.Create(),
            HashAlgorithms.SHA384 => SHA384.Create(),
            HashAlgorithms.SHA512 => SHA512.Create(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}