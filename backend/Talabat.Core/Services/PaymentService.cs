using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Core.Donation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Core.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IGenericRepository<Donation> _donationRepository;
		private readonly string _secretKey;

		public PaymentService(IConfiguration configuration, IGenericRepository<Donation> donationRepository)
		{
			_configuration = configuration;
			_donationRepository = donationRepository;
			_secretKey = _configuration["StripeKeys:SecretKey"];
			StripeConfiguration.ApiKey = _secretKey;
		}


		public async Task<PaymentIntent> CreateOrUpdatePaymentIntentId(Donation donation)
		{
			var options = new PaymentIntentCreateOptions
			{
				Amount = (long)(donation.Amount * 100),
				Currency = "usd",
				PaymentMethodTypes = new List<string> { "card" },
			};
			var service = new PaymentIntentService();
			var paymentIntent = await service.CreateAsync(options);

			// Store the donation
			donation.IntentId= paymentIntent.Id;
			donation.InsertDate = DateTime.Now;
			await _donationRepository.Add(donation);
			_donationRepository.SaveChanges();

			return paymentIntent;
		}

	}
}
