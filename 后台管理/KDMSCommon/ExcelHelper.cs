using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;
using NPOI.SS.Util;

namespace KDMSCommon
{
    /// <summary>
    /// NPOI操作EXCEL
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 将EXCEL数据导入到DataTable中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="readHead">是否读表头</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, bool readHead)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.Cells.Count;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            int startRow;
            if (readHead)
            {
                startRow = sheet.FirstRowNum;
            }
            else
            {
                startRow = sheet.FirstRowNum + 1;
            }
            for (int i = startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null) dataRow[j] = row.GetCell(j).ToString();
                    }
                    dt.Rows.Add(dataRow);
                }

            }
            return dt;
        }


        /// <summary>
        /// 将EXCEL数据导入到DataTable中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="readHead">是否读表头</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTableForAsset(string filePath, bool readHead)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            int startRow;
            if (readHead)
            {
                startRow = sheet.FirstRowNum;
            }
            else
            {
                startRow = sheet.FirstRowNum + 1;
            }
            for (int i = startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < 14; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).CellType == CellType.Formula)
                            {

                                //dataRow[j] = row.GetCell(j).StringCellValue.ToString();
                                if (j == 11)
                                {
                                    dataRow[j] = row.GetCell(j).StringCellValue;
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                }

                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }

                        }

                    }
                    dt.Rows.Add(dataRow);
                }

            }
            return dt;
        }
        /// <summary>
        /// datatable导出到excel，指定表头信息
        /// </summary>
        /// <param name="HeaderText">excel表头信息，如:{"编号","名称"}</param>
        /// <param name="HeaderField">excel对应的DataTable中字段名称，如{"ID","Name"}</param>
        /// <param name="SourceTable">DataTable数据源</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void ExportToLocal(string[] HeaderText, string[] HeaderField, DataTable SourceTable,
            string fileName)
        {
            if (SourceTable == null)
            {
                return;
            }

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                string sheetName = String.IsNullOrEmpty(SourceTable.TableName) ? "Sheet1" : SourceTable.TableName;
                ISheet sheet = workbook.CreateSheet(sheetName);

                //导出excel表头;
                IRow hRow = sheet.CreateRow(0);
                hRow.HeightInPoints = 25;
                ICellStyle headStyle = workbook.CreateCellStyle();
                headStyle.Alignment = HorizontalAlignment.Center;
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.Boldweight = 700;
                headStyle.SetFont(font);
                for (int m = 0; m < HeaderText.Length; m++)
                {
                    hRow.CreateCell(m).SetCellValue(HeaderText[m]);
                    hRow.GetCell(m).CellStyle = headStyle;
                }
                string tempValue = "";
                for (int i = 0; i < SourceTable.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < HeaderField.Length; j++)
                    {
                        tempValue = SourceTable.Rows[i][HeaderField[j]].ToString();
                        if (HeaderField[j] == "WithdrawStatus")
                        {
                            tempValue = "未提现";
                        }
                        row.CreateCell(j).SetCellValue(tempValue);
                    }
                }
                workbook.Write(stream);
                sheet = null;
                workbook = null;
                stream.Flush();
                stream.Position = 0L;
                stream.Close();
            }

        }


        /// <summary>
        /// 两个表
        /// </summary>
        /// <param name="HeaderText"></param>
        /// <param name="HeaderField"></param>
        /// <param name="SourceTable"></param>
        /// <param name="HeaderText2"></param>
        /// <param name="HeaderField2"></param>
        /// <param name="SourceTable2"></param>
        /// <param name="fileName"></param>
        public static void ExportToLocal2(string[] HeaderText, string[] HeaderField, DataTable SourceTable,
            string[] HeaderText2, string[] HeaderField2, DataTable SourceTable2, string fileName)
        {
            if (SourceTable == null)
            {
                return;
            }

            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                string sheetName = String.IsNullOrEmpty(SourceTable.TableName) ? "Sheet1" : SourceTable.TableName;
                ISheet sheet = workbook.CreateSheet(sheetName);

                //导出excel表头;
                IRow hRow = sheet.CreateRow(0);
                hRow.HeightInPoints = 25;
                ICellStyle headStyle = workbook.CreateCellStyle();
                headStyle.Alignment = HorizontalAlignment.Center;
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 12;
                font.Boldweight = 700;
                headStyle.SetFont(font);
                for (int m = 0; m < HeaderText.Length; m++)
                {
                    hRow.CreateCell(m).SetCellValue(HeaderText[m]);
                    hRow.GetCell(m).CellStyle = headStyle;
                }

                for (int i = 0; i < SourceTable.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < HeaderField.Length; j++)
                    {
                        row.CreateCell(j).SetCellValue(SourceTable.Rows[i][HeaderField[j]].ToString());
                    }
                }




                ////////////
                string sheetName2 = String.IsNullOrEmpty(SourceTable2.TableName) ? "Sheet2" : SourceTable2.TableName;
                ISheet sheet2 = workbook.CreateSheet(sheetName2);

                //导出excel表头;
                IRow hRow2 = sheet2.CreateRow(0);
                hRow2.HeightInPoints = 25;
                ICellStyle headStyle2 = workbook.CreateCellStyle();
                headStyle2.Alignment = HorizontalAlignment.Center;
                IFont font2 = workbook.CreateFont();
                font2.FontHeightInPoints = 12;
                font2.Boldweight = 700;
                headStyle2.SetFont(font2);
                for (int m = 0; m < HeaderText2.Length; m++)
                {
                    hRow2.CreateCell(m).SetCellValue(HeaderText2[m]);
                    hRow2.GetCell(m).CellStyle = headStyle2;
                }

                for (int i = 0; i < SourceTable2.Rows.Count; i++)
                {
                    IRow row2 = sheet2.CreateRow(i + 1);
                    for (int j = 0; j < HeaderField2.Length; j++)
                    {
                        row2.CreateCell(j).SetCellValue(SourceTable2.Rows[i][HeaderField2[j]].ToString());
                    }
                }




                workbook.Write(stream);
                sheet = null;
                sheet2 = null;
                workbook = null;
                stream.Flush();
                stream.Position = 0L;
                stream.Close();
            }

        }


        /// <summary>
        /// 从模板导出Excel
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="HeaderField"></param>
        /// <param name="SourceTable"></param>
        /// <param name="fileName"></param>
        /// <param name="numRow">模板第几行输出</param>
        public static void ExportFromTemplate(string templateFile, string[] HeaderField, DataTable SourceTable,
            string fileName, int numRow)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(templateFile, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
                ISheet sheet = hssfworkbook.GetSheet("Sheet1");

                for (int i = 0; i < SourceTable.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + numRow);
                    for (int j = 0; j < HeaderField.Length; j++)
                    {
                        row.CreateCell(j).SetCellValue(SourceTable.Rows[i][HeaderField[j]].ToString());
                    }
                }
                FileStream file2 = new FileStream(fileName, FileMode.Create);
                hssfworkbook.Write(file2);

                file.Close();
                file2.Close();
            }



        }


        /// <summary>
        /// 自动设置Excel列宽
        /// </summary>
        /// <param name="sheet">Excel表</param>
        private static void AutoSizeColumns(ISheet sheet, int maxColumn)
        {
            //列宽自适应，只对英文和数字有效
            for (int i = 0; i <= maxColumn; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= maxColumn; columnNum++)
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
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
        }

        public static byte[] DataTableToExcel(DataTable dt, string[] HeaderText, string[] HeaderField)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");

            HSSFCellStyle dateStyle = (HSSFCellStyle)book.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)book.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");

            //IRow row = sheet.CreateRow(0);//标题行
            //IRow row1 = sheet.CreateRow(1);//查询条件
            IRow row2 = sheet.CreateRow(0);//表头行

            /******************写入标题行，合并居中*********************/
            //ICell cell = row.CreateCell(0);
            // cell.SetCellValue(title);
            //ICellStyle style = book.CreateCellStyle();
            //style.Alignment = HorizontalAlignment.Center;
            //IFont font = book.CreateFont();
            //font.FontHeightInPoints = 18;
            //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            //font.FontName = "標楷體";
            //style.SetFont(font);
            //cell.CellStyle = style;
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count));


            /******************写入查询条件，合并居左*********************/
            //ICell cel11 = row1.CreateCell(0);
            // cel11.SetCellValue("导出条件：" + exportCondition);
            //ICellStyle style1 = book.CreateCellStyle();
            //style1.Alignment = HorizontalAlignment.Left;
            //IFont font1 = book.CreateFont();
            //font1.FontHeightInPoints = 12;
            //style1.SetFont(font1);
            //style1.WrapText = true;

            //cel11.CellStyle = style1;
            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, dt.Columns.Count));


            ICellStyle style2 = book.CreateCellStyle();
            style2.Alignment = HorizontalAlignment.Center;
            IFont font2 = book.CreateFont();
            font2.FontHeightInPoints = 12;
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            style2.SetFont(font2);

            ICell cellnum = row2.CreateCell(0);
            cellnum.SetCellValue("序号");
            cellnum.CellStyle = style2;
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    ICell cell2 = row2.CreateCell(i + 1);
            //    cell2.SetCellValue(dt.Columns[i].ColumnName);
            //    cell2.CellStyle = style2;
            //}
            string tempValue = "";
            for (int m = 0; m < HeaderText.Length; m++)
            {
                ICell cell2 = row2.CreateCell(m + 1);
                cell2.SetCellValue(HeaderText[m]);
                cell2.CellStyle = style2;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row3 = sheet.CreateRow(i + 1);
                row3.CreateCell(0).SetCellValue(i + 1);
                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                //    row3.CreateCell(j + 1).SetCellValue(dt.Rows[i][j].ToString());
                //}
                for (int j = 0; j < HeaderField.Length; j++)
                {
                    tempValue = dt.Rows[i][HeaderField[j]].ToString();
                    if (HeaderField[j] == "OrderSource")
                    {
                        tempValue = Convert.ToInt32(tempValue) <= 2 ? "线上" : "线下"; ;
                        row3.CreateCell(j + 1).SetCellValue(tempValue);
                    }

                    else if (HeaderField[j] == "DeliveryTime")
                    {
                        System.DateTime dateV;
                        System.DateTime.TryParse(tempValue, out dateV);
                        ICell newCell = row3.CreateCell(j + 1); 
                        newCell.CellStyle = dateStyle; //格式化显示                                              
                        newCell.SetCellValue(dateV);
                    }
                    else if (HeaderField[j] == "TotalNumber")
                    {
                        int intV = 0;
                        int.TryParse(tempValue, out intV);
                        row3.CreateCell(j + 1).SetCellValue(intV);
                    }
                    else if (HeaderField[j] == "OrderAmount" || HeaderField[j] == "AgencyFundAmount")
                    {
                        double doubV = 0;
                        double.TryParse(tempValue, out doubV);
                        row3.CreateCell(j + 1).SetCellValue(doubV);
                    }
                    else
                        row3.CreateCell(j + 1).SetCellValue(tempValue);
                }

            }

            //IRow rowOut = sheet.CreateRow(3 + dt.Rows.Count);//导出人
            //ICell cellOut = rowOut.CreateCell(0);
            // cellOut.SetCellValue("导出人：" + name + ",导出时间：" + DateTime.Now.ToString());

            //ICellStyle styleout = book.CreateCellStyle();
            //styleout.Alignment = HorizontalAlignment.Left;
            //IFont fontout = book.CreateFont();
            //fontout.FontHeightInPoints = 12;
            //styleout.SetFont(fontout);
            //cellOut.CellStyle = styleout;
            //sheet.AddMergedRegion(new CellRangeAddress(dt.Rows.Count, dt.Rows.Count, 0, HeaderField.Length));
            //写入到客户端
            AutoSizeColumns(sheet, HeaderField.Length);
            sheet.SetColumnWidth(0, 5 * 256);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            //HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + excelName + ".xls"));
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            byte[] byteExcel = ms.ToArray();
            book = null;
            ms.Close();
            ms.Dispose();
            return byteExcel;
        }
    }
}
