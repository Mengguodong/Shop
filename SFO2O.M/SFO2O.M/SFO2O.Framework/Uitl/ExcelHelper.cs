using System;
using System.Data;
using System.IO;
using System.Web;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Linq;
using NPOI.XSSF.UserModel;
using NPOI.HPSF;

namespace SFO2O.Framework.Uitl
{
    public class ExcelHelper
    {
        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportDataTableFromExcel(string excelFilePath)
        {
            HSSFWorkbook hssfworkbook;
            //excelFilePath = excelFilePath.Replace("http://","").Replace("/","\\");
            using (FileStream file = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);

                NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
             

                DataTable dt = new DataTable();
                for (int j = 0; j < 4; j++)
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
                }

                while (rows.MoveNext())
                {
                    NPOI.SS.UserModel.IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);


                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }

        public static DataTable ConvertToDataTableFromExcel(string excelFilePath,DataTable dt) {
           string FileType =  excelFilePath.Substring(excelFilePath.LastIndexOf(".") + 1).ToLower();
           if (FileType.Equals("xlsx"))
           {
               return GetDataTableByXlsx(excelFilePath, dt);
           }
           else {
               return GetDataTableByXls(excelFilePath, dt);
           }
        }

        private static DataTable GetDataTableByXls(string excelFilePath, DataTable dt)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);

                NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                int r = 0;
                while (rows.MoveNext())
                {
                    r++;
                    if (r == 1)
                    {
                        continue;
                    }

                    NPOI.SS.UserModel.IRow row = (HSSFRow)rows.Current;
                    DataRow rw = dt.NewRow();
                    for (int i = 1; i <= row.LastCellNum-1; i++)
                    {

                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);

                        if (cell == null)
                        {
                            continue;
                        }
                        else
                        {
                            rw[i - 1] = cell.ToString();
                        }

                    }

                    dt.Rows.Add(rw);
                }
                return dt;
            }
        }

        private static DataTable GetDataTableByXlsx(string excelFilePath, DataTable dt)
        {
            XSSFWorkbook hssfworkbook;

            using (FileStream file = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new XSSFWorkbook(file);

                ISheet sheet = hssfworkbook.GetSheetAt(0);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                int r = 0;
                while (rows.MoveNext())
                {
                    r++;
                    if (r == 1)
                    {
                        continue;
                    }

                    NPOI.XSSF.UserModel.XSSFRow row = (NPOI.XSSF.UserModel.XSSFRow)rows.Current;
                    DataRow rw = dt.NewRow();
                    for (int i = 1; i <= row.LastCellNum; i++)
                    {

                        ICell cell = row.GetCell(i);

                        if (cell == null)
                        {
                            continue;
                        }
                        else
                        { 
                            rw[i - 1] = cell.ToString(); 
                        }
                    }

                    dt.Rows.Add(rw);
                }
                return dt;
            }
        }


        /// <summary>
        /// 设置大标题
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        public void SetBigTitle(ISheet sheet, HSSFWorkbook workbook, string bigTitle, int colspan)
        {
            SetBTitle(sheet, workbook, bigTitle, colspan, 0);
        }
        public void SetBigTitle(ISheet sheet, HSSFWorkbook workbook, string bigTitle, int colspan, int rowIndex)
        {
            SetBTitle(sheet, workbook, bigTitle, colspan, rowIndex);
        }

        private void SetBTitle(ISheet sheet, HSSFWorkbook workbook, string bigTitle, int colspan, int rowIndex)
        {
            #region 大标题
            IRow itemList = sheet.CreateRow(rowIndex);
            IFont bigTitleFont = workbook.CreateFont();
            bigTitleFont.FontName = "黑体";
            bigTitleFont.Boldweight = (short)3;// 加粗
            bigTitleFont.FontHeightInPoints = (short)14;// 字体大小   
            ICellStyle bigTitleStyle = workbook.CreateCellStyle();
            bigTitleStyle.SetFont(bigTitleFont);
            var bigTitleCell = CreateCell(itemList, 0, CellType.String, bigTitle);
            bigTitleCell.CellStyle = bigTitleStyle;
            bigTitleStyle.Alignment = HorizontalAlignment.Center;
            bigTitleStyle.VerticalAlignment = VerticalAlignment.Center;
            // 合并单元格            
            CellRangeAddress range = new CellRangeAddress(rowIndex, rowIndex + 1, 0, colspan);
            sheet.AddMergedRegion(range);
            #endregion
        }

        /// <summary>
        /// 设置时间列
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        /// <param name="bigTitle"></param>
        /// <param name="colspan"></param>
        public void SetTimeRow(ISheet sheet, HSSFWorkbook workbook, string bigTitle, int colspan)
        {
            #region 显示报表时间
            IRow itemList = sheet.CreateRow(2);
            IFont bigTitleFont = workbook.CreateFont();
            //bigTitleFont.FontName = "黑体";
            //bigTitleFont.Boldweight = (short)3;// 加粗
            bigTitleFont.FontHeightInPoints = (short)8;// 字体大小   
            ICellStyle bigTitleStyle = workbook.CreateCellStyle();
            bigTitleStyle.SetFont(bigTitleFont);
            var bigTitleCell = CreateCell(itemList, 0, CellType.String, bigTitle);
            bigTitleCell.CellStyle = bigTitleStyle;
            bigTitleStyle.Alignment = HorizontalAlignment.Left;
            bigTitleStyle.VerticalAlignment = VerticalAlignment.Bottom;
            // 合并单元格            
            CellRangeAddress range = new CellRangeAddress(2, 2, 0, colspan);
            sheet.AddMergedRegion(range);
            #endregion
        }

        /// <summary>
        /// 设置每列的小标题
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        /// <param name="subTitle"></param>
        public void SetSubTitle(ISheet sheet, HSSFWorkbook workbook, string subTitle)
        {
            SetSubTitle(sheet, workbook, subTitle, 2);
        }
        /// <summary>
        /// 设置每列的小标题
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook"></param>
        /// <param name="subTitle"></param>
        public void SetSubTitle(ISheet sheet, HSSFWorkbook workbook, string subTitle, int rowindex)
        {
            #region 设置每列的小标题
            var titleRow = sheet.CreateRow(rowindex);
            IFont headfont = workbook.CreateFont();
            headfont.FontHeightInPoints = (short)12;// 字体大小               
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.SetFont(headfont);
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            headStyle.BorderBottom = BorderStyle.Thin;
            headStyle.BorderLeft = BorderStyle.Thin;
            headStyle.BorderRight = BorderStyle.Thin;
            headStyle.BorderTop = BorderStyle.Thin;
            //headStyle.FillBackgroundColor = 100;
            var tableTitle = subTitle;
            var colIndex = 0;
            tableTitle.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(
                title =>
                {
                    var cell = CreateCell(titleRow, colIndex, CellType.String, title);
                    cell.CellStyle = headStyle;
                    colIndex++;
                }
            );
            #endregion
        }
        /// <summary>
        /// 设置列自动适应宽度
        /// </summary>
        /// <param name="sheet"></param>
        public void AutoColumnWidth(ISheet sheet, int maxColumnNum)
        {
            #region
            //获取当前列的宽度，然后对比本列的长度，取最大值  
            for (int columnNum = 0; columnNum <= maxColumnNum; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过  
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 2) * 256);
            }
            #endregion
        }

        /// <summary>
        /// 创建Cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="cellStyle"></param>
        /// <returns></returns>
        public ICell CreateCell(IRow row, int column, CellType type, string value, ICellStyle cellStyle)
        {
            #region
            ICell cell = row.CreateCell(column);
            cell.SetCellType(type);
            cell.SetCellValue(value);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;
            return cell;
            #endregion
        }

        public ICell CreateCell(IRow row, int column, CellType type, string value)
        {
            return CreateCell(row, column, type, value, null);
        }


        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion
            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }
                    #region 表头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }
                    #endregion

                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(1);

                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //headerRow.Dispose();
                    }
                    #endregion
                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion
                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                //sheet.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }
        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }
}
