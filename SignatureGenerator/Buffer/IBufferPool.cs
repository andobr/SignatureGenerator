namespace SignatureGenerator.Buffer;

public interface IBufferPool
{
    byte[] Rent(int minimumLength);
    void Return(byte[] array);
}