using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDMSModel
{
    public class User_Event
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// EventName
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 事件开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 事件最后处理时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 事件处理状态
        /// </summary>
        public int EventStatus { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 当前操作人
        /// </summary>
        public string NowOperator { get; set; }
        /// <summary>
        /// WorkId
        /// </summary>
        public Guid? WorkId { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
    }
}
