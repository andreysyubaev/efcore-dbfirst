using efcore_dbfirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efcore_dbfirst.Service
{
    public class CategoryService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Category> Categories { get; set; } = new();

        public void GetAll()
        {
            var categories = _db.Categories.ToList();
            Categories.Clear();
            foreach (var category in categories)
                Categories.Add(category);
        }

        public CategoryService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Category category)
        {
            var _сategory = new Category
            {
                Name = category.Name,
            };

            _db.Add<Category>(_сategory);
            Commit();
            Categories.Add(_сategory);
        }

        public void Remove(Category category)
        {
            _db.Remove<Category>(category);
            if (Commit() > 0)
                if (Categories.Contains(category))
                    Categories.Remove(category);
        }
    }
}
