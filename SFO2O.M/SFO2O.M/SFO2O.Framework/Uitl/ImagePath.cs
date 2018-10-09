﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFO2O.Framework.Uitl
{
    public class ImagePath
    {
        /// <summary>
        /// 转化为小图
        /// </summary>
        /// <param name="orignalPath"></param>
        /// <returns></returns>
        public static string ConvertToProductSmallPath(string orignalPath, int width = 100)
        {
            var path = SFO2O.Framework.Uitl.PathHelper.GetImageUrl(orignalPath).Replace("\\", "/");

            var list = path.Split('.');
            if (list.Length > 2)
            {
                list[list.Length - 2] += "_" + width.ToString();
            }
            return string.Join(".", list);
        }
    }
}
