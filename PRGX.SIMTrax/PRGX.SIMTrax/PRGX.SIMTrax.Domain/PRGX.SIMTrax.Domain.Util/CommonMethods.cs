using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace PRGX.SIMTrax.Domain.Util
{
    public static class CommonMethods
    {
        public static string GetEmailTemplate()
        {
            string emailTemplate = @"<div style='border-collapse:Black;border-width:1px;border-style:solid;border-left-width:0px;border-right-width:0px;border-left-color:#0090C6;border-right-color:#0090C6;
                                        background-image: url(cid:email_border),url(cid:email_border); background-size: 20px 100%;
                                        background-repeat: no-repeat; background-position: left,right;'>
                                        <div id='cipsheader'>
                                            <div style='padding-left: 25px; -webkit-box-sizing: border-box; -moz-box-sizing: border-box;
                                                -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box;'>
                                                <img src='cid:email_cips_logo'
                                                    alt='SIM LOGO' /></div>
                                            <br />
                                        </div>
                                        <div id='cipsmessage'>
                                            {0}
                                        </div>
                                        <div style='width: 100%; background-color: #3D3D3D; color: White;'>
                                            <div style='padding-top: 15px; padding-left: 25px; -webkit-box-sizing: border-box;
                                                -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box;
                                                box-sizing: border-box;'>
                                                <img src='cid:email_prgx_logo' alt='Delivered By PRGX' />
                                            </div>
                                            <hr />
                                            <div style='text-align: center; font-family: Times New Roman; color: #EFEFEF;height:30px'>
                                                This is an automated email, Please do not reply to this mail. 
                                            </div>
                                        </div>
                                      </div>
                                    ";

            emailTemplate = @"<table width='100%' cellpadding='0' cellspacing='0'>
    <tr>
        <td style='width: 17px; background-color: #F2F2F2'></td>
        <td>


            <table width='100%' cellspacing='0' cellpadding='0'>
                <tr style='padding-left: 25px; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box;'>
                    <td style='height: 154px'>
                        <img src='cid:email_SIM_logo' style='margin-left: 25px; max-width: 286px;' alt='SIM LOGO' />
                    </td>
                </tr>
                <tr>
                    <td>{0}
                    </td>
                </tr>
                <tr style='background-color: #3D3D3D; color: White; margin-left: 20px; margin-right: 20px; height: 54px;'>
                    <td style='padding-top: 15px; -webkit-box-sizing: border-box; -moz-box-sizing: border-box; -o-box-sizing: border-box; -ms-box-sizing: border-box; box-sizing: border-box;'>
                        <img src='cid:email_prgx_logo' style='margin-left: 25px; max-width: 286px;' alt='Delivered By PRGX' />
                    </td>
                </tr>
                <tr style='background-color: #3D3D3D;'>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr style='background-color: #3D3D3D; height: 41px'>
                    <td style='text-align: center; font-family: Helvetica; color: #EFEFEF; font-size: 13px;'>This is an automated email, please do not reply to this email. 
                    </td>
                </tr>
            </table>

        </td>
        <td style='width: 17px; background-color: #F2F2F2'></td>
    </tr>
    <br />
</table>
";

            return emailTemplate;

        }

        public static void CreateFileFolder(string folderPath)
        {
            Logger.Info("CommonMethods.CreateFilePath(): Entering the method.");
            try
            {
                string fileServerMapPath = System.Web.HttpContext.Current.Server.MapPath(folderPath);
                // Create the directory (on server) if it's doesn't exists
                if (!Directory.Exists(fileServerMapPath))
                    Directory.CreateDirectory(fileServerMapPath);
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.CreateFilePath(): Caught an exception." + ex);
                throw;
            }
            Logger.Info("CommonMethods.CreateFilePath(): Exiting the method.");
        }

        public static DataTable GetDataListFromExcel(string fileExtension, string fileLocation)
        {
            Logger.Info("CommonMethods.GetDataListFromExcel() : Entering the method");

            string conStr = "";
            switch (fileExtension)
            {
                case ".xls": //Excel 97-03
                    conStr = Configuration.Excel97ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = Configuration.Excel07ConnectionString;
                    break;
            }
            conStr = string.Format(conStr, fileLocation);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dataList = new DataTable();
            cmdExcel.Connection = connExcel;
            try
            {
                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetNameWithDollarSuffix = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString().Trim().TrimStart('\'').TrimEnd('\'');
                connExcel.Close();
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + sheetNameWithDollarSuffix + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataList);
                connExcel.Close();
                Logger.Info("CommonMethods.GetDataListFromExcel() : Exiting the method");
                return dataList;
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.GetDataListFromExcel() : Caught an exception" + ex);
                throw ex;
            }
            finally
            {
                if (connExcel.State != ConnectionState.Closed || null == connExcel)
                {
                    connExcel.Dispose();
                    connExcel.Close();
                }
            }
        }

        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static List<ItemList> EnumDropDownList(this Type enumType)
        {

            var list = new List<ItemList>();
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                var listItem = new ItemList();
                var field = enumType.GetField(item.ToString());
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                listItem.Text = attributes.Length == 0
                ? item.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
                listItem.Value = (int)Enum.Parse(enumType, item.ToString());
                list.Add(listItem);
            }
            return list;
        }

        public static string passphrase = "ShriRam";

        public static String EncryptMD5Password(string text)
        {
            Logger.Info(" CommonMethods.EncryptMD5Password(): Entering the method");
            String encryptedString = text;
            //Object / Varibles creation
            Byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            try
            {
                //encrypt the string (get the hash value)                
                hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(text));
                encryptedString = BitConverter.ToString(hashedDataBytes);
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.EncryptMD5Password() : Caught an exception" + ex);
            }
            Logger.Info("CommonMethods.EncryptMD5Password(): Exiting the method");
            return encryptedString;
        }

        public static bool DeleteFileOnServer(string filepath)
        {
            Logger.Info("CommonMethods.DeleteFileOnServer() : Entering the method");
            bool result = false;
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.DeleteFileOnServer() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CommonMethods.DeleteFileOnServer() : Exiting the method");
            return result;
        }
        public static bool DeleteDirectoryOnServer(string directoryPath)
        {
            Logger.Info("CommonMethods.DeleteDirectoryOnServer() : Entering the method");
            bool result = false;
            try
            {
                if (System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.Delete(directoryPath,true);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.DeleteDirectoryOnServer() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CommonMethods.DeleteDirectoryOnServer() : Exiting the method");
            return result;
        }

        public static bool CheckSaveDeleteFileOnServer(string filepath)
        {
            Logger.Info("CommonMethods.CheckSaveDeleteFileOnServer() : Entering the method");
            bool result = false;
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                System.Web.HttpContext.Current.Request.Files[0].SaveAs(filepath);
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Error("CommonMethods.CheckSaveDeleteFileOnServer() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CommonMethods.CheckSaveDeleteFileOnServer() : Exiting the method");
            return result;
        }


        public static string GetUniqueKey(Random random)
        {
            //
            string alphanemericString = RandomString(5, random);
            string numberString = RandomNumberString(2, random);
            string specialCharacter = RandomSpecialCharacter(random);
            // return password
            return (alphanemericString + specialCharacter + numberString);
        }

        private static string RandomSpecialCharacter(Random random)
        {
            string str = "@$%!&_#";
            //Random random = new Random();
            string ch;
            ch = str[random.Next(str.Length)].ToString();
            return ch;

        }

        private static string RandomString(int size, Random random)
        {
            StringBuilder builder = new StringBuilder();
            //Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                int asciiToAdd = (i % 2 == 0) ? 65 : 97;
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + asciiToAdd)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static string RandomNumberString(int size, Random random)
        {
            StringBuilder builder = new StringBuilder();
            //Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static MemoryStream CreateDownloadExcel<T>(List<T> list, string include = "", string exclude = "", string sheetName = "Sheet1", string excelHeading = "Report", List<ExcelDownloadFilterList> filtersList = null)
        {
            var stream = new MemoryStream();
            try
            {
                string sourceFilePath = System.Web.HttpContext.Current.Server.MapPath(Constants.GENERIC_EXCEL_WORKBOOK_PATH);
                // check begin
                string fileTemplate = sourceFilePath;
                using (FileStream file = new FileStream(fileTemplate, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook hssfWorkBook = new HSSFWorkbook(file);
                    ISheet mySheet = hssfWorkBook.GetSheet("Sheet1");
                    hssfWorkBook.SetSheetName(hssfWorkBook.GetSheetIndex("Sheet1"), sheetName);
                    int count = typeof(T).GetProperties().Count();
                    PropertyInfo[] props = typeof(T).GetProperties();
                    List<PropertyInfo> propList = GetSelectedProperties(props, include, exclude);
                    int k = 1, j = 1;

                    mySheet.AddMergedRegion(new CellRangeAddress(k, k + 2, j, propList.Count));
                    IRow reportHeading = mySheet.CreateRow(mySheet.LastRowNum + 1);
                    ICell headingCell = reportHeading.CreateCell(j);
                    headingCell.SetCellValue(excelHeading);
                    ICellStyle cellStyleHeading = mySheet.Workbook.CreateCellStyle();
                    IFont headingFont = mySheet.Workbook.CreateFont();
                    headingFont.Boldweight = (short)FontBoldWeight.Bold;
                    headingFont.FontHeightInPoints = 16;
                    cellStyleHeading.SetFont(headingFont);
                    cellStyleHeading.VerticalAlignment = VerticalAlignment.Center;
                    headingCell.CellStyle = cellStyleHeading;

                    k = k + 3;
                    mySheet.AddMergedRegion(new CellRangeAddress(k, k, j, propList.Count));
                    IRow summary = mySheet.CreateRow(k);
                    ICell summaryCell = summary.CreateCell(j);
                    ICellStyle cellStyleSummary = mySheet.Workbook.CreateCellStyle();
                    cellStyleSummary.FillForegroundColor = HSSFColor.Grey50Percent.Index;
                    cellStyleSummary.FillPattern = FillPattern.SolidForeground;
                    IFont summaryFont = mySheet.Workbook.CreateFont();
                    summaryFont.Color = HSSFColor.White.Index;
                    summaryFont.Boldweight = (short)FontBoldWeight.Bold;
                    cellStyleSummary.SetFont(summaryFont);
                    summaryCell.CellStyle = cellStyleSummary;
                    if (filtersList != null && filtersList.Count > 0)
                    {
                        summaryCell.SetCellValue("Filter Summary");
                        int summaryCount = mySheet.LastRowNum + 1;
                        foreach (var filter in filtersList)
                        {
                            IRow summaryRow = mySheet.CreateRow(summaryCount);
                            ICell summaryTypeCell = summaryRow.CreateCell(j);
                            summaryTypeCell.SetCellValue(filter.FilterType);
                            if ((j + 1) <= propList.Count)
                            {
                                mySheet.AddMergedRegion(new CellRangeAddress(summaryCount, summaryCount, j + 1, propList.Count));
                            }
                            else
                            {
                                mySheet.AddMergedRegion(new CellRangeAddress(summaryCount, summaryCount, j + 1, j + 2));
                            }
                            ICell summaryNameCell = summaryRow.CreateCell(j + 1);
                            summaryNameCell.SetCellValue(filter.FilterValue);
                            summaryNameCell.CellStyle.WrapText = true;
                            summaryCount++;
                        }
                    }
                    else
                    {
                        summaryCell.SetCellValue("No filters applied");
                    }

                    k = mySheet.LastRowNum + 2;
                    IRow runningRow1 = mySheet.CreateRow(k);
                    ICell runningCell1 = null;
                    var userCulture = (System.Globalization.CultureInfo)System.Web.HttpContext.Current.Session[Constants.USER_PREFERENCE_CULTURE];
                    foreach (var prop in propList)
                    {
                        IEnumerable<DisplayAttribute> displayAttributes = prop.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>();
                        var disAtt = "";
                        foreach (DisplayAttribute displayAttribute in displayAttributes)
                        {
                            disAtt = ReadResource.GetResourceForGlobalization(displayAttribute.Name, userCulture);
                        }
                        ICellStyle cellStyle = mySheet.Workbook.CreateCellStyle();
                        cellStyle.FillForegroundColor = HSSFColor.Grey50Percent.Index;
                        cellStyle.FillPattern = FillPattern.SolidForeground;
                        IFont font = mySheet.Workbook.CreateFont();
                        font.Color = HSSFColor.White.Index;
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        cellStyle.SetFont(font);
                        runningCell1 = runningRow1.CreateCell(j++);
                        runningCell1.SetCellValue(disAtt.ToString());
                        runningCell1.CellStyle = cellStyle;
                    }
                    //storing values of the list into the excel
                    IDataFormat dataFormatCustom = hssfWorkBook.CreateDataFormat();
                    ICellStyle cellDateStyle = mySheet.Workbook.CreateCellStyle();
                    cellDateStyle.DataFormat = dataFormatCustom.GetFormat("dd-MM-yyyy");
                    IDataFormat doubleFormatCustom = hssfWorkBook.CreateDataFormat();
                    ICellStyle cellDoubleStyle = mySheet.Workbook.CreateCellStyle();
                    cellDoubleStyle.DataFormat = doubleFormatCustom.GetFormat("0.00");
                    foreach (var item in list)
                    {
                        IRow runningRow = mySheet.CreateRow(++k);
                        ICell runningCell = null;
                        //Iterate through property collection for columns
                        j = 1;
                        foreach (var prop in propList)
                        {
                            var propVal = item.GetType().GetProperty(prop.Name).GetValue(item);
                            // create cell content
                            var dataType = item.GetType().GetProperty(prop.Name).PropertyType.GetTypeInfo();
                            runningCell = runningRow.CreateCell(j++);
                            if (propVal != null)
                            {
                                if (dataType == typeof(string))
                                {
                                    string cellVal = Convert.ToString(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                }
                                else if (dataType == typeof(Int64))
                                {
                                    long cellVal = Convert.ToInt64(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                }
                                else if (dataType == typeof(Int16))
                                {
                                    short cellVal = Convert.ToInt16(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                }
                                else if (dataType == typeof(Int32))
                                {
                                    int cellVal = Convert.ToInt32(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                }
                                else if (dataType == typeof(bool))
                                {
                                    bool cellVal = Convert.ToBoolean(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                }
                                else if (dataType == typeof(DateTime) || dataType == typeof(Nullable<DateTime>))
                                {
                                    Nullable<DateTime> cellVal = (Nullable<DateTime>)(propVal);
                                    runningCell.SetCellValue(cellVal.GetValueOrDefault());
                                    runningCell.CellStyle = cellDateStyle;
                                }
                                else if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
                                {
                                    double cellVal = Convert.ToDouble(Convert.ChangeType(propVal, dataType));
                                    runningCell.SetCellValue(cellVal);
                                    runningCell.CellStyle = cellDoubleStyle;
                                }
                                else
                                {
                                    runningCell.SetCellValue(propVal.ToString());
                                }
                                runningCell.CellStyle.WrapText = true;
                            }
                            else
                            {
                                runningCell.SetCellValue("");
                            }
                            //runningCell.CellStyle.WrapText = true;
                        }
                    }
                    for (int col = 1; col <= propList.Count; col++)
                    {
                        mySheet.AutoSizeColumn(col, true);
                        var width = mySheet.GetColumnWidth(col);
                        if (width <= 25600)
                        {
                            mySheet.SetColumnWidth(col, width + 256);
                        }
                        else
                        {
                            mySheet.SetColumnWidth(col, 25600);
                        }
                    }
                    hssfWorkBook.Write(stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stream;
        }

        private static List<PropertyInfo> GetSelectedProperties(PropertyInfo[] props, string include, string exclude)
        {
            List<PropertyInfo> propList = new List<PropertyInfo>();
            if (include != "") //Do include first
            {
                var includeProps = include.ToLower().Split(',').ToList();
                foreach (var item in props)
                {
                    var propName = includeProps.Where(a => a.Trim() == item.Name.ToLower()).FirstOrDefault();
                    if (!string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else if (exclude != "") //Then do exclude
            {
                var excludeProps = exclude.ToLower().Split(',');
                foreach (var item in props)
                {
                    var propName = excludeProps.Where(a => a.Trim() == item.Name.ToLower()).FirstOrDefault();
                    if (string.IsNullOrEmpty(propName))
                        propList.Add(item);
                }
            }
            else //Default
            {
                propList.AddRange(props.ToList());
            }
            return propList;
        }

        public static string GetBaseUrl()
        {
            var port = string.Empty;
            if (Configuration.Environment != Constants.ENVIRONMENT_PROD)
            {
                port = System.Web.HttpContext.Current.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + System.Web.HttpContext.Current.Request.Url.Port;
            }
            var url = string.Format("{0}://{1}{2}", System.Web.HttpContext.Current.Request.Url.Scheme,
                                    System.Web.HttpContext.Current.Request.Url.Host, port);
            if (!url.EndsWith("/"))
                url += "/";
            return url;
        }

        public static bool IsVATApplicable(string country, bool isVatPresent) 
        {
            var isVATApplicable = true;
            string[] countryEU = new string[]{"Austria", "Azores", "Belgium", "Bulgaria", "Canary Islands", "Croatia", "Cyprus", "Czech Republic", 
                "Denmark", "Estonia", "Finland", "France", "Germany", "Gibraltar", "Greece", "Hungary", "Ireland", "Italy",
                "Latvia", "Lithuania", "Luxembourg", "Madeira", "Malta", "Netherlands", "Poland", "Portugal","Romania", "Slovakia", 
                "Slovenia", "Spain", "Sweden", "Isle Of Man"};
            //int[] countryEUCodes = new int[] { 124, 131, 143, 164, 166, 167, 168, 177, 182, 183, 190, 192, 193, 208, 214, 215, 217, 230, 236, 237, 245, 263, 284, 285, 289, 306, 307, 312, 318 };
            if (countryEU.Contains(country) && isVatPresent)
            {
                isVATApplicable = false;
            }
            return isVATApplicable;
        }
        
        public static string global_encryption_key = "ShreeRamFromRaremile";
        public static string Encrypt(string toEncrypt)
        {
            string EncryptionKey = global_encryption_key;// Configuration.EncriptionKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(toEncrypt);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    toEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }

            return toEncrypt;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = global_encryption_key;// Configuration.EncriptionKey;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static bool IsValidFileSignature(byte[] file, string fileName)
        {
            bool isValidFileSignature = false;
            string mimeType = GetMimeType(file, fileName);
            List<string> fileContentType = new List<string> { "application/pdf", ".DOCX/.PPTX/.XLSX", ".DOC/.PPT/.XLS", "image/jpeg", "image/png", "image/gif" };
            if (fileContentType.Contains(mimeType))
            {
                isValidFileSignature = true;
            }
            return isValidFileSignature;
        }

        #region FileSignatures
        private static readonly byte[] BMP = { 66, 77 };
        private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
        private static readonly byte[] EXE_DLL = { 77, 90 };
        private static readonly byte[] GIF = { 71, 73, 70, 56 };
        private static readonly byte[] ICO = { 0, 0, 1, 0 };
        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] MP3 = { 255, 251, 48 };
        private static readonly byte[] OGG = { 79, 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };
        private static readonly byte[] SWF = { 70, 87, 83 };
        private static readonly byte[] TIFF = { 73, 73, 42, 0 };
        private static readonly byte[] TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
        private static readonly byte[] TTF = { 0, 1, 0, 0, 0 };
        private static readonly byte[] WAV_AVI = { 82, 73, 70, 70 };
        private static readonly byte[] WMV_WMA = { 48, 38, 178, 117, 142, 102, 207, 17, 166, 217, 0, 170, 0, 98, 206, 108 };
        private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };
        private static readonly byte[] DOCUMENT_X = { 80, 75, 03, 04, 20, 00, 06, 00 };
        private static readonly byte[] DOCUMENT = { 208, 207, 17, 224, 161, 177, 26, 225 };
        #endregion

        public static string GetMimeType(byte[] file, string fileName)
        {

            string mime = "application/octet-stream"; //DEFAULT UNKNOWN MIME TYPE

            //Ensure that the filename isn't empty or null
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }

            //Get the file extension
            string extension = Path.GetExtension(fileName) == null
                                   ? string.Empty
                                   : Path.GetExtension(fileName).ToUpper();

            //Get the MIME Type
            if (file.Take(0).SequenceEqual(BMP))
            {
                mime = "image/bmp";
            }
            //else if (file.Take(8).SequenceEqual(DOC))
            //{
            //    mime = "application/msword";
            //}
            else if (file.Take(2).SequenceEqual(EXE_DLL))
            {
                mime = "application/x-msdownload"; //both use same mime type
            }
            else if (file.Take(4).SequenceEqual(GIF))
            {
                mime = "image/gif";
            }
            else if (file.Take(4).SequenceEqual(ICO))
            {
                mime = "image/x-icon";
            }
            else if (file.Take(3).SequenceEqual(JPG))
            {
                mime = "image/jpeg";
            }
            else if (file.Take(3).SequenceEqual(MP3))
            {
                mime = "audio/mpeg";
            }
            else if (file.Take(14).SequenceEqual(OGG))
            {
                if (extension == ".OGX")
                {
                    mime = "application/ogg";
                }
                else if (extension == ".OGA")
                {
                    mime = "audio/ogg";
                }
                else
                {
                    mime = "video/ogg";
                }
            }
            else if (file.Take(7).SequenceEqual(PDF))
            {
                mime = "application/pdf";
            }
            else if (file.Take(16).SequenceEqual(PNG))
            {
                mime = "image/png";
            }
            else if (file.Take(7).SequenceEqual(RAR))
            {
                mime = "application/x-rar-compressed";
            }
            else if (file.Take(3).SequenceEqual(SWF))
            {
                mime = "application/x-shockwave-flash";
            }
            else if (file.Take(4).SequenceEqual(TIFF))
            {
                mime = "image/tiff";
            }
            else if (file.Take(11).SequenceEqual(TORRENT))
            {
                mime = "application/x-bittorrent";
            }
            else if (file.Take(5).SequenceEqual(TTF))
            {
                mime = "application/x-font-ttf";
            }
            else if (file.Take(4).SequenceEqual(WAV_AVI))
            {
                mime = extension == ".AVI" ? "video/x-msvideo" : "audio/x-wav";
            }
            else if (file.Take(16).SequenceEqual(WMV_WMA))
            {
                mime = extension == ".WMA" ? "audio/x-ms-wma" : "video/x-ms-wmv";
            }
            //else if (file.Take(4).SequenceEqual(ZIP_DOCX))
            //{
            //    mime = extension == ".DOCX" ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/x-zip-compressed";
            //}
            else if (file.Take(8).SequenceEqual(DOCUMENT_X))
            {
                mime = ".DOCX/.PPTX/.XLSX";
            }
            else if (file.Take(8).SequenceEqual(DOCUMENT))
            {
                mime = ".DOC/.PPT/.XLS";
            }
            return mime;
        }

        #region PasswordEncryption

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }

        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        #endregion
    }
}
