using KOPracticeWithApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOPracticeWithApi.Services
{
    public interface IItemService
    {
        List<ItemModel> GetItems();
        ItemModel GetItem(int id);
        int AddItem(ItemModel item);
        int DeleteItem(int id);
        int UpdateItem(ItemModel item);
        bool IsPublished(int id, bool hideShow);
    }
}
