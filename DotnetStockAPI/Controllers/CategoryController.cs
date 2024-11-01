using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        // สร้าง Object ของ ApplicationDbContext
        private readonly ApplicationDbContext _context;

        // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        //CRUD Category

        [HttpGet]
        public ActionResult<category> GetCategories()
        {
            //LINQ
            var categories = _context.categories.ToList();

            return Ok(categories);
        }

        //Get by ID
        [HttpGet("{id}")]
        public ActionResult<category> GetCategory(int id)
        {
            var category = _context.categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //Create Method
        [HttpPost]
        public ActionResult<category> AddCategory([FromBody] category category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null.");
            }

            _context.categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }

        // Update
        [HttpPut("{id}")]
        public ActionResult<category> UpdateCategory(int id, [FromBody] category category)
        {
            var cat = _context.categories.Find(id);

            if (cat == null)
            {
                return NotFound();
            }

            cat.categoryname = category.categoryname;
            cat.categorystatus = category.categorystatus;

            _context.SaveChanges();

            return Ok(cat);
        }

        // Delete 
        [HttpDelete("{id}")]
        public ActionResult<category> DeleteCategory(int id)
        {
            var cat = _context.categories.Find(id);

            if (cat == null)
            {
                return NotFound();
            }
            // ลบข้อมูล Category
            _context.categories.Remove(cat); // delete from category where id = 1
            _context.SaveChanges(); // commit

            // ส่งข้อมูลกลับไปให้ Client เป็น JSON
            return Ok(cat);
        }


    }
}