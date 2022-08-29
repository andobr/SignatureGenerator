namespace SignatureGenerator.Models;

public record HashedBlock(int Number, string Hash)
{
    public override string ToString() => $"{Number} -> {Hash}";
}