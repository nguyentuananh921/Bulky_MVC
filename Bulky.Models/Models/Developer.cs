using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Models
{
    //https://codewithmukesh.com/blog/specification-pattern-in-aspnet-core/
    public class Developer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal EstimatedIncome { get; set; }
        public Address Address { get; set; }
        public int Followers { get; set; } //https://codewithmukesh.com/blog/repository-pattern-in-aspnet-core/
    }
}
