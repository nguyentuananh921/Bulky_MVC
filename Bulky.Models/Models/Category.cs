using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBook.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]//Server Side Validation
        [DisplayName("Category Name")]//Server Side Validation
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")] //Server Side Validation
        public int DisplayOrder { get; set; }
    }
}
