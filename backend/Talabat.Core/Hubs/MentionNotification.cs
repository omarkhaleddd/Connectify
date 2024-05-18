using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Hubs.Interfaces;

namespace Talabat.APIs.Hubs
{
	public class MentionNotification : Hub<INotificationHub>
	{
	}
}
