using Microsoft.AspNetCore.SignalR;
using Talabat.Core.Hubs.Interfaces;
using Talabat.Core.Entities.Core;
using Talabat.Core.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Talabat.APIs.Hubs
{
	public class AccountNotificationHub : Hub<INotificationHub>
	{
	
	}
}