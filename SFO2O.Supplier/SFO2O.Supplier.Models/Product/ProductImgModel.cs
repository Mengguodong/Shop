﻿using Common.EntLib.DataExtensions.DataMapper.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models.Product
{
    [Serializable]
    [DataContract]
    /// <summary>
    /// ProductImgModel
    /// </summary>
    public class ProductImgModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// SPU
        /// </summary>
        [DataMember(Name = "SPU")]
        [Display(Name = "SPU")]
        public string SPU { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary>
        [DataMember(Name = "ImagePath")]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// ImageType
        /// </summary>
        [DataMember(Name = "ImageType")]
        [Display(Name = "ImageType")]
        public int ImageType { get; set; }

        /// <summary>
        /// SortValue
        /// </summary>
        [DataMember(Name = "SortValue")]
        [Display(Name = "SortValue")]
        public int SortValue { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [DataMember(Name = "CreateTime")]
        [Display(Name = "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Createby
        /// </summary>
        [DataMember(Name = "Createby")]
        [Display(Name = "Createby")]
        public string Createby { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductImgModel> _schema;
        static ProductImgModel()
        {
            _schema = new ObjectSchema<ProductImgModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SPU, "SPU");

            _schema.AddField(x => x.ImagePath, "ImagePath");

            _schema.AddField(x => x.ImageType, "ImageType");

            _schema.AddField(x => x.SortValue, "SortValue");

            _schema.AddField(x => x.CreateTime, "CreateTime");

            _schema.AddField(x => x.Createby, "Createby");
            _schema.Compile();
        }
    }

}
