﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using NPOI.XWPF.UserModel;
using NPOI.HSSF.Model; // InternalWorkbook  xls
using NPOI.HSSF.UserModel; // HSSFWorkbook, HSSFSheet
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.Data.OleDb;
using NPOI.SS.UserModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading; // XSSFWorkbook, XSSFSheet  xlsx

namespace AutoGenExcel
{
    public partial class FrmAutoGenExcel : Form
    {
        public FrmAutoGenExcel()
        {
            InitializeComponent();
        }

        private string pathTemplates = ConfigurationManager.AppSettings["pathTemplates"];
        private string pathOutput = ConfigurationManager.AppSettings["pathOutput"];
        private string pathMaster = ConfigurationManager.AppSettings["pathMaster"];
        private string pathImage = ConfigurationManager.AppSettings["pathImage"];
        private string pathSignature = ConfigurationManager.AppSettings["pathSignature"];
        private DataRow[] dr;
        private DataSet ds = new DataSet();
        private string sheetRname = ConfigurationManager.AppSettings["รายชื่อผู้ว่าราชการจังหวัด"];
        private string sheetPname = ConfigurationManager.AppSettings["รายชื่อพนักงาน"];
        private string masterfile = ConfigurationManager.AppSettings["Masterประชารัฐ.xlsx"];
        private string ProvinceID = ConfigurationManager.AppSettings["ProvinceID"];
        private string RegionID = ConfigurationManager.AppSettings["RegionID"];
        private string กลุ่มการออกเอกสาร = ConfigurationManager.AppSettings["กลุ่มการออกเอกสาร"];
        private string FormatDate = ConfigurationManager.AppSettings["FormatDate"];
        private string Condition = "";
        private string ConditionWord = "";
        private string caption = "";
        private string Getpathfolder = "";
        private string Employee = ConfigurationManager.AppSettings["พนักงาน"];
        private string Governor = ConfigurationManager.AppSettings["ผู้ว่าราชการจังหวัด"];
        private string sheetN = "";
        private string EmpName = ConfigurationManager.AppSettings["EmpName"];
        private SaveFileDialog savefile = new SaveFileDialog();

        private void FrmAutoGenExcel_Load(object sender, EventArgs e)
        {
            textBox1.Text = pathOutput;
            Getpathfolder = pathOutput + "\\";
            ds = getFilterData(pathMaster + masterfile);

            dt = connect_excel(pathMaster + masterfile, sheetRname + "$");
            dt.TableName = sheetRname;
            ds.Tables.Add(dt);

            dt = connect_excel(pathMaster + masterfile, sheetPname + "$");
            dt.TableName = sheetPname;
            ds.Tables.Add(dt);

            SetcbTemplate();
            SetcbGroupRecieve(cbGroupRecieve);
            SetcbFilter(cbFilter, "R");
        }

        private Boolean CheckPromptGen(string value1, string value2) //การcheckการuploadไฟล์ว่าจะต้องไม่มีไฟล์ซ้ำกันน่ะ
        {
            if (value1 != "" && value2 != "")
            {
                btnGenExcel.Enabled = true;
                return true;
            }
            else
            {
                btnGenExcel.Enabled = false;
                return false;
            }
        }

        private void SetcbTemplate() //แสดงlistไฟล์ในfolder(รูปแบบเอกสาร)
        {
            string[] Files = Directory.GetFiles(pathTemplates, "*.xls")
                    .Union(Directory.GetFiles(pathTemplates, "*.xlsx"))
                    .Select(x => x.Replace(pathTemplates, ""))
                    .OrderBy(f => f)
                    .ToArray();
            cbTemplate.Items.Clear();
            if (Files != null)
                Files = Files.Where(s => !s.Contains("~")).ToArray();
            cbTemplate.DataSource = Files.ToArray();
        }

        private void SetcbGroupRecieve(ComboBox cb) //แสดงlistของMaster(รายการผู้รับ)
        {
            cb.Items.Clear();
            cb.DisplayMember = "Text";
            cb.ValueMember = "Value";
            var items = new[] {
                new { Text = Governor, Value = "R" },
                new { Text = Employee, Value = "P" },
            };
            cb.DataSource = items;
            cb.SelectedIndex = 0;
        }

        private OleDbConnection conn; //เชื่อมต่อฐานข้อมูล
        private OleDbDataAdapter adapter; //ปรุบปรุงข้อมูลในฐานข้อมูล
        private DataTable dt;//เรียกข้อมูลในตารง

        private void SetcbFilter(ComboBox cb, String GroupRecieve) //การแสดงlistของคัดกรองรายการโดยจะสัมพันกับรายการผู้รับ
        {
            DataTable dtmp = new DataTable();
            if (GroupRecieve == "R")//ภูมิภาค
            {
                dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                dtmp.Rows.Add(new object[] { "ALL" });
                cb.DataSource = dtmp.DefaultView.ToTable(true, RegionID);
                cb.DisplayMember = RegionID;
                cb.ValueMember = RegionID;
            }
            else //จังหวัด
            {
                dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                dtmp.Rows.Add(new object[] { "ALL" });
                cb.DataSource = dtmp.DefaultView.ToTable(true, ProvinceID);
                cb.DisplayMember = ProvinceID;
                cb.ValueMember = ProvinceID;
            }
        }

        private void Setcbhost(ComboBox cb, String GroupRecieve)
        {
            DataTable dtmp = new DataTable();
            if (GroupRecieve == "R")//ภูมิภาค
            {
                dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                cb.DataSource = dtmp.DefaultView.ToTable(true, RegionID);
                cb.DisplayMember = RegionID;
                cb.ValueMember = RegionID;
            }
            else //จังหวัด
            {
                dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                cb.DataSource = dtmp.DefaultView.ToTable(true, ProvinceID);
                cb.DisplayMember = ProvinceID;
                cb.ValueMember = ProvinceID;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setcbhost(cbhost, cbGroupRecieve.SelectedValue.ToString());
            if (cbGroupRecieve.SelectedValue == "P")
            {
                label6.Location = new System.Drawing.Point(7, 93);
                label6.Size = new System.Drawing.Size(150, 20);
                label6.Visible = true;
                cbEmployeename.Visible = true;
                setcbEmployeename(cbEmployeename, cbGroupRecieve.SelectedValue.ToString());
            }
            else
            {
                label6.Visible = false;
                cbEmployeename.Visible = false;
            }
            if (cbFilter.SelectedValue == "ALL")
            {
                cbEmployeename.Text = "ALL";
            }
        }

        private void cbGroupRecieve_SelectedIndexChanged(object sender, EventArgs e) //รายการผู้รับเรียใช้methodcbTemplate_SelectedIndexChangedและSetcbFilter
        {
            cbTemplate_SelectedIndexChanged(sender, e);
            SetcbFilter(cbFilter, cbGroupRecieve.SelectedValue.ToString());
        }

        private void cbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            if ((cbTemplate.Items.Count > 0) && (cbGroupRecieve.Items.Count > 0))
            {
                if ((cbGroupRecieve.SelectedItem.ToString() != "") && (cbTemplate.SelectedItem.ToString() != "")) //ถ้ารูปแบบเอกสารและรายการผู้รับไม่ว่าง
                {
                    CheckPromptGen(cbGroupRecieve.SelectedItem.ToString(), cbTemplate.SelectedItem.ToString()); //checkการซ้ำกันของไฟล์
                    if (File.Exists(pathTemplates + cbTemplate.SelectedItem))
                    {
                        if (Path.GetExtension(pathTemplates + cbTemplate.SelectedItem) == ".xls")
                        {
                            connect_xls(pathTemplates + cbTemplate.SelectedItem, "read", null);
                        }
                        if (Path.GetExtension(pathTemplates + cbTemplate.SelectedItem) == ".xlsx")
                        {
                            connect_xlsx(pathTemplates + cbTemplate.SelectedItem, "read", null);
                        }
                    }//if
                }//if
            }//if
        }

