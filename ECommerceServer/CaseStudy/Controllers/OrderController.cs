using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Helpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authorization;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal;
            try
            {
                CustomerDAO uDao = new(_ctx);
                Customer? trayOwner = await uDao.GetByEmail(helper.Email);
                OrderDAO tDao = new(_ctx);
                int trayId = await tDao.AddOrder(trayOwner!.Id, helper.Selections!);
                retVal = trayId > 0
                ? "Cart " + trayId + " saved!"
               : "Cart not saved";
            }
            catch (Exception ex)
            {
                retVal = "Cart not saved " + ex.Message;
            }
            return retVal;
        }

        [Route("{email}")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> orders;
            CustomerDAO uDao = new(_ctx!);
            Customer? orderOwner = await uDao.GetByEmail(email);
            OrderDAO tDao = new(_ctx!);
            orders = await tDao.GetAll(orderOwner!.Id);
            return orders;
        }

        [Route("{orderid}/{email}")]
        [HttpGet]
        public async Task<ActionResult<List<OrderDetailsHelper>>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new(_ctx!);
            return await dao.GetOrderDetails(orderid, email);
        }

    }
}
