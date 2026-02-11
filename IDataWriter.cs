namespace FileInterface;

public interface IDataWriter
{
    void WriteData(string data);
    Task WriteDataAsync(string data);
}
