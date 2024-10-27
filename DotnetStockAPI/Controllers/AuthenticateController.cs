using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        //Constructor for initial ApplicationDbContext
        public AuthenticateController(ApplicationDbContext context){
            _context = context;
        }

        // test connection DB
        [HttpGet("testConnectDB")]
        public void TestConnection(){
            if(_context.Database.CanConnect()){
                Response.WriteAsync("Connected");
            }
            else{
                Response.WriteAsync("Not Connected");
            }
        }
    }
}