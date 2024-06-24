using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Core.Donation
{
	public class Donation : BaseEntity
	{
		public string IntentId { get; set; }
		public decimal Amount { get; set; }
		public int PostId { get; set; }
	}
}
