namespace FileInterface;

public class FileDataWriter : IDataWriter
{
    private readonly string _filePath;
    
    public FileDataWriter(string filePath)
    {
        _filePath = filePath;
        
        // Создаем директорию, если не существует
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
    }
    
    public void WriteData(string data)
    {
        // Добавляем временную метку к данным
        var formattedData = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {data}{Environment.NewLine}";
        
        File.AppendAllText(_filePath, formattedData);
        Console.WriteLine($"Данные записаны в файл: {_filePath}");
    }
    
    public async Task WriteDataAsync(string data)
    {
        var formattedData = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {data}{Environment.NewLine}";
        await File.AppendAllTextAsync(_filePath, formattedData);
        Console.WriteLine($"Данные асинхронно записаны в файл: {_filePath}");
    }
}
