using KOPracticeWithApi.Models;
using KOPracticeWithApi.Models.ViewModel;
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
           /* var items = await _itemService.GetAllItemsAsync();
            var viewModels = items.Select(item => ConvertToViewModel(item)).ToList();
            return Json(viewModels);*/
            List<ItemModel> itemModel = _itemService.GetItems();
            var viewModels = itemModel.Select(item => ConvertToViewModel(item)).ToList();
            return Ok(viewModels);
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

        public ItemViewModel ConvertToViewModel(ItemModel item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                IsPublished = item.IsPublished,
            };
        }

    }
}
