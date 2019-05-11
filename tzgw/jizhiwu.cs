using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace tzgw
{
    public partial class jizhiwu : Form
    {
        public jizhiwu()
        {
            InitializeComponent();
            InitprinterComboBox();
            ckchange();
            readprinttemp();
            cj.Text = Class1.workshop;
        }

        private void readprinttemp()
        {
            DirectoryInfo source = new DirectoryInfo(@"C:\data\tzgw-labeltemplete");
            foreach (FileInfo diSourceSubDir in source.GetFiles())
            {
                comboBox2.Items.Add(diSourceSubDir.ToString().Replace(".btw",""));
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.Text = comboBox2.Items[0].ToString();
            }
        }



        private static jizhiwu childFromInstanc;
        public static jizhiwu ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new jizhiwu();
                }

                return childFromInstanc;
            }
        }
        private void jizhiwu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            qritm();
            
        }


        private void qritm()
        {
            string sql = string.Format("select [itemcode] as 物料编码,[desc1] as 牌号,[fukuan] as 幅宽,[yxq] as 有效期,[cctj] as 储存温度,[cctj2] as 储存湿度,[pinming] as 品名 from masterdata2 where desc1 like '%{0}%'", ph.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Show();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                pm.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                ph.Text= dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                yxq.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                cctj.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                cctj2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                b6.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            dt1.Text = DateTime.Now.ToString();
        }

        private void jt_TextChanged(object sender, EventArgs e)
        {
            if (jt.Text.Length > 0)
            {
                batchbig.Text = dt1.Text + (100 + Convert.ToInt32(jt.Text)).ToString().Substring(1, 2);
                if (roll1.Text.Length > 0)
                {
                    batch.Text = batchbig.Text + (100 + Convert.ToInt32(roll1.Text)).ToString().Substring(1, 2);
                }
            }

        }

        private void roll1_TextChanged(object sender, EventArgs e)
        {
            if (roll1.Text.Length > 0)
            {
                batch.Text = batchbig.Text + (100 + Convert.ToInt32(roll1.Text)).ToString().Substring(1, 2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = string.Format("insert into stockjz(branch,sloc,material,batch,batchbig,style,qty,uom,productdate,rmdate,comm1) values('fc','{9}','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",b6.Text,batch.Text,batchbig.Text,comboBox1.Text,qty.Text,uom.Text,dt1.Text,dt2.Text,comm1.Text,cj.Text);
            int c1 = Class1.ExcuteScal(sql);
            if (c1 == 1)
            {

                printlab();

                if (checkBox3.Checked == true)
                {
                    roll1.Text = (1 + Convert.ToInt16(roll1.Text)).ToString();
                }

                if (checkBox1.Checked == true)
                {
                    clean1();
                }
                else
                {
                    qrstock1();
                }

                
            }
            else
            {
                MessageBox.Show("提交失败，请重试或直接打印标签");
            }

        }
        private void clean1()
        {
            ph.Text = "";
            pm.Text = "";
            comboBox1.Text = "";
            yxq.Text = "";
            cctj.Text = "温度≤30℃";
            cctj2.Text = "相对湿度≤65%";
            b6.Text = "";
            jt.Text = "";
            roll1.Text = "";
            batchbig.Text = "";
            batch.Text = "";
            qty.Text = "";
            uom.Text = "㎡";
            comm1.Text = "";
        }
        private void InitprinterComboBox()
        {
            List<String> list = LocalPrinter.GetLocalPrinters(); //获得系统中的打印机列表
            foreach (String s in list)
            {
                comboBox3.Items.Add(s); //将打印机名称添加到下拉框中
            }

            comboBox3.Text = dftprinter.GetDefaultPrinter();
        }


        private void printlab()
        {
            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();

            if (comboBox2.Text != "")
            {
                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\" + comboBox2.Text, false, "");
                btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt16(textBox1.Text);  //设置同序列打印的份数
                btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
                btFormat.SetNamedSubStringValue("ph", ph.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("batch", batch.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("batchbig", batchbig.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("qty", qty.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("type1", comboBox1.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("dt1", dt1.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("dt2", dt2.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("uom", uom.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("comm1", comm1.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("cctj2", cctj2.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("cctj", cctj.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("yxq", yxq.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("gg", comboBox1.Text); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("pm", pm.Text); //向bartender模板传递变量
                btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
                btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
            }
            else
            {
                MessageBox.Show("请选择打印模板");
            }


            
            Class1.killbartender();

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            printlab();
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ckchange();
            
        }
        private void ckchange()
        {
            if (checkBox2.Checked == true)
            {
                b1.Text = "牌号*(ph)";
                b2.Text = "品名*(pm)";
                b3.Text = "规格*(gg)";
                b4.Text = "有效期*(yxq)";
                b5.Text = "储存温度*(cctj)";
                b14.Text = "储存湿度*(cctj2)";
                b7.Text = "生产日期*(dt1)";
                b8.Text = "碳纤维生产日期*(dt2)";
                b9.Text = "批号*(batchbig)";
                b10.Text = "卷号*(batch)";
                b11.Text = "数量*(qty)";
                b12.Text = "单位*(uom)";
                b13.Text = "备注*(comm1)";
            }
            else
            {
                b1.Text = "牌号";
                b2.Text = "品名";
                b3.Text = "规格";
                b4.Text = "有效期";
                b5.Text = "储存温度";
                b14.Text = "储存湿度";
                b7.Text = "生产日期";
                b8.Text = "碳纤维生产日期";
                b9.Text = "批号";
                b10.Text = "卷号";
                b11.Text = "数量";
                b12.Text = "单位";
                b13.Text = "备注";
            }

        }

        private void label14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("打印模板位置C:\\data\\tzgw-labeltemplete\\");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            qrstock1();
        }


        private void qrstock1()
        {
            string sql = string.Format("select material as 物料编码,batch as 卷号,style as 幅宽,qty as 数量,comm1 as 备注,rmdate as 原料日期,productdate as 生产日期 from stockjz where batchbig='{0}' order by productdate desc,material,style,qty", batchbig.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
                dataGridView2.Show();
            }
            else
            {
                dataGridView2.DataSource = null;
            }
        }

        private void b8_Click(object sender, EventArgs e)
        {
            dt2.Text = DateTime.Now.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Selected == true)
                {
                    string sql0 = string.Format("delete from stockjz where batch='{0}' and sloc = '{2}'", dataGridView2.Rows[i].Cells[1].Value.ToString(), Class1.workshop, cj.Text);
                    int c = Class1.ExcuteScal(sql0);
                    if (c == 0)
                    {
                        MessageBox.Show("只能删除未入库物料");
                    }
                }

            }
            qrstock1();
        }

        private void ph_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认添加记录?"+b6.Text+"|"+ph.Text+"|"+comboBox1.Text, "添加", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("INSERT INTO [dbo].[masterdata2]([itemcode] ,[desc1] ,[fukuan] ,[yxq] ,[cctj] ,[pinming] ,[cctj2]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", b6.Text, ph.Text, comboBox1.Text, yxq.Text, cctj.Text, pm.Text,cctj2.Text);
                int c1 = Class1.ExcuteScal(sql);
                if (c1 == 1)
                {
                    qritm();
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
                
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认修改记录?" + b6.Text + "|" + ph.Text + "|" + comboBox1.Text, "添加", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("update [dbo].[masterdata2] set [desc1]='{1}' ,[fukuan]='{2}' ,[yxq] ='{3}',[cctj]='{4}' ,[pinming]='{5}',[cctj2]='{6}' where [itemcode]='{0}'", b6.Text, ph.Text, comboBox1.Text, yxq.Text, cctj.Text, pm.Text,cctj2.Text);
                int c1 = Class1.ExcuteScal(sql);
                if (c1 == 1)
                {
                    qritm();
                }
                else
                {
                    MessageBox.Show("更新失败");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除记录?" + b6.Text + "|" + ph.Text + "|" + comboBox1.Text, "添加", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("delete from [dbo].[masterdata2] where itemcode='{0}'", b6.Text);
                int c1 = Class1.ExcuteScal(sql);
                if (c1 == 1)
                {
                    qritm();
                    clean1();
                }
                else
                {
                    MessageBox.Show("删除失败");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            clean1();
        }

        private void b6_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select [desc1] as 牌号,[fukuan] as 幅宽,[yxq] as 有效期,[cctj] as 储存温度,[cctj2] as 储存湿度,[pinming] as 品名 from masterdata2 where itemcode= '{0}'", b6.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                ph.Text = dt.Rows[0][0].ToString();
                comboBox1.Text = dt.Rows[0][1].ToString();
                yxq.Text = dt.Rows[0][2].ToString();
                cctj.Text = dt.Rows[0][3].ToString();
                cctj2.Text = dt.Rows[0][4].ToString();
                pm.Text = dt.Rows[0][5].ToString();
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sql = string.Format("select material as 物料编码,batch as 卷号,style as 幅宽,qty as 数量,comm1 as 备注,rmdate as 原料日期,productdate as 生产日期 from stockjz where batchbig='{0}' order by productdate desc,material,style,qty", batchbig.Text);

            if (e.RowIndex >= 0)
            {
                b6.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                batch.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();

                qty.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                comm1.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                dt2.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
                dt1.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();

                if (batch.Text.Length == 10)
                {
                    batchbig.Text = batch.Text.Substring(0,8);
                }


            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            
            jizhiwh jzw = jizhiwh.ChildFromInstanc;
            if (jzw != null)
            {
                jzw.Owner = this;
                jzw.Show();
            }
        }

        private void jizhiwu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
