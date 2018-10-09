using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Text;
using NPOI.HPSF;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;

namespace SFO2O.Admin.Common
{
    public class DataTableExporter
    {
        private static readonly DateTime FullDate = new DateTime(2010, 10, 10, 10, 10, 10);
        private const String DefaultDateFormat = "yyyy-MM-dd HH:mm:ss";
        private IWorkbook iworkbook;
        public Encoding Encoding { get; private set; }
        public EnumExcelType ExcelType { get; private set; }

        /// <summary>
        /// DataTable导出到Excel
        /// </summary>
        /// <param name="enumExceltype">Excel文件格式</param>
        public DataTableExporter(EnumExcelType enumExceltype) : this(enumExceltype, Encoding.GetEncoding(936)) { }
        /// <summary>
        /// DataTable导出到Excel
        /// </summary>
        /// <param name="enumExceltype">Excel文件格式</param>
        /// <param name="encoding"></param>
        public DataTableExporter(EnumExcelType enumExceltype, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            switch (enumExceltype)
            {
                case EnumExcelType.XLS:
                    iworkbook = new HSSFWorkbook();
                    break;
                case EnumExcelType.XLSX:
                    iworkbook = new XSSFWorkbook();
                    break;
                default:
                    throw new ArgumentException("参数enumExceltype的值“" + enumExceltype.ToString() + "”无效。");
            }
            this.ExcelType = enumExceltype;
            this.Encoding = encoding;
        }

        #region 右击文件 属性信息
        private void SetSummaryInformation()
        {
            switch (ExcelType)
            {
                case EnumExcelType.XLS:
                    SetXlsSummaryInformation();
                    break;
                case EnumExcelType.XLSX:
                    SetXlsxSummaryInformation();
                    break;
                default:
                    break;
            }
        }

