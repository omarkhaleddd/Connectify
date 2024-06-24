using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Core.Donation;

namespace Talabat.Core.Services
{
	public interface IPaymentService
	{
		Task<PaymentIntent> CreateOrUpdatePaymentIntentId(Donation donation);
	}
}
