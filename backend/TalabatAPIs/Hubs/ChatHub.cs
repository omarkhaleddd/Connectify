using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Talabat.Core.Entities.Core;
using Talabat.Core.Repositories;
using Talabat.APIs.DTO;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.APIs.Exstentions;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly IGenericRepository<Message> _repositoryMessage;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _manager;

        public ChatHub(ILogger<ChatHub> logger, IGenericRepository<Message> genericRepository, UserManager<AppUser> manager, IMapper mapper)
        {
            _logger = logger;
            _repositoryMessage = genericRepository;
            _mapper = mapper;
            _manager = manager;
        }
        public async Task Send(string senderId, string recieverId, string message)
        {
            var sender = await _manager.GetUserByIdAsync(senderId);
            var reciever = await _manager.GetUserByIdAsync(recieverId);
            var newMessage = new MessageDto
            {
                messageText = message,
                senderId = senderId,
                senderName = "",
                recieverId = recieverId,
                recieverName = "",
                messageDate = DateTime.Now
            };

            if (sender == null)
            {
                newMessage.senderName = "unKnown";
            }
            if (reciever == null)
            {
                newMessage.recieverName = "unKnown";
            }
            newMessage.senderName = sender.UserName;
            newMessage.recieverName = reciever.UserName;

            await Clients.All.SendAsync("RecieveMessage", recieverId, reciever.UserName, message, DateTime.Now);
            var mappedMessage = _mapper.Map<MessageDto, Message>(newMessage);
            await _repositoryMessage.Add(mappedMessage);
            _repositoryMessage.SaveChanges();
        }
        public async Task JoinGroup(string groupName, string userId)
        {
            var user = await _manager.GetUserByIdAsync(userId);
            await Groups.AddToGroupAsync(groupName, user.DisplayName);
            await Clients.OthersInGroup(groupName).SendAsync($"{user.DisplayName} joined the group");

            _logger.LogInformation(Context.ConnectionId);
        }
        public async Task SendMessageToGroup(string groupName, string senderId, string message)
        {
            var sender = await _manager.GetUserByIdAsync(senderId);
            if (sender != null)
            {
                await Clients.Group(groupName).SendAsync("recieveMessageGrp", sender.Id, sender.DisplayName, message);
                MessageDto msg = new MessageDto()
                {
                    senderId = senderId,
                    senderName = sender.UserName,
                    messageText = message,
                    messageDate = DateTime.Now,
                };
                var mappedMsg = _mapper.Map<MessageDto, Message>(msg);
                await _repositoryMessage.Add(mappedMsg);
                _repositoryMessage.SaveChanges();
            }
        }
    }
}
