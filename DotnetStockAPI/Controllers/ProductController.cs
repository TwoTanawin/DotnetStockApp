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
    public class ProductController : ControllerBase
    {
        // สร้าง Object ของ ApplicationDbContext
        private readonly ApplicationDbContext _context;

        // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<product> GetProducts()
        {
            // var products = _context.products.ToList();

            // var products = _context.products.Where(p => p.unitinstock >= 10).ToList();

            //jon categoy table
            var products = _context.products
            .Join(
                _context.categories,
                p => p.categoryid,
                c => c.categoryid,
                (p, c) => new
                {
                    p.productid,
                    p.productname,
                    p.unitprice,
                    p.unitinstock,
                    p.productpicture,
                    p.categoryid,
                    p.createddate,
                    p.modifieddate,
                    c.categoryname
                }
            );

            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<product> GetProduct(int id)
        {
            var product = _context.products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<product> CreateProduct([FromBody] product product)
        {
            _context.products.Add(product);
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpPut]
        public ActionResult<product> UpdateProduct(int id, [FromForm] product product)
        {

            var productData = _context.products.Find(id);

            if (productData == null)
            {
                return NotFound();
            }

            productData.productname = product.productname;
            productData.unitprice = product.unitprice;
            productData.unitinstock = product.unitinstock;
            productData.categoryid = product.categoryid;
            productData.modifieddate = product.modifieddate;

            _context.SaveChanges();

            return Ok(productData);
        }

        [HttpDelete]
        public ActionResult<product> DeleteProduct(int id)
        {
            var product = _context.products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.products.Remove(product);

            _context.SaveChanges();

            return Ok(product);
        }


    }
}