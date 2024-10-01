using Npgsql;
using WebServer.DTOs;
using WebServer.Models;
using WebServer.Resources;

namespace WebServer.Repositories
{
    public class MessagesRepository(ILogger<MessagesRepository> logger, NpgsqlDataSource dataSource)
    {
        private readonly ILogger<MessagesRepository> _logger = logger;
        private readonly NpgsqlDataSource _dataSource = dataSource;

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
                _logger.LogCritical(ex, SharedResources.DbCommandExecutionError);
                throw;
            }

            await connection.CloseAsync();
            return;
        }
    }
}
