using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.OrderServices.Dtos;
using Store.Services.Services.PaymentServices;
using Stripe;

namespace Store.Web.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        const string endpointSecret = "whsec_d4a8125dbb5396f1befb2072c82888560665ab1362f7ecd9344eb8b4a25c4e52";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(CustomerBasketDto input)
            => Ok(await _paymentService.CreateOrUpdatePaymentIntent(input));

        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent paymentIntent;

                if (stripeEvent.Type == "Payment_intent.payment_failed")
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    _logger.LogInformation("Payment Failed :", paymentIntent.Id);

                    var order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);

                    _logger.LogInformation("order update to payment failed :", order.Id);

                }
                else if (stripeEvent.Type == "Payment_intent.succeeded")
                {
                    if (stripeEvent.Type == "Payment_intent.payment_succeeded")
                    {
                        paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                        _logger.LogInformation("Payment succeeded :", paymentIntent.Id);

                        var order = await _paymentService.UpdateOrderPaymentSuccessed(paymentIntent.Id);

                        _logger.LogInformation("order update to payment succeeded :", order.Id);

                    }
                }
                else if (stripeEvent.Type == "Payment_intent.created")
                {
                    _logger.LogInformation("Payment created");
                }

                else
                {
                    Console.WriteLine("Unhandled event type:{0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest();
            }
        }
    }
}
