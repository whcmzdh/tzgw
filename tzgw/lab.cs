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
    public partial class lab : Form
    {
        public lab()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
             
        }

        private void lab_Load(object sender, EventArgs e)
        {

        }


        private static lab childFromInstanc;
        public static lab ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new lab();
                }

                return childFromInstanc;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 15)
            {
                freshitem2();

            }
            else
            {
                freshitem();
            }
            flashlaboption();

        }

        private void freshlabrecord()
        {
            if (labitem.Text != "")
            {
                string sql = string.Format("select laboption as 检验项目,labvalue as 检测值,comm1 as 备注,date1 as 检测日期,oper as 操作员 from labrecord where itemcode='{0}' and batch='{1}' order by laboption,date1", labitem.Text.Trim(), textBox1.Text.Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    dataGridView1.DataSource = null;
                }



            }
        }


        private void freshitem()
        {
            freshlabrecord();
            if (textBox1.Text != "")
            {
                string sql = string.Format("select top 1 T2.desc1,T1.stockin,T2.sp1,T1.sloc,T2.itemcode,T1.len,T1.pro1,T1.rmbatch,T1.rmbatch1,T1.rmbatch2,T1.rmbatch3,T1.rmbatch4,T1.rmbatch5,T1.qa,T1.pro1,T1.res,T3.rmbatch1,T3.rmbatch2,T3.rmbatch3,T3.rmbatch4,T3.rmbatch5,T1.c5,T1.qatype,T1.qa2,T1.qa2r,T1.t1,T1.t2,T1.boxno,T1.tpno from stock T1 left join masterdata T2 on T1.material=T2.itemcode left join stock T3 on T1.rmbatch=T3.batch where T1.batch='{0}'", textBox1.Text.Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    desc1.Text = dt.Rows[0][0].ToString().Trim();
                    stockin.Text= dt.Rows[0][1].ToString().Trim();
                    sp1.Text= dt.Rows[0][2].ToString().Trim();
                    sloc.Text=dt.Rows[0][3].ToString().Trim();
                    itemcode.Text= dt.Rows[0][4].ToString().Trim();
                    qty.Text= dt.Rows[0][5].ToString().Trim();
                    weight.Text= dt.Rows[0][6].ToString().Trim();
                    pc0.Text= dt.Rows[0][7].ToString().Trim();
                    pc01.Text= dt.Rows[0][8].ToString().Trim();
                    pc02.Text= dt.Rows[0][9].ToString().Trim();

                    pc1.Text = dt.Rows[0][16].ToString().Trim();
                    pc2.Text = dt.Rows[0][17].ToString().Trim();
                    pc3.Text= dt.Rows[0][18].ToString().Trim();
                    pc4.Text= dt.Rows[0][19].ToString().Trim();
                    pc5.Text= dt.Rows[0][20].ToString().Trim();

                    fwx.Text= dt.Rows[0][21].ToString().Trim();
                    if (fwx.Text.Length >= 4)
                    {
                       fwx.Text = fwx.Text.Substring(4, fwx.Text.Length - 4); 
                    }
                    fwy.Text= dt.Rows[0][21].ToString().Trim();
                    if (fwy.Text.Length >= 3)
                    {
                        fwy.Text = fwy.Text.Substring(0, 3);
                    }
                    comboBox1.Text= dt.Rows[0][13].ToString().Trim();
                    labitem.Text= dt.Rows[0][4].ToString().Trim();
                    qatype.Text = dt.Rows[0][22].ToString().Trim();
                    qa2.Text = dt.Rows[0][23].ToString().Trim();
                    qa2r.Text = dt.Rows[0][24].ToString().Trim();
                    t1.Text = dt.Rows[0][25].ToString().Trim();
                    t2.Text = dt.Rows[0][26].ToString().Trim();
                    boxno.Text = dt.Rows[0][27].ToString().Trim();
                    tpno.Text = dt.Rows[0][28].ToString().Trim();
                }
                else
                {

                    desc1.Text = "";
                    stockin.Text = "";
                    sp1.Text = "";
                    sloc.Text="";
                    itemcode.Text = "";
                    qty.Text = "";
                    weight.Text = "";
                    pc0.Text = "";
                    pc01.Text = "";
                    pc02.Text = "";
                    pc1.Text = "";
                    pc2.Text = "";
                    pc3.Text = "";
                    pc4.Text = "";
                    pc5.Text = "";
                    comboBox1.Text = "";
                    labitem.Text = "";
                    fwx.Text = "";
                    fwy.Text = "";
                    qatype.Text = "";
                    qa2.Text = "";
                    qa2r.Text = "";
                    t1.Text = "";
                    t2.Text = "";
                    boxno.Text = "";
                    tpno.Text = "";
                }

                freshdv3();

            }
        }

        private void freshitem2()
        {
            freshlabrecord();
            if (textBox1.Text != "")
            {
                string sql = string.Format("select top 1 T2.desc1,T1.stockin,T2.sp1,T1.sloc,T2.itemcode,T1.len,T1.pro1,T1.rmbatch,T1.rmbatch1,T1.rmbatch2,T1.rmbatch3,T1.rmbatch4,T1.rmbatch5,T1.qa,T1.pro1,T1.res,T1.rmbatch1,T1.rmbatch2,T1.rmbatch3,T1.rmbatch4,T1.rmbatch5,T1.c5,T1.qatype,T1.qa2,T1.qa2r,T1.t1,T1.t2,T1.boxno,T1.tpno from stock T1 left join masterdata T2 on T1.material=T2.itemcode where T1.batch='{0}'", textBox1.Text.Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    desc1.Text = dt.Rows[0][0].ToString().Trim();
                    stockin.Text = dt.Rows[0][1].ToString().Trim();
                    sp1.Text = dt.Rows[0][2].ToString().Trim();
                    sloc.Text = dt.Rows[0][3].ToString().Trim();
                    itemcode.Text = dt.Rows[0][4].ToString().Trim();
                    qty.Text = dt.Rows[0][5].ToString().Trim();
                    weight.Text = dt.Rows[0][6].ToString().Trim();
                    pc0.Text = dt.Rows[0][7].ToString().Trim();
                    pc01.Text = dt.Rows[0][8].ToString().Trim();
                    pc02.Text = dt.Rows[0][9].ToString().Trim();

                    pc1.Text = dt.Rows[0][16].ToString().Trim();
                    pc2.Text = dt.Rows[0][17].ToString().Trim();
                    pc3.Text = dt.Rows[0][18].ToString().Trim();
                    pc4.Text = dt.Rows[0][19].ToString().Trim();
                    pc5.Text = dt.Rows[0][20].ToString().Trim();

                    fwx.Text = dt.Rows[0][21].ToString().Trim();
                    if (fwx.Text.Length >= 4)
                    {
                        fwx.Text = fwx.Text.Substring(4, fwx.Text.Length - 4);
                    }
                    fwy.Text = dt.Rows[0][21].ToString().Trim();
                    if (fwy.Text.Length >= 3)
                    {
                        fwy.Text = fwy.Text.Substring(0, 3);
                    }
                    comboBox1.Text = dt.Rows[0][13].ToString().Trim();
                    labitem.Text = dt.Rows[0][4].ToString().Trim();
                    qatype.Text = dt.Rows[0][22].ToString().Trim();
                    qa2.Text = dt.Rows[0][23].ToString().Trim();
                    qa2r.Text = dt.Rows[0][24].ToString().Trim();
                    t1.Text = dt.Rows[0][25].ToString().Trim();
                    t2.Text = dt.Rows[0][26].ToString().Trim();
                    boxno.Text = dt.Rows[0][27].ToString().Trim();
                    tpno.Text = dt.Rows[0][28].ToString().Trim();
                }
                else
                {

                    desc1.Text = "";
                    stockin.Text = "";
                    sp1.Text = "";
                    sloc.Text = "";
                    itemcode.Text = "";
                    qty.Text = "";
                    weight.Text = "";
                    pc0.Text = "";
                    pc01.Text = "";
                    pc02.Text = "";
                    pc1.Text = "";
                    pc2.Text = "";
                    pc3.Text = "";
                    pc4.Text = "";
                    pc5.Text = "";
                    comboBox1.Text = "";
                    labitem.Text = "";
                    fwx.Text = "";
                    fwy.Text = "";
                    qatype.Text = "";
                    qa2.Text = "";
                    qa2r.Text = "";
                    t1.Text = "";
                    t2.Text = "";
                    boxno.Text = "";
                    tpno.Text = "";
                }

                

            }
        }
        private void freshdv3()
        {
            if (pc0.Text != "")
            {
                string sql2 = string.Format("select batch as 卷号,batchbig as 批号,qa as 质检,t1 as 开始日期,t2 as 结束日期,c5 as 产线,sloc as 库位,boxno as 箱号,qatype as 类别 from stock where rmbatch='{0}'", pc0.Text);
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                ds1 = Class1.GetAllDataSet(sql2);
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    dataGridView3.DataSource = dt1;
                    for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                    {
                        if (dataGridView3.Rows[i].Cells[0].Value.ToString().Trim() == textBox1.Text)
                        {
                            dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                        }
                    }
                }
                else
                {
                    dataGridView3.DataSource = null;
                }

            }
            else
            {
                dataGridView3.DataSource = null;
            }
        }
        private void flashlaboption()
        {

            if (labitem.Text != "")
            {
                string sql = string.Format("select labpj as 检测项目,'' as 检测值,'' as 结论,rtrim(force) as 必检,upper as 指标上限, lower as 指标下限,rtrim(upoum) as 单位,rtrim(comm1) as 关键指标,comm2 as 备注2 from laboption where itemcode='{0}' order by labpj", labitem.Text.Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                    button1.Visible = true;
                    button4.Visible = true;
                    button8.Visible = true;

                }
                else
                {
                    dataGridView2.DataSource = null;
                    button1.Visible = false;
                    button4.Visible = false;
                    button8.Visible = false;
                }

            }
            else
            {
                dataGridView2.DataSource = null;
                button1.Visible = false;
                button4.Visible = false;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (labitem.Text != "")
            {
               
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    string a0 = dataGridView2.Rows[i].Cells[3].Value.ToString().Trim();
                    string a1 = dataGridView2.Rows[i].Cells[4].Value.ToString().Trim();
                    string a2 = dataGridView2.Rows[i].Cells[5].Value.ToString().Trim();
                    string a3 = dataGridView2.Rows[i].Cells[6].Value.ToString().Trim();
                    string a4 = dataGridView2.Rows[i].Cells[7].Value.ToString().Trim();
                    string a5 = dataGridView2.Rows[i].Cells[8].Value.ToString().Trim();
                    string a6 = dataGridView2.Rows[i].Cells[0].Value.ToString().Trim();

                    if (a1 == "")
                    {
                        a1 = "99999";
                    }
                    if (a2 == "")
                    {
                        a2 = "0";
                    }
                    string sql = string.Format("update laboption set force='{0}',upper='{1}',lower='{2}',upoum='{3}',comm1='{4}',comm2='{5}' where labpj='{6}' and itemcode='{7}'", a0, a1, a2, a3, a4, a5, a6, labitem.Text);
                    int s = Class1.ExcuteScal(sql);
                    if (s == 1)
                    {
                       // MessageBox.Show("更新成功");
                    }
                }
                flashlaboption();
            }
            else
            {
                MessageBox.Show("请输入物料");
            }
            
        }

        private void labitem_TextChanged(object sender, EventArgs e)
        {
            flashlaboption();
            freshlabrecord();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainmenu mn = (mainmenu)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }



     

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (MessageBox.Show("是否切换到批次"+ dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString().Trim()+",当前未上传的质检结果将丢失?", "切换", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    textBox1.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (labitem.Text != "")
            {
                string str = Interaction.InputBox("请输入检测项目名称", "新增项目", "", -1, -1);
                string item = labitem.Text;
                string sql0 = string.Format("update laboption set labpj='{0}' where labpj='{0}' and itemcode='{1}'", str, item);
                int c = Class1.ExcuteScal(sql0);
                if (c == 0)
                {
                    string sql = string.Format("insert into laboption(itemcode,labpj,upper,lower) values('{0}','{1}','9999','0')", item, str);
                    int d = Class1.ExcuteScal(sql);
                    if (d == 1)
                    {
                        flashlaboption();
                    }
                }
                else
                {
                    MessageBox.Show("项目已存在!");
                }
            }
            else
            {
                MessageBox.Show("输入物料编码!");
            }
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql0 = string.Format("update stock set qa='{0}',qatype='{2}' where batch='{1}'", comboBox1.Text.Trim(), textBox1.Text,qatype.Text.Trim());
            int s = Class1.ExcuteScal(sql0);
            if (s == 1)
            {
                string sql = string.Format("update labwait set status='{1}' where batch='{0}'", textBox1.Text, comboBox1.Text.Trim());
                int x = Class1.ExcuteScal(sql);
                if (x == 1)
                {
                    loadsongjian();
                }
                freshdv3();
            }
            if (fwx.Text != "" & fwy.Text != "")
            {
                if (MessageBox.Show("是否更新纺位在线状态?", "在线", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string sqlx = string.Format("update wsdevice set lastqa='{0}' where workshop='{1}' and devicenum='{2}'", comboBox1.Text.Trim(), fwy.Text.Trim(), fwx.Text.Trim());
                    int x = Class1.ExcuteScal(sqlx);
                }

            }
        }




        private void finditm()
        {
            string sql = string.Format("select top 500 T1.batch as 卷号,itemcode as 物料,rtrim(T1.qatype) as 类别,rtrim(T1.c5) as 工位,rtrim(T1.boxno) as 箱号,T1.tpno as 托盘,T1.qa2 as 车间判定,T1.qa2r as 车间备注,T2.desc1 as 描述 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1");
            if (rollx.Text != "")
            {
                sql = sql + string.Format(" and T1.batch like '%{0}%'", rollx.Text);
            }
            if (qc5.Text != "")
            {
                sql = sql + string.Format(" and T1.c5 like '%{0}%'", qc5.Text);
            }
            if (qsloc.Text != "")
            {
                sql = sql + string.Format(" and T1.sloc like '%{0}%'", qsloc.Text);
            }
            if (qtcheck.Checked==true)
            {
                sql = sql + string.Format(" and T1.t2>= '{0}' and T1.t2<='{1}'", qt1.Text,qt2.Text);
            }


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView4.DataSource = dt;
            }
            else
            {
                dataGridView4.DataSource = null;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rollx.Text = "";

        }

        private void dataGridView4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                string av = dataGridView2.Rows[i].Cells[1].Value.ToString().Trim();
                string avcom = dataGridView2.Rows[i].Cells[2].Value.ToString().Trim();
                string a0 = dataGridView2.Rows[i].Cells[3].Value.ToString().Trim();
                string a1 = dataGridView2.Rows[i].Cells[4].Value.ToString().Trim();
                string a2 = dataGridView2.Rows[i].Cells[5].Value.ToString().Trim();
                string a3 = dataGridView2.Rows[i].Cells[6].Value.ToString().Trim();
                string a4 = dataGridView2.Rows[i].Cells[7].Value.ToString().Trim();
                string a5 = dataGridView2.Rows[i].Cells[8].Value.ToString().Trim();
                string a6 = dataGridView2.Rows[i].Cells[0].Value.ToString().Trim();

                if (av != "" & dataGridView2.Rows[i].DefaultCellStyle.BackColor!= Color.LawnGreen)
                {
                    string sql1 = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", textBox1.Text.Trim(), labitem.Text.Trim(), a6, av, DateTime.Now, Class1.curuser+"-"+pp.Text,avcom);
                    int c = Class1.ExcuteScal(sql1);
                    if (c == 1)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                        freshlabrecord();
                    }
                }
               
                
            }
               

            
        }

        private void rollx_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString().Trim() != "")
            {
                double up1 = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim());
                double dw1 = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString().Trim());
                double x = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString().Trim());
                
                    if (x >= dw1 & x <= up1)
                    {
                        dataGridView2.Rows[e.RowIndex].Cells[2].Value = "合格";
                    }
                    else
                    {
                        dataGridView2.Rows[e.RowIndex].Cells[2].Value = "不合格";
                    }
            

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        string dbatch = textBox1.Text;
                        string dlaboption = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                        string dvalue = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                        string ddate = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                        string duser = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();


                        string sql = string.Format("delete from labrecord where batch='{0}' and laboption='{1}' and labvalue='{2}' and date1='{3}' and oper='{4}'", dbatch, dlaboption, dvalue, ddate, duser);
                        int c = Class1.ExcuteScal(sql);
                        if (c == 1)
                        {
                            freshlabrecord();
                        }
                        else
                        {
                            MessageBox.Show("删除失败");
                        }
                    }
                }
            

                
            
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim())
                {

                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void loadsongjian()
        {
            string sql = string.Format("select T1.batch as 卷号,T2.material as 物料,T1.date1 as 提交日期,T2.c5 as 工位,T2.t1 as 开始日期,T2.t2 as 结束日期,t1.type1 as 分类 from labwait T1 left join stock T2 on T1.batch=T2.batch where T1.workshop='{0}' and T1.status='未确认'", comboBox2.Text);

            if (comboBox3.Text != "")
            {
                sql = sql + string.Format(" and type1 like '%{0}%'",comboBox3.Text);
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView5.DataSource = dt;
            }
            else
            {
                dataGridView5.DataSource = null;
            }
        }

        private void dataGridView5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (groupBox2.Visible == false)
            {
                groupBox2.Visible = true;
                groupBox6.Visible = false;
            }
            else
            {
                groupBox6.Visible = true;
                groupBox2.Visible = false;
            }
        }

       

        private void button12_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否批量添加选中项目的 预氧丝密度:" + x3.Text + "?", "批量更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string batch = "";
                string itemcode = "";
                string laboption = "";
                string labvalue = "";
                string date1 = "";
                string oper = "";
                string comm1 = "";

                if (groupBox2.Visible == true)
                {
                    for (int i = 0; i < dataGridView5.Rows.Count; i++)
                    {
                        if (dataGridView5.Rows[i].Selected == true)
                        {
                            batch = dataGridView5.Rows[i].Cells[0].Value.ToString().Trim();
                            itemcode = dataGridView5.Rows[i].Cells[1].Value.ToString().Trim();
                            laboption = "预氧丝密度";
                            labvalue = x3.Text;
                            date1 = DateTime.Now.ToString();
                            oper = Class1.curuser+"-"+pp.Text;

                            string SQL = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','')", batch, itemcode, laboption, labvalue, date1, oper);
                            string SQL2 = string.Format("update labwait set status='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c = Class1.ExcuteScal(SQL);
                            if (c == 1)
                            {
                                int c1 = Class1.ExcuteScal(SQL2);
                            }

                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Selected == true)
                        {
                            batch = dataGridView4.Rows[i].Cells[0].Value.ToString().Trim();
                            itemcode = dataGridView4.Rows[i].Cells[1].Value.ToString().Trim();
                            laboption = "预氧丝密度";
                            labvalue = x3.Text;
                            date1 = DateTime.Now.ToString();
                            oper = Class1.curuser + "-" + pp.Text;
  
                            string SQL3 = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','')", batch, itemcode, laboption, labvalue, date1, oper);
                            int c = Class1.ExcuteScal(SQL3);
                            string SQL4 = string.Format("update labwait set status='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c3 = Class1.ExcuteScal(SQL4);
                        }
                    }
                }
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否批量添加选中项目的 含胶量:" + x2.Text + "?", "批量更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string batch = "";
                string itemcode = "";
                string laboption = "";
                string labvalue = "";
                string date1 = "";
                string oper = "";
                string comm1 = "";

                if (groupBox2.Visible == true)
                {
                    for (int i = 0; i < dataGridView5.Rows.Count; i++)
                    {
                        if (dataGridView5.Rows[i].Selected == true)
                        {
                            batch = dataGridView5.Rows[i].Cells[0].Value.ToString().Trim();
                            itemcode = dataGridView5.Rows[i].Cells[1].Value.ToString().Trim();
                            laboption = "含胶量";
                            labvalue = x2.Text;
                            date1 = DateTime.Now.ToString();
                            oper = Class1.curuser + "-" + pp.Text;

                            string SQL = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','')", batch, itemcode, laboption, labvalue, date1, oper);
                            string SQL2 = string.Format("update labwait set status='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c = Class1.ExcuteScal(SQL);
                            if (c == 1)
                            {
                                int c1 = Class1.ExcuteScal(SQL2);
                            }

                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Selected == true)
                        {
                            batch = dataGridView4.Rows[i].Cells[0].Value.ToString().Trim();
                            itemcode = dataGridView4.Rows[i].Cells[1].Value.ToString().Trim();
                            laboption = "含胶量";
                            labvalue = x2.Text;
                            date1 = DateTime.Now.ToString();
                            oper = Class1.curuser + "-" + pp.Text;

                            string SQL3 = string.Format("insert into labrecord values('{0}','{1}','{2}','{3}','{4}','{5}','')", batch, itemcode, laboption, labvalue, date1, oper);
                            int c2 = Class1.ExcuteScal(SQL3);
                            string SQL4 = string.Format("update labwait set status='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c3 = Class1.ExcuteScal(SQL4);
                        }
                    }
                }
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            finditm();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            loadsongjian();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否批量判定质检等级为:"+comboBox1.Text+"?", "批量更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string batch = "";

                if (groupBox2.Visible == true)
                {
                    for (int i = 0; i < dataGridView5.Rows.Count; i++)
                    {
                        if (dataGridView5.Rows[i].Selected == true)
                        {
                            batch = dataGridView5.Rows[i].Cells[0].Value.ToString().Trim();
                            
                            string SQL = string.Format("update stock set qa='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c = Class1.ExcuteScal(SQL);
                            string SQLX = string.Format("update labwait set status='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c2 = Class1.ExcuteScal(SQLX);
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Selected == true)
                        {
                            batch = dataGridView4.Rows[i].Cells[0].Value.ToString().Trim();

                            string SQL2 = string.Format("update stock set qa='{1}' where batch='{0}'", batch, comboBox1.Text);
                            int c = Class1.ExcuteScal(SQL2);
                        }
                    }
                }
            }

        }

        private void x1_TextChanged(object sender, EventArgs e)
        {

        }

        labadd la = labadd.ChildFromInstanc;
        private void button16_Click(object sender, EventArgs e)
        {
 
            if (la != null)
            {
                la.Owner = this;
                la.Show();
                
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (la != null)
            {
                if (groupBox2.Visible == true)
                {
                    for (int i = 0; i < dataGridView5.Rows.Count; i++)
                    {
                        if (dataGridView5.Rows[i].Selected == true)
                        {

                            la.addlab(dataGridView5.Rows[i].Cells[0].Value.ToString(),dataGridView5.Rows[i].Cells[1].Value.ToString());
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dataGridView4.Rows.Count; i++)
                    {
                        if (dataGridView4.Rows[i].Selected == true)
                        {
                            la.addlab(dataGridView4.Rows[i].Cells[0].Value.ToString(), dataGridView4.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                }
            }
        }

        private void qt1_ValueChanged(object sender, EventArgs e)
        {
            qt2.Value = qt1.Value;
        }

        private void button17_Click(object sender, EventArgs e)
        {

            if (Class1.curuser != "labadmin")
            {
                MessageBox.Show("请登录管理员账号操作");
            }
            else
            {
                string str = Interaction.InputBox("删除的检测项目名称", "删除项目", "", -1, -1);
                string delsql = string.Format("delete from laboption where labpj='{0}' and itemcode='{1}' and (select count(labpj) from laboption where itemcode='{1}')>1", str, labitem.Text);

                int c = Class1.ExcuteScal(delsql);
                if (c == 1)
                {
                    MessageBox.Show("删除成功");
                    flashlaboption();
                }
                else
                {
                    MessageBox.Show("删除失败，至少要保留一个检测项目");
                }
            }

            

        }

        private void button18_Click(object sender, EventArgs e)
        {
            
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {

                    string dbatch = textBox1.Text;
                    string dlaboption = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    string dvalue = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    string ddate = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string duser = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    string sql = string.Format("delete from labrecord where batch='{0}' and laboption='{1}' and labvalue='{2}' and date1='{3}' and oper='{4}'", dbatch, dlaboption, dvalue, ddate, duser);
                    int c = Class1.ExcuteScal(sql);

                }
                freshlabrecord();
            

        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Hide();
            lab2 lb2 = lab2.ChildFromInstanc;
            if (lb2 != null)
            {
                lb2.Owner = this;
                lb2.Show();

            }
        }
    }
}