        private void createDtPicker(int i, string caption)
        {
            int positionX = 12;
            int positionY = 20 * i;
            Label newLabel = new Label();
            newLabel.Name = "newLabel" + i;
            newLabel.Location = new System.Drawing.Point(positionX + 75, positionY);
            newLabel.Size = new System.Drawing.Size(150, 20);
            newLabel.Text = caption;
            panel2.Controls.Add(newLabel);
            DateTimePicker newDatePicker = new DateTimePicker();
            newDatePicker.Name = "newDatePicker" + i;
            newDatePicker.Location = new System.Drawing.Point(positionX + 250, positionY);
            newDatePicker.Size = new System.Drawing.Size(200, 20);
            panel2.Controls.Add(newDatePicker);
            positionY += 30;
        }

        private string copyFile(string source, string destination, string save)
        {
            if (Directory.Exists(save))
            { Directory.CreateDirectory("Output"); }
            File.Copy(source, destination);
            return destination;
        }

        private void btnGenExcel_Click(object sender, EventArgs e)
        {
            if (File.Exists(pathTemplates + cbTemplate.SelectedItem))
            {
                if (cbGroupRecieve.SelectedValue.ToString() == "R")
                {
                    sheetN = sheetRname;
                }
                else { sheetN = sheetPname; }
                string destination = pathTemplates + cbTemplate.SelectedItem;
                if (Getpathfolder == "")
                {
                    if (Path.GetExtension(destination) == ".xls") { connect_xls(destination, "write", pathOutput); }
                    if (Path.GetExtension(destination) == ".xlsx") { connect_xlsx(destination, "write", pathOutput); }
                }
                else
                {
                    if (Path.GetExtension(destination) == ".xls") { connect_xls(destination, "write", Getpathfolder); }
                    if (Path.GetExtension(destination) == ".xlsx") { connect_xlsx(destination, "write", Getpathfolder); }
                }
            }
        }

        private string Tagsignature(string value, string status, string data)
        {
            string dummy = "";
            var dtmp = new DataTable();
            if (status == sheetPname)
            {
                dtmp = ds.Tables[sheetPname].DefaultView.ToTable(true, "signature");
            }
            if (status == sheetRname)
            {
                dtmp = ds.Tables[sheetRname].DefaultView.ToTable(true, "signature");
            }
            if (cbGroupRecieve.SelectedValue.ToString() == "R" && data != null)
            {
                if (status == sheetPname)
                {
                    dr = ds.Tables[sheetPname].Select(ProvinceID + " = '" + data + "'");
                    for (int h = 0; h < dr.Length; h++)
                    {
                        string showdata = dr[h]["signature"].ToString();
                        dummy = showdata;
                    }
                }
                if (status == sheetRname)
                {
                    dr = ds.Tables[sheetRname].Select(ProvinceID + " = '" + data + "'");
                    for (int h = 0; h < dr.Length; h++)
                    {
                        string showdata = dr[h]["signature"].ToString();
                        dummy = showdata;
                    }
                }
            }
            else
            {
                if (status == sheetPname)
                {
                    dr = ds.Tables[sheetPname].Select(EmpName + " = '" + cbsignature.Text + "'");
                    for (int h = 0; h < dr.Length; h++)
                    {
                        string showdata = dr[h]["signature"].ToString();
                        dummy = showdata;
                    }
                }
                if (status == sheetRname)
                {
                    dr = ds.Tables[sheetRname].Select(EmpName + " = '" + cbsignature.Text + "'");
                    for (int h = 0; h < dr.Length; h++)
                    {
                        string showdata = dr[h]["signature"].ToString();
                        dummy = showdata;
                    }
                }
            }
            return dummy;
        }

