using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBilling.Data;
using SimpleBilling.Models;
using SimpleBilling.Models.Domain;

namespace SimpleBilling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BillingDBContext dBContext;

        public CategoryController(BillingDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = dBContext.Categories.ToList();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(AddCategoryRequestDTO request)
        {
            var domainModelCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            dBContext.Categories.Add(domainModelCategory);
            dBContext.SaveChanges();

            return Ok(domainModelCategory);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetCategoryById(Guid id)
        {
            UpdateCategoryRequestDTO updateCategory = new UpdateCategoryRequestDTO();
            var cat  = dBContext.Categories.Find(id);
            updateCategory.Id = cat.Id; 
            updateCategory.Name = cat.Name;
            return Ok(updateCategory);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public ActionResult UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDTO updateRequest)
        {
            var existingCategory = dBContext.Categories.Find(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = updateRequest.Name;
            // Map other properties as needed

            dBContext.Entry(existingCategory).State = EntityState.Modified;
            dBContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public ActionResult DeleteCategory(Guid id)
        {
            var category = dBContext.Categories.Find(id);
            if (category is not null)
            {
                dBContext.Categories.Remove(category);
                dBContext.SaveChanges();

            }
            return Ok();
        }
    }
}
