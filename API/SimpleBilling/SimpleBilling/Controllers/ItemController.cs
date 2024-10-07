using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBilling.Data;
using SimpleBilling.Models.Domain;
using SimpleBilling.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleBilling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly BillingDBContext dBContext;
        public ItemController(BillingDBContext dBContext)
        {
            this.dBContext = dBContext;
        }


        [HttpGet]
        public IActionResult GetAllItems()
        {
            var items = dBContext.Items.ToList();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddItem(AddItemRequestDTO request)
        {
            var domainModelItem = new Item
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Unit = request.Unit
            };

            dBContext.Items.Add(domainModelItem);
            dBContext.SaveChanges();

            return Ok(domainModelItem);
        }



        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult GetItemById(Guid id)
        {
            UpdateItemRequestDTO item = new UpdateItemRequestDTO();
            var requestedItem = dBContext.Items.Find(id);
            item.Id = requestedItem.Id;
            item.Name = requestedItem.Name;
            item.CategoryId = requestedItem.CategoryId;
            item.Price = requestedItem.Price;
            item.Unit = requestedItem.Unit;
            return Ok(item);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public ActionResult UpdateItem(Guid id, [FromBody] UpdateItemRequestDTO updateRequest)
        {
            var existingItem = dBContext.Items.Find(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateRequest.Name;
            existingItem.Unit = updateRequest.Unit;
            existingItem.Price = updateRequest.Price;
            existingItem.CategoryId = updateRequest.CategoryId;
            // Map other properties as needed

            dBContext.Entry(existingItem).State = EntityState.Modified;
            dBContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public ActionResult DeleteItem(Guid id)
        {
            var item = dBContext.Items.Find(id);
            if (item is not null)
            {
                dBContext.Items.Remove(item);
                dBContext.SaveChanges();

            }
            return Ok();
        }


    }
}
