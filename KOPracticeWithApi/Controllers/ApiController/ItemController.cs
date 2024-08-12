using KOPracticeWithApi.Models;
using KOPracticeWithApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KOPracticeWithApi.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult GetItems()
        {
            List<ItemModel> itemModel = _itemService.GetItems();
            return Ok(itemModel);
        }
        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            ItemModel item = _itemService.GetItem(id);
            return Ok(item);
        }
        [HttpPost]
        public IActionResult Add(ItemModel itemModel)
        {
            _itemService.AddItem(itemModel);
            return CreatedAtAction(nameof(GetItem), new { id = itemModel.Id }, itemModel);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, ItemModel itemModel)
        {
            if (id != itemModel.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the body.");
            }


            var existingItem = _itemService.GetItem(id);
            if (existingItem == null)
            {
                return NotFound("The item was not found.");
            }


            existingItem.Name = itemModel.Name;
            existingItem.Price = itemModel.Price;
            existingItem.IsPublished = itemModel.IsPublished;

            _itemService.UpdateItem(existingItem);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _itemService.GetItem(id);
            if (item == null)
                return NotFound();
            _itemService.DeleteItem(id);
            return NoContent();
        }
    }
}
