using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.ViewModels.Product
{
    public class ProductJsonModel
    {
        [JsonProperty("SPU")]
        public SPU SPU;

        [JsonProperty("SKUS")]
        public SKU[] SKUS;

        [JsonProperty("SpuEx")]
        public SpuEx SpuEx;
    }

    public class SPU
    {

        [JsonProperty("Spu")]
        public string Spu;

        [JsonProperty("CategoryId")]
        public int CategoryId;

        [JsonProperty("Name")]
        public TripleDes Name;

        [JsonProperty("Tag")]
        public TripleDes[] Tag;

        [JsonProperty("Price")]
        public double Price;

        [JsonProperty("Description")]
        public TripleDes Description;

        [JsonProperty("BrandId")]
        public int BrandId;

        [JsonProperty("CountryOfManufactureId")]
        public string CountryOfManufactureId;

        [JsonProperty("SalesTerritory")]
        public int SalesTerritory;

        [JsonProperty("Unit")]
        public KeyAndOther Unit;

        [JsonProperty("IsExchangeInCHINA")]
        public int IsExchangeInCHINA;

        [JsonProperty("IsExchangeInHK")]
        public int IsExchangeInHK;

        [JsonProperty("IsReturn")]
        public int IsReturn;

        [JsonProperty("MinForOrder")]
        public int MinForOrder;

        [JsonProperty("NetWeightUnitId")]
        public string NetWeightUnitId;

        [JsonProperty("NetContentUnitId")]
        public string NetContentUnitId;

        [JsonProperty("IsDutyOnSeller")]
        public int IsDutyOnSeller;

        [JsonProperty("Images")]
        public Image[] Images;

        [JsonProperty("PreOnSaleTime")]
        public DateTime? PreOnSaleTime;

        [JsonProperty("CommissionInCHINA")]
        public string CommissionInCHINA;

        [JsonProperty("CommissionInHK")]
        public string CommissionInHK;
    }

    public class SKU
    {

        [JsonProperty("Sku")]
        public string Sku;

        [JsonProperty("MainDicKey")]
        public string MainDicKey;

        [JsonProperty("SubDicKey")]
        public string SubDicKey;

        [JsonProperty("MainKey")]
        public string MainKey;

        [JsonProperty("MainValue")]
        public TripleDes MainValue;

        [JsonProperty("SubKey")]
        public string SubKey;

        [JsonProperty("SubValue")]
        public TripleDes SubValue;

        [JsonProperty("NetWeight")]
        public double NetWeight;

        [JsonProperty("NetContent")]
        public double NetContent;

        [JsonProperty("Specifications")]
        public TripleDes Specifications;

        [JsonProperty("AlcoholPercentage")]
        public TripleDes AlcoholPercentage;

        [JsonProperty("Smell")]
        public TripleDes Smell;

        [JsonProperty("Color")]
        public KeyAndOther Color;

        [JsonProperty("Size")]
        public KeyAndOther Size;

        [JsonProperty("CapacityRestriction")]
        public TripleDes CapacityRestriction;

        [JsonProperty("Price")]
        public double Price;

        [JsonProperty("BarCode")]
        public string BarCode;

        [JsonProperty("AlarmStockQty")]
        public int AlarmStockQty;
    }

    public class SpuEx
    {

        [JsonProperty("Materials")]
        public KeyAndOther Materials;

        [JsonProperty("Pattern")]
        public TripleDes Pattern;

        [JsonProperty("Flavour")]
        public TripleDes Flavour;

        [JsonProperty("Ingredients")]
        public TripleDes Ingredients;

        [JsonProperty("StoragePeriod")]
        public TripleDes StoragePeriod;

        [JsonProperty("StoringTemperature")]
        public TripleDes StoringTemperature;

        [JsonProperty("SkinType")]
        public TripleDes SkinType;

        [JsonProperty("GenderId")]
        public int GenderId;

        [JsonProperty("AgeGroup")]
        public TripleDes AgeGroup;

        [JsonProperty("Model")]
        public TripleDes Model;

        [JsonProperty("BatteryTime")]
        public TripleDes BatteryTime;

        [JsonProperty("Voltage")]
        public TripleDes Voltage;

        [JsonProperty("Power")]
        public TripleDes Power;

        [JsonProperty("Warranty")]
        public TripleDes Warranty;

        [JsonProperty("SupportedLanguage")]
        public SupportedLanguage SupportedLanguage;

        [JsonProperty("PetAgeUnit")]
        public UnitKey PetAgeUnit;

        [JsonProperty("PetType")]
        public KeyAndOther PetType;

        [JsonProperty("PetAge")]
        public TripleDes PetAge;

        [JsonProperty("Location")]
        public TripleDes Location;

        [JsonProperty("Weight")]
        public TripleDes Weight;

        [JsonProperty("WeightUnit")]
        public UnitKey WeightUnit;

        [JsonProperty("Volume")]
        public TripleDes Volume;

        [JsonProperty("VolumeUnit")]
        public UnitKey VolumeUnit;

        [JsonProperty("Length")]
        public TripleDes Length;

        [JsonProperty("LengthUnit")]
        public UnitKey LengthUnit;

        [JsonProperty("Width")]
        public TripleDes Width;

        [JsonProperty("WidthUnit")]
        public UnitKey WidthUnit;

        [JsonProperty("Height")]
        public TripleDes Height;

        [JsonProperty("HeightUnit")]
        public UnitKey HeightUnit;

        [JsonProperty("Flavor")]
        public TripleDes Flavor;
    }

    public class Image
    {

        [JsonProperty("path")]
        public string Path;

        [JsonProperty("SortId")]
        public int SortId;
    }

    public class KeyAndOther
    {

        [JsonProperty("Key")]
        public string Key;

        [JsonProperty("Other")]
        public TripleDes Other;
    }

    public class SupportedLanguage
    {

        [JsonProperty("keys")]
        public string[] Keys;

        [JsonProperty("Others")]
        public TripleDes[] Others;
    }

    public class TripleDes
    {

        [JsonProperty("Content_S")]
        public string ContentS;

        [JsonProperty("Content_T")]
        public string ContentT;

        [JsonProperty("Content_E")]
        public string ContentE;

        public override string ToString()
        {
            return ContentS + "," + ContentT + "," + ContentE;
        }
    }

    public class UnitKey
    {

        [JsonProperty("Key")]
        public string Key;
    }
}
