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
    public partial class tppick2 : Form
    {
        public tppick2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            boxleft.Text = "tp-"+Class1.workshop+"-"+DateTime.Now.ToString("yyMMdd");
            c7.Text = Class1.workshop;

        }

        private void boxpick_Load(object sender, EventArgs e)
        {

        }

        private static tppick2 childFromInstanc;
        public static tppick2 ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new tppick2();
                }

                return childFromInstanc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            tansi_hou th = (tansi_hou)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
         
        }



        private void finditem()
        {
            string sql = "";
            string sqlt = "";
            string sql0 = "select top 1000 convert(int,replace(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),left(replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''),charindex('-',replace(T1.boxno,left(T1.boxno,charindex('-',T1.boxno)),''))),'')) as R1,T1.boxno as 箱号,tpno as 托盘,count(T1.batch) as 卷数,T2.desc1 as 描述,sum(T1.pro1) as 重量,sum(T1.len) as 长度 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where 1=1";

                if (c1.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.material='{0}'", c1.Text.Trim());
                }
              
                if (c6.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.batchbig='{0}'", c6.Text.Trim());
                }
          
                if (c7.Text != "")
                {
                    sqlt = sqlt+ string.Format(" and T1.sloc='{0}'", c7.Text.Trim());
                }

                if (c2.Text != "")
                {
                    sqlt = sqlt + string.Format(" and T1.qatype='{0}'", c2.Text.Trim());
                }

            string sqlx = " group by T2.desc1,T1.boxno,T1.tpno order by R1";


            sql = sql0 + sqlt + sqlx;
            //MessageBox.Show(sql);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (sql != "")
            {
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
            else
            {
                dataGridView1.DataSource = null;
            }
               
       
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认清空托盘信息?", "清空", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (boxleft.Text != "")
                {
                    string sql = string.Format("update stock set tpno='{1}' where tpno='{0}' and sloc='{2}'", boxleft.Text, "", Class1.workshop);
                    int c = Class1.ExcuteScal(sql);
                    freshleftbox();
                }
                finditem();
            }
                

        }

        private void boxleft_TextChanged(object sender, EventArgs e)
        {
            freshleftbox();
        }

        private void freshleftbox()
        {
            string sql = string.Format("select top 1000 boxno as 箱号,material as 物料,count(batch) as 卷数,sum(pro1) as 重量,sum(len) as 长度 from stock where tpno='{0}' group by boxno,material order by boxno",boxleft.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
                int x = 0; //juanshu
                decimal y = 0; //zhongliang

                //MessageBox.Show(x);
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    x = x + Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                    y = y + Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value);
                }

                jsc.Text = x.ToString();
                zlc.Text = y.ToString();
                boxcount.Text = dataGridView2.Rows.Count.ToString();
            }
            else
            {
                dataGridView2.DataSource = null;
                boxcount.Text = "0";
                jsc.Text = "0";
                zlc.Text = "0";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (boxleft.Text != "")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected == true & dataGridView1.Rows[i].Cells[1].Value.ToString()!="")
                    {
                        string sql0 = string.Format("update stock set tpno='{0}' where boxno='{1}' and sloc='{2}'", boxleft.Text, dataGridView1.Rows[i].Cells[1].Value.ToString(), Class1.workshop);
                        if (dataGridView1.Rows[i].Cells[2].Value.ToString().Trim()!="")
                        {
                            if (MessageBox.Show("此箱"+ dataGridView1.Rows[i].Cells[1].Value.ToString() + "已装入其他托盘"+ dataGridView1.Rows[i].Cells[2].Value.ToString() + "，确定覆盖?", "覆盖", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                Class1.ExcuteScal(sql0);
                            }
                        }
                        else
                        {
                            Class1.ExcuteScal(sql0);
                        }
                       

                    }


                }
                freshleftbox();
                finditem();

            }
            else
            {
                MessageBox.Show("请输入托盘号");
            } 

        }

    

        private void button4_Click(object sender, EventArgs e)
        {
            if (boxleft.Text != "")
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Selected == true)
                    {
                        string sql0 = string.Format("update stock set tpno='' where boxno='{0}' and sloc='{1}'", dataGridView2.Rows[i].Cells[0].Value.ToString(),Class1.workshop);
                        Class1.ExcuteScal(sql0);
                    }


                }
                freshleftbox();

            }
            finditem();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear1();
        }


        private void clear1()
        {
            c1.Text = "";
            c6.Text = "";
            
        }



        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void c9_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            finditem();
        }

        private void checkBox0_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\box", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
            btFormat.SetNamedSubStringValue("batch", boxleft.Text); //向bartender模板传递变量
            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
            Class1.killbartender();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select top 1 tpno from stock where tpno like '%{0}%' order by tpno desc", boxleft.Text);
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

        private void button10_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int countr = 0;
            decimal qty = 0;

            for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
                string sql = string.Format("select tpno,count(batch),sum(pro1) from stock where boxno='{0}' group by tpno", dataGridView3.Rows[i].Cells[0].Value.ToString().Trim());
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView3.Rows[i].Cells[1].Value = dt.Rows[0][0].ToString().Trim();
                    dataGridView3.Rows[i].Cells[2].Value = dt.Rows[0][1].ToString().Trim();
                    dataGridView3.Rows[i].Cells[3].Value = dt.Rows[0][2].ToString().Trim();
                    countr = countr + Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value);
                    qty = qty + Convert.ToDecimal(dataGridView3.Rows[i].Cells[3].Value);
                    if (dataGridView3.Rows[i].Cells[1].Value.ToString().Trim().IndexOf("tp") != -1 || dataGridView3.Rows[i].Cells[1].Value.ToString().Trim()=="未找到")
                    {
                        dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                else
                {
                    dataGridView3.Rows[i].Cells[1].Value ="未找到";
                    dataGridView3.Rows[i].Cells[2].Value = "0";
                    dataGridView3.Rows[i].Cells[3].Value = "0";
                }
                dataGridView3.Rows[i].Cells[4].Value = "";



            }
            dxr.Text = countr.ToString();
            dxj.Text = (dataGridView3.Rows.Count - 1).ToString();
            dxz.Text = qty.ToString();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
               if(dataGridView3.Rows[i].DefaultCellStyle.BackColor != Color.Red && dataGridView3.Rows[i].Selected==true)
                {
                    string sql = string.Format("update stock set tpno='{1}' where boxno='{0}'",dataGridView3.Rows[i].Cells[0].Value.ToString().Trim(),boxleft.Text);
                    //MessageBox.Show(sql);
                    int a = Class1.ExcuteScal(sql);
                    if (a > 0)
                    {
                        dataGridView3.Rows[i].Cells[4].Value = "成功";
                    }
                }



            }
            freshleftbox();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            dxr.Text = "0";
            dxj.Text = "0";
            dxz.Text = "0";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            string[] batchname = new string[100];
            for (int s = 0; s < 100; s++)
            {
                batchname[s] = "0";
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                int flagx = 0;
                int spos = dataGridView2.Rows[i].Cells[0].Value.ToString().IndexOf("-");
                string databatchname = dataGridView2.Rows[i].Cells[0].Value.ToString().Substring(0, spos);
                    
                for (int s = 0; s < 100; s++)
                {
                    if (batchname[s] == databatchname)
                    {
                        flagx = 1;
                        break;
                    }
                }
                if (flagx == 0)
                {
                    for (int s = 0; s < 100; s++)
                    {
                        if (batchname[s] == "0")
                        {
                            batchname[s] = databatchname;
                            break;
                        }
                    }
                }
            }

            int fix1 = 0;
            for (int s = 0; s < 100; s++)
            {
                if (batchname[s] != "0")
                {
                    fix1 = fix1 + 1;
                }
            }

            for (int s1 = 0; s1 < fix1; s1++)
            {

                string cur = "";
                string[,] listbox = new string[100, 4];
                for (int s = 0; s < 100; s++)
                {
                    listbox[s, 0] = "x";
                }
                decimal boxtotal = 0;
                decimal rolltotal = 0;
                decimal weighttotal = 0;
                int flag = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    flag = 0;
                    int spos2 = dataGridView2.Rows[i].Cells[0].Value.ToString().IndexOf("-");
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString().Substring(0, spos2) == batchname[s1])
                    {
                        string list1 = dataGridView2.Rows[i].Cells[0].Value.ToString();
                        string[] listspl = list1.Split('-');
                        cur = listspl[1];
                        for (int s = 0; s < 100; s++)
                        {
                            if (listbox[s, 0] != "x" & listbox[s, 0] == cur)
                            {
                                boxtotal = Convert.ToDecimal(listbox[s, 1]);
                                rolltotal = Convert.ToDecimal(listbox[s, 2]);
                                weighttotal = Convert.ToDecimal(listbox[s, 3]);
                                boxtotal = boxtotal + 1;
                                rolltotal = rolltotal + Convert.ToDecimal(dataGridView2.Rows[i].Cells[2].Value.ToString());
                                weighttotal = weighttotal + Convert.ToDecimal(dataGridView2.Rows[i].Cells[3].Value.ToString());
                                listbox[s, 1] = boxtotal.ToString();
                                listbox[s, 2] = rolltotal.ToString();
                                listbox[s, 3] = weighttotal.ToString();
                                flag = 1;
                            }
                        }

                        if (flag == 0)
                        {
                            for (int s = 0; s < 100; s++)
                            {
                                if (listbox[s, 0] == "x")
                                {
                                    string list2 = dataGridView2.Rows[i].Cells[0].Value.ToString();
                                    string[] listsp2 = list2.Split('-');

                                    listbox[s, 0] = listsp2[1];
                                    listbox[s, 1] = "1";
                                    listbox[s, 2] = dataGridView2.Rows[i].Cells[2].Value.ToString();
                                    listbox[s, 3] = dataGridView2.Rows[i].Cells[3].Value.ToString();
                                    break;
                                }

                            }


                        }
                    }
                    
                    


                }

                string relt = "";
                for (int s = 0; s < 100; s++)
                {
                    if (listbox[s, 0] != "x")
                    {
                        relt = relt + "类别:" + listbox[s, 0] + " 箱数:" + listbox[s, 1] + " 卷数:" + listbox[s, 2] + "  重量:" + listbox[s, 3];
                        relt = relt + "\r\n";
                    }

                }

                MessageBox.Show(batchname[s1]+"\r\n"+relt);







            }
            

            

        }



        

    }
}
