using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace Identity.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Please enter SubCategory Name.")]

        public string SubCategoryName { get; set; }
        public DateTime? createdOn { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        public string CategoryName { get; set; }
        public virtual Category categories { get; set; }
    }
}
