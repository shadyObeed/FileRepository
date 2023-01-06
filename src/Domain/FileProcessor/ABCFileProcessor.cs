using System.Text;

namespace Domain.FileProcessor;

public class AbcFileProcessor : IFileProcessor
{
    public Stream Process(Stream stream)
    {
        //todo fix
        /*var newStream = new MemoryStream();
        stream.Seek(3, SeekOrigin.Begin);
        stream.CopyTo(newStream, stream.Length - 6);*/
        
        MemoryStream outPutStream = new MemoryStream();
        stream.Seek(0, SeekOrigin.Begin);
        byte[] block = new byte[3];
        
        while (stream.Read(block, 0, 3) > 0)
        {
            if (IsFirstBlock(stream) | IsLastBlock(stream) | IsHarmlessBlock(block))
            {
                outPutStream.WriteByte(block[0]);
                outPutStream.WriteByte(block[1]);
                outPutStream.WriteByte(block[2]);
            }
            // Block is invalid, replace with "A255C"
            else
            {
                outPutStream.WriteByte(Convert.ToByte('A'));
                outPutStream.WriteByte(255);
                outPutStream.WriteByte(Convert.ToByte('C'));
            }
        }
        
        // reset the stream position to the beginning
        outPutStream.Position = 0;
        return outPutStream;
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
    
    private static bool IsHarmlessBlock(byte[] block)
    {
        return block[0] == 'A' && block[1] >= '1' && block[1] <= '9' && block[2] == 'C';
    }
    
    private static bool IsLastBlock(Stream stream)
    {
        return stream.Length == stream.Position;
    }
    
    private static bool IsFirstBlock(Stream stream)
    {
        return stream.Position == 3;
    }
    
    private static bool CheckValidFinish(byte[] buffer)
    {
        return buffer[0] != 7 || buffer[1] != 8 || buffer[2] != 9;
    }

    private static bool CheckValidStart(byte[] buffer)
    {
        return buffer[0] != 1 || buffer[1] != 2 || buffer[2] != 3;
    }
}