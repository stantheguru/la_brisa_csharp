using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace la_brisa.Models
{
    public class Holiday
    {
        [Key]
        public int HolidayID { get; set; }

        [Required]
        public string HolidayName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
