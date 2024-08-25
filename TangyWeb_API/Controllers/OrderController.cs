using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Stripe.Checkout;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;
using TangyWeb_API.Helper;
using TangyWeb_API.Service;

namespace TangyWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
      
        private readonly IOrderRepository _orderRepository;
       private readonly IMailService _mailService;
        private readonly IEmailSender _emailSender;
        public OrderController(IOrderRepository orderRepository, IMailService mailService, IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _mailService = mailService;
            _emailSender = emailSender;  
        }
        [HttpPost]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }catch (Exception ex)
            {
                throw;
            }
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderRepository.GetAll());
        }

        [HttpGet("{orderHeaderId}")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if (orderHeaderId == null || orderHeaderId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var orderHeader = await _orderRepository.Get(orderHeaderId.Value);
            if(orderHeader == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(orderHeader);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            try
            {
                var orders = await _orderRepository.GetAllUserOrder(userId);

                if (orders == null || !orders.Any())
                {
                    return NotFound($"No orders found for user with ID {userId}");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        //[HttpGet("{userId}")]
        //public async Task<IActionResult> GetOrderbyUser(string? userId)
        //{
        //    if (userId == null)
        //    {
        //        return BadRequest(new ErrorModelDTO()
        //        {
        //            ErrorMessage = "Invalid Id",
        //            StatusCode = StatusCodes.Status400BadRequest
        //        });
        //    }
        //    var orderHeader = await _orderRepository.GetOrderByUser(userId);
        //    if (orderHeader == null)
        //    {
        //        return BadRequest(new ErrorModelDTO()
        //        {
        //            ErrorMessage = "Invalid Id",
        //            StatusCode = StatusCodes.Status404NotFound
        //        });
        //    }
        //    return Ok(orderHeader);
        //}

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDTO paymentDTO)
        {
            paymentDTO.Order.OrderHeader.OrderDate = DateTime.Now; 
            var result =await  _orderRepository.Create(paymentDTO.Order);
            return Ok(result);
        }

		
		[HttpPost]
		[ActionName("PaymentSuccessful")]
		public async Task<IActionResult> PaymentSuccessful([FromBody] OrderHeaderDTO orderHeaderDTO)
		{
            var service = new SessionService();
            var sessionDetails = service.Get(orderHeaderDTO.SessionId);
            if(sessionDetails.PaymentStatus == "paid")
            {
                var result = await _orderRepository.MarkPaymentSuccessful(orderHeaderDTO.Id);
                string emailContent = $"Cảm ơn anh/chị: {orderHeaderDTO.Name}<br>Mã đơn hàng của bạn: {orderHeaderDTO.Id}<br>Giao đến địa chỉ là: {orderHeaderDTO.StreetAddress} {orderHeaderDTO.City}";
                await _emailSender.SendEmailAsync(orderHeaderDTO.Email, "Cảm ơn bạn đã mua hàng từ Lành Stores", emailContent);
                if (result == null)
                {
                    return BadRequest(new ErrorModelDTO()
                    {
                        ErrorMessage = "Can not mark payment as successfully"
                    }); 
                }
                return Ok(result);
            }
			return BadRequest();
		}
	}
}
