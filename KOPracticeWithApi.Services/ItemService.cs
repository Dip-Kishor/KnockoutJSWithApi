using KOPracticeWithApi.Data;
using KOPracticeWithApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOPracticeWithApi.Services
{
    public class ItemService : IItemService
    {
        private readonly KOPracticeContext _context; 
        public ItemService(KOPracticeContext context) 
        {
            _context = context;
        }
        public int AddItem(ItemModel item)
        {
            _context.Items.Add(item);
            return _context.SaveChanges();
        }

        public int DeleteItem(int id)
        {
            ItemModel item = GetItem(id);
            _context.Items.Remove(item);
            return _context.SaveChanges();
        }

        public ItemModel? GetItem(int id)
        {
            return _context.Items.Find(id);
        }

        public List<ItemModel> GetItems()
        {
            return _context.Items.ToList();
        }

        public int UpdateItem(ItemModel item)
        {
            _context.Items.Update(item);
            return _context.SaveChanges();
        }
        public bool IsPublished(int itemId, bool hideShow)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == itemId);
            if(item != null) 
            {
                item.IsPublished = hideShow;
                _context.Items.Where(i => i.IsPublished).ToList();
                _context.Items.Update(item);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

       
    }
}
