using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;

namespace Talabat.Core.Hubs.Interfaces
{
	public interface INotificationHub
	{
		public Task SendNotification(Notification notification);
	}
}
