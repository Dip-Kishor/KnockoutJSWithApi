using KOPracticeWithApi.Models;
using KOPracticeWithApi.Models.ViewModel;
using KOPracticeWithApi.Services;
using IHostingEnvironmentMvc = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KOPracticeWithApi.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IHostingEnvironmentMvc _hostingEnvironment;
        public ItemController(IItemService itemService, IHostingEnvironmentMvc hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
            if (item == null)
            {
                return NotFound();
            }
            var viewModels = ConvertToViewModel(item);
            return Ok(viewModels);
        }
        [HttpPost]
        public IActionResult Add([FromForm]ItemViewModel viewModel,[FromForm] IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                string filePath = Path.Combine(uploads, ImageFile.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                    viewModel.ImageUrl = "/uploads/" + ImageFile.FileName;
                }
            }
            if (ModelState.IsValid)
            {
                var item = ConvertToModel(viewModel);
                 _itemService.AddItem(item);
                return CreatedAtAction(nameof(GetItem), new { id = viewModel.Id }, viewModel);
            }
            return BadRequest(ModelState);
            
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
            existingItem.ImageUrl = itemModel.ImageUrl;
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
                ImageUrl = item.ImageUrl,
                IsPublished = item.IsPublished,
            };
        }
        public ItemModel ConvertToModel(ItemViewModel viewModel)
        {
            return new ItemModel
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                ImageUrl = viewModel.ImageUrl,
                IsPublished = viewModel.IsPublished,
            };
        }


    }
}
