namespace SignatureGenerator.Models;

public record InputParameters(string FileName, int BlockSize)
{
    public long BlockCount => (long) Math.Ceiling((decimal) new FileInfo(FileName).Length / BlockSize);
}