        private void SetXlsSummaryInformation()
        {
            var workbook = (HSSFWorkbook)iworkbook;
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

        private void SetXlsxSummaryInformation()
        {
            var workbook = (XSSFWorkbook)iworkbook;
            var props = workbook.GetProperties();
            var exteProps = props.ExtendedProperties.props.GetProperties();
            exteProps.Company = "NPOI";
            exteProps.Application = "创建程序信息"; //填加xlsx文件创建程序信息
            //exteProps.AppVersion = workbook.GetType().Assembly.GetName().Version.ToString();
            var coreProps = props.CoreProperties.GetUnderlyingProperties();
            coreProps.SetCreatorProperty("文件作者信息"); //填加xlsx文件作者信息
            coreProps.SetLastModifiedByProperty("最后保存者信息"); //填加xlsx文件最后保存者信息
            coreProps.SetDescriptionProperty("作者信息"); //填加xlsx文件作者信息
            coreProps.SetTitleProperty("标题信息"); //填加xlsx文件标题信息
            coreProps.SetSubjectProperty("主题信息"); //填加xlsx文件主题信息
            coreProps.SetCreatedProperty(DateTime.Now);
        }
        #endregion

        /// <summary>
        /// 把数据添加到Excel表格
        /// </summary>
        /// <typeparam name="T">模板类型</typeparam>
        /// <param name="dtSource"></param>
        /// <param name="strSheetName"></param>
        /// <param name="strDateFormat"></param>
        public void AddTable<T>(DataTable dtSource, string strSheetName = null)
        {
            this.AddTable(dtSource, typeof(T), strSheetName);
        }

        /// <summary>
        /// 把数据添加到Excel表格
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="modelType">模型类型</param>
        /// <param name="strSheetName">表格名称</param>
        public void AddTable(DataTable dtSource, Type modelType, string strSheetName = null)
        {
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            var encoding = Encoding;
            var Columns = dtSource.Columns;

            var list = new List<ExportSettting>();

            #region 映射数据模型
            foreach (var property in modelType.GetProperties())
            {
                ColumnAttribute attr = null;
                object[] attrs = property.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    attr = (ColumnAttribute)attrs[0];
                }
                else
                {
                    continue;
                }
                var ColumnName = attr.ColumnName ?? property.Name;
                if (Columns.Contains(ColumnName))
                {
                    var column = Columns[ColumnName];
                    var type = property.PropertyType;
                    if (type.IsGenericType && !type.IsGenericTypeDefinition && (typeof(Nullable<>) == type.GetGenericTypeDefinition()))
                    {
                        //可空类型用基础类型替换
                        type = type.GetGenericArguments()[0];
                    }
                    if (type.IsEnum)
                    {
                        var enumBaseType = type.GetEnumUnderlyingType();
                        var dataType = column.DataType;
                        if (!dataType.IsEquivalentTo(enumBaseType))
                        {
                            throw new ArgumentException(string.Format("枚举类型“{0}”的基础类型“{1}”与数据类型“{2}”不匹配。", type, enumBaseType, dataType));
                        }
                    }
                    var exportSetting = new ExportSettting()
                    {
                        ColumnIndex = column.Ordinal,
                        DisplayName = attr.DisplayName,
                        DateType = type,
                        TypeString = type.ToString()
                    };
                    //取得格式和列宽
                    if (type == typeof(DateTime))
                    {
                        var strFormat = attr.DataFormatString ?? DefaultDateFormat;
                        var dateStyle = iworkbook.CreateCellStyle();
                        var dateFormat = iworkbook.CreateDataFormat();
                        dateStyle.DataFormat = dateFormat.GetFormat(strFormat);
                        exportSetting.CellStyle = dateStyle;
                        exportSetting.ColumnWidth = encoding.GetBytes(FullDate.ToString(strFormat)).Length;
                    }
                    else
                    {
                        exportSetting.ColumnWidth = encoding.GetBytes(exportSetting.DisplayName).Length;
                    }
                    list.Add(exportSetting);
                }
            }
            #endregion

            ISheet sheet;
            //创建表格
            if (string.IsNullOrEmpty(strSheetName))
            {
                sheet = iworkbook.CreateSheet();
            }
            else
            {
                sheet = iworkbook.CreateSheet(strSheetName);
            }

            var intStyle = iworkbook.CreateCellStyle();
            var intFormat = iworkbook.CreateDataFormat();
            intStyle.DataFormat = intFormat.GetFormat("0");
            #region 新建表，填充表头，填充列头，样式
            var headStyle = iworkbook.CreateCellStyle();
            int rowIndex = 0;
            #region 表头及样式
            {
                //var headerRow = sheet.CreateRow(rowIndex++);
                //headerRow.HeightInPoints = 25;
                //headerRow.CreateCell(0).SetCellValue("");
                ////var headStyle = workbook.CreateCellStyle();
                ////headStyle.Alignment = CellHorizontalAlignment.CENTER;
                //var font = workbook.CreateFont();
                //font.FontHeightInPoints = 20;
                //font.Boldweight = 700;
                //headStyle.SetFont(font);
                //headerRow.GetCell(0).CellStyle = headStyle;
                //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, columns.Length - 1));
                ////headerRow.Dispose();
            }
            #endregion

            #region 列头及样式
            {
                var headerRow = sheet.CreateRow(rowIndex++);

                //var headStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                var font = iworkbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.Boldweight = 700;
                headStyle.SetFont(font);

                for (int i = 0; i < list.Count; i++)
                {
                    var headerCell = headerRow.CreateCell(i);
                    headerCell.SetCellValue(list[i].DisplayName);
                    headerCell.CellStyle = headStyle;
                }
            }
            #endregion

            #endregion
            var cellStyle = iworkbook.CreateCellStyle();
            var rowStyle = iworkbook.CreateCellStyle();
            var rows = dtSource.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                #region 填充内容
                var dataRow = sheet.CreateRow(rowIndex++);
                rowStyle.VerticalAlignment = VerticalAlignment.Center;
                dataRow.RowStyle = rowStyle;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.WrapText = false;
                for (int j = 0; j < list.Count; j++)
                {
                    var newCell = dataRow.CreateCell(j);
                    var exportSetting = list[j];
                    var data = row[exportSetting.ColumnIndex];
                    if (data == DBNull.Value)
                    {
                        continue;
                    }
                    var type = exportSetting.DateType;
                    string typeString = exportSetting.TypeString;
                    newCell.CellStyle = cellStyle;
                    string drValue = null;
                    switch (typeString)
                    {
                        case "System.String"://字符串类型
                            drValue = data.ToString();
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            var date = (DateTime)data;
                            var ms = date.Millisecond;
                            if (ms > 0)
                            {
                                //去除毫秒部分，防止四舍五入导致显示不一致
                                date = date.AddMilliseconds(0 - ms);
                            }
                            newCell.SetCellValue(date);
                            newCell.CellStyle = exportSetting.CellStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            newCell.SetCellValue((bool)data);
                            break;
                        case "System.Byte"://整型
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            var drInt = Convert.ToInt64(data);
                            newCell.SetCellValue(drInt);
                            drValue = drInt.ToString();
                            newCell.CellStyle = intStyle;//格式化显示
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            var drFloat = Convert.ToDouble(data);
                            newCell.SetCellValue(drFloat);
                            drValue = drFloat.ToString();
                            break;
                        default:
                            if (type.IsEnum)
                            {
                                var enumDescription = ((Enum)Enum.ToObject(type, data)).GetDescription();
                                newCell.SetCellValue(enumDescription);
                                drValue = enumDescription;
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(drValue))
                    {
                        int intTemp = encoding.GetBytes(drValue).Length;
                        if (intTemp > exportSetting.ColumnWidth)
                        {
                            exportSetting.ColumnWidth = intTemp;
                        }
                    }
                }
                #endregion
            }
            #region 设置列宽
            if (rows.Count > 0)
            {
                //冻结首行
                sheet.CreateFreezePane(0, 1);
                for (int i = 0; i < list.Count; i++)
                {
                    sheet.SetColumnWidth(i, (list[i].ColumnWidth + 1) * 256);
                }
            }
            #endregion
        }

        public void Clear()
        {
            iworkbook.Clear();
        }

        /// <summary>
        /// 导出Excel文件的字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] Export()
        {
            var ms = new MemoryStream();
            iworkbook.Write(ms);
            return ms.ToArray();
        }

        class ExportSettting
        {
            public Int32 ColumnIndex { get; set; }
            public Int32 ColumnWidth { get; set; }
            public String DisplayName { get; set; }
            public Type DateType { get; set; }
            public String TypeString { get; set; }
            public ICellStyle CellStyle { get; set; }
        }
    }

    /// <summary>
    /// 导出列设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// 对应列名
        /// </summary>
        public String ColumnName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// 获取或设置字段值的显示格式。
        /// </summary>
        public String DataFormatString { get; set; }
    }
    /// <summary>
    /// EXCEL 文件格式
    /// </summary>
    public enum EnumExcelType
    {
        /// <summary>
        /// Excel 2003 以及更早的版本
        /// </summary>
        XLS = 0,
        /// <summary>
        /// EXCEL 2007 以及后续的版本
        /// </summary>
        XLSX = 1
    }
}
