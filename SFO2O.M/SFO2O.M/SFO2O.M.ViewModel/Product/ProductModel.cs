using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.M.ViewModel.Product
{
    public class ProductModel
    {
        /// <summary>
        /// Id
        /// </summary> 
        public int Id { get; set; }

        /// <summary>
        /// Spu
        /// </summary> 
        public string Spu { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary> 
        public int CategoryId { get; set; }

        /// <summary>
        /// SupplierId
        /// </summary> 
        public int SupplierId { get; set; }

        /// <summary>
        /// Name
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// Tag
        /// </summary> 
        public string Tag { get; set; }

        /// <summary>
        /// ProductPrice
        /// </summary> 
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Description
        /// </summary> 
        public string Description { get; set; }

        /// <summary>
        /// Brand
        /// </summary> 
        public string Brand { get; set; }

        /// <summary>
        /// CountryOfManufacture
        /// </summary> 
        public string CountryOfManufacture { get; set; }

        /// <summary>
        /// SalesTerritory
        /// </summary> 
        public int SalesTerritory { get; set; }

        /// <summary>
        /// Unit
        /// </summary> 
        public string Unit { get; set; }

        /// <summary>
        /// IsExchange
        /// </summary> 
        public int IsExchange { get; set; }

        /// <summary>
        /// IsReturn
        /// </summary> 
        public int IsReturn { get; set; }

        /// <summary>
        /// MinForOrder
        /// </summary> 
        public int MinForOrder { get; set; }

        /// <summary>
        /// MinPrice
        /// </summary> 
        public decimal MinPrice { get; set; }

        /// <summary>
        /// LanguageVersion
        /// </summary> 
        public int LanguageVersion { get; set; }

        /// <summary>
        /// Title
        /// </summary> 
        public string Title { get; set; }

        /// <summary>
        /// ImagePath
        /// </summary> 
        public string ImagePath { get; set; }
    }
}
