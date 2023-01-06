namespace Domain.FileProcessor;

public abstract class BaseFileProcessor
{
    private readonly int _blockSize;

    protected BaseFileProcessor(int blockSize)
    {
        _blockSize = blockSize;
    }
    
    protected abstract bool IsHarmlessBlock(byte[] block);
    
    protected abstract bool CheckValidStart(byte[] buffer);
    
    protected abstract bool CheckValidFinish(byte[] buffer);

    protected abstract void WriteLastBlock(MemoryStream outputStream);
    
    protected void WriteValidBlock(MemoryStream outputStream, byte[] block)
    {
        foreach (var byteInBlock in block)
        {
            outputStream.WriteByte(byteInBlock);
        }
    }
    
    protected  void ReplaceInvalidBlock(MemoryStream outputStream)
    {
        // Block is invalid, replace with "A255C"
        outputStream.WriteByte((byte)'A');
        outputStream.WriteByte((byte)255);
        outputStream.WriteByte((byte)'C');
    }
    
    public bool IsLastBlock(Stream stream)
    {
        return stream.Length == stream.Position;
    }
    
    public bool IsFirstBlock(Stream stream)
    {
        return stream.Position == _blockSize;
    }

}