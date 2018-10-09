using Common.EntLib.DataExtensions.DataMapper.Schema;
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
    /// ProductExtTempModel
    /// </summary>
    public class ProductExtTempModel
    {

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// SpuId
        /// </summary>
        [DataMember(Name = "SpuId")]
        [Display(Name = "SpuId")]
        public int SpuId { get; set; }

        /// <summary>
        /// Materials
        /// </summary>
        [DataMember(Name = "Materials")]
        [Display(Name = "Materials")]
        public string Materials { get; set; }

        /// <summary>
        /// Pattern
        /// </summary>
        [DataMember(Name = "Pattern")]
        [Display(Name = "Pattern")]
        public string Pattern { get; set; }

        /// <summary>
        /// Flavour
        /// </summary>
        [DataMember(Name = "Flavour")]
        [Display(Name = "Flavour")]
        public string Flavour { get; set; }

        /// <summary>
        /// Ingredients
        /// </summary>
        [DataMember(Name = "Ingredients")]
        [Display(Name = "Ingredients")]
        public string Ingredients { get; set; }

        /// <summary>
        /// StoragePeriod
        /// </summary>
        [DataMember(Name = "StoragePeriod")]
        [Display(Name = "StoragePeriod")]
        public string StoragePeriod { get; set; }

        /// <summary>
        /// StoringTemperature
        /// </summary>
        [DataMember(Name = "StoringTemperature")]
        [Display(Name = "StoringTemperature")]
        public string StoringTemperature { get; set; }

        /// <summary>
        /// SkinType
        /// </summary>
        [DataMember(Name = "SkinType")]
        [Display(Name = "SkinType")]
        public string SkinType { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember(Name = "Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// AgeGroup
        /// </summary>
        [DataMember(Name = "AgeGroup")]
        [Display(Name = "AgeGroup")]
        public string AgeGroup { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        [DataMember(Name = "Model")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        /// <summary>
        /// BatteryTime
        /// </summary>
        [DataMember(Name = "BatteryTime")]
        [Display(Name = "BatteryTime")]
        public string BatteryTime { get; set; }

        /// <summary>
        /// Voltage
        /// </summary>
        [DataMember(Name = "Voltage")]
        [Display(Name = "Voltage")]
        public string Voltage { get; set; }

        /// <summary>
        /// Power
        /// </summary>
        [DataMember(Name = "Power")]
        [Display(Name = "Power")]
        public string Power { get; set; }

        /// <summary>
        /// Warranty
        /// </summary>
        [DataMember(Name = "Warranty")]
        [Display(Name = "Warranty")]
        public string Warranty { get; set; }

        /// <summary>
        /// SupportedLanguage
        /// </summary>
        [DataMember(Name = "SupportedLanguage")]
        [Display(Name = "SupportedLanguage")]
        public string SupportedLanguage { get; set; }

        /// <summary>
        /// PetType
        /// </summary>
        [DataMember(Name = "PetType")]
        [Display(Name = "PetType")]
        public string PetType { get; set; }

        /// <summary>
        /// PetAge
        /// </summary>
        [DataMember(Name = "PetAge")]
        [Display(Name = "PetAge")]
        public string PetAge { get; set; }

        /// <summary>
        /// PetAgeUnit
        /// </summary>
        [DataMember(Name = "PetAgeUnit")]
        [Display(Name = "PetAgeUnit")]
        public string PetAgeUnit { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember(Name = "Location")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        [DataMember(Name = "Weight")]
        [Display(Name = "Weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// WeightUnit
        /// </summary>
        [DataMember(Name = "WeightUnit")]
        [Display(Name = "WeightUnit")]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        [DataMember(Name = "Volume")]
        [Display(Name = "Volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// VolumeUnit
        /// </summary>
        [DataMember(Name = "VolumeUnit")]
        [Display(Name = "VolumeUnit")]
        public string VolumeUnit { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        [DataMember(Name = "Length")]
        [Display(Name = "Length")]
        public decimal Length { get; set; }

        /// <summary>
        /// LengthUnit
        /// </summary>
        [DataMember(Name = "LengthUnit")]
        [Display(Name = "LengthUnit")]
        public string LengthUnit { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [DataMember(Name = "Width")]
        [Display(Name = "Width")]
        public decimal Width { get; set; }

        /// <summary>
        /// WidthUnit
        /// </summary>
        [DataMember(Name = "WidthUnit")]
        [Display(Name = "WidthUnit")]
        public string WidthUnit { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        [DataMember(Name = "Height")]
        [Display(Name = "Height")]
        public decimal Height { get; set; }

        /// <summary>
        /// HeightUnit
        /// </summary>
        [DataMember(Name = "HeightUnit")]
        [Display(Name = "HeightUnit")]
        public string HeightUnit { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary>
        [DataMember(Name = "LanguageVersion")]
        [Display(Name = "LanguageVersion")]
        public int LanguageVersion { get; set; }

        /// <summary>
        /// Flavor
        /// </summary>
        [DataMember(Name = "Flavor")]
        [Display(Name = "Flavor")]
        public string Flavor { get; set; }

        /// <summary>
        /// 表中所有的成员字段
        /// </summary>
        private static readonly ObjectSchema<ProductExtTempModel> _schema;
        static ProductExtTempModel()
        {
            _schema = new ObjectSchema<ProductExtTempModel>();
            _schema.AddField(x => x.Id, "Id");

            _schema.AddField(x => x.SpuId, "SpuId");

            _schema.AddField(x => x.Materials, "Materials");

            _schema.AddField(x => x.Pattern, "Pattern");

            _schema.AddField(x => x.Flavour, "Flavour");

            _schema.AddField(x => x.Ingredients, "Ingredients");

            _schema.AddField(x => x.StoragePeriod, "StoragePeriod");

            _schema.AddField(x => x.StoringTemperature, "StoringTemperature");

            _schema.AddField(x => x.SkinType, "SkinType");

            _schema.AddField(x => x.Gender, "Gender");

            _schema.AddField(x => x.AgeGroup, "AgeGroup");

            _schema.AddField(x => x.Model, "Model");

            _schema.AddField(x => x.BatteryTime, "BatteryTime");

            _schema.AddField(x => x.Voltage, "Voltage");

            _schema.AddField(x => x.Power, "Power");

            _schema.AddField(x => x.Warranty, "Warranty");

            _schema.AddField(x => x.SupportedLanguage, "SupportedLanguage");

            _schema.AddField(x => x.PetType, "PetType");

            _schema.AddField(x => x.PetAge, "PetAge");

            _schema.AddField(x => x.PetAgeUnit, "PetAgeUnit");

            _schema.AddField(x => x.Location, "Location");

            _schema.AddField(x => x.Weight, "Weight");

            _schema.AddField(x => x.WeightUnit, "WeightUnit");

            _schema.AddField(x => x.Volume, "Volume");

            _schema.AddField(x => x.VolumeUnit, "VolumeUnit");

            _schema.AddField(x => x.Length, "Length");

            _schema.AddField(x => x.LengthUnit, "LengthUnit");

            _schema.AddField(x => x.Width, "Width");

            _schema.AddField(x => x.WidthUnit, "WidthUnit");

            _schema.AddField(x => x.Height, "Height");

            _schema.AddField(x => x.HeightUnit, "HeightUnit");

            _schema.AddField(x => x.LanguageVersion, "LanguageVersion");

            _schema.AddField(x => x.Flavor, "Flavor");
            _schema.Compile();
        }
    }
}