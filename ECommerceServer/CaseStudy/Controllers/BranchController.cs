﻿using CaseStudy.DAL;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        readonly AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet("{lat}/{lng}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Branch>?>> Index(float lat, float lng)
        {
            BranchDAO dao = new(_db);
            return await dao.GetThreeClosestBranches(lat, lng);
        }
    }
}
