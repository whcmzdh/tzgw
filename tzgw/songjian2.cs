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
    public partial class songjian2 : Form
    {
        public songjian2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            songjian thsj = (songjian)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                string status1 = "0";
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "首卷")
                {
                    status1 = "0";
                }
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "循环1")
                {
                    status1 = "1";
                }
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "循环2")
                {
                    status1 = "2";
                }
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "其他")
                {
                    status1 = "3";
                }
                string devn = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value).Trim();
                string proj = comboBox1.Text;
                string time1 = dateTimePicker1.Text + " " + Convert.ToString(dataGridView1.Rows[i].Cells[1].Value).Trim(); ;
                string rm = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value).Trim();
                string batchbig = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value).Trim();
                string comm1 = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value).Trim();

                string strSQL1 = string.Format("insert into labrecord2(workshop,date1,shift1,devicenum,project,status,date2,rm,batchbig,comm1,itemcode) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", Class1.workshop,time1,Class1.shift1,devn,proj,status1,time1,rm,batchbig,comm1,textBox2.Text);
                if (dataGridView1.Rows[i].DefaultCellStyle.BackColor != Color.LawnGreen)
                {
                    int sx = Class1.ExcuteScal(strSQL1);
                    if (sx == 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                    }
                }
                
            }
        }

        private static songjian2 childFromInstanc;
        public static songjian2 ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new songjian2();
                }

                return childFromInstanc;
            }
        }

        public void init1()
        {
            textBox1.Text = Class1.workshop + "-" + Class1.shift1;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dataGridView1.Rows[e.RowIndex].Cells[1].Value = DateTime.Now.ToString("HH:mm");
                string sql1 = string.Format("select top 1 status+1 from labrecord2 where workshop='{0}' and shift1='{1}' and date1='{2}' and devicenum='{3}'", Class1.workshop, Class1.shift1, dateTimePicker1.Text, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                string sql2 = string.Format("select top 1 batchcus,batchbig from wsdevice where workshop='{0}' and devicenum='{1}'", Class1.workshop, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql1);
                dt = ds.Tables[0];
                string x = "0";
                if (dt.Rows.Count > 0)
                {
                    x= dt.Rows[0][0].ToString().Trim();
                }

                if (x == "0")
                {
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "首卷";
                }
                else if (x == "1")
                {
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "循环1";
                }
                else if (x == "2")
                {
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "循环2";
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = "其他";
                }

                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();
                ds2 = Class1.GetAllDataSet(sql2);
                dt2 = ds2.Tables[0];
  
                if (dt2.Rows.Count > 0)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = dt2.Rows[0][0].ToString().Trim();
                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = dt2.Rows[0][1].ToString().Trim();
                }


               



            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("SELECT labpj FROM laboption where itemcode='{0}'",textBox2.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                comboBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(dt.Rows[i][0].ToString());
                    
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
            }

        }
    }
}
