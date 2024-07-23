using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Talabat.APIs.Hubs;

namespace Talabat.Core.Services
{
	public class NotificationService : BackgroundService
	{
		private readonly IHubContext<AccountNotificationHub> _hubContext;

		public NotificationService(IHubContext<AccountNotificationHub> hubContext)
		{
			_hubContext = hubContext;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				// Your background task logic here
				await Task.Delay(1000, stoppingToken); // Example delay, replace with your logic
			}
		}

		public async Task SendNotificationToAll(string message)
		{
			await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
		}
	}

}
