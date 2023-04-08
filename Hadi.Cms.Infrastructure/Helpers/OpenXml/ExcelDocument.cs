using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hadi.Cms.Infrastructure.Helpers.OpenXml
{
    public class ExcelDocument
    {
        private static string s_letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private List<ExcelColumn> m_columns;
        private List<string[]> m_rows;
        private uint m_styleIndex;

        public ExcelDocument()
        {
            m_columns = new List<ExcelColumn>();
            m_rows = new List<string[]>();
        }

        public void AddRow(params string[] cells)
        {
            m_rows.Add(cells);
        }
        public void AddColumn(string name, int? width = null)
        {
            var column = new ExcelColumn { Name = name };

            if (width != null)
            {
                column.Width = width;
            }

            m_columns.Add(column);
        }
        public Stream CreateExcelDocument()
        {
            var streamExcel = new MemoryStream();

            var spreadsheetDocument = SpreadsheetDocument.Create(streamExcel, SpreadsheetDocumentType.Workbook);
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            var worksheet = new Worksheet();
            var sheetData = new SheetData();

            // Create header row
            sheetData.AppendChild(CreateRow(1, m_columns.Select(c => c.Name).ToArray()));

            // Create Content rows
            for (var i = 0; i < m_rows.Count; i++)
            {
                sheetData.AppendChild(CreateRow(i + 3, m_rows[i]));
            }

            worksheet.Append(sheetData);
            worksheetPart.Worksheet = worksheet;

            Sheets sheets = new Sheets();
            spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(sheets);

            Sheet sheet1 = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };

            sheets.Append(sheet1);
            workbookPart.Workbook.Save();
            spreadsheetDocument.Close();

            return streamExcel;
        }

        public static string[,] ReadToArray(string excelFileName, bool firstRowIsColumnHeader = true, string sheetName = null)
        {
            using (FileStream stream = File.OpenRead(excelFileName))
            {
                return ReadToArray(excelFileName, firstRowIsColumnHeader, sheetName);
            }
        }
        public static string[,] ReadToArray(Stream excelStream, bool firstRowIsColumnHeader = true, string sheetName = null)
        {
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(excelStream, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheetName == null ? sheets.First().Id.Value : sheets.First(s => s.Name == sheetName).Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();

                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> rows = sheetData.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>().Where(r => r.Descendants<CellValue>().Any());

                // Checking columns/rows count
                int columnsCount = rows.SelectMany(r => r.Descendants<Cell>())
                    .Max(c => GetColumnIndexFromCellReference(c.CellReference.Value)) + 1;

                int rowsCount = (int)rows.Max(r => r.RowIndex.Value);

                // Reading Data
                string[,] values = new string[rowsCount, columnsCount];

                foreach (DocumentFormat.OpenXml.Spreadsheet.Row row in rows) //this will also include your header row...
                {
                    var cells = row.Descendants<Cell>().Where(c => c.Descendants<CellValue>().Any());

                    for (int i = 0; i < cells.Count(); i++)
                    {
                        var cell = cells.ElementAt(i);
                        values[row.RowIndex.Value - 1, GetColumnIndexFromCellReference(cell.CellReference.Value)] = GetCellValue(spreadSheetDocument, cell);
                    }
                }

                return values;
            }
        }
        public static DataTable ReadToDataTable(string excelFileName, bool firstRowIsColumnHeader = true, string sheetName = null)
        {
            using (FileStream stream = File.OpenRead(excelFileName))
            {
                return ReadToDataTable(stream, firstRowIsColumnHeader, sheetName);
            }
        }
        public static DataTable ReadToDataTable(Stream excelStream, bool firstRowIsColumnHeader = true, string sheetName = null)
        {
            string[,] values = ReadToArray(excelStream, firstRowIsColumnHeader, sheetName);

            DataTable dataTable = new DataTable();

            if (values.GetLength(0) > 0)
            {
                // Preparing columns
                for (int i = 0; i < values.GetLength(1); i++)
                    dataTable.Columns.Add(values[0, i]);

                for (int rowIndex = 1; rowIndex < values.GetLength(0); rowIndex++)
                {
                    DataRow row = dataTable.NewRow();

                    for (int columnIndex = 0; columnIndex < values.GetLength(1); columnIndex++)
                    {
                        row[columnIndex] = values[rowIndex, columnIndex];
                    }

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        #region Internal

        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(

                    new Font(                                                               // Index 0 – The default font.
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Arial" }),

                    new Font(                                                               // Index 1 – The bold font.
                        new Bold(),
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),

                    new Font(                                                               // Index 2 – The Italic font.
                        new Italic(),
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),

                    new Font(                                                               // Index 2 – The Times Roman font. with 16 size
                        new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(

                    new Fill(                                                           // Index 0 – The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),

                    new Fill(                                                           // Index 1 – The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),

                    new Fill(                                                           // Index 2 – The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),

                new Borders(

                    new Border(                                                         // Index 0 – The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),

                    new Border(                                                         // Index 1 – Applies a Left, Right, Top, Bottom border to a cell

                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },

                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },

                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },

                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(

                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 – The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 – Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 – Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 – Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 – Yellow Fill
                    new CellFormat(                                                                   // Index 5 – Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    )
                    { FontId = 0, FillId = 0, BorderId = 1, ApplyAlignment = true },
                    new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }) { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 – Border
                )
            ); // return
        }
        private DocumentFormat.OpenXml.Spreadsheet.Row CreateRow(int rowIndex, params string[] values)
        {
            DocumentFormat.OpenXml.Spreadsheet.Row row = new DocumentFormat.OpenXml.Spreadsheet.Row();
            row.RowIndex = (UInt32)rowIndex;

            for (int i = 0; i < values.Length; i++)
            {
                row.AppendChild(CreateCell(s_letters[i].ToString() + rowIndex, values[i]));
            }

            return row;
        }
        private Cell CreateCell(string cellReference, string cellValue)
        {
            Cell cell = new Cell() { StyleIndex = m_styleIndex };
            cell.DataType = CellValues.InlineString;
            cell.CellReference = cellReference;

            InlineString inlineString = new InlineString();
            Text text = new Text();
            text.Text = cellValue;

            inlineString.AppendChild(text);
            cell.AppendChild(inlineString);

            return cell;
        }

        private static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        private static int? GetColumnIndexFromName(string columnName)
        {
            int? columnIndex = null;

            string[] colLetters = Regex.Split(columnName, "([A-Z]+)");
            colLetters = colLetters.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (colLetters.Count() <= 2)
            {
                int index = 0;
                foreach (string col in colLetters)
                {
                    List<char> col1 = colLetters.ElementAt(index).ToCharArray().ToList();
                    int? indexValue = s_letters.IndexOf(col1.ElementAt(index));

                    if (indexValue != -1)
                    {
                        // The first letter of a two digit column needs some extra calculations
                        if (index == 0 && colLetters.Count() == 2)
                        {
                            columnIndex = columnIndex == null ? (indexValue + 1) * 26 : columnIndex + ((indexValue + 1) * 26);
                        }
                        else
                        {
                            columnIndex = columnIndex == null ? indexValue : columnIndex + indexValue;
                        }
                    }

                    index++;
                }
            }

            return columnIndex;
        }
        private static int GetColumnIndexFromCellReference(string cellReference)
        {
            return (int)GetColumnIndexFromName(GetColumnName(cellReference));
        }
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }

        #endregion
    }
}
