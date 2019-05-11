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
    public partial class songjian : Form
    {
        public songjian()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            gw.Text = Class1.workshop;
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }

        public void init1(string x)
        {
            boxno.Text = x;
            dateTimePicker1.Value = DateTime.Now;
            freshdata2();
        }

        private static songjian childFromInstanc;
        public static songjian ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new songjian();
                }

                return childFromInstanc;
            }
        }

        private void songjian_Load(object sender, EventArgs e)
        {

        }

        private void batch_TextChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                freshdata1();

            }
            else
            {
                if (batch.Text.Length == 11)
                {

                        dxq.Rows.Add();
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[0].Value = batch.Text;
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[1].Value = "未提交";
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[2].Value = comboBox1.Text;
                    batch.Text = "";
                    
                }


            }
                    
        }

        private void freshdata2()
        {
            string sql1 = string.Format("select top 500 batch as 卷号,workshop as 车间,date1 as 日期,status as 状态,type1 as 类型 from labwait where workshop='{0}' and status='未确认' order by batch desc,date1 desc", Class1.workshop);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
            }
            else
            {
                dataGridView2.DataSource = null;
            }
        }

        private void freshdata1()
        {
            string sql1 = "";
            string sql2 = "";
 
            sql1 = string.Format("select top 500 stock.batch as 卷号,isnull(labwait.status,'未提交') as 状态,c5 as 工位,material as 物料,t1 as 上卷时间,t2 as 下卷时间,pro1 as 重量,len as 长度,qa as 质检,qatype as 类型,boxno as 箱号,rmbatch as 原丝批次 from stock left join labwait on stock.batch=labwait.batch where 1=1");

            if (boxno.Text != "")
            {
                sql2 = sql2+ string.Format(" and boxno='{0}'", boxno.Text);
            }
            if (batch.Text != "")
            {
                sql2 = sql2+ string.Format(" and stock.batch='{0}'", batch.Text);
            }
            if (batchbig.Text != "")
            {
                sql2 = sql2+ string.Format(" and batchbig='{0}'", batchbig.Text);
            }
            if (checkBox1.Checked == true)
            {
                sql2 = sql2 + string.Format(" and year(t2)='{0}' and month(t2)='{1}' and day(t2)='{2}'", dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            }
            if (gw.Text != "")
            {
                sql2 = sql2 + string.Format(" and c5 like '%{0}%'", gw.Text);
            }
            string sql = sql1 + sql2;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {

                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length>=3 && dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0,3) == "未确认")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }

                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length >= 3 && dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 3) == "已完成")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                    }
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dxq.Rows.Count; i++)
            {
                if (dxq.Rows[i].Selected == true)
                {
                    string b1 = dxq.Rows[i].Cells[0].Value.ToString().Trim();
                    string b2 = dxq.Rows[i].Cells[2].Value.ToString().Trim();
                    string sql = string.Format("insert into labwait(batch,workshop,date1,status,type1) values('{0}','{1}','{2}','{3}','{4}')", b1, Class1.workshop, DateTime.Now.ToString("yyyy/MM/dd HH:mm"), "未确认", b2);
                        //MessageBox.Show(sql);
                        int c = Class1.ExcuteScal(sql);
                        if (c == 1)
                        {
                            dxq.Rows[i].Cells[1].Value="成功";
                        }

                }
            }
            if (MessageBox.Show("是否清空待选区?", "清空", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dxq.Rows.Clear();
            }
            freshdata1();
            freshdata2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            freshdata2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void boxno_TextChanged(object sender, EventArgs e)
        {
            freshdata1();
        }

        private void batchbig_TextChanged(object sender, EventArgs e)
        {
            freshdata1();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            freshdata1();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                if (dataGridView2.Rows[i].Selected == true)
                {
                    string sql = string.Format("delete from labwait where batch='{0}' and status='未确认'",dataGridView2.Rows[i].Cells[0].Value.ToString().Trim());
                    //MessageBox.Show(sql);
                    int c = Class1.ExcuteScal(sql);

                }
            }
            freshdata2();
            freshdata1();
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    dataGridView2.Rows[i].Selected = false;
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString()==dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString())
                    {
                        dataGridView2.Rows[i].Selected = true;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void b5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Selected==true)
                {
                    string batch = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string sql = string.Format("SELECT T1.laboption,T1.labvalue,T1.date1,T1.comm1  FROM labrecord T1 where T1.batch='{0}' and T1.laboption in ('含胶量','预氧丝密度') order by T1.laboption", batch);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = Class1.GetAllDataSet(sql);
                    dt = ds.Tables[0];
                    string output1 = "";
                    if (dt.Rows.Count > 0)
                    {
                        for (int s = 0; s < dt.Rows.Count; s++)
                        {
                            output1 = output1 + "\n\r " + dt.Rows[s][0].ToString().Trim() + " 检测值:" + dt.Rows[s][1].ToString().Trim() + " | " + dt.Rows[s][2].ToString().Trim() + " | " + dt.Rows[s][3].ToString().Trim();
                            //output1 = output1 + "\n\r " + dt.Rows[s][0].ToString().Trim() + " ----- "+dt.Rows[s][5].ToString().Trim();
                        }
                        MessageBox.Show(output1);
                    }
                }


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            freshdata1();
        }

        private void gw_TextChanged(object sender, EventArgs e)
        {
            freshdata1();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true)
                    {
                        dxq.Rows.Add();
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[0].Value = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[1].Value = "未提交";
                        dxq.Rows[this.dxq.Rows.Count - 1].Cells[2].Value = comboBox1.Text;

                    }
                }
            }
            else
            {
                MessageBox.Show("检测类型为空");
            }
           

           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvRow in dxq.SelectedRows)
            {
                dxq.Rows.Remove(dgvRow);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                if (textBox1.Text == dataGridView2.Rows[i].Cells[0].Value.ToString().Trim())
                {
                    dataGridView2.FirstDisplayedScrollingRowIndex = i;
                    dataGridView2.Rows[i].Selected = true;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("送检类型为空!");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            songjian2 sj2 = songjian2.ChildFromInstanc;
            if (sj2 != null)
            {
                sj2.Owner = this;
                sj2.init1();
                sj2.Show();

            }
        }
    }

}
