using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Model.Activity
{
    /// <summary>
    /// 专题活动实体
    /// </summary>
    public class ActivityModel
    {
        /// <summary>
        /// 活动ID，主键
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int ID { get; set; }

        /// <summary>
        /// 专题关键词，唯一
        /// </summary>
        [DataMember(Name = "Key")]
        [Display(Name = "Key")]
        public string Key { get; set; }

        /// <summary>
        /// 专题标题
        /// </summary>
        [DataMember(Name = "Title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// 页面头部显示标题
        /// </summary>
        [DataMember(Name = "HeadTitle")]
        [Display(Name = "HeadTitle")]
        public string HeadTitle { get; set; }

        /// <summary>
        /// 专题活动描述
        /// </summary>
        [DataMember(Name = "Discription")]
        [Display(Name = "Discription")]
        public string Discription { get; set; }

        /// <summary>
        /// 专题分享图片地址
        /// </summary>
        [DataMember(Name = "ImgPath")]
        [Display(Name = "ImgPath")]
        public string ImgPath { get; set; }

        /// <summary>
        /// 专题 开始时间
        /// </summary>
        [DataMember(Name = "StartTime")]
        [Display(Name = "StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 专题 结束时间
        /// </summary>
        [DataMember(Name = "EndTime")]
        [Display(Name = "EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 专题 创建时间
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 专题状态 是否启用，1:启用，0：禁用
        /// </summary>
        [DataMember(Name = "Status")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        /// <summary>
        /// TempType:1、普通模板  2、分类模板 3、品牌模板
        /// </summary>
        [DataMember(Name = "TempType")]
        [Display(Name = "TempType")]
        public int TempType { get; set; }

        /// <summary>
        ///SPUs： 代表页面所有的SPU以及模块名称，e.g：
        ///普通模板数据结构：    标题一：spu1,spu2,spu3|标题二：spu1,spu2   
        ///类目模板数据结构：    类目一，xx/xx.html：spu1,spu2|类目2，xx/xx.html:spu1,spu2        
        ///品牌模板数据结构：    品牌名称，id,xxx地址|主推产品：spu1，spu2|更多好物:spu1,spu2 
        /// </summary>
        [DataMember(Name = "SPUs")]
        [Display(Name = "SPUs")]
        public string SPUs { get; set; }

        /// <summary>
        /// 2016.07.25 为专题模板品牌页添加,主推产品的专有描述MainProductDescription 
        /// 数据格式e.g   SPU:对应描述|SPU1:描述1
        /// </summary>
        [DataMember(Name = "MainProductDescription")]
        [Display(Name = "MainProductDescription")]
        public string MainProductDescription { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ActivityModel> _schema;
        static ActivityModel()
        {
            _schema = new ObjectSchema<ActivityModel>();

            _schema.AddField(x => x.ID, "Id");

            _schema.AddField(x => x.Key, "Key");

            _schema.AddField(x => x.Title, "Title");

            _schema.AddField(x => x.HeadTitle, "HeadTitle");

            _schema.AddField(x => x.Discription, "Discription");

            _schema.AddField(x => x.ImgPath, "ImgPath");

            _schema.AddField(x => x.StartTime, "StartTime");

            _schema.AddField(x => x.EndTime, "EndTime");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Status, "Status");

            _schema.AddField(x => x.TempType, "TempType");

            _schema.AddField(x=>x.SPUs,"SPUs");

            _schema.AddField(x => x.MainProductDescription, "MainProductDescription");

            _schema.Compile();
        }
    }
}
