using System.Text;

namespace Domain.FileProcessor;

public class AbcFileProcessor : BaseFileProcessor, IFileProcessor
{
    public AbcFileProcessor() : base(3)
    {
        
    }
    public Stream Process(Stream stream)
    {
        MemoryStream outputStream = new MemoryStream();
        stream.Seek(0, SeekOrigin.Begin);
        byte[] block = new byte[3];
        
        while (stream.Read(block, 0, 3) > 0)
        {   
            if (IsLastBlock(stream))
            {
                WriteLastBlock(outputStream);
            }
            else if (IsFirstBlock(stream) | IsHarmlessBlock(block))
            {
                WriteValidBlock(outputStream, block);
            }
            else
            {
                ReplaceInvalidBlock(outputStream);
            }
        }

        // reset the stream position to the beginning
        outputStream.Position = 0;
        return outputStream;
    }

    public bool SupportsFileType(Stream stream)
    {
        // Read the first 3 bytes
        stream.Seek(0, SeekOrigin.Begin);
        byte[] buffer = new byte[3];
        int bytesRead = stream.Read(buffer, 0, 3);
        if (bytesRead != 3)
        {
            // Invalid file
            return false;
        }
        if (CheckValidStart(buffer))
        {
            return false;
        }

        // Read the last 3 bytes
        stream.Seek(-3, SeekOrigin.End);
        bytesRead = stream.Read(buffer, 0, 3);
        if (bytesRead != 3)
        {
            // Invalid file
            return false;
        }
        if (CheckValidFinish(buffer))
        {
            return false;
        }
        return true;
    }
    
    protected override bool IsHarmlessBlock(byte[] block)
    {
        return block[0] == (byte)'A' && block[1] >= (byte)'1' && block[1] <= (byte)'9' && block[2] == (byte)'C';
    }

    protected override bool CheckValidStart(byte[] buffer)
    {
        return buffer[0] != 1 || buffer[1] != 2 || buffer[2] != 3;
    }
    
    protected override bool CheckValidFinish(byte[] buffer)
    {
        return buffer[0] != 7 || buffer[1] != 8 || buffer[2] != 9;
    }
    
    protected override void WriteLastBlock(MemoryStream outputStream)
    {
        outputStream.WriteByte(7);
        outputStream.WriteByte(8);
        outputStream.WriteByte(9);
    }
}