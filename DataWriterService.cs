namespace FileInterface;

public class DataWriterService
{
    private readonly IDataWriter _dataWriter;
    
    public DataWriterService(IDataWriter dataWriter)
    {
        _dataWriter = dataWriter;
    }
    
    public void Log(string message)
    {
        _dataWriter.WriteData(message);
    }
    
    public async Task LogAsync(string message)
    {
        await _dataWriter.WriteDataAsync(message);
    }
}