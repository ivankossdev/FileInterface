using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
namespace FileInterface;

public class SqliteDataWriter : IDataWriter, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly string _tableName;
    
    public SqliteDataWriter(string databasePath, string tableName = "LogEntries")
    {
        _tableName = tableName;
        
        // Создаем директорию, если не существует
        var directory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
        
        // Создаем подключение к базе данных
        _connection = new SqliteConnection($"Data Source={databasePath}");
        _connection.Open();
        
        // Создаем таблицу, если она не существует
        CreateTableIfNotExists();
    }
    
    private void CreateTableIfNotExists()
    {
        var createTableSql = $@"
            CREATE TABLE IF NOT EXISTS {_tableName} (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Timestamp DATETIME NOT NULL,
                Data TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            )";
        
        using var command = new SqliteCommand(createTableSql, _connection);
        command.ExecuteNonQuery();
    }
    
    public void WriteData(string data)
    {
        var sql = $"INSERT INTO {_tableName} (Timestamp, Data) VALUES (@timestamp, @data)";
        
        using var command = new SqliteCommand(sql, _connection);
        command.Parameters.AddWithValue("@timestamp", DateTime.Now);
        command.Parameters.AddWithValue("@data", data);
        
        command.ExecuteNonQuery();
        Console.WriteLine($"Данные записаны в SQLite таблицу {_tableName}");
    }
    
    public async Task WriteDataAsync(string data)
    {
        var sql = $"INSERT INTO {_tableName} (Timestamp, Data) VALUES (@timestamp, @data)";
        
        await using var command = new SqliteCommand(sql, _connection);
        command.Parameters.AddWithValue("@timestamp", DateTime.Now);
        command.Parameters.AddWithValue("@data", data);
        
        await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Данные асинхронно записаны в SQLite таблицу {_tableName}");
    }
    
    // Метод для чтения всех записей из таблицы (дополнительно)
    public List<(int Id, DateTime Timestamp, string Data)> ReadAllData()
    {
        var result = new List<(int, DateTime, string)>();
        var sql = $"SELECT Id, Timestamp, Data FROM {_tableName} ORDER BY Timestamp";
        
        using var command = new SqliteCommand(sql, _connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            result.Add((
                reader.GetInt32(0),
                reader.GetDateTime(1),
                reader.GetString(2)
            ));
        }
        
        return result;
    }
    
    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}
