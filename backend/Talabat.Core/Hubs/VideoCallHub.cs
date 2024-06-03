using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.APIs.Hubs
{
	public class VideoCallHub : Hub
	{
		private static ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();

		public override async Task OnConnectedAsync()
		{
			var connectionId = Context.ConnectionId;
			await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);
			await base.OnConnectedAsync();
		}

		public async Task RegisterUser(string username)
		{
			Users[username] = Context.ConnectionId;
			await Clients.All.SendAsync("UserListUpdated", Users);
		}

		public async Task InitiateCall(string targetConnectionId, string offer)
		{
			
				await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
			
		}

		public async Task SendAnswer(string targetConnectionId, string answer)
		{
			await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
		}

		public async Task SendIceCandidate(string targetConnectionId, string candidate)
		{
			await Clients.Client(targetConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var username = Users.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
			if (username != null)
			{
				Users.TryRemove(username, out _);
				await Clients.All.SendAsync("UserListUpdated", Users.Keys);
			}
			await base.OnDisconnectedAsync(exception);
		}
	}
}
