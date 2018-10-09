using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models
{
    [DataContract(Name = "PagedList{0}")]
    [Serializable]
    public class PagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public PagedList()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        public PagedList(IEnumerable<T> contents, int currentIndex, int recordCount, int pageSize)
        {
            ContentList = contents.ToList();
            RecordCount = recordCount;
            PageSize = pageSize;
            CurrentIndex = currentIndex;

        }

        /// <summary>
        /// 记录列表
        /// </summary>
        [DataMember(Name = "ContentList")]
        public List<T> ContentList { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember(Name = "RecordCount")]
        public long RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember(Name = "PageCount")]
        public long PageCount
        {
            get
            {
                return PageSize == 0 ? 0 : RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;
            }
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        [DataMember(Name = "CurrentIndex")]
        public long CurrentIndex { get; set; }

        /// <summary>
        /// 页记录数
        /// </summary>
        [DataMember(Name = "PageSize")]
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "ErrorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// TotalObject
        /// </summary>
        [DataMember(Name = "TotalObject")]
        public T TotalObject { get; set; }
    }
}
