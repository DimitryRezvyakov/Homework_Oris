using Application.DTOs;
using Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class IndexPageModel
    {
        public List<Category> Categories { get; set; } = new();
        public List<HotelModel> Hotels { get; set; } = new();

        public IndexPageModel(List<HotelModel> hotels, List<HotelTag> tags)
        {
            foreach (var tag in tags)
            {
                if (!Categories.Any(c => c.Name == tag.Type))
                    Categories.Add(new Category() { Name = tag.Type });
                Categories.FirstOrDefault(c => c.Name == tag.Type)!.Items.Add(new Item() { Id = tag.Id, Value = tag.Name});
            }

            Hotels = hotels;
        }
    }

    public class Category
    {
        public string Name { get; set; } = "ND";
        public List<Item> Items { get; set; } = new();
    }

    public class Item
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class HotelModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Raiting { get; set; }
        public string Cost { get; set; }
        public object[] Stars { get; set; }
        public string Image { get; set; }
    }
}
