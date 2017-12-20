//extern alias EF;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace KDMSModel
{
    //using EF::System.ComponentModel.DataAnnotations.Schema;
    [Table("tb_Cate", Schema = "dbo")]

    public class Cates
    {
        public List<Cate> LCates { get; set; }
    }
    public class Cate
    {        
        [Key]
        public int Id { get; set; }
        
        [MaxLength(50),Column(TypeName = "nvarchar")]
        public string CateType { get; set; }

        [MaxLength(100), Required(ErrorMessage = "菜名不能为空")]
        [Column(TypeName = "nvarchar")]
        public string CateName { get; set; }

        [Required(ErrorMessage = "价格不能为空")]
        [Column(TypeName = "numeric")]
        public Decimal CatePrice { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public int ShopId { get; set; }
    }
}