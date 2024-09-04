using CaseStudy.DAL;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrandController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        public BrandController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Brand>>> Index()
        {
            BrandDAO dao = new(_ctx!);
            List<Brand> allBrands = await dao.GetAll();
            return allBrands;
        }

    }
}
