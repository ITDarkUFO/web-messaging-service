using Npgsql;
using WebServer.DTOs;
using WebServer.Models;

namespace WebServer.Repositories
{
    public class MessagesRepository
    {
        private readonly ILogger<MessagesRepository> _logger;

        private readonly string _connectionString;
        private readonly NpgsqlDataSourceBuilder _dataSourceBuilder;
        private readonly NpgsqlDataSource _dataSource;

        public MessagesRepository(ILoggerFactory loggerFactory, string connectionString)
        {
            _logger = loggerFactory.CreateLogger<MessagesRepository>();

            _connectionString = connectionString;
            _dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            _dataSourceBuilder.UseLoggerFactory(loggerFactory);

            _dataSource = _dataSourceBuilder.Build();
        }

        public async Task<List<ChatMessageDTO>> GetMessages(DateTime startDate, DateTime endDate)
        {
            List<ChatMessageDTO> messages = [];

            using var connection = await _dataSource.OpenConnectionAsync();
            using var command = _dataSource
                .CreateCommand("SELECT * FROM messages WHERE messagetimestamp >= @startDate AND messagetimestamp <= @endDate;");

            command.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.Timestamp, startDate);
            command.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.Timestamp, endDate);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                messages.Add(new()
                {
                    MessageText = reader.GetString(1),
                    MessageTimestamp = reader.GetDateTime(2),
                    MessageIndex = reader.GetInt32(3)
                });
            }

            await connection.CloseAsync();
            return messages;
        }

        public async Task SendMessage(ChatMessage message)
        {
            using var connection = await _dataSource.OpenConnectionAsync();
            using var command = _dataSource
                .CreateCommand("INSERT INTO messages (messagetext, messagetimestamp, messageindex) VALUES (@messageText, @messageTime, @messageIndex);");

            command.Parameters.AddWithValue("messageText", NpgsqlTypes.NpgsqlDbType.Text, message.MessageText);
            command.Parameters.AddWithValue("messageTime", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.Now);
            command.Parameters.AddWithValue("messageIndex", NpgsqlTypes.NpgsqlDbType.Integer, message.MessageIndex);

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Возникла неизвестная ошибка при выполнении команды");
                throw;
            }

            await connection.CloseAsync();
            return;
        }
    }
}
