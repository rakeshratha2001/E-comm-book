using Book.DataAccess.Repository.IRepository;
using Book.Models;
using BulkyBook.DataAccess.Repository;
using E_comm_book.DataAccess.Data;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class UnitWork : IUnitWork
    {

        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category  { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }


        public UnitWork(ApplicationDbContext db)  
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db); 
            ShoppingCart = new ShoppingCartRepository(db);
             Category= new CategoryRepository(_db);
             Product= new ProductRepository(_db);
            Company = new CompanyRepository(_db);
        } 
        public void Save()
        {
            _db.SaveChanges();
        }


    }
}
