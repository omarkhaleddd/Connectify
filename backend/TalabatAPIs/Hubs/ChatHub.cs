using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Talabat.Core.Entities.Core;
using Talabat.Core.Repositories;
using Talabat.APIs.DTO;

namespace Talabat.APIs.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IGenericRepository<Message> _repositoryMessage;
        private readonly IMapper _mapper;

        public ChatHub(ILogger<ChatHub> logger ,IGenericRepository<Message> genericRepository ,IMapper mapper) {
            _logger = logger;
            _repositoryMessage = genericRepository;
            _mapper = mapper;
        }
        public async Task Send(string userId,string displayName, string message)
        {
            await Clients.All.SendAsync("RecieveMessage",userId,displayName, message , DateTime.Now);

            var newMessage = new MessageDto
            {
                messageText = message,
                userId = userId,
                displayName = displayName,
            };

            var mappedMessage = _mapper.Map<MessageDto, Message>(newMessage);
            await _repositoryMessage.Add(mappedMessage);
            _repositoryMessage.SaveChanges();
        }
    }
}
