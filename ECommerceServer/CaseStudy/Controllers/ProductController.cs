using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using CaseStudy.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        readonly AppDbContext? _btx;

        public ProductController(AppDbContext context) // injected here
        {
            _btx = context;
        }

        [HttpGet]
        [Route("{braid}")]
        public async Task<ActionResult<List<Product>>> Index(int braid)
        {
            ProductDAO dao = new(_btx!);
            List<Product> itemsForBrand = await dao.GetAllByBrand(braid);
            return itemsForBrand;
        }
    }
}
