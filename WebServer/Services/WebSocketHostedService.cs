using AutoMapper;
using SharedLibrary.DTOs;
using SharedLibrary.Models;

namespace WebServer.Services
{
    public class WebSocketHostedService
        (ILogger<WebSocketHostedService> logger, IMapper mapper, MessageEventAggregator eventAggregator,
        WebSocketManager webSocketManager) : IHostedService
    {
        private readonly ILogger<WebSocketHostedService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly MessageEventAggregator _eventAggregator = eventAggregator;
        private readonly WebSocketManager _webSocketManager = webSocketManager;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.OnMessageReceived += OnMessageReceived;
            _logger.LogInformation("Служба WebSocketHostedService запущена");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.OnMessageReceived -= OnMessageReceived;
            _logger.LogInformation("Служба WebSocketHostedService остановлена");
            return Task.CompletedTask;
        }

        private async void OnMessageReceived(object? sender, ChatMessage message)
        {
            var messageDTO = _mapper.Map<ChatMessageDTO>(message);
            await _webSocketManager.SendMessageToAllClientsAsync(messageDTO);
        }
    }
}
