using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetStockAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MultipleOrigins")]
    public class ProductController : ControllerBase
    {
        // สร้าง Object ของ ApplicationDbContext
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public ActionResult<product> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 100,
            [FromQuery] string? searchQuery = null,
            [FromQuery] int? selectedCategory = null
        )
        {
            // skip คือ การข้ามข้อมูล
            int skip = (page - 1) * limit;

            // LINQ สำหรับการดึงข้อมูลจากตาราง Products ทั้งหมด
            // var products = _context.products.ToList();

            // แบบอ่านที่มีเงื่อนไข
            // select * from products where unitinstock >= 10
            // var products = _context.products.Where(p => p.unitinstock >= 10).ToList();

            // แบบเชื่อมกับตารางอื่น products เชื่อมกับ categories
            var query = _context.products
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

            // ถ้ามีการค้นหา
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => EF.Functions.ILike(p.productname!, $"%{searchQuery}%"));
            }

            // ถ้ามีการค้นหาตาม Category
            if (selectedCategory.HasValue)
            {
                query = query.Where(p => p.categoryid == selectedCategory.Value);
            }

            // นับจำนวนข้อมูลทั้งหมด
            var totalRecords = query.Count();

            var products = query
            .OrderByDescending(p => p.productid)
            .Skip(skip)
            .Take(limit)
            .ToList();

            // ส่งข้อมูลกลับไปให้ Client เป็น JSON
            return Ok(
                new
                {
                    Total = totalRecords,
                    Products = products
                }
            );
        }

        [HttpGet("{id}")]
        public ActionResult<product> GetProduct(int id)
        {
            var product = _context.products
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
            ).FirstOrDefault(p => p.productid == id);


            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<product>> CreateProduct([FromForm] product product, IFormFile? image)
        {
            _context.products.Add(product);

            if (image != null)
            {
                // Create new file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                //set upload folder
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                product.productpicture = fileName;
            }
            else
            {
                product.productpicture = "noimg.jpg";
            }

            _context.SaveChanges();

            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult<product>> UpdateProduct(int id, [FromForm] product product, IFormFile? image)
        {

            // var productData = _context.products.Find(id);
            var exisitngProduct = _context.products.FirstOrDefault(p => p.productid == id);

            if (exisitngProduct == null)
            {
                return NotFound();
            }

            exisitngProduct.productname = product.productname;
            exisitngProduct.unitprice = product.unitprice;
            exisitngProduct.unitinstock = product.unitinstock;
            exisitngProduct.categoryid = product.categoryid;
            exisitngProduct.modifieddate = product.modifieddate;

            if (image != null)
            {
                // Create new file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                //set upload folder
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                if (exisitngProduct.productpicture != "noimg.jpg")
                {
                    System.IO.File.Delete(Path.Combine(uploadFolder, exisitngProduct.productpicture!));
                }

                exisitngProduct.productpicture = fileName;
            }

            _context.SaveChanges();

            return Ok(exisitngProduct);
        }

        [HttpDelete]
        public ActionResult<product> DeleteProduct(int id)
        {
            
            var product = _context.products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.productpicture != "noimg.jpg")
            {
                string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
                System.IO.File.Delete(Path.Combine(uploadFolder, product.productpicture!));
            }

            _context.products.Remove(product);

            _context.SaveChanges();

            return Ok(product);
        }


    }
}