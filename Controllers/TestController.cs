using GymMembership.Data;
using GymMembership.Models;
using GymMembership.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymMembership.Controllers
{
    [Route("/api/v1/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPaymentService _midtransVAService;

        public TestController(
            [FromKeyedServices("Midtrans-Virtual-Account")] IPaymentService midtransVAService
        )
        {
            _midtransVAService = midtransVAService;
        }

        [HttpPost("midtrans/va")]
        public async Task<ActionResult<MidtransBCAVAResponseData>> PostTestMidtransVABCA(CreatePaymentData order)
        {
            string transactionId = await _midtransVAService.CreatePayment(new User { }, order);
            return Ok(transactionId);
        }
    }

}