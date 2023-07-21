using BulkyBook.DataAccess.Data.ApplicationDbContext;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Customer obj)
        {
            _db.Customers.Update(obj);
        }       
    }
}
