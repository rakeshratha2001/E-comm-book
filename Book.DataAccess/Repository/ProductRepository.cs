using Book.DataAccess.Repository.IRepository;
using Book.Models;
using E_comm_book.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
          var objfromdb=_db.products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objfromdb != null)
            {
                objfromdb.Title = obj.Title;
                objfromdb.ISBN = obj.ISBN;

                objfromdb.Description = obj.Description;
                objfromdb.Price = obj.Price;
                objfromdb.Price50 = obj.Price50;
                objfromdb.ListPrice = obj.ListPrice;
                objfromdb.Price100 = obj.Price100;
                objfromdb.CategoryId = obj.CategoryId;
                objfromdb.Author = obj.Author;
                if (obj.ImageUrl != null)
                {
                    objfromdb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
