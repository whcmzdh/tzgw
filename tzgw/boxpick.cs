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
    public partial class boxpick : Form
    {
        public boxpick()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            boxleft.Text = Class1.fromboxbatch + "-";
            c3.Text = Class1.workshop +"-"+ DateTime.Now.ToString("yyyyMMdd");
            c7.Text = Class1.workshop;

        }

        private void boxpick_Load(object sender, EventArgs e)
        {

        }

        private static boxpick childFromInstanc;
        public static boxpick ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new boxpick();
                }

                return childFromInstanc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flag = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                {
                    MessageBox.Show("第"+(i+1).ToString()+ "物料 # 编码 # 错误，请检查");
                    flag = 1;
                    break;
                }
                if (dataGridView2.Rows[i].DefaultCellStyle.BackColor == Color.Yellow)
                {
                    MessageBox.Show("第" + (i + 1).ToString() + "物料 # 类别 # 错误，请检查");
                    flag = 1;
                    break;
                }
                if (dataGridView2.Rows[i].DefaultCellStyle.BackColor == Color.Orange)
                {
                    MessageBox.Show("第" + (i + 1).ToString() + "物料 # 批次 # 错误，请注意!");
                    
                    break;
                }
                if (dataGridView2.Rows[i].DefaultCellStyle.BackColor == Color.LightBlue)
                {
                    MessageBox.Show("第" + (i + 1).ToString() + "物料 # 无装箱日期 # ，请注意!");
                }

            }

            if (flag == 0)
            {
                Class1.backboxbatch = boxleft.Text;
                tansi_hou th = (tansi_hou)this.Owner;
                if (boxleft.Text.Length > 9)
                {
                    th.backbox();
                    if (MessageBox.Show("是否更新此箱类别为" + textBox1.Text + "?", "更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string sql1 = string.Format("update stock set qatype='{0}' where boxno='{1}'", textBox1.Text, boxleft.Text);
                        int a = Class1.ExcuteScal(sql1);

                    }

                }
                this.Owner.Show();
                this.Dispose();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                if(c2.Text.Length == Convert.ToInt32(len1.Text))
                {
                    finditem2();
                }
            }


        }


        private void finditem2()
        {
            string sql00 = "";
            if (bcbtn.Checked == true)
            {
                sql00 = string.Format("update stock set boxno='{0}',dateboxin='{3}' where batch='{1}' and sloc='{2}' and left(boxno,2)='ts'", boxleft.Text, c2.Text, Class1.workshop, dateTimePicker5.Text);
            }
            else
            {
                sql00 = string.Format("update stock set boxno='{0}' where batch='{1}' and sloc='{2}' and left(boxno,2)='ts'", boxleft.Text, c2.Text, Class1.workshop);

            }
            int c=Class1.ExcuteScal(sql00);
            if (c ==1)
            {
                label5.BackColor = Color.LawnGreen;
                freshleftbox();
                c2.Text = "";
                c2.Focus();

            }
            else
            {
                label5.BackColor = Color.Red;
                c2.Text = "";
                c2.Focus();
            }
            
            
        }

        private void finditem()
        {

            string sql = "";
            string sqlt = "";
            string sql0 = "";

            if (textBox2.Text != "")
            {
                sql0 = string.Format("select top {0} T1.batch as 卷号,T2.desc1 as 描述,T1.pro1 as 重量,T1.boxno as 箱号,T1.material as 物料,T1.batchbig as 批次,T1.t1 as 开始生产时间,T1.t2 as 结束生产时间,T1.qa as 质检,T1.qatype as 类型,T1.sloc as 库位 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1", textBox2.Text);

            }
            else
            {
                sql0 = string.Format("select T1.batch as 卷号,T2.desc1 as 描述,T1.pro1 as 重量,T1.boxno as 箱号,T1.material as 物料,T1.batchbig as 批次,T1.t1 as 开始生产时间,T1.t2 as 结束生产时间,T1.qa as 质检,T1.qatype as 类型,T1.sloc as 库位 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1", textBox2.Text);
            }



            if (checkBox0.Checked == true)
            {
                if (c1.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.material='{0}'", c1.Text.Trim());
                }
                if (c2.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.batch='{0}'", c2.Text.Trim());
                }
                if (c3.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.boxno='{0}'", c3.Text.Trim());
                }
                if (c6.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.batchbig='{0}'", c6.Text.Trim());
                }
                if (c9.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.qatype='{0}'", c9.Text.Trim());
                }
            }
            else
            {
                if (c1.Text != "")
                {
                    sqlt = sqlt+ string.Format(" and T1.material like '%{0}%'", c1.Text.Trim());
                }
                if (c2.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.batch like '%{0}%'", c2.Text.Trim());
                }
                if (c3.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.boxno like '%{0}%'", c3.Text.Trim());
                }
                if (c6.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.batchbig like '%{0}%'", c6.Text.Trim());
                }
                if (c9.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.qatype='{0}'", c9.Text.Trim());
                }
            }


            if (c4.Checked ==true)
            {
                sqlt = sqlt + string.Format(" and T1.t1>='{0}' and T1.t1<='{1}'", dateTimePicker1.Text, dateTimePicker3.Text);
            }
            if (c5.Checked == true)
            {
                sqlt = sqlt + string.Format(" and T1.t2>='{0}' and T1.t2<='{1}'", dateTimePicker2.Text, dateTimePicker4.Text);
            }

            string sqlx = "";

            if (c7.Text != "")
            {
                sqlx = string.Format(" and T1.sloc='{0}'", c7.Text.Trim());
            }

            string sqlx2 = string.Format(" and T1.boxno<>'{0}' order by T1.batch", boxleft.Text);

                sql = sql0 + sqlt + sqlx+ sqlx2;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (sql != "")
            {
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    decimal rwt = 0;
                    int rbx = 1;
                    dataGridView1.DataSource = dt;
                    string startbox= dataGridView1.Rows[0].Cells[3].Value.ToString().Trim();
                    for (int c = 0; c < dt.Rows.Count; c++)
                    {
                        string combox= dataGridView1.Rows[c].Cells[3].Value.ToString().Trim();
                        if (combox != startbox)
                        {
                            rbx = rbx + 1;
                        }
                        startbox= dataGridView1.Rows[c].Cells[3].Value.ToString().Trim();
                        rwt = rwt + Convert.ToDecimal(dataGridView1.Rows[c].Cells[2].Value.ToString().Trim());
                        dataGridView1.Rows[c].Selected = true;

                    }
                    rjs.Text = dt.Rows.Count.ToString();
                    rzl.Text = rwt.ToString();
                    rxs.Text = rbx.ToString();


                }
                else
                {
                    dataGridView1.DataSource = null;
                    rxs.Text = "0";
                    rjs.Text = "0";
                    rzl.Text = "0";
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }
               
          
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (MessageBox.Show("确认清空此箱内容?", "清空", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (boxleft.Text != "")
                {
                    if (ccbtn.Checked == true)
                    {
                        sql = string.Format("update stock set boxno='{1}',dateboxin=NULL where boxno='{0}' and sloc='{2}'", boxleft.Text, Class1.workshop + "-" + DateTime.Now.ToString("yyyyMMdd"), Class1.workshop);
                    }
                    else
                    {
                        sql = string.Format("update stock set boxno='{1}' where boxno='{0}' and sloc='{2}'", boxleft.Text, Class1.workshop + "-" + DateTime.Now.ToString("yyyyMMdd"), Class1.workshop);
                    }
                    int c = Class1.ExcuteScal(sql);
                    freshleftbox();
                }
                finditem();
            }
 

        }

        private void boxleft_TextChanged(object sender, EventArgs e)
        {
            string[] sArray = boxleft.Text.Split('-');
            if (sArray.Length == 3)
            {
                textBox1.Text = sArray[1].ToString();
                textBox3.Text = sArray[0].ToString();
            }
            else
            {
                textBox1.Text = "";
                textBox3.Text = "";
            }

            freshleftbox();
            

        }

        private void freshleftbox()
        {
            string sql = string.Format("select batch as 卷号,material as 物料,sloc as 库位,t1 as 开始日期,t2 as 完工日期,pro1 as 重量,len as 长度,qatype as 类别,batchbig as 批号,dateboxin as 装箱日期 from stock where boxno='{0}'",boxleft.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
                boxcount.Text = dt.Rows.Count.ToString();
                string x = dt.Rows[0][1].ToString().Trim();
                string y = textBox1.Text;
                string batchboxnum = "0";
                int findm = boxleft.Text.IndexOf('-');
                if (findm!=-1)
                {
                    batchboxnum = boxleft.Text.Substring(0, findm);
                    
                }
                
         
                //MessageBox.Show(x);
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.DefaultCellStyle.BackColor = SystemColors.Control;
                    if (dataGridView2.Rows[i].Cells[7].Value.ToString().Trim() != y)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString().Trim() != x)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    if (dataGridView2.Rows[i].Cells[8].Value.ToString().Trim() != batchboxnum)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                    }
                    if (dataGridView2.Rows[i].Cells[9].Value.ToString().Trim() =="")
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }


                }
                //dateTimePicker5.Text = dataGridView2.Rows[0].Cells[9].Value.ToString().Trim();
            }
            else
            {
                dataGridView2.DataSource = null;
                boxcount.Text = "0";
                dateTimePicker5.Text = DateTime.Now.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql0 = "";
            if (boxleft.Text != "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {

                        if (bcbtn.Checked == true)
                        {
                            sql0 = string.Format("update stock set boxno='{0}',dateboxin='{3}' where batch='{1}' and sloc='{2}' and left(boxno,2)='ts'", boxleft.Text, dataGridView1.Rows[i].Cells[0].Value.ToString(), Class1.workshop, dateTimePicker5.Text);
                        }
                        else
                        {
                            sql0 = string.Format("update stock set boxno='{0}' where batch='{1}' and sloc='{2}' and left(boxno,2)='ts'", boxleft.Text, dataGridView1.Rows[i].Cells[0].Value.ToString(), Class1.workshop);
                        }
                        Class1.ExcuteScal(sql0);
                    }


                }
                freshleftbox();
                finditem();

            }
            else
            {
                MessageBox.Show("请输入箱号");
            } 

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {

                c1.Enabled = false;
                c3.Enabled = false;
                c4.Checked = false;
                c5.Checked = false;
                checkBox0.Checked = true;
                c1.Text = "";
                c2.Text = "";
                c3.Text = "";
                c6.Text = "";
                c2.Focus();
                if (boxleft.Text.Length <= 9)
                {
                    MessageBox.Show("注意：箱号过短");
                }
            }
            else
            {

                c1.Enabled = true;
                c3.Enabled = true;


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql0 = "";
            if (boxleft.Text != "")
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Selected == true)
                    {
                        if (ccbtn.Checked == true)
                        {
                            sql0 = string.Format("update stock set boxno='{1}',dateboxin=NULL where batch='{0}' and sloc='{2}'", dataGridView2.Rows[i].Cells[0].Value.ToString(), Class1.workshop + "-" + DateTime.Now.ToString("yyyyMMdd"), Class1.workshop);

                        }
                        else
                        {
                            sql0 = string.Format("update stock set boxno='{1}' where batch='{0}' and sloc='{2}'", dataGridView2.Rows[i].Cells[0].Value.ToString(), Class1.workshop + "-" + DateTime.Now.ToString("yyyyMMdd"), Class1.workshop);
                        }
                        Class1.ExcuteScal(sql0);
                    }


                }
                freshleftbox();

            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear1();
        }


        private void clear1()
        {
            c1.Text = "";
            c2.Text = "";
            c3.Text = "";
            c6.Text = "";
            c9.Text = "";
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (c4.Checked == true)
            {
                dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            }
            
        }

        private void checkBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            if (c5.Checked == true)
            {
                dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void c9_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认将此箱所有卷类别更改为"+textBox1.Text+"?", "批量更改", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (boxleft.Text != "")
                {
                    string sql = string.Format("update stock set qatype='{1}' where boxno='{0}' and sloc='{2}'", boxleft.Text, textBox1.Text, Class1.workshop);
                    int c = Class1.ExcuteScal(sql);
                    freshleftbox();
                }
                
            }

         }

        private void button7_Click(object sender, EventArgs e)
        {
            finditem();
        }

        private void c4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void c9_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select top 1 T1.boxno from stock T1 where T1.boxno like '%{0}%' order by convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) desc", boxleft.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show(dt.Rows[0][0].ToString());
            }
            else
            {
                MessageBox.Show("无");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            showboxlist sbl = showboxlist.ChildFromInstanc;
            if (sbl != null)
            {
                sbl.Owner = this;
                sbl.Show();
            }
        }

        private void c1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {
            dateTimePicker5.Text = DateTime.Now.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认将此箱所有卷批次更改为" + textBox3.Text + "?", "批量更改", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (boxleft.Text != "")
                {
                    string sql = string.Format("update stock set batchbig='{1}' where boxno='{0}' and sloc='{2}'", boxleft.Text, textBox3.Text, Class1.workshop);
                    int c = Class1.ExcuteScal(sql);
                    freshleftbox();
                }

                string[] sArray = boxleft.Text.Split('-');
                string x = boxleft.Text.Replace(sArray[0].ToString(), textBox3.Text);

                if (MessageBox.Show("是否同时修改箱号为" + x + "?", "更改", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string sql = string.Format("update stock set boxno='{1}' where boxno='{0}' and sloc='{2}'", boxleft.Text, x, Class1.workshop);
                    int c = Class1.ExcuteScal(sql);
                    boxleft.Text = x;
                }




            }
        }
    }
}
