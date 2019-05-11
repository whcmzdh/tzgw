using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tzgw
{
    public partial class quickrec : Form
    {
        public quickrec()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
        }


        private static quickrec childFromInstanc;
        public static quickrec ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new quickrec();
                }

                return childFromInstanc;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            t1.Text = DateTime.Now.Hour.ToString()+":"+ DateTime.Now.Minute.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Add();
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value = textBox1.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[1].Value = t1.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[2].Value = "上 "+textBox2.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[3].Value = "N";
            clear1();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Add();
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value = textBox1.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[1].Value = t1.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[2].Value = "下 " + textBox2.Text;
            dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[3].Value = "N";
            clear1();

        }

        private void clear1()
        {

            textBox1.Text = "";
            textBox2.Text = "";
            t1.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
            textBox1.Focus();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[2].Value.ToString().Substring(0, 1) == "上")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LawnGreen;
                }

            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认移除选定行", "移除", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (DataGridViewRow dgvRow in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(dgvRow);
                }
            }

        }

        public void init1()
        {
            
            String content;
            try
            {
                FileStream fs = new FileStream("c:\\data\\temprec.txt", FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                content = sr.ReadLine();
                while (null != content)
                {
                    string[] sArray = content.Split(';');
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value = sArray[0];
                    dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[1].Value = sArray[1];
                    dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[2].Value = sArray[2];
                    dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[3].Value = sArray[3];
                    content = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            catch (IOException e)
            {
                
            }

           

        }


        private void button4_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("c:\\data\\temprec.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            int s = dataGridView1.RowCount;
            for (int i = 0; i < s; i++)
            {
                sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString()+";"+ dataGridView1.Rows[i].Cells[1].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[2].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[3].Value.ToString());
            }
            sw.Close();
            fs.Close();
            
            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();

        }

        private void quickrec_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileStream fs = new FileStream("c:\\data\\temprec.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            int s = dataGridView1.RowCount;
            for (int i = 0; i < s; i++)
            {
                sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[1].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[2].Value.ToString() + ";" + dataGridView1.Rows[i].Cells[3].Value.ToString());
            }
            sw.Close();
            fs.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tansi_hou th = (tansi_hou)this.Owner;
            int s = dataGridView1.RowCount;
            for (int i = 0; i < s; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "N")
                {
                    th.inputgw(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString());
                    dataGridView1.Rows[i].Cells[3].Value = "Y";
                    break;
                }
            }

                
        }

        private void quickrec_Load(object sender, EventArgs e)
        {

        }
    }
}
