using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SRD.Core
{
    /// <summary>
    /// 数据更新
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class UpdateEntity:IUpdateEntity
    {

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 是否更新
        /// </summary>
        [DataMember]
        public bool? NeedUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected UpdateEntity()
        {
            LastUpdateTime = DateTime.UtcNow;
            NeedUpdate = true;
        }
    }
}
