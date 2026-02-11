namespace FileInterface;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Пример работы с интерфейсом записи данных ===\n");
        
        // 1. Запись в файл
        Console.WriteLine("1. Запись в файл:");
        var fileWriter = new FileDataWriter("logs/data.log");
        var fileService = new DataWriterService(fileWriter);
        
        fileService.Log("Тестовое сообщение в файл");
        await fileService.LogAsync("Асинхронное сообщение в файл");
        
        // 2. Запись в SQLite
        Console.WriteLine("\n2. Запись в SQLite:");
        
        using var sqliteWriter = new SqliteDataWriter("logs/database.db", "Logs");
        var sqliteService = new DataWriterService(sqliteWriter);
        
        sqliteService.Log("Тестовое сообщение в SQLite");
        await sqliteService.LogAsync("Асинхронное сообщение в SQLite");
        
        // 3. Чтение данных из SQLite (демонстрация)
        var allData = sqliteWriter.ReadAllData();
        Console.WriteLine($"\n3. Прочитано {allData.Count} записей из SQLite:");
        
        foreach (var record in allData)
        {
            Console.WriteLine($"   ID: {record.Id}, Время: {record.Timestamp}, Данные: {record.Data}");
        }
        
        // 4. Использование через интерфейс (полиморфизм)
        Console.WriteLine("\n4. Использование полиморфизма:");
        
        List<IDataWriter> writers = new List<IDataWriter>
        {
            new FileDataWriter("logs/polymorph.log"),
            sqliteWriter
        };
        
        foreach (var writer in writers)
        {
            writer.WriteData("Полиморфная запись");
        }
        
        Console.WriteLine("\n=== Завершено ===");
    }
}
