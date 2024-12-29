using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBilling.Data;
using SimpleBilling.Models.Domain;

namespace SimpleBilling.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BillingDBContext _context;

        public OrderController(BillingDBContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(s=>s.OrderDetails).ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(AddOrderRequestDTO orderRequest)
        {
            // Validate Order Details
            if (orderRequest.OrderDetails == null || !orderRequest.OrderDetails.Any())
            {
                return BadRequest(new { message = "Order must have at least one order detail." });
            }

            var orderId = Guid.NewGuid();

            // Map DTO to Domain Model
            var order = new Order
            {
                Id = orderId,
                OrderDate = orderRequest.OrderDate,
                OrderDetails = orderRequest.OrderDetails.Select(detail => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    CategoryId = detail.CategoryId,
                    ItemId = detail.ItemId,
                    ItemName = detail.ItemName,
                    CategoryName = detail.CategoryName,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    OrderId = orderId, // Use the generated Order ID here
                }).ToList()
            };

            // Attach foreign keys to prevent null-related errors
            foreach (var detail in order.OrderDetails)
            {
                // Ensure the Category exists
                var category = await _context.Categories.FindAsync(detail.CategoryId);
                if (category == null)
                {
                    return BadRequest(new { message = $"Invalid CategoryId: {detail.CategoryId}" });
                }
                detail.Category = category;

                // Ensure the Item exists
                var item = await _context.Items.FindAsync(detail.ItemId);
                if (item == null)
                {
                    return BadRequest(new { message = $"Invalid ItemId: {detail.ItemId}" });
                }
                detail.Item = item;
            }

            // Save to Database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }



        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
