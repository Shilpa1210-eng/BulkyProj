
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _db.Products.Where(p => p.CategoryId == categoryId);
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);

        }
    }
}
