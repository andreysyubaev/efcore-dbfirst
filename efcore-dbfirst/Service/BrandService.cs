using efcore_dbfirst.Models;
using efcore_dbfirst.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efcore_dbfirst.Service
{
    public class BrandService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Brand> Brands { get; set; } = new();

        public void GetAll()
        {
            var brands = _db.Brands.ToList();
            Brands.Clear();
            foreach (var brand in brands)
                Brands.Add(brand);
        }

        public BrandService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Brand brand)
        {
            var _brand = new Brand
            {
                Name = brand.Name,
            };

            _db.Add<Brand>(_brand);
            Commit();
            Brands.Add(_brand);
        }

        public void Remove(Brand brand)
        {
            _db.Remove<Brand>(brand);
            if (Commit() > 0)
                if (Brands.Contains(brand))
                    Brands.Remove(brand);
        }
    }
}
