using efcore_dbfirst.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efcore_dbfirst.Service
{
    public class TagService
    {
        private readonly ElectronicsStoreContext _db = DBService.Instance.Context;

        public ObservableCollection<Tag> Tags { get; set; } = new();

        public void GetAll()
        {
            var tags = _db.Tags.ToList();
            Tags.Clear();
            foreach (var tag in tags)
                Tags.Add(tag);
        }

        public TagService()
        {
            GetAll();
        }

        public int Commit() => _db.SaveChanges();

        public void Add(Tag tag)
        {
            var _tag = new Tag
            {
                Name = tag.Name,
            };

            _db.Add<Tag>(_tag);
            Commit();
            Tags.Add(_tag);
        }

        public void Remove(Tag tag)
        {
            _db.Remove<Tag>(tag);
            if (Commit() > 0)
                if (Tags.Contains(tag))
                    Tags.Remove(tag);
        }
    }
}
