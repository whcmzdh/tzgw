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
    public partial class itemselect : Form
    {
        public itemselect()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            from1.Text = Class1.fromform;
            workshop1.Text = Class1.workshop;
        }


        private static itemselect childFromInstanc;
        public static itemselect ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new itemselect();
                }

                return childFromInstanc;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            query1();
        }


        private void query1()
        {
            string sql = string.Format("select itemcode as 物料编码,desc1 as 物料描述,sp1 as 线密度 from masterdata where desc1 like '%{0}%'", textBox1.Text);
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
                label2.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label2.Text != "-")
            {
                if (from1.Text == "ys")
                {
                    Class1.itemyuansi1 = label2.Text;
                    yuansi ys = (yuansi)this.Owner;
                    this.Owner.Show();
                    ys.itemcodeload();
                    this.Close();
                }
                if (from1.Text == "ts")
                {
                    Class1.itemtansi1 = label2.Text;
                    tansi_hou tsh = (tansi_hou)this.Owner;
                    this.Owner.Show();
                    tsh.itemcodeload();
                    this.Close();
                }
                if (from1.Text == "mu")
                {
                    Class1.itemmu1 = label2.Text;
                    aftergenyuansi ay = (aftergenyuansi)this.Owner;
                    this.Owner.Show();
                    ay.itemcodeload();
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (from1.Text == "ys")
            {

                yuansi ys = (yuansi)this.Owner;
                this.Owner.Show();
                this.Close();
            }
            if (from1.Text == "ts")
            {

                tansi_hou tsh = (tansi_hou)this.Owner;
                this.Owner.Show();
                this.Close();
            }
            if (from1.Text == "mu")
            {
               
                aftergenyuansi ay = (aftergenyuansi)this.Owner;
                this.Owner.Show();
                this.Close();
            }

        }


        private int itemrightcheck(string testitem)
        {
            switch (Class1.workshop + testitem)
            {
                default: return 0;
                case "ys11": return 1;
                case "ys22": return 1;
                case "ys33": return 1;
                case "ys44": return 1;
                case "ts77": return 1;
                case "ts010": return 1;
                case "ts020": return 1;
                case "ts88": return 1;
                case "ts25": return 1;
                case "ys66": return 1;
            }
            
        }
         

        private void button3_Click(object sender, EventArgs e)
        {
            if (itemrightcheck(t1.Text.Substring(0,1)) == 1)
            {
                string strSQL1 = string.Format("insert into masterdata(itemcode,desc1,sp1,sp2,testm,sp3,sp4,sp5,printername,lilun) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", t1.Text.Trim(), t2.Text.Trim(), t3.Text.Trim(), t4.Text.Trim(), t5.Text.Trim(), gy1.Text.Trim(), t8.Text.Trim(), t9.Text.Trim(), t7.Text.Trim(), t10.Text.Trim());
                int c = Class1.ExcuteScal(strSQL1);
                if (c == 1)
                {
                    string sql2 = string.Format("insert into laboption(itemcode,labpj) values('{0}','项目1')", t1.Text.Trim());
                    c = Class1.ExcuteScal(sql2);
                    MessageBox.Show("添加成功");
                    query1();
                }
            }
            else
            {
                MessageBox.Show("请登录相关账号");
            }

        }

        private void t1_TextChanged(object sender, EventArgs e)
        {
            string strSQL1 = string.Format("select top 1 desc1,sp1,sp2,testm,printername,sp4,sp5,lilun,sp3 from masterdata where itemcode='{0}'", t1.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(strSQL1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                add1.Enabled = false;
                mdf1.Enabled = true;
                t2.Text = dt.Rows[0][0].ToString().Trim(); //desc1
                t3.Text = dt.Rows[0][1].ToString().Trim(); //xianmidu
                t4.Text = dt.Rows[0][2].ToString().Trim(); //suoxie
                t5.Text = dt.Rows[0][3].ToString().Trim(); //shiyanliao
                t7.Text = dt.Rows[0][4].ToString().Trim(); //dayinji
                t8.Text = dt.Rows[0][5].ToString().Trim(); //qixian
                t9.Text = dt.Rows[0][6].ToString().Trim(); //biaozhun
                t10.Text = dt.Rows[0][7].ToString().Trim(); //lilun
                gy1.Text = dt.Rows[0][8].ToString().Trim(); //gy
               

            }
            else
            {
                add1.Enabled = true;
                mdf1.Enabled = false;
                t2.Text = ""; //desc1
                t3.Text = "0.0001"; //xianmidu
                t4.Text = "xK"; //suoxie
                t5.Text = "Y"; //shiyanliao
                t7.Text = "PT1"; //dayinji
                t8.Text = "12个月"; //qixian
                t9.Text = ""; //biaozhun
                t10.Text = "60"; //lilun
                gy1.Text = "1"; //gy
            }

            w1.Text = t1.Text;
            b1.Text = t1.Text;

        }

        private void mdf1_Click(object sender, EventArgs e)
        {
            if (itemrightcheck(t1.Text.Substring(0, 1)) == 1)
            {
                string strSQL1 = string.Format("update masterdata set desc1='{0}',sp1='{1}',sp2='{2}',sp3='{3}',sp4='{4}',sp5='{5}',printername='{6}',testm='{7}',lilun='{8}' where itemcode='{9}'", t2.Text.Trim(), t3.Text.Trim(), t4.Text.Trim(), gy1.Text.Trim(), t8.Text.Trim(), t9.Text.Trim(), t7.Text.Trim(), t5.Text.Trim(), t10.Text.Trim(), t1.Text.Trim());
                int c = Class1.ExcuteScal(strSQL1);
                if (c == 1)
                {
                    MessageBox.Show("修改成功");
                    query1();
                }
            }
            else
            {
                MessageBox.Show("请登录相关账号");
            }
            
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void wucha1x_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void b1_TextChanged(object sender, EventArgs e)
        {
            freshb1();
        }


        private void freshb1()
        {
            if (b1.Text != "")
            {
                string sql1 = string.Format("select boxid, boxnm, r1, r2 from box where workshop = '{0}' and itemcode = '{1}'", workshop1.Text, b1.Text);
                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql1);
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
        }

        private void w1_TextChanged(object sender, EventArgs e)
        {
            string strSQL1 = string.Format("select xiaxian,shangxian,fweight,flen from wucha where itemcode='{0}'", w1.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(strSQL1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                wucha1x.Text = dt.Rows[0][0].ToString().Trim();
                wucha1s.Text = dt.Rows[0][1].ToString().Trim();
                v1.Text = dt.Rows[0][2].ToString().Trim();
                l1.Text = dt.Rows[0][3].ToString().Trim();

                if (dt.Rows.Count > 1)
                {
                    wucha2x.Text = dt.Rows[1][0].ToString().Trim();
                    wucha2s.Text = dt.Rows[1][1].ToString().Trim();
                    v2.Text = dt.Rows[1][2].ToString().Trim();
                    l2.Text = dt.Rows[1][3].ToString().Trim();
                }
                else
                {
                    wucha2x.Text = "";
                    wucha2s.Text = "";
                    v2.Text = "";
                    l2.Text = "";
                }
            }
            else
            {
                wucha1x.Text = "";
                wucha1s.Text = "";
                v1.Text = "";
                l1.Text = "";
                wucha2x.Text = "";
                wucha2s.Text = "";
                v2.Text = "";
                l2.Text = "";
            }
        }

        private void workshop1_TextChanged(object sender, EventArgs e)
        {
           
            //
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            t1.Text = label2.Text;
        }

        private void sql123(string sql0, string sql1, string sql2)
        {
            int c0 = 0;
            if (sql0 != "")
            {
                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql0);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //MessageBox.Show(sql1);
                    c0 = Class1.ExcuteScal(sql1); //update
                    if (c0 == 1)
                    {
                        MessageBox.Show("更新成功");
                    }
                }
                else
                {
                    //MessageBox.Show(sql2);
                    c0 = Class1.ExcuteScal(sql2); //insert
                    if (c0 == 1)
                    {
                        MessageBox.Show("新增成功");
                    }
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string sql0 = "";
            string sql1 = "";
            string sql2 = "";
 

            if (wucha1s.Text != "" & wucha1x.Text != "" & v1.Text != "" & l1.Text != "")
            {
                sql0 = string.Format("select top 1 * from wucha where itemcode='{0}' and id1='1'", w1.Text);
                sql1 = string.Format("update wucha set xiaxian='{0}',shangxian='{1}',fweight='{2}',flen='{3}' where itemcode='{4}' and id1='1'", wucha1x.Text.Trim(), wucha1s.Text.Trim(), v1.Text.Trim(), l1.Text.Trim(), w1.Text.Trim());
                sql2 = string.Format("insert into wucha(itemcode,id1,xiaxian,shangxian,fweight,flen) values('{0}','{1}','{2}','{3}','{4}','{5}')", w1.Text, '1', wucha1x.Text.Trim(), wucha1s.Text.Trim(), v1.Text.Trim(), l1.Text.Trim());
                sql123(sql0, sql1, sql2);   
            }

           

            if (wucha2s.Text != "" & wucha2x.Text != "" & v2.Text != "" & l2.Text != "")
            {
                sql0 = string.Format("select top 1 * from wucha where itemcode='{0}' and id1='2'", w1.Text);
                sql1 = string.Format("update wucha set xiaxian='{0}',shangxian='{1}',fweight='{2}',flen='{3}' where itemcode='{4}' and id1='2'", wucha2x.Text.Trim(), wucha2s.Text.Trim(), v2.Text.Trim(), l2.Text.Trim(), w1.Text.Trim());
                sql2 = string.Format("insert into wucha(itemcode,id1,xiaxian,shangxian,fweight,flen) values('{0}','{1}','{2}','{3}','{4}','{5}')", w1.Text, '2', wucha2x.Text.Trim(), wucha2s.Text.Trim(), v2.Text.Trim(), l2.Text.Trim());
                sql123(sql0, sql1, sql2);
            }


        }

        private void add3_Click(object sender, EventArgs e)
        {
            string sql0 = "";
            string sql1 = "";
            string sql2 = "";
            if (dataGridView2.DataSource != null)
            {
                for (int c = 0; c < dataGridView2.Rows.Count-1; c++)
                {

                    string c0 = dataGridView2.Rows[c].Cells[0].Value.ToString().Trim();
                    string c1 = dataGridView2.Rows[c].Cells[1].Value.ToString().Trim();
                    string c2 = dataGridView2.Rows[c].Cells[2].Value.ToString().Trim();
                    string c3 = dataGridView2.Rows[c].Cells[3].Value.ToString().Trim();

                    sql0 = string.Format("select top 1 * from box where itemcode='{0}' and workshop='{1}' and boxnm='{2}'", b1.Text, workshop1.Text,c1);
                    sql1 = string.Format("update box set boxid='{0}',boxnm='{1}',r1='{2}',r2='{3}',boxlv='{4}' where workshop='{5}' and itemcode='{6}' and boxid='{7}' and boxnm='{8}'",c0,c1,c2,c3,c1,workshop1.Text,b1.Text,c0,c1);
                    sql2 = string.Format("insert into box(workshop,itemcode,boxid,boxnm,r1,r2,boxlv,reqnum,maxcon) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','1','1')", workshop1.Text,b1.Text,c0,c1,c2,c3,c1);
                    sql123(sql0,sql1,sql2);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource != null)
            {
                for (int c = 0; c < dataGridView2.Rows.Count - 1; c++)
                {
                    if (dataGridView2.Rows[c].Selected == true)
                    {

                        string c0 = dataGridView2.Rows[c].Cells[0].Value.ToString().Trim();
                        string c1 = dataGridView2.Rows[c].Cells[1].Value.ToString().Trim();
                        string c2 = dataGridView2.Rows[c].Cells[2].Value.ToString().Trim();
                        string c3 = dataGridView2.Rows[c].Cells[3].Value.ToString().Trim();
                        string sql0 = string.Format("delete from box where workshop='{0}' and itemcode='{1}' and boxid='{2}' and boxnm='{3}'",workshop1.Text,b1.Text,c0,c1);
                        
                        int cc= Class1.ExcuteScal(sql0);
                        if (cc > 0)
                        {
                            b1_TextChanged(sender,e);
                        }
                    }

                }

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = string.Format("insert into box(workshop,itemcode,boxid,boxnm,r1,r2,boxlv) values('{0}','{1}','1','A','1','1','A')",workshop1.Text.Trim(),b1.Text.Trim());
            string sql0 = string.Format("select top 1 * from box where workshop='{0}' and itemcode='{1}'",workshop1.Text.Trim(),b1.Text.Trim());
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql0);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                int c2 = Class1.ExcuteScal(sql);
                if (c2 == 1)
                {
                    freshb1();
                }
            }
        }
    }
}
