namespace Domain.FileProcessor;

public class EfgFileProcessor : BaseFileProcessor, IFileProcessor
{
    public EfgFileProcessor() : base(3)
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
            if (IsFirstBlock(stream) | IsHarmlessBlock(block))
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
            // First 3 bytes are not 5, 6, 7
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
            // Last 3 bytes are not 1, 2, 3
            return false;
        }

        return true;
    }
    
    protected override bool IsHarmlessBlock(byte[] block)
    {
        return block[0] == 'E' && block[1] >= 'a' && block[1] <= 'z' && block[2] == 'G';
    }

    protected override bool CheckValidStart(byte[] buffer)
    {
        return buffer[0] != 5 || buffer[1] != 6 || buffer[2] != 7;
    }
    
    protected override bool CheckValidFinish(byte[] buffer)
    {
        return buffer[0] != 1 || buffer[1] != 2 || buffer[2] != 3;
    }
    
    protected override void WriteLastBlock(MemoryStream outputStream)
    {
        outputStream.WriteByte(Convert.ToByte('1'));
        outputStream.WriteByte(Convert.ToByte('2'));
        outputStream.WriteByte(Convert.ToByte('3'));
    }


}