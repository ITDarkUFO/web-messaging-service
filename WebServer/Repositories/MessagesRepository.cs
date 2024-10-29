using AutoMapper;
using Npgsql;
using SharedLibrary.DTOs;
using SharedLibrary.Models;
using SharedLibrary.Resources;
using WebServer.Services;

namespace WebServer.Repositories
{
    public class MessagesRepository(ILogger<MessagesRepository> logger, IMapper mapper, MessageEventAggregator eventAggregator, NpgsqlDataSource dataSource)
    {
        private readonly ILogger<MessagesRepository> _logger = logger;
        private readonly MessageEventAggregator _eventAggregator = eventAggregator;
        private readonly NpgsqlDataSource _dataSource = dataSource;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ChatMessageDTO>> GetMessages(DateTime startDate, DateTime endDate)
        {
            List<ChatMessageDTO> messages = [];

            using var connection = await _dataSource.OpenConnectionAsync();
            using var command = _dataSource
                .CreateCommand("SELECT * FROM messages WHERE messagetimestamp >= @startDate AND messagetimestamp <= @endDate;");

            command.Parameters.AddWithValue("startDate", NpgsqlTypes.NpgsqlDbType.TimestampTz, startDate);
            command.Parameters.AddWithValue("endDate", NpgsqlTypes.NpgsqlDbType.TimestampTz, endDate);

            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                _logger.LogCritical(ex, SharedResources.DbCommandExecutionError);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, SharedResources.ExceptionError);
                throw;
            }

            return messages;
        }

        public async Task<ChatMessageDTO> SendMessage(ChatMessage message)
        {
            using var connection = await _dataSource.OpenConnectionAsync();
            using var command = _dataSource
                .CreateCommand("INSERT INTO messages (messagetext, messagetimestamp, messageindex) VALUES (@messageText, @messageTime, @messageIndex);");

            var sendingTime = DateTime.UtcNow;
            message.MessageTimestamp = sendingTime;
            command.Parameters.AddWithValue("messageTime", NpgsqlTypes.NpgsqlDbType.TimestampTz, sendingTime);
            command.Parameters.AddWithValue("messageText", NpgsqlTypes.NpgsqlDbType.Text, message.MessageText);
            command.Parameters.AddWithValue("messageIndex", NpgsqlTypes.NpgsqlDbType.Integer, message.MessageIndex);

            try
            {
                await command.ExecuteNonQueryAsync();
                _eventAggregator.RaiseMessageReceived(message);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogCritical(ex, SharedResources.DbCommandExecutionError);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, SharedResources.ExceptionError);
                throw;
            }

            return _mapper.Map<ChatMessageDTO>(message);
        }
    }
}
