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
    public partial class pregen : Form
    {
        public pregen()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            workshop.Text = Class1.workshop;
            itmlist();
            //Init1();
            dt1.Text = DateTime.Now.ToString();
            dt2.Text = DateTime.Now.ToString();
            dt3.Text = DateTime.Now.ToString();
            dt4.Text = DateTime.Now.ToString();
            listView1.HideSelection = false;
            comboBox2.Text = Class1.shift1;
        }

        public DataSet dsgen = new DataSet(); //listview location
        public DataTable dtgen = new DataTable();

        public DataSet dsbox = new DataSet(); //box batch info
        public DataTable dtbox = new DataTable();

        private void itmlist()
        {
            itm.Items.Clear();
            string sql = string.Format("select distinct itemrun from wsdevice where workshop='{0}'", Class1.workshop);
            dsgen = Class1.GetAllDataSet(sql);
            dtgen = dsgen.Tables[0];
            if (dtgen.Rows.Count > 0)
            {
                for (int i = 0; i < dtgen.Rows.Count; i++)
                {
                    itm.Items.Add(dtgen.Rows[i][0].ToString());
                }
            }
        }
        private void freshlistview(string SQL)
        {
            gw.Text = "";
            dsgen = Class1.GetAllDataSet(SQL);
            dtgen = dsgen.Tables[0];
            if (dtgen.Rows.Count > 0)
            {
                listView1.Items.Clear();
                for (int x = 0; x < dtgen.Rows.Count - 1; x++)
                {
                    listView1.Items.Add(dtgen.Rows[x][0].ToString());
                    gw.Text = gw.Text + dtgen.Rows[x][0].ToString() + ",";
                }
                listView1.Items.Add(dtgen.Rows[dtgen.Rows.Count - 1][0].ToString());
                gw.Text = gw.Text + dtgen.Rows[dtgen.Rows.Count - 1][0].ToString();

                foreach (ListViewItem e in listView1.Items)
                {
                    e.Selected = true;
                }
            }
            else
            {
                listView1.Items.Clear();

            }
        }

        private void Init1()
        {
            string sql = string.Format("select devicenum,status2,t3,itemrun from wsdevice where workshop='{0}'", workshop.Text);
            freshlistview(sql);
        }

        private void pregen_Load(object sender, EventArgs e)
        {

        }


        private static pregen childFromInstanc;
        public static pregen ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new pregen();
                }

                return childFromInstanc;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void itmchange()
        {
            checkBox3.Checked = false;
            string sql = "";
            if (itm.Text != "")
            {
                sql = string.Format("select devicenum,status2,t3,t4,itemrun from wsdevice where workshop='{0}' and itemrun='{1}'", workshop.Text, itm.Text);
            }
            else
            {
                sql = string.Format("select top 1 devicenum,status2,t3,t4,itemrun from wsdevice where workshop='{0}'", workshop.Text);
            }

            if (checkBox1.Checked == true)
            {
                sql = sql + " and status1='运行'";
            }
            if (checkBox2.Checked == true)
            {
                sql = sql + " and status2='运行'";
            }
            if (s1.Checked == true)
            {
                sql = sql + string.Format(" and t3>='{0}'", dt1.Text);
            }
            if (s2.Checked == true)
            {
                sql = sql + string.Format(" and t3<='{0}'", dt2.Text);
            }
            if (s3.Checked == true)
            {
                sql = sql + string.Format(" and t4>='{0}'", dt3.Text);
            }
            if (s4.Checked == true)
            {
                sql = sql + string.Format(" and t4<='{0}'", dt4.Text);
            }

            freshlistview(sql);


        }

        private void itm_SelectedIndexChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void itm_TextChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void s1_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void s3_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void s2_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void s4_CheckedChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void dt3_ValueChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void dt4_ValueChanged(object sender, EventArgs e)
        {
            itmchange();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            lvitemselectchange();

        }

        private void lvitemselectchange()
        {
            if (checkBox3.Checked == true)
            {
                gw.Text = "";
                foreach (ListViewItem lv in listView1.Items)
                {
                    if (lv.Selected == true)
                    {
                        gw.Text = gw.Text + lv.Text + ",";
                    }
                }
                if (gw.Text.Length > 0)
                {
                    gw.Text = gw.Text.Substring(0, gw.Text.Length - 1);
                }
            }
        }


        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            checkBox3.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = false;
            gw.Text = "";
            foreach (ListViewItem lv in listView1.Items)
            {
                lv.Selected = true;
                gw.Text = gw.Text + lv.Text + ",";
            }
            if (gw.Text.Length > 0)
            {
                gw.Text = gw.Text.Substring(0, gw.Text.Length - 1);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fboxno.Text = workshop.Text + "-" + DateTime.Now.ToString("yyyyMMdd");
            genlabel.Text = "生成标签";
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            textBox1.Text = "";

            if (itm.Text != "")
            {
                string sqlitm = string.Format("select boxnm,comm2 from box where workshop='{0}' and itemcode='{1}'", workshop.Text, itm.Text);
                dsbox = Class1.GetAllDataSet(sqlitm);
                dtbox = dsbox.Tables[0];
                if (dtbox.Rows.Count > 0)
                {
                    for (int i = 0; i < dtbox.Rows.Count; i++)
                    {
                        comboBox1.Items.Add(dtbox.Rows[i][0].ToString());
                    }
                }
            }


            string sql = string.Format("select rtrim(workshop)+'-'+convert(nvarchar,devicenum) as 工位,status2 as 状态,t3 as 上卷时间,'' as 下卷时间,itemrun as 物料,batchbig as 批号,batchcus as 原丝批次,'' as 卷号,'' as 重量,'' as 长度,'' as 箱号,'' as 类别 from wsdevice where workshop='{0}' and devicenum in ({1})", workshop.Text, gw.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                qtyr.Text = dt.Rows.Count.ToString();

            }
            else
            {
                dataGridView1.DataSource = null;
            }


            amode.Visible = false;
            mmode.Visible = true;
            genlabel.Visible = true;
            button19.Visible = false;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (genlabel.Text == "生成标签")
            {
                if (comboBox1.Text != "")
                {

                    if (dtbox.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtbox.Rows.Count; i++)
                        {
                            if (dtbox.Rows[i][0].ToString().Trim() == comboBox1.Text)
                            {
                                textBox1.Text = dtbox.Rows[i][1].ToString().Trim();
                            }
                        }
                    }
                }
            }
            
        }


        private void fillrx()
        {
            if (Class1.isNumeric(textBox1.Text))
            {
                if (textBox1.Text.Length == 11)
                {
                    if (dataGridView1.DataSource != null)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            dataGridView1.Rows[i].Cells[7].Value = (Convert.ToInt64(textBox1.Text) + i + 1).ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("卷号必须是11位");
                }

            }
            else
            {
                MessageBox.Show("开始卷号不是一个有效数字");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {



        }



        private void filldw(string fv, int loc)
        {
            if (dataGridView1.DataSource != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[loc].Value = fv;
                }
            }
        }

        private void cleardw(int loc)
        {
            if (dataGridView1.DataSource != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[loc].Value = "";
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            dt6.Text = DateTime.Now.ToString();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            dt5.Text = DateTime.Now.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            fillrx();
            filldw(fboxno.Text, 10);
            filldw(dt6.Text, 3);
            filldw(comboBox1.Text, 11);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            cleardw(11);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cleardw(8);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cleardw(9);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            filldw(flen.Text, 9);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            filldw(comboBox1.Text, 11);
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            filldw(fqty.Text, 8);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            filldw(fbatch.Text, 5);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            cleardw(5);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            filldw(fboxno.Text, 10);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            cleardw(10);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            filldw(dt5.Text, 2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            cleardw(2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cleardw(3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            filldw(dt6.Text, 3);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            cleardw(7);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string sql = "";
            int upflag = 0;
            if (dataGridView1.DataSource != null)
            {
                if (genlabel.Text == "生成标签")
                {
                    if (MessageBox.Show("生成后是否更新工位状态(上卷时间,批次,物料)?", "工位状态", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        upflag = 1;
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string c5 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string t1 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string t2 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string mt = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string batchbig = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string rmbatch = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        string batch = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        string wt = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        string lenx = dataGridView1.Rows[i].Cells[9].Value.ToString();
                        string boxno = dataGridView1.Rows[i].Cells[10].Value.ToString();
                        string typea = dataGridView1.Rows[i].Cells[11].Value.ToString();
                        string st2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        sql = string.Format("insert into stock(branch,sloc,c5,t1,t2,material,batchbig,rmbatch,batch,displaybatch,pro1,len,boxno,qatype,tpno,shift1) values('tz','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{7}','{8}','{9}','{10}','{11}','','{12}')",workshop.Text,c5,t1,t2,mt,batchbig,rmbatch,batch,wt,lenx,boxno,typea,comboBox2.Text);
                        
                        int c = Class1.ExcuteScal(sql);
                        if (c == 1)
                        {
                            if (upflag == 1)
                            {
                                string sqlup = string.Format("update wsdevice set t3='{0}',itemrun='{1}',batchbig='{2}',status2='{3}' where workshop='{4}' and devicenum='{5}'",t2,mt,batchbig,st2,workshop.Text,c5.Replace(workshop.Text+"-","").Trim());
                                int c2 = Class1.ExcuteScal(sqlup);

                            }
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                        }
                        else
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                        
                    }
                }
                else
                {
                    //修改标签
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string c5 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string t1 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string t2 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string mt = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string batchbig = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string rmbatch = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        string batch = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        string wt = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        string lenx = dataGridView1.Rows[i].Cells[9].Value.ToString();
                        string boxno = dataGridView1.Rows[i].Cells[10].Value.ToString();
                        string typea = dataGridView1.Rows[i].Cells[11].Value.ToString();

                        sql = string.Format("update stock set c5='{0}',t1='{1}',t2='{2}',material='{3}',batchbig='{4}',rmbatch='{5}',pro1='{6}',len='{7}',boxno='{8}',qatype='{9}' where batch='{10}'",c5,t1,t2,mt,batchbig,rmbatch,wt,lenx,boxno,typea,batch);

                        int c = Class1.ExcuteScal(sql);
                        if (c == 1)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                        }
                        else
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }

                    }
                }
            }
        }

        private void mmode_Click(object sender, EventArgs e)
        {
            genlabel.Text = "修改标签";
            mmode.Visible = false;
            amode.Visible = true;
            button1.Visible = false;
            button19.Visible = true;
            dataGridView1.DataSource = null;

        }

        private void amode_Click(object sender, EventArgs e)
        {
            genlabel.Text = "生成标签";
            mmode.Visible = true;
            amode.Visible = false;
            button1.Visible = true;
            button19.Visible = false;
            dataGridView1.DataSource = null;
        }

        private void whenmodify()
        {
            if (genlabel.Text == "修改标签")
            {
                textBox1.Text = "";


                string sql = string.Format("select c5 as 工位,'',t1 as 上卷时间,t2 as 下卷时间,material as 物料,batchbig as 批号,rmbatch as 原丝批次,batch as 卷号,pro1 as 重量,len as 长度,boxno as 箱号,qatype as 类别 from stock where sloc='{1}' and len(batch)=11", fboxno.Text, workshop.Text);
                if (fboxno.Text != "")
                {
                    sql = sql + string.Format(" and boxno='{0}'", fboxno.Text);
                }
                if (fbatch.Text != "")
                {
                    sql = sql + string.Format(" and batchbig='{0}'", fbatch.Text);
                }
                if (comboBox1.Text != "")
                {
                    sql = sql + string.Format(" and qatype='{0}'", comboBox1.Text);
                }
                if (itm.Text != "")
                {
                    sql = sql + string.Format(" and material='{0}'", itm.Text);
                }
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;

                    dataGridView1.ReadOnly = false;
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        if (c.Index == 7)
                        {
                            c.ReadOnly = true;
                        }
                    }
                }
                else
                {
                    dataGridView1.DataSource = null;
                }


            }
        }


        private void fboxno_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void fbatch_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            whenmodify();
        }

        private void button20_Click(object sender, EventArgs e)
        {

            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}