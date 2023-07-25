using BulkyBook.DataAccess.Data.ApplicationDbContext;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;        

        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }   

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db); 
            ProductRepository = new ProductRepository(_db);
            CustomerRepository = new CustomerRepository(_db);
            CompanyRepository = new CompanyRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
