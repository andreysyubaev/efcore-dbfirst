using efcore_dbfirst.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efcore_dbfirst.Service
{
    public class ProductService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Product> Products { get; set; } = new();

        public void GetAll()
        {
            var products = _db.Products.ToList();
            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }

        public ProductService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Product product)
        {
            _db.Products.Add(product);
            Commit();
            Products.Add(product);
        }

        public void Remove(Product product)
        {
            _db.Entry(product)
               .Collection(p => p.Tags)
               .Load();

            product.Tags.Clear();

            _db.Products.Remove(product);
            Commit();

            Products.Remove(product);
        }
    }
}
