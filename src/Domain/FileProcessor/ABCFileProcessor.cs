namespace Domain.FileProcessor;

public class AbcFileProcessor : IFileProcessor
{
    public Stream Process(Stream input)
    {
        throw new NotImplementedException();
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
        if (buffer[0] != 1 || buffer[1] != 2 || buffer[2] != 3)
        {
            // First 3 bytes are not 1, 2, 3
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
        if (buffer[0] != 7 || buffer[1] != 8 || buffer[2] != 9)
        {
            // Last 3 bytes are not 7, 8, 9
            return false;
        }

        return true;
    }
}