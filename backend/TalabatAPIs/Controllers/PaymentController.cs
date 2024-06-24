using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Core.Donation;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentController : APIBaseController
	{
		private readonly IPaymentService _paymentService;
		private readonly IMapper _mapper;

		public PaymentController(IPaymentService paymentService, IMapper mapper)
		{
			_paymentService = paymentService;
			_mapper = mapper;
		}

		[HttpPost("create-payment-intent")]
		public async Task<IActionResult> CreatePaymentIntent(DonationDto donationDto)
		{
			var donation = _mapper.Map<DonationDto,Donation>(donationDto);
			var paymentIntent = await _paymentService.CreateOrUpdatePaymentIntentId(donation);
			return Ok(new { clientSecret = paymentIntent.ClientSecret });
		}
	}
}
