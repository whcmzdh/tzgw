using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tzgw
{
    public partial class aftergen : Form
    {
        public aftergen()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            Font font = new Font("宋体", 14);
            dataGridView1.Font = font;
            InitprinterComboBox();
            comboBox2.Text = Class1.shift1;
        }

        public DataSet dsx = new DataSet();
        public DataTable dtx = new DataTable();

        private static aftergen childFromInstanc;
        public static aftergen ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new aftergen();
                }

                return childFromInstanc;
            }
        }

        private void aftergen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("卷号"));
            dt.Columns.Add(new DataColumn("工位"));
            dt.Columns.Add(new DataColumn("上卷时间"));
            dt.Columns.Add(new DataColumn("下卷时间"));
            dt.Columns.Add(new DataColumn("原丝批次"));
            dt.Columns.Add(new DataColumn("物料"));
            dt.Columns.Add(new DataColumn("重量"));
            dt.Columns.Add(new DataColumn("长度"));
            dt.Columns.Add(new DataColumn("批次"));
            dt.Columns.Add(new DataColumn("类别"));
            if (start1.Text.Length == 11)
            {
                if (Class1.isNumeric(start1.Text) && Class1.isNumeric(qty1.Text))
                {
                    for (int i = 1; i <= Convert.ToInt32(qty1.Text); i++)
                    {
                        
                        DataRow dr1 = dt.NewRow();
                        dr1[0] = Convert.ToInt64(start1.Text) + i-1;
                        dr1[6] = wt.Text;
                        dr1[7] = len1.Text;
                        dr1[8] = batch0.Text;
                        dr1[9] = typea.Text;
                        dt.Rows.Add(dr1);

                    }
                    dataGridView1.DataSource = dt;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string batch0 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            string sql0 = string.Format("select top 1 '已存在',batch from stock where batch='{0}'", batch0);

                            DataTable dts = new DataTable();
                            DataSet dss = new DataSet();
                            dss = Class1.GetAllDataSet(sql0);
                            dts = dss.Tables[0];
                            if (dts.Rows.Count == 1)
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                dataGridView1.Rows[i].Cells[1].Value = dts.Rows[0][0].ToString().Trim();

                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("卷号必须是11位");
                dataGridView1.DataSource = null;
            }


        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
    
              
        }



        private void readdata()
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() != "")
                {
                    string batch0 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string sql0 = string.Format("select top 1 '已存在',batch from stock where batch='{0}'", batch0);

                    DataTable dts = new DataTable();
                    DataSet dss = new DataSet();
                    dss = Class1.GetAllDataSet(sql0);
                    dts = dss.Tables[0];
                    if (dts.Rows.Count == 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        dataGridView1.Rows[i].Cells[1].Value = dts.Rows[0][0].ToString().Trim();

                    }

                    string devnum = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string sql = string.Format("select t3,t4,batchcus,itemrun,batchbig from wsdevice where workshop='{0}' and devicenum='{1}'", Class1.workshop, devnum);
                    DataTable dts1 = new DataTable();
                    DataSet dss1 = new DataSet();
                    dss1 = Class1.GetAllDataSet(sql);
                    dts1 = dss1.Tables[0];
                    if (dts1.Rows.Count == 1)
                    {
                        dataGridView1.Rows[i].Cells[2].Value = Convert.ToDateTime(dts1.Rows[0][0].ToString().Trim()).ToString("HH:mm yyyy/MM/dd");
                        dataGridView1.Rows[i].Cells[3].Value = DateTime.Now.ToString("HH:mm yyyy/MM/dd");
                        dataGridView1.Rows[i].Cells[4].Value = dts1.Rows[0][2].ToString().Trim();
                        dataGridView1.Rows[i].Cells[5].Value = dts1.Rows[0][3].ToString().Trim();
                        dataGridView1.Rows[i].Cells[8].Value = dts1.Rows[0][4].ToString().Trim();
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[2].Value = "";
                        dataGridView1.Rows[i].Cells[3].Value = "";
                        dataGridView1.Rows[i].Cells[4].Value = "";
                        dataGridView1.Rows[i].Cells[5].Value = "";
                        dataGridView1.Rows[i].Cells[8].Value = "";
                    }

                }
            }
            
        }
        private void InitprinterComboBox()
        {
            List<String> list = LocalPrinter.GetLocalPrinters(); //获得系统中的打印机列表
            foreach (String s in list)
            {
                printerlist.Items.Add(s); //将打印机名称添加到下拉框中
            }

            printerlist.Text = dftprinter.GetDefaultPrinter();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string prttype = "";
            int prtline = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    prtline = i + 1;
                }
            }

            prttype = Interaction.InputBox("请选择打印行数,0-全部", "打印", prtline.ToString(), -1, -1);
            if (prttype == "0")
            {
                BarTender.Application btApp;
                BarTender.Format btFormat;
                btApp = new BarTender.Application();
                if (comboBox1.Text == "A")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                }
                else
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");

                }

                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

                int s = dataGridView1.RowCount;
                for (int i = 0; i < s - 1; i++)
                {
                    btFormat.SetNamedSubStringValue("desc1", desc1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("batch", dataGridView1.Rows[i].Cells[0].Value.ToString()); //向bartender模板传递变量            
                    btFormat.SetNamedSubStringValue("batch0", batch0.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("pro1", wt.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("len", len1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("date1", date1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("c5", Class1.workshop + "-" + dataGridView1.Rows[i].Cells[1].Value.ToString()); //向bartender模板传递变量


                    btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性

                }

                btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

                Class1.killbartender();
            }
            else
            {
                int s = dataGridView1.RowCount;
                if ((Convert.ToInt16(prttype) - 1) >= 0 && (Convert.ToInt16(prttype) - 1) < s - 1)
                {
                    int r = Convert.ToInt16(prttype) - 1;
                    BarTender.Application btApp;
                    BarTender.Format btFormat;
                    btApp = new BarTender.Application();
                    if (comboBox1.Text == "A")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                    }
                    else
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");

                    }
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                    btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

                    btFormat.SetNamedSubStringValue("desc1", desc1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("batch", dataGridView1.Rows[r].Cells[0].Value.ToString()); //向bartender模板传递变量            
                    btFormat.SetNamedSubStringValue("batch0", batch0.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("pro1", wt.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("len", len1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("date1", date1.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("c5",Class1.workshop+"-"+dataGridView1.Rows[r].Cells[1].Value.ToString()); //向bartender模板传递变量


                    btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性

                    btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

                    Class1.killbartender();
                }
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString().Length == 11 & dataGridView1.Rows[i].Cells[1].Value.ToString().Length >0)
                    {
                        if (dataGridView1.Rows[i].DefaultCellStyle.BackColor != Color.Red)
                        {
                            string batch = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            string gw = Class1.workshop+"-"+dataGridView1.Rows[i].Cells[1].Value.ToString();
                            string gw2= dataGridView1.Rows[i].Cells[1].Value.ToString();
                            string t1 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            string t2 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            string rmbatch = dataGridView1.Rows[i].Cells[4].Value.ToString();
                            string material = dataGridView1.Rows[i].Cells[5].Value.ToString();
                            string pro1 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                            string len1 = dataGridView1.Rows[i].Cells[7].Value.ToString();
                            string batchbig = dataGridView1.Rows[i].Cells[8].Value.ToString();
                            string typea = dataGridView1.Rows[i].Cells[9].Value.ToString();

                            string sqlx = string.Format("insert into stock(branch,sloc,batch,displaybatch,c5,t1,t2,rmbatch,material,pro1,len,batchbig,qatype,boxno,stockin,tpno,shift1) values('tz','{0}','{1}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','','{13}')", Class1.workshop, batch, gw, t1, t2, rmbatch, material, pro1, len1, batchbig, typea, Class1.workshop + "-" + DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),comboBox2.Text);
                            int c = Class1.ExcuteScal(sqlx);
                            if (c == 1)
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                                string sqls = string.Format("update wsdevice set t3='{0}',status2='运行' where devicenum='{2}' and workshop='{1}'",t2,Class1.workshop,gw2);
                                int x = Class1.ExcuteScal(sqls);
                                
                            }
                            else
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;

                            }

                        }
                        

                        
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select desc1 from masterdata where itemcode='{0}'",textBox1.Text);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                desc1.Text = dt.Rows[0][0].ToString().Trim();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void printerlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dftprinter.SetDefaultPrinter(printerlist.Text))
            {
               // MessageBox.Show("设置默认打印机成功,当前默认打印机为" + printerlist.Text);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            readdata();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            filldt(wt.Text,6);
        }



        private void filldt(string fillx,int lx)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.Rows[i].Cells[lx].Value = fillx;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            filldt(len1.Text,7);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            filldt(batch0.Text,8);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            filldt(textBox1.Text,5);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            filldt(typea.Text, 9);
        }

        private void len1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

