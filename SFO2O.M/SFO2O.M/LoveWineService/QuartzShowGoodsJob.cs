using Quartz;
using SFO2O.BLL.Product;
using SFO2O.Model.Product;
using SFO2O.Utility.Uitl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace LoveWineService
{
    public class QuartzShowGoodsJob : IJob
    {

        public QuartzShowGoodsJob()
        {

        }

        ProductBll bll = new ProductBll();
        public void Execute(JobExecutionContext context)
        {
            List<SkuInfo> list = new List<SkuInfo>();
            LogHelper.Info( DateTime.Now + "开始查询所有未上架商品的列表");
            //查询所有待上架的商品列表
            list = bll.GetPreShowSku();

            foreach (var item in list)
            {
              

                //调用bll方法更改上架状态
                if (item.PreOnSaleTime<=DateTime.Now)
                {
                    item.Status = 3;
                    //上架
                    bool result = bll.UpdatePreShowSku(item);
                    //更新库存
                    if (result)
                    {
                        bool stockResult = bll.InsertStock(item);
                    }


                    if (!result)
                    {
                        LogHelper.Info("更新上架状态失败！sku："+item.Sku);
                    }
                }


            }


           
        }

    }
}
