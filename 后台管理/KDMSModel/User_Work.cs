using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDMSModel
{
    public class User_Work
    {
        /// <summary>
        /// Id
        /// </summary>
		public Guid Id { get; set; }
        /// <summary>
        /// 餐券
        /// </summary>
        public string MeaCoupon { get; set; }
        /// <summary>
        /// 编辑内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 达人名字
        /// </summary>
        public string Celebrity { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// created
        /// </summary>
        public DateTime? created { get; set; }
    }
}
