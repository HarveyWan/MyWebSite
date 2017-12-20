using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KDMSModel
{
    [Table("MealShop", Schema = "dbo")]
    public class MealShop
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar")]
        public string ShopName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
