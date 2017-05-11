using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRD.Core
{
    /// <summary>
    /// 数据更新接口
    /// </summary>
    public interface IUpdateEntity
    {
        /// <summary>
        /// 最后更新时间
        /// </summary>
        DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 是否更新
        /// </summary>
        bool? NeedUpdate { get; set; }
    }
}