        private string Tagsheet(string value, string status, string data, string Employeename)
        {
            int indexStart = value.IndexOf("<%");
            int indexEnd = value.IndexOf("%>");
            if (value.ToLower().Contains("<%sheet:"))
            {
                value = value.Replace("<%Sheet:", "");
                indexStart = value.IndexOf("<%");
                indexEnd = value.IndexOf("%>");

                var cntF = IdxOfAll.findIdx(value, "[%").ToList();

                for (int k = 0; k < cntF.Count; k++)
                {
                    string dummy = "";
                    DataRow[] drtmp;
                    int indexStartF = value.IndexOf("[%");
                    int indexEndF = value.IndexOf("%]");
                    caption = value.Substring(indexStartF + 2, indexEndF - indexStartF - 2);
                    char sarray = ',';
                    string[] word = caption.Split(sarray);
                    for (int b = 0; b < word.Length; b++)
                    {
                        string kum = word[b];
                        var dtmp = new DataTable();
                        if (kum == word[b])
                        {
                            try
                            {
                                if (status == sheetPname)
                                {
                                    dtmp = ds.Tables[sheetPname].DefaultView.ToTable(true, word[b]);
                                }
                                if (status == sheetRname)
                                {
                                    dtmp = ds.Tables[sheetRname].DefaultView.ToTable(true, word[b]);
                                }
                                if (Employeename != "")
                                {
                                    if (status == sheetPname)
                                    {
                                        dtmp = ds.Tables[sheetPname].DefaultView.ToTable();
                                        drtmp = dtmp.Select(EmpName + "= '" + Employeename + "'" + Condition);
                                        dr = ds.Tables[sheetPname].Select(EmpName + " = '" + Employeename + "'");
                                        string showdata = showdata = drtmp[0][word[b]].ToString();
                                        dummy = showdata;
                                        indexEndF = value.IndexOf("%]");
                                        value = value.Replace(value.Substring(indexStartF, indexEndF - indexStartF + 2), dummy);
                                    }
                                    if (status == sheetRname)
                                    {
                                        dtmp = ds.Tables[sheetRname].DefaultView.ToTable();
                                        drtmp = dtmp.Select(ProvinceID + "= '" + data + "'" + Condition);
                                        string showdata = drtmp[0][word[b]].ToString();
                                        dummy = showdata;
                                        indexEndF = value.IndexOf("%]");
                                        value = value.Replace(value.Substring(indexStartF, indexEndF - indexStartF + 2), dummy);
                                    }
                                }

                                if (data != null && Employeename == "")
                                {
                                    if (status == sheetPname)
                                    {
                                        dtmp = ds.Tables[sheetPname].DefaultView.ToTable();
                                        drtmp = dtmp.Select(ProvinceID + " = '" + data + "'" + Condition);
                                        dr = ds.Tables[sheetPname].Select(ProvinceID + " = '" + data + "'");
                                        string showdata = showdata = drtmp[0][word[b]].ToString();
                                        dummy = showdata;
                                        indexEndF = value.IndexOf("%]");
                                        value = value.Replace(value.Substring(indexStartF, indexEndF - indexStartF + 2), dummy);
                                    }
                                    if (status == sheetRname)
                                    {
                                        dtmp = ds.Tables[sheetRname].DefaultView.ToTable();
                                        drtmp = dtmp.Select(ProvinceID + " = '" + data + "'" + Condition);
                                        string showdata = drtmp[0][word[b]].ToString();
                                        dummy = showdata;
                                        indexEndF = value.IndexOf("%]");
                                        value = value.Replace(value.Substring(indexStartF, indexEndF - indexStartF + 2), dummy);
                                    }
                                }
                                if (data == null && Employeename == "")
                                {
                                    dummy = dtmp.Rows[0][word[b]].ToString();
                                    indexEndF = value.IndexOf("%]");
                                    value = value.Replace(value.Substring(indexStartF, indexEndF - indexStartF + 2), dummy);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("ไม่พบคลอลัม " + kum + "หรือเงื่อนไข" + kum + Condition + " ในFile Masterประชารัฐ.xlsx");
                            }
                        }
                    }
                }
                indexEnd = value.IndexOf("%>");
                value = value.Remove(indexEnd, 2);
            }
            return value;
        }

        private string Tagdate(string value, int num, string mode)
        {
            if (value.ToLower().Contains("<%getdate:"))
            {
                if (mode == "read")
                {
                    int indexStart = value.IndexOf("<%");
                    int indexEnd = value.IndexOf("%>");
                    caption = value.Substring(indexStart + 10, indexEnd - indexStart - 10);
                    createDtPicker(num, caption);
                    num++;
                }
                if (mode == "write")
                {
                    int indexStart = value.IndexOf("<%");
                    int indexEnd = value.IndexOf("%>");
                    DateTimePicker dummy = panel2.Controls.Find("newDatePicker" + num, true).FirstOrDefault() as DateTimePicker;//สร้าง DateTimePickerไว้panel2

                    if (dummy != null)
                    {
                        value = value.Replace(value.Substring(indexStart, indexEnd - indexStart + 2), dummy.Value.ToString(ConfigurationManager.AppSettings["dateFormat"])); //เก็บค่าค่าวันเดือนปีลงในตัวแปรValue
                    }
                    num++;
                }
            }
            return value;
        }

        private string CheckTag(string txt, int w, string caption, string data, string mode, string Employeename)
        {
            if (mode == "read")
            {
                if (txt.ToLower().Contains("<%getdate:"))
                {
                    txt = Tagdate(txt, w, mode);
                }
            }
            if (mode == "write")
            {
                if (txt.ToLower().Contains("<%sheet:") && caption == sheetPname)
                {
                    txt = Tagsheet(txt, sheetPname, data, Employeename);
                }

                if (txt.ToLower().Contains("<%sheet:") && caption == sheetRname)
                {
                    txt = Tagsheet(txt, sheetRname, data, Employeename);
                }
                if (txt.ToLower().Contains("<%signature:%>") && caption == sheetPname)
                {
                    txt = Tagsignature(txt, sheetPname, data);
                }

                if (txt.ToLower().Contains("<%signature:%>") && caption == sheetRname)
                {
                    txt = Tagsignature(txt, sheetRname, data);
                }
                if (txt.ToLower().Contains("<%getdate:"))
                {
                    txt = Tagdate(txt, w, mode);
                }
            }

            return txt;
        }

        private void ReadTag(string destination, string sheetN, string data, string mode, string typeexcle)
        {
            ReadTag(destination, sheetN, data, mode, typeexcle, "");
        }

        private void ReadTag(string destination, string sheetN, string data, string mode, string typeexcle, string EmpName)
        {
            if (mode == "write")
            {
                if (typeexcle == "xls")
                {
                    IRow rowSource = null;
                    HSSFWorkbook wb_xls = null;
                    HSSFSheet sh_xls;
                    using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read)) { wb_xls = new HSSFWorkbook(file); }

                    sh_xls = (HSSFSheet)wb_xls.GetSheet(wb_xls.GetSheetAt(0).SheetName);

                    int cntGetdate = 1;

                    string column = ConfigurationManager.AppSettings["excelCol"];

                    var columns = column.Split(',');
                    bool checkcreateRow = true;
                    int excelRow = int.Parse(ConfigurationManager.AppSettings["excelRow"]);
                    for (int j = 1; j <= excelRow; j++)
                    {
                        int cntCheckcreateRow = 0;
                        for (int i = 0; i <= columns.Length - 1; i++)
                        {
                            string s = columns[i] + j.ToString();
                            var cr = new CellReference(s);
                            var row = sh_xls.GetRow(cr.Row);
                            if (row == null) continue;
                            var cell = row.GetCell(cr.Col);
                            if (cell != null)
                            {
                                string value = cell.StringCellValue;
                                var cnt = IdxOfAll.findIdx(value, "<%").ToList();
                                string loopTmp = null;
                                for (int p = 0; p < cnt.Count; p++) //นับtag
                                {
                                    if (value != null)
                                    {
                                        Condition = "";
                                        string nameimage = "";
                                        int indexStartC = value.IndexOf("(%");
                                        int indexEndC = value.IndexOf("%)");
                                        string colName = null;
                                        if (indexStartC > 0)
                                        {
                                            Condition = " AND " + value.Substring(indexStartC + 2, indexEndC - indexStartC - 2);
                                            ConditionWord = value.Substring(value.IndexOf("(%"), value.IndexOf("%)") - value.IndexOf("(%") + 2);
                                        }
                                        int indexStart = value.IndexOf("<%");
                                        int indexEnd = value.IndexOf("%>");
                                        if (value.ToLower().Contains("<%loop:>"))
                                        {
                                            if (cntCheckcreateRow == 0)
                                            { checkcreateRow = true; }
                                            else { checkcreateRow = false; }
                                            cntCheckcreateRow += 1;

                                            string value2 = value.Substring(8, value.Length - 10);
                                            indexStart = value2.IndexOf("<%");
                                            indexEnd = value2.IndexOf("%>");
                                            if (value2.ToLower().Contains("<%sheet:"))
                                            {
                                                int cntF = IdxOfAll.findIdx(value, "[%").ToList().Count;
                                                if (cntF == 1)
                                                {
                                                    cntF = 2;
                                                }
                                                for (int k = 0; k < cntF - 1; k++)
                                                {
                                                    int indexStartF = value2.IndexOf("<%");
                                                    int indexEndF = value2.IndexOf("[%");
                                                    if (indexEndF > 0)
                                                    { colName = value2.Substring(indexStartF + 8, indexEndF - indexStartF - 8); }
                                                    if (colName != null)
                                                    {
                                                        DataTable dtmp = new DataTable();
                                                        DataRow[] drtmp;
                                                        if (colName == sheetRname)
                                                        {
                                                            dtmp = ds.Tables[colName].DefaultView.ToTable();
                                                            drtmp = dtmp.Select(" (1=1) " + Condition);
                                                            if (ConditionWord != "")
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(ConditionWord, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                        }
                                                        if (colName == sheetPname)
                                                        {
                                                            dtmp = ds.Tables[colName].DefaultView.ToTable();
                                                            drtmp = dtmp.Select(" (1=1) " + Condition);
                                                            if (ConditionWord != "")
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(ConditionWord, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (colName == null)
                                                    {
                                                        if (ConditionWord != "")
                                                        {
                                                            loopTmp = CheckTag(value2, 0, colName, null, mode, "").Replace(ConditionWord, "");
                                                        }
                                                        else
                                                        {
                                                            loopTmp = CheckTag(value2, 0, colName, null, mode, "");
                                                        }
                                                        loopTmp += loopTmp;
                                                        cell.SetCellValue(loopTmp);
                                                    }
                                                }//for
                                                try
                                                {
                                                    value = loopTmp;
                                                    char sarray = '+';
                                                    string[] wordloop = loopTmp.Split(sarray);
                                                    try
                                                    {
                                                        if (checkcreateRow)
                                                        {
                                                            sh_xls.ShiftRows(j, sh_xls.LastRowNum, wordloop.Length);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show(ex.ToString());
                                                    }
                                                    excelRow += sh_xls.LastRowNum;

                                                    for (int b = 0; b < wordloop.Length; b++)
                                                    {
                                                        string kum = wordloop[b];
                                                        if (checkcreateRow)
                                                        {
                                                            rowSource = sh_xls.GetRow(j);
                                                            IRow rowInsert = sh_xls.CreateRow(j + b);
                                                            ICell cellSource = rowSource.GetCell(i);
                                                            ICell cellInsert = rowInsert.CreateCell(i);
                                                            cellInsert.SetCellValue(kum);
                                                        }
                                                        else
                                                        {
                                                            rowSource = sh_xls.GetRow(j + b);
                                                            ICell cellInsert = rowSource.CreateCell(i);
                                                            cellInsert.SetCellValue(kum);
                                                        }
                                                    }
                                                    rowSource = sh_xls.GetRow(j - 1);
                                                    rowSource.CreateCell(i).SetCellValue("");
                                                }
                                                catch
                                                {
                                                    MessageBox.Show("ไม่พบSheet " + colName + " หรือไม่พบเงื่อนไข " + Condition + " ในFile Masterประชารัฐ.xlsx");
                                                }
                                            }//sheet
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (value.ToLower().Contains("<%signature:"))
                                                {
                                                    data = cbhost.SelectedValue.ToString();
                                                    nameimage = CheckTag(value, 0, sheetN, data, mode, "");
                                                    byte[] dataimage = File.ReadAllBytes(pathSignature + nameimage);
                                                    int pictureIndex = wb_xls.AddPicture(dataimage, NPOI.SS.UserModel.PictureType.PNG);
                                                    ICreationHelper helper = wb_xls.GetCreationHelper();
                                                    IDrawing drawing = sh_xls.CreateDrawingPatriarch();
                                                    IClientAnchor anchor = helper.CreateClientAnchor();
                                                    anchor.Col1 = i;
                                                    anchor.Row1 = j - 1;
                                                    IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
                                                    picture.Resize();
                                                    value = value.Replace("<%signature:", "");
                                                    value = value.Replace("%>", "");
                                                    cell.SetCellValue(value);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                MessageBox.Show("ไม่พบลายเซ็นของคุณ " + nameimage + " อยู่ในระบบ");
                                            }
                                            if (value.ToLower().Contains("<%getdate:"))
                                            {
                                                loopTmp = CheckTag(value, cntGetdate, null, null, mode, "");
                                                cntGetdate++;
                                                cell.SetCellValue(loopTmp);
                                            }
                                            if (value.ToLower().Contains("<%sheet:"))
                                            {
                                                if (ConditionWord != "")
                                                {
                                                    loopTmp = CheckTag(value, 0, sheetN, data, mode, EmpName).Replace(ConditionWord, "");
                                                }
                                                else
                                                {
                                                    loopTmp = CheckTag(value, 0, sheetN, data, mode, EmpName);
                                                }
                                                cell.SetCellValue(loopTmp);
                                            }
                                        }

                                        Condition = "";
                                    }//tag
                                }//if value!=null
                            }
                        }
                    }
                    using (var fileDummy = new FileStream(destination, FileMode.Create, FileAccess.ReadWrite))
                    {
                        wb_xls.Write(fileDummy); //เขียนค่าvalueลงในExcel
                        fileDummy.Close();
                    }
                }
                //##########################################################################################################################
                if (typeexcle == "xlsx")
                {
                    IRow rowSource = null;
                    XSSFWorkbook wb_xlsx = null;
                    XSSFSheet sh_xlsx;
                    using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read)) { wb_xlsx = new XSSFWorkbook(file); }
                    sh_xlsx = (XSSFSheet)wb_xlsx.GetSheet(wb_xlsx.GetSheetAt(wb_xlsx.ActiveSheetIndex).SheetName);

                    int cntGetdate = 1;

                    string column = ConfigurationManager.AppSettings["excelCol"];
                    int cntPhysicalRow = 0;
                    int cntPhysicalCol = 0;
                    var columns = column.Split(',');
                    bool checkcreateRow = true;
                    int excelRow = int.Parse(ConfigurationManager.AppSettings["excelRow"]);
                    for (int j = 1; j <= excelRow; j++)
                    {
                        if (sh_xlsx.GetRow(j) != null)
                        {
                            cntPhysicalRow += 1;
                        }
                        int cntCheckcreateRow = 0;
                        for (int i = 0; i <= columns.Length - 1; i++)
                        {
                            string s = columns[i] + j.ToString();
                            var cr = new CellReference(s);
                            var row = sh_xlsx.GetRow(cr.Row);
                            if (row == null) continue;
                            var cell = row.GetCell(cr.Col);

                            if (cell != null)
                            {
                                cntPhysicalCol += 1;
                            }
                            if (cell != null)
                            {
                                string value = cell.StringCellValue;
                                var cnt = IdxOfAll.findIdx(value, "<%").ToList();
                                string loopTmp = null;
                                for (int p = 0; p < cnt.Count; p++) //นับtag
                                {
                                    if (value != null)
                                    {
                                        Condition = "";
                                        string nameimage = "";
                                        int indexStartC = value.IndexOf("(%");
                                        int indexEndC = value.IndexOf("%)");
                                        string colName = null;
                                        if (indexStartC > 0)
                                        {
                                            Condition = " AND " + value.Substring(indexStartC + 2, indexEndC - indexStartC - 2);
                                            ConditionWord = value.Substring(value.IndexOf("(%"), value.IndexOf("%)") - value.IndexOf("(%") + 2);
                                        }
                                        int indexStart = value.IndexOf("<%");
                                        int indexEnd = value.IndexOf("%>");
                                        if (value.ToLower().Contains("<%loop:>"))
                                        {
                                            if (cntCheckcreateRow == 0)
                                            { checkcreateRow = true; }
                                            else { checkcreateRow = false; }
                                            cntCheckcreateRow += 1;

                                            string value2 = value.Substring(8, value.Length - 10);
                                            indexStart = value2.IndexOf("<%");
                                            indexEnd = value2.IndexOf("%>");
                                            if (value2.ToLower().Contains("<%sheet:"))
                                            {
                                                int cntF = IdxOfAll.findIdx(value, "[%").ToList().Count;
                                                if (cntF == 1)
                                                {
                                                    cntF = 2;
                                                }
                                                for (int k = 0; k < cntF - 1; k++)
                                                {
                                                    int indexStartF = value2.IndexOf("<%");
                                                    int indexEndF = value2.IndexOf("[%");
                                                    if (indexEndF > 0)
                                                    { colName = value2.Substring(indexStartF + 8, indexEndF - indexStartF - 8); }
                                                    if (colName != null)
                                                    {
                                                        DataTable dtmp = new DataTable();
                                                        DataRow[] drtmp;
                                                        if (colName == sheetRname)
                                                        {
                                                            dtmp = ds.Tables[colName].DefaultView.ToTable();
                                                            drtmp = dtmp.Select(" (1=1) " + Condition);
                                                            if (ConditionWord != "")
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(ConditionWord, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                        }
                                                        if (colName == sheetPname)
                                                        {
                                                            dtmp = ds.Tables[colName].DefaultView.ToTable();
                                                            drtmp = dtmp.Select(" (1=1) " + Condition);
                                                            if (ConditionWord != "")
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(ConditionWord, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int y = drtmp.Length - 1; y >= 0; y--)
                                                                {
                                                                    string numdata = drtmp[y][ProvinceID].ToString();
                                                                    loopTmp += CheckTag(value2, 0, colName, numdata, mode, "").Replace(colName, "") + "+";
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (colName == null)
                                                    {
                                                        if (ConditionWord != "")
                                                        {
                                                            loopTmp = CheckTag(value2, 0, colName, null, mode, "").Replace(ConditionWord, "");
                                                        }
                                                        else
                                                        {
                                                            loopTmp = CheckTag(value2, 0, colName, null, mode, "");
                                                        }
                                                        loopTmp += loopTmp;
                                                        cell.SetCellValue(loopTmp);
                                                    }
                                                }//for
                                                try
                                                {
                                                    value = loopTmp;
                                                    char sarray = '+';
                                                    string[] wordloop = loopTmp.Split(sarray);
                                                    try
                                                    {
                                                        if (checkcreateRow)
                                                        {
                                                            sh_xlsx.ShiftRows(j, sh_xlsx.LastRowNum, wordloop.Length);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show(ex.ToString());
                                                    }
                                                    excelRow += sh_xlsx.LastRowNum;

                                                    for (int b = 0; b < wordloop.Length; b++)
                                                    {
                                                        string kum = wordloop[b];
                                                        if (checkcreateRow)
                                                        {
                                                            rowSource = sh_xlsx.GetRow(cntPhysicalRow - 1);
                                                            IRow rowInsert = sh_xlsx.CreateRow(j + b);
                                                            ICell cellSource = rowSource.GetCell(i);
                                                            ICell cellInsert = rowInsert.CreateCell(i);
                                                            cellInsert.SetCellValue(kum);
                                                        }
                                                        else
                                                        {
                                                            rowSource = sh_xlsx.GetRow(j + b);
                                                            ICell cellInsert = rowSource.CreateCell(i);
                                                            cellInsert.SetCellValue(kum);
                                                        }
                                                    }
                                                    rowSource = sh_xlsx.GetRow(j - 1);
                                                    rowSource.CreateCell(i).SetCellValue("");
                                                }
                                                catch
                                                {
                                                    MessageBox.Show("ไม่พบSheet " + colName + " หรือไม่พบเงื่อนไข " + Condition + " ในFile Masterประชารัฐ.xlsx");
                                                }
                                            }//sheet
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (value.ToLower().Contains("<%signature:"))
                                                {
                                                    data = cbhost.SelectedValue.ToString();
                                                    nameimage = CheckTag(value, 0, sheetN, data, mode, "");
                                                    byte[] dataimage = File.ReadAllBytes(pathSignature + nameimage);
                                                    int pictureIndex = wb_xlsx.AddPicture(dataimage, NPOI.SS.UserModel.PictureType.PNG);
                                                    ICreationHelper helper = wb_xlsx.GetCreationHelper();
                                                    IDrawing drawing = sh_xlsx.CreateDrawingPatriarch();
                                                    IClientAnchor anchor = helper.CreateClientAnchor();
                                                    anchor.Col1 = i;
                                                    anchor.Row1 = j - 1;
                                                    IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
                                                    picture.Resize();
                                                    value = value.Replace("<%signature:", "");
                                                    value = value.Replace("%>", "");
                                                    cell.SetCellValue(value);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                MessageBox.Show("ไม่พบลายเซ็นของคุณ " + nameimage + " อยู่ในระบบ");
                                            }
                                            if (value.ToLower().Contains("<%getdate:"))
                                            {
                                                loopTmp = CheckTag(value, cntGetdate, null, null, mode, "");
                                                cntGetdate++;
                                                cell.SetCellValue(loopTmp);
                                            }
                                            if (value.ToLower().Contains("<%sheet:"))
                                            {
                                                if (ConditionWord != "")
                                                {
                                                    loopTmp = CheckTag(value, 0, sheetN, data, mode, EmpName).Replace(ConditionWord, "");
                                                }
                                                else
                                                {
                                                    loopTmp = CheckTag(value, 0, sheetN, data, mode, EmpName);
                                                }
                                                cell.SetCellValue(loopTmp);
                                            }
                                        }

                                        Condition = "";
                                    }//tag
                                }//if value!=null

                                if (value == string.Empty || cnt.Count == 0)
                                {
                                    ICell cellInsert = row.CreateCell(cr.Col);
                                    cellInsert.SetCellValue(value);
                                }
                            }
                            else
                            {
                                ICell cellInsert = row.CreateCell(cr.Col);
                                cellInsert.SetCellValue("");
                            }
                        }
                    }
                    using (var fileDummy = new FileStream(destination, FileMode.Create, FileAccess.ReadWrite))
                    {
                        wb_xlsx.Write(fileDummy); //เขียนค่าvalueลงในExcel
                        fileDummy.Close();
                    }
                }
            }
        }

        //Excel.xls_____________________________________________________________________________________________________________________________________________

        private void connect_xls(string destination, string mode, string save) //ติดต่อExcelแบบ.xls
        {
            int frequency = 1;
            int numprint = 1;
            DataRow[] dr1;
            int[] numrow = new int[99];
            int h = 1;
            int cntt = 1;
            int cntGetdate = 1;
            bool IsseclectemployeeName = false;
            int excelRow = int.Parse(ConfigurationManager.AppSettings["excelRow"]);
            string column = ConfigurationManager.AppSettings["excelCol"];
            var columns = column.Split(',');
            DataTable dtmp = new DataTable();
            HSSFWorkbook wb_xls = null;
            HSSFSheet sh_xls;
            if (save == null)
            {
                save = pathOutput;
            }
            var fName = destination.Substring(destination.LastIndexOf("\\") + 1, destination.Length - destination.LastIndexOf("\\") - 1);
            if (mode == "write")
            {
                Condition = "";
                if (cbFilter.SelectedValue.ToString() == "ALL")
                { cntt = cbFilter.Items.Count - 1; }
                else
                {
                    if (cbGroupRecieve.SelectedValue.ToString() == "R")
                    { Condition = " AND " + RegionID + " = '" + cbFilter.Text + "'"; }
                    else { Condition = " AND " + ProvinceID + "= '" + cbFilter.Text + "'"; }
                }
                if ((cbGroupRecieve.SelectedValue.ToString() == "P" && cbFilter.SelectedValue.ToString() == "ALL") || (cbGroupRecieve.SelectedValue.ToString() == "R" && cbFilter.SelectedValue.ToString() == "ALL"))
                {
                    dr = ds.Tables[0].Select();
                    if (dr.Length > 0)
                    {
                        for (h = 0; h < dr.Length; h++)
                        {
                            if (sheetN == sheetRname)
                            {
                                dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                                dr = dtmp.Select(" (1=1) " + Condition);
                                string keyworddata = dr[h][RegionID].ToString();
                                dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                                dr1 = dtmp.Select(ProvinceID + "= '" + keyworddata + "'");
                                numrow[h] = dr1.Length;
                            }
                            if (sheetN == sheetPname)
                            {
                                dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                                dr = dtmp.Select(" (1=1) " + Condition);
                                string keyworddata = dr[h][ProvinceID].ToString();
                                dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                                dr1 = dtmp.Select(" ProvinceID = '" + keyworddata + "'");
                                numrow[h] = dr1.Length;
                            }
                        }
                    }
                }
                else
                {
                    numrow[0] = 1;
                    h = cbEmployeename.SelectedIndex;
                    IsseclectemployeeName = true;
                }
                for (int l = 1; l <= cntt; l++)
                {
                    for (int n = 1; n <= numrow[l - 1]; n++)
                    {
                        using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read)) { wb_xls = new HSSFWorkbook(file); }
                        sh_xls = (HSSFSheet)wb_xls.GetSheet(wb_xls.GetSheetAt(0).SheetName);
                        if (sheetN == sheetRname)
                        {
                            dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                            dr = dtmp.Select(" (1=1) " + Condition);
                            string data = dr[l - 1][RegionID].ToString();
                            destination = copyFile(pathTemplates + cbTemplate.SelectedItem, save + DateTime.Now.ToString(FormatDate) + l.ToString() + data + cbTemplate.SelectedItem, save); //Genชื่อไฟล์โดยมีรูปแบบ yyyMMdd_HHmmssf
                            ReadTag(destination, sheetN, data, mode, "xls");
                        }

                        if (sheetN == sheetPname)
                        {
                            dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                            dr = dtmp.Select(" (1=1) " + Condition);
                            string data = dr[l - 1][ProvinceID].ToString();
                            dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                            dr1 = dtmp.Select(" ProvinceID = '" + data + "'");
                            string Employeename = "";

                            if (dr1.Length > 0)
                            {
                                if (IsseclectemployeeName != true)
                                {
                                    h = n - 1;
                                }

                                Employeename = dr1[h][EmpName].ToString();
                            }

                            destination = copyFile(pathTemplates + cbTemplate.SelectedItem, save + DateTime.Now.ToString(FormatDate) + l.ToString() + Employeename + cbTemplate.SelectedItem, save); //Genชื่อไฟล์โดยมีรูปแบบ yyyMMdd_HHmmssf

                            ReadTag(destination, sheetN, data, mode, "xls", Employeename);
                        }
                        numprint += frequency;
                    }
                }
                if (cntt > 0)
                {
                    MessageBox.Show("Complete generate " + (numprint - 1) + " file(s)");
                    OpenFolder(pathOutput);
                }
            }
            try
            {
                using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read)) { wb_xls = new HSSFWorkbook(file); }
            }
            catch (Exception)
            {
                MessageBox.Show("กรุณาปิดไฟล์ชื่อ " + fName);
                cbTemplate_SelectedIndexChanged(null, null);
            }
            try
            {
                sh_xls = (HSSFSheet)wb_xls.GetSheet(wb_xls.GetSheetAt(0).SheetName);
                for (int i = 0; i <= columns.Length - 1; i++)
                {
                    for (int j = 1; j <= excelRow; j++)
                    {
                        string s = columns[i] + j.ToString();
                        var cr = new CellReference(s);
                        var row = sh_xls.GetRow(cr.Row);
                        if (row == null) continue;
                        var cell = row.GetCell(cr.Col);
                        if (cell != null)
                        {
                            string value = cell.StringCellValue;
                            var cnt = IdxOfAll.findIdx(value, "<%").ToList();
                            int indexStart = value.IndexOf("<%");
                            int indexEnd = value.IndexOf("%>");
                            if (value.ToLower().Contains("<%getdate:"))
                            {
                                value = CheckTag(value, cntGetdate, null, null, mode, "");
                                cntGetdate++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //Excel.xlsx________________________________________________________________________________________________________________________________________________
        private void connect_xlsx(string destination, string mode, string save)
        {
            int frequency = 1;
            int numprint = 1;
            DataRow[] dr1;
            int[] numrow = new int[99];
            int h = 1;
            bool IsseclectemployeeName = false;
            XSSFWorkbook wb_xlsx = null;
            XSSFSheet sh_xlsx;
            int cntt = 1;
            int cntGetdate = 1;
            int excelRow = int.Parse(ConfigurationManager.AppSettings["excelRow"]);
            string column = ConfigurationManager.AppSettings["excelCol"];
            var columns = column.Split(',');
            DataTable dtmp = new DataTable();
            if (save == null)
            {
                save = pathOutput;
            }
            var fName = destination.Substring(destination.LastIndexOf("\\") + 1, destination.Length - destination.LastIndexOf("\\") - 1);
            if (mode == "write")
            {
                Condition = " ";
                if (cbFilter.SelectedValue.ToString() == "ALL")
                { cntt = cbFilter.Items.Count - 1; }
                else
                {
                    if (cbGroupRecieve.SelectedValue.ToString() == "R")
                    { Condition = " AND " + RegionID + " = '" + cbFilter.Text + "'"; }
                    else { Condition = " AND " + ProvinceID + " = '" + cbFilter.Text + "'"; }
                }
                if ((cbGroupRecieve.SelectedValue.ToString() == "P" && cbFilter.SelectedValue.ToString() == "ALL") || (cbGroupRecieve.SelectedValue.ToString() == "R" && cbFilter.SelectedValue.ToString() == "ALL"))
                {
                    dr = ds.Tables[0].Select();
                    if (dr.Length > 0)
                    {
                        for (h = 0; h < dr.Length; h++)
                        {
                            if (sheetN == sheetRname)
                            {
                                dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                                dr = dtmp.Select(" (1=1) " + Condition);
                                string keyworddata = dr[h][RegionID].ToString();
                                dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                                dr1 = dtmp.Select(ProvinceID + " = '" + keyworddata + "'");
                                numrow[h] = dr1.Length;
                            }
                            if (sheetN == sheetPname)
                            {
                                dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                                dr = dtmp.Select(" (1=1) " + Condition);
                                string keyworddata = dr[h][ProvinceID].ToString();
                                dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                                dr1 = dtmp.Select(ProvinceID + " = '" + keyworddata + "'");
                                numrow[h] = dr1.Length;
                            }
                        }
                    }
                }
                else
                {
                    numrow[0] = 1;
                    h = cbEmployeename.SelectedIndex;
                    IsseclectemployeeName = true;
                }
                for (int l = 1; l <= cntt; l++)
                {
                    for (int n = 1; n <= numrow[l - 1]; n++)
                    {
                        using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read)) { wb_xlsx = new XSSFWorkbook(file); }
                        sh_xlsx = (XSSFSheet)wb_xlsx.GetSheet(wb_xlsx.GetSheetAt(wb_xlsx.ActiveSheetIndex).SheetName);
                        if (sheetN == sheetRname)
                        {
                            dtmp = ds.Tables[0].DefaultView.ToTable(true, RegionID);
                            dr = dtmp.Select(" (1=1) " + Condition);
                            string data = dr[l - 1][RegionID].ToString();
                            destination = copyFile(pathTemplates + cbTemplate.SelectedItem, save + DateTime.Now.ToString(FormatDate) + l.ToString() + data + cbTemplate.SelectedItem, save); //Genชื่อไฟล์โดยมีรูปแบบ yyyMMdd_HHmmssf
                            ReadTag(destination, sheetN, data, mode, "xlsx");
                        }
                        if (sheetN == sheetPname)
                        {
                            dtmp = ds.Tables[0].DefaultView.ToTable(true, ProvinceID);
                            dr = dtmp.Select(" (1=1) " + Condition);
                            string data = dr[l - 1][ProvinceID].ToString();
                            dtmp = ds.Tables[sheetN].DefaultView.ToTable();
                            dr1 = dtmp.Select(ProvinceID + "= '" + data + "'");
                            string Employeename = "";

                            if (dr1.Length > 0)
                            {
                                if (IsseclectemployeeName != true)
                                {
                                    h = n - 1;
                                }

                                Employeename = dr1[h][EmpName].ToString();
                            }

                            destination = copyFile(pathTemplates + cbTemplate.SelectedItem, save + DateTime.Now.ToString(FormatDate) + l.ToString() + Employeename + cbTemplate.SelectedItem, save); //Genชื่อไฟล์โดยมีรูปแบบ yyyMMdd_HHmmssf

                            ReadTag(destination, sheetN, data, mode, "xlsx", Employeename);
                        }
                        numprint += frequency;
                    }
                }
                if (cntt > 0)
                {
                    MessageBox.Show("Complete generate " + (numprint - 1) + " file(s)");
                    OpenFolder(pathOutput);
                }
            }

            try
            {
                using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read))
                {
                    wb_xlsx = new XSSFWorkbook(file);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("กรุณาปิดไฟล์ชื่อ " + fName);
                cbTemplate_SelectedIndexChanged(null, null);
            }
            try
            {
                using (var file = new FileStream(destination, FileMode.Open, FileAccess.Read))
                {
                    wb_xlsx = new XSSFWorkbook(file);
                }
                sh_xlsx = (XSSFSheet)wb_xlsx.GetSheet(wb_xlsx.GetSheetAt(wb_xlsx.ActiveSheetIndex).SheetName);

                for (int i = 0; i <= columns.Length - 1; i++)
                {
                    for (int j = 1; j <= excelRow; j++)
                    {
                        string s = columns[i] + j.ToString();
                        var cr = new CellReference(s);
                        var row = sh_xlsx.GetRow(cr.Row);
                        if (row == null) continue;
                        var cell = row.GetCell(cr.Col);
                        if (cell != null)
                        {
                            string value = cell.StringCellValue;
                            var cnt = IdxOfAll.findIdx(value, "<%").ToList();
                            int indexStart = value.IndexOf("<%");
                            int indexEnd = value.IndexOf("%>");
                            if (value.ToLower().Contains("<%getdate:"))
                            {
                                value = CheckTag(value, cntGetdate, null, null, mode, null);
                                cntGetdate++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private DataSet getFilterData(string destination)
        {
            DataTable dt = connect_excel(destination, กลุ่มการออกเอกสาร); //เชื่อมต่อExcel sheetชื่อกลุ่มการออกเอกสาร
            ds.Tables.Add(dt);
            return ds;
        }

        private DataTable connect_excel(string destination, string sheetName) //เชื่อมต่อExcelกับc#
        {
            if (Path.GetExtension(destination).ToLower() == ".xls")
            {
                conn = new OleDbConnection(string.Concat("Provider=Microsoft.Jet.OleDb.4.0;"
                    , "Data Source=", Application.StartupPath, destination, ";"
                    , "Extended Properties=Excel 8.0"));
            }
            if (Path.GetExtension(destination).ToLower() == ".xlsx")
            {
                try
                {
                    conn = new OleDbConnection(string.Concat("Provider=Microsoft.ACE.OLEDB.12.0;"
                        , "Data Source=", Application.StartupPath, destination, ";"
                        , "Extended Properties=Excel 12.0 Xml"));
                }
                catch
                {
                    try
                    {
                        conn = new OleDbConnection(string.Concat("Provider=Microsoft.ACE.OLEDB.14.0;"
                            , "Data Source=", Application.StartupPath, destination, ";"
                            , "Extended Properties=Excel 14.0 Xml"));
                    }
                    catch
                    {
                        try
                        {
                            conn = new OleDbConnection(string.Concat("Provider=Microsoft.ACE.OLEDB.15.0;"
                                , "Data Source=", Application.StartupPath, destination, ";"
                                , "Extended Properties=Excel 15.0 Xml"));
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (conn != null)
            {
                conn.Open();
            }
            if (sheetName == sheetRname + "$")
            {
                dt = conn.GetSchema("Tables"); //ข้อมูลในตาราง
                adapter = new OleDbDataAdapter(string.Concat("SELECT * FROM [", sheetName, "]  "), conn); //เลือกข้อมูลทั้งหมดในExcel sheetกลุ่มการออกเอกสาร
                new OleDbCommandBuilder(adapter);
                dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            if (sheetName == sheetPname + "$")
            {
                dt = conn.GetSchema("Tables"); //ข้อมูลในตาราง
                adapter = new OleDbDataAdapter(string.Concat("SELECT * FROM [", sheetName, "]   "), conn); //เลือกข้อมูลทั้งหมดในExcel sheetกลุ่มการออกเอกสาร /*AND FullName LIKE '%สุโข%'*/
                new OleDbCommandBuilder(adapter);
                dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            else
            {
                dt = conn.GetSchema("Tables"); //ข้อมูลในตาราง
                adapter = new OleDbDataAdapter(string.Concat("SELECT * FROM [", sheetName, "] "), conn); //เลือกข้อมูลทั้งหมดในExcel sheetกลุ่มการออกเอกสาร
                new OleDbCommandBuilder(adapter);
                dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string destination = pathTemplates + cbTemplate.SelectedItem;
            FolderBrowserDialog openfolder = new FolderBrowserDialog();
            DialogResult result = openfolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                string pathfolder = openfolder.SelectedPath;
                Getpathfolder = pathfolder + "\\";
                textBox1.Text = pathfolder;
            }
        }

        private void setcbEmployeename(ComboBox cb, String GroupRecieve)
        {
            cbEmployeename.Items.Clear();
            dr = ds.Tables[sheetPname].Select(ProvinceID + " = '" + cbFilter.Text + "'");
            if (dr.Length > 0)
            {
                for (int h = 0; h < dr.Length; h++)
                {
                    string tmpName = dr[h][EmpName].ToString();

                    var items = new[]
                    {
                        new { Text =tmpName , Value = tmpName}
                    };
                    cb.Items.Add(items[0].Value);
                }
                cb.SelectedIndex = 0;
            }
        }

        private void setcbsignatureEmployeename(ComboBox cb, String GroupRecieve)
        {
            cbsignature.Items.Clear();
            dr = ds.Tables[sheetPname].Select(ProvinceID + " = '" + cbhost.Text + "'");
            if (dr.Length > 0)
            {
                for (int h = 0; h < dr.Length; h++)
                {
                    string sName = dr[h][EmpName].ToString();

                    var items = new[]
                    {
                        new { Text =sName , Value = sName}
                    };
                    cb.Items.Add(items[0].Value);
                }
                cb.SelectedIndex = 0;
            }
        }

        private void cbhost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbGroupRecieve.SelectedValue == "P")
            {
                label5.Location = new System.Drawing.Point(41, 120);
                cbhost.Location = new System.Drawing.Point(94, 117);
                label7.Visible = true;
                cbsignature.Visible = true;
                setcbsignatureEmployeename(cbsignature, cbhost.SelectedValue.ToString());
            }
            else
            {
                label7.Visible = false;
                cbsignature.Visible = false;
            }
            if (cbGroupRecieve.SelectedValue == "R")
            {
                label5.Location = new System.Drawing.Point(41, 93);
                cbhost.Location = new System.Drawing.Point(94, 93);
            }
        }

        private static void InsertRows(ref HSSFSheet sheet1, int fromRowIndex, int rowCount)
        {
            sheet1.ShiftRows(fromRowIndex, sheet1.LastRowNum, rowCount, true, false, true);

            for (int rowIndex = fromRowIndex; rowIndex < fromRowIndex + rowCount; rowIndex++)
            {
                IRow rowSource = sheet1.GetRow(rowIndex + rowCount);
                IRow rowInsert = sheet1.CreateRow(rowIndex);
                rowInsert.Height = rowSource.Height;
                for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
                {
                    ICell cellSource = rowSource.GetCell(colIndex);
                    ICell cellInsert = rowInsert.CreateCell(colIndex);
                    if (cellSource != null)
                    {
                        cellInsert.CellStyle = cellSource.CellStyle;
                    }
                }
            }
        }

        private void OpenFolder(string path)
        {
            ProcessStartInfo StartInformation = new ProcessStartInfo();

            StartInformation.FileName = path;

            Process process = Process.Start(StartInformation);

            // process.EnableRaisingEvents = true;
        }
    }//class
}//name