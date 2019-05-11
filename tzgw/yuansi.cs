using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;

namespace tzgw
{
    public partial class yuansi : Form
    {
        public yuansi()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            init1();
            this.Text = Class1.workshop + " " + Class1.companyinfo;
        }

        private void yuansi_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public static string quitflag="0";
        private static yuansi childFromInstanc;
        public static yuansi ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new yuansi();
                }

                return childFromInstanc;
            }
        }

       

        private void init1()
        {
            
            readrmbatch();
            readlistviewstatus();
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            dateTimePicker2.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            dateTimePicker3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            dateTimePicker4.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");

            if (Class1.workshop == "ys1" || Class1.workshop == "ys3")
            {
                comboBox1.Text = "流水与工位顺序保持一致";

            }
            else
            {
                comboBox1.Text = "流水与完工顺序保持一致";
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (itemcode.Text != "")
            {
                if (MessageBox.Show("确认开始 #" + fw.Text + ",开始时间" + dateTimePicker1.Value.ToString() + "?", "开始", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string sql = string.Format("update wsdevice set status1='运行',t1='{1}',speed1='{3}' where workshop='{2}' and devicenum='{0}'", fw.Text.Trim(), dateTimePicker1.Value, Class1.workshop, speed1.Text.Trim());
                    int c = Class1.ExcuteScal(sql);
                    if (c > 0)
                    {
                        quitflag = "0";
                        status1.Text = "运行";
                        readlistviewstatus();
                        groupBox1.Enabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("物料号为空!");
            }
           

        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认停止 #"+fw.Text+"\n" + dateTimePicker1.Value.ToString() +"-"+ dateTimePicker2.Value.ToString() + "\n长度"+len.Text+"\n重量"+pro1.Text+"?", "停止", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (itemcode.Text != "" & tpno.Text != "")
                {

                    //插入stock
                    string sql = string.Format("insert into stock(branch,sloc,material,batch,displaybatch,stockin,t1,t2,rmbatch1,rmbatch2,rmbatch3,rmbatch4,rmbatch5,tpno,qa,pro1,res,len,pro1len,speedx) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','M','{14}','{14}','{15}','{15}','{16}')", "tz", Class1.workshop, itemcode.Text, batch.Text, batch.Text, DateTime.Now.ToString(), dateTimePicker1.Value.ToString(), dateTimePicker2.Value.ToString(), rm1.Text, rm2.Text, rm3.Text, rm4.Text, rm5.Text, tpno.Text, pro1.Text, len.Text,speed1.Text);
                    int c2 = Class1.ExcuteScal(sql);

                    if (comboBox1.Text == "流水与完工顺序保持一致")
                    {
                        l.Text = (1 + Convert.ToInt16(l.Text)).ToString();
                    }


                    if (c2 == 1)
                    {
                        
                        //更新lastbatch序号流水
                        string sql1 = string.Format("update lastbatch set l='{0}' where workshop='{1}' and itemcode='{2}'", l.Text.Trim(), Class1.workshop, itemcode.Text);
                        int c1 = Class1.ExcuteScal(sql1);
                        string sql21 = string.Format("update duansi set pro1='{0}' where batch='{1}'",len.Text,batch.Text);
                        int c0 = Class1.ExcuteScal(sql21);
                        string sql22 = string.Format("update duansi set dslocfinish=pro1-dslocaccu where batch='{0}'",batch.Text);
                        c0 = Class1.ExcuteScal(sql22);
                        groupBox1.Enabled = false;



                        //更新wsdevice状态到停止
                        string sql3 = string.Format("update wsdevice set status1='停止',t2='{1}',duansi='0',duansimin='0' where workshop='{2}' and devicenum='{0}'", fw.Text.Trim(), dateTimePicker2.Value, Class1.workshop);
                        int c = Class1.ExcuteScal(sql3);
                        if (c > 0)
                        {
                            quitflag = "1";
                            status1.Text = "停止";
                            readlistviewstatus();
                            loadfwinfo(fw.Text);
                        }
                    }

                    
                }
                else
                {
                    MessageBox.Show("物料号或托盘号为空!");
                }
            }
               


        }

        public void readlistviewstatus()
        {
            //listview按状态显示颜色
            
            string sql = string.Format("select devicenum,status1,duansi from wsdevice where workshop='{0}'",Class1.workshop);
            DataSet ds1 = new DataSet();
            ds1 = Class1.GetAllDataSet(sql);
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (ListViewItem lv in listView1.Items)
                {
                    lv.BackColor = Color.White;
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        
                        if (lv.Text == dt1.Rows[j][0].ToString().Trim() && dt1.Rows[j][1].ToString().Trim() == "运行")
                        {
                            lv.BackColor = Color.LightGreen;
                            if (Convert.ToInt32(dt1.Rows[j][2]) > 0)
                            {
                                lv.BackColor = Color.Yellow;
                            }
                            if (Convert.ToInt32(dt1.Rows[j][2]) >= 3)
                            {
                                lv.BackColor = Color.Red;
                            }
                        }
                        

                    }



                }
            }
            flashtplist();
        }
        public void Dtcalc()
        {
            //此处需要加入强行退出条件
            while(quitflag=="0")
            {
                DateTime dt1= Convert.ToDateTime(dateTimePicker1.Value);
                DateTime dt2 = Convert.ToDateTime(dateTimePicker2.Value);
                double ts = ((TimeSpan)(dt2-dt1)).TotalSeconds;

                if (checkBox1.Checked == true)
                {
                    pro1.Text = pro2.Text;
                    len.Text = lenall.Text;
                }
                else
                {
                    pro1.Text = (Convert.ToDecimal(speed1.Text) * Convert.ToDecimal(ts) / 60 * Convert.ToDecimal(kz.Text)).ToString("#0.00");
                    pro2.Text = pro1.Text;
                }

                Thread.Sleep(1000);
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker4.Value = DateTime.Now;


            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void readrmbatch()
        {
            
            //读取最后一次提交的批次信息，lastbatch表 1-5分别存放5种原材料批次
            string sql = string.Format("select top 1 batch1,batch2,batch3,batch4,batch5,p,c,l,itemcode,batchstart from lastbatch where workshop='{0}' and itemcode='{1}'",Class1.workshop, itemcode.Text);
            DataSet ds1 = new DataSet();
            ds1=Class1.GetAllDataSet(sql);
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            if (dt1.Rows.Count> 0)
            {
                rm1.Text = dt1.Rows[0][0].ToString().Trim();
                rm2.Text = dt1.Rows[0][1].ToString().Trim();
                rm3.Text = dt1.Rows[0][2].ToString().Trim();
                rm4.Text = dt1.Rows[0][3].ToString().Trim();
                rm5.Text = dt1.Rows[0][4].ToString().Trim();
                p.Text = dt1.Rows[0][5].ToString().Trim();
                c.Text = dt1.Rows[0][6].ToString().Trim();
                l.Text= dt1.Rows[0][7].ToString().Trim();
                itemcode.Text= dt1.Rows[0][8].ToString().Trim();
                batchstart.Text= dt1.Rows[0][9].ToString().Trim();

            }
            else
            {
                rm1.Text = "";
                rm2.Text = "";
                rm3.Text = "";
                rm4.Text = "";
                rm5.Text = "";
                p.Text = "1";
                c.Text = "1";
                l.Text = "1";
                batchstart.Text = "";

            }

        }

        private void lockrm_Click(object sender, EventArgs e)
        {
            if (lockrm.Text == "锁定")
            {
                lockrm.Text = "解锁";
                rm1.Enabled = false;
                rm2.Enabled = false;
                rm3.Enabled = false;
                rm4.Enabled = false;
                rm5.Enabled = false;
                p.Enabled = false;
                c.Enabled = false;
                l.Enabled = false;
                batchstart.Enabled = false;
                groupBox3.Enabled = false;

                string sql1 = string.Format("update lastbatch set batch1='{0}',batch2='{1}',batch3='{2}',batch4='{3}',batch5='{4}',p='{5}',c='{6}',l='{7}',batchstart='{10}' where itemcode='{8}' and workshop='{9}'", rm1.Text, rm2.Text, rm3.Text, rm4.Text, rm5.Text, p.Text, c.Text, l.Text, itemcode.Text, Class1.workshop, batchstart.Text);
               // MessageBox.Show(sql1);
                int cq = Class1.ExcuteScal(sql1);
                if (cq == 0)
                {
                    string sql2 = string.Format("insert into lastbatch values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','','{10}')", Class1.workshop, rm1.Text, rm2.Text, rm3.Text, rm4.Text, rm5.Text, p.Text, c.Text, l.Text, itemcode.Text,batchstart.Text);
                    int cq2 = Class1.ExcuteScal(sql2);
                }
            }
            else
            {


                lockrm.Text = "锁定";
                rm1.Enabled = true;
                rm2.Enabled = true;
                rm3.Enabled = true;
                rm4.Enabled = true;
                rm5.Enabled = true;
                p.Enabled = true;
                c.Enabled = true;
                l.Enabled = true;
                batchstart.Enabled = true;
                groupBox3.Enabled = true;


            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listindex1.Text = "";
                for (int j = 0; j < listView1.SelectedItems.Count; j++)
                {
                    for (int i = 0; i < listView1.SelectedItems[j].SubItems.Count; i++)
                    {
                        listindex1.Text = listindex1.Text + listView1.SelectedItems[j].SubItems[i].Text;
                    }
                }
            }
            //如果lv选中一个,读取第一个的值到右侧
            if (listView1.SelectedItems.Count == 1)
            {
                fw.Text = listView1.SelectedItems[0].SubItems[0].Text;
                loadfwinfo(fw.Text);
            }

            if (tpno.Text != "")
            {
                flashtplist();
            }

        }

        private void loadfwinfo(string fw1)
        {
            //读取纺位信息
            if (groupBox1.Enabled == false)
            {
                groupBox1.Enabled = true;
            }
            quitflag = "1"; //关闭计时进程标记
            string sql = string.Format("select status1,t1,speed1,t2,duansi,duansimin from wsdevice where workshop='{1}' and devicenum='{0}'",fw1.Trim(),Class1.workshop);
            DataSet ds1 = new DataSet();
            ds1 = Class1.GetAllDataSet(sql);
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            int fwl = 1000;
            if (comboBox1.Text == "流水与工位顺序保持一致")
            {
                fwl = 1000 + Convert.ToInt16(l.Text) + Convert.ToInt16(fw.Text) - 1;
            }
            else
            {
                fwl = 1000 + Convert.ToInt16(l.Text);
            }
            
            batch.Text = batchstart.Text + "-" + p.Text + "-" + c.Text + "-" + fwl.ToString().Substring(1,3) + "-" + sp2.Text + "-" + fw1.Trim()+"#"+testm.Text.Trim();
            if (dt1.Rows.Count > 0)
            {
                status1.Text = dt1.Rows[0][0].ToString().Trim();
                speed1.Text = dt1.Rows[0][2].ToString().Trim();
                dateTimePicker1.Value = Convert.ToDateTime(dt1.Rows[0][1]);

                DateTime dtpk1 = Convert.ToDateTime(dateTimePicker1.Value);
                DateTime dtpk2 = dtpk1.AddMinutes(Convert.ToDouble(textBox1.Text));
                dateTimePicker2.Text = dtpk2.ToString();
                dateTimePicker5.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
                dscs.Text = dt1.Rows[0][4].ToString().Trim(); //duansi cishu
                dssj.Text= dt1.Rows[0][5].ToString().Trim(); //duansi shijian
                dslen.Text = (Convert.ToDecimal(speed1.Text) * Convert.ToDecimal(dssj.Text)).ToString();//duansi changdu

                if (checkBox1.Checked == false)
                {
                    DateTime dt10 = Convert.ToDateTime(dateTimePicker1.Value);
                    DateTime dt20 = Convert.ToDateTime(dateTimePicker2.Value);
                    double ts = ((TimeSpan)(dt20 - dt10)).TotalMinutes;
                    
                    len.Text = (Convert.ToDecimal(speed1.Text) * Convert.ToDecimal(ts) - Convert.ToDecimal(label39.Text)).ToString();
                    pro1.Text = (Convert.ToDecimal(len.Text) * Convert.ToDecimal(kz.Text)).ToString("#0.00");

                }
                else
                {
                    pro1.Text = pro2.Text;
                    len.Text = lenall.Text;
                }

                if (status1.Text.Trim() == "运行")
                {

                    button1.Enabled = false;
                    button2.Enabled = true;
                    quitflag = "0";
                    //Thread datethread = new Thread(new ThreadStart(Dtcalc));
                    //datethread.IsBackground = true;
                    //datethread.Start();
                }
                else
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                }
            }
            else
            {

                status1.Text = "-";
                speed1.Text = "0";
                dateTimePicker1.Value = Convert.ToDateTime(DateTime.Now);
                dateTimePicker2.Value = Convert.ToDateTime(DateTime.Now);
                pro1.Text = "0";
                len.Text = "0";
                dscs.Text = "0";
                dssj.Text = "0";
                dslen.Text = "0";

            }

            freshduansi();
            dtcalc1();

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            readlistviewstatus();
        }

        private void allstart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认开始所有停止中的纺位,开始时间" + dateTimePicker3.Value.ToString()+"?", "开始", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (Class1.isNumeric(speedall.Text.Trim()))
                {
                    foreach (ListViewItem lv in listView1.Items)
                    {
                        if (lv.BackColor == Color.White & lv.Selected == true)
                        {
                            string sql = string.Format("update wsdevice set t1='{0}',status1='运行',speed1='{1}' where workshop='{2}' and devicenum='{3}'", dateTimePicker3.Value, speedall.Text.Trim(), Class1.workshop, lv.Text);
                            //MessageBox.Show(sql);
                            int c = Class1.ExcuteScal(sql);
                            if (c > 0)
                            {
                                readlistviewstatus();
                            }
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("不是一个有效速度:"+ speedall.Text.Trim());
                }

            }
        }

        private void allstop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认停止所有运行中的纺位（除断丝纺位）\n"+dateTimePicker3.Value.ToString() +"-"+ dateTimePicker4.Value.ToString()+"\n长度"+lenall.Text+"\n重量"+pro2.Text+"?", "停止", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (tpno.Text != "" && lenall.Text!="" && pro2.Text!="")
                {
                    foreach (ListViewItem lv in listView1.Items)
                    {
                        if (lv.BackColor == Color.LightGreen & lv.Selected==true)
                        {
                            string sql = string.Format("update wsdevice set t2='{0}',status1='停止' where workshop='{1}' and devicenum='{2}'", dateTimePicker4.Value, Class1.workshop, lv.Text);
                            int cq = Class1.ExcuteScal(sql);
                            if (cq > 0)
                            {
                                quitflag = "1";
                                int fwl = 1000;
                                if (comboBox1.Text == "流水与工位顺序保持一致")
                                {
                                    fwl = 1000 + Convert.ToInt16(l.Text) + Convert.ToInt16(lv.Text) - 1;
                                }
                                else
                                {
                                    fwl = 1000 + Convert.ToInt16(l.Text);
                                }
                                string batchadd = batchstart.Text + "-" + p.Text + "-" + c.Text + "-" + fwl.ToString().Substring(1,3) + "-" + sp2.Text + "-" + lv.Text + "#" + testm.Text.Trim();
                                string sqladd = string.Format("insert into stock(branch,sloc,material,batch,displaybatch,stockin,t1,t2,rmbatch1,rmbatch2,rmbatch3,rmbatch4,rmbatch5,tpno,qa,pro1,res,len,pro1len,speedx) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','M','{14}','{14}','{15}','{15}','{16}')", "tz", Class1.workshop, itemcode.Text, batchadd, batchadd, DateTime.Now.ToString(), dateTimePicker3.Value.ToString(), dateTimePicker4.Value.ToString(), rm1.Text, rm2.Text, rm3.Text, rm4.Text, rm5.Text, tpno.Text, pro2.Text,lenall.Text,speedall.Text);
                                int cq2 = Class1.ExcuteScal(sqladd);
                                if (comboBox1.Text == "流水与完工顺序保持一致")
                                {
                                    l.Text = (Convert.ToInt16(l.Text) + 1).ToString();
                                }

                                //readlistviewstatus();

                            }
                        }

                    }
                    readlistviewstatus();

                }
                else
                {
                    MessageBox.Show("托盘号为空或长度/重量为空");
                }
                ////


                 

                ////




            }
        }

        public void itemcodeload()
        {
            
            itemcode.Text = "";
            itemcode.Text = Class1.itemyuansi1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Class1.fromform = "ys";
            this.Hide();
            itemselect ist = itemselect.ChildFromInstanc;
            if (ist != null)
            {
                ist.Owner = this;
                ist.Show();
            }

        }

        private void itemcode_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select desc1,sp1,sp2,testm from masterdata where itemcode='{0}'",itemcode.Text);
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                desc1.Text = ds.Tables[0].Rows[0][0].ToString().Trim();
                kz.Text = ds.Tables[0].Rows[0][1].ToString().Trim();
                sp2.Text= ds.Tables[0].Rows[0][2].ToString().Trim();
                if (ds.Tables[0].Rows[0][3].ToString().Trim() == "Y")
                {
                    testm.Text = "-实验";
                }
                else
                {
                    testm.Text = "";
                }
                string sql2 = string.Format("update lastbatch set itemcode='{0}' where workshop='{1}' and itemcode='{2}'", itemcode.Text,Class1.workshop, itemcode.Text.Trim());
                int c = Class1.ExcuteScal(sql2);

            }
            else
            {
                desc1.Text = "";
                kz.Text = "1";
                sp2.Text = "";
                testm.Text = "";
            }

            readrmbatch();
        }

        private void flashtplist()
        {
            if(tpno.Text!="")
            {
                string sql = string.Format("select T1.batch as 批次,rtrim(T1.material) as 物料,T2.desc1 as 型号,T1.pro1 as 重量,T1.len as 长度,rtrim(T1.qa) as 质量,T1.t1 as 开始时间,T1.t2 as 结束时间,T1.speedx as 速度,convert(int,T2.sp1*1000000) as 线密度 from stock T1 left join masterdata T2 on T1.material=T2.itemcode  where tpno='{0}' order by batch", tpno.Text.Trim());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    yscount.Text = dt.Rows.Count.ToString();
                    decimal weightc = 0;
                    int lenc = 0;
                    decimal biger = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString()) >= biger)
                        {
                            biger = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }
                        weightc = weightc + Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value.ToString().Trim());
                        lenc=lenc+ Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value.ToString().Trim());
                    }
                    weightcount.Text = weightc.ToString();
                    lencount.Text = lenc.ToString();
                    feicount.Text= (biger * dataGridView1.Rows.Count - weightc).ToString("#0.0000");
                }
                else
                {
                    dataGridView1.DataSource = null;
                    yscount.Text = "0";
                    weightcount.Text = "0";
                    lencount.Text = "0";
                    feicount.Text = "0";
                }



            }

            
        }

        private void tpno_TextChanged(object sender, EventArgs e)
        {
            flashtplist();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tpno.Text = Class1.workshop+"-"+DateTime.Now.ToString("yyyyMMdd");
        }

        private void p_TextChanged(object sender, EventArgs e)
        {
            c.Text = "001";
        }

        private void c_TextChanged(object sender, EventArgs e)
        {
            //2019/4/1 cancel the auto plus
            //if (Class1.workshop == "ys1" || Class1.workshop == "ys3")
            //{
            //    if (l.Text != "")
            //    {
            //        l.Text = (28 + Convert.ToInt16(l.Text)).ToString();
            //    }
            //    else
            //    {
            //        l.Text = "0001";
            //    }
                
            //}
            //else
            //{
            //    l.Text = "0001";
            //}
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Class1.killbartender();
            quitflag = "1";
            mainmenu mn = (mainmenu)this.Owner;
            this.Owner.Show();
            this.Dispose();
        }



        private void label1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\box", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt16(textBox2.Text);  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
            btFormat.SetNamedSubStringValue("batch", tpno.Text); //向bartender模板传递变量
            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
            Class1.killbartender();
        }

        private void button8_Click(object sender, EventArgs e)
        {


            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\yuansi", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt16(textBox2.Text);  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

            int s = dataGridView1.RowCount;
            for (int i = 0; i <= s - 1; i++)
            {
                btFormat.SetNamedSubStringValue("batch", dataGridView1.Rows[i].Cells[0].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("desc1", dataGridView1.Rows[i].Cells[2].Value.ToString()); //向bartender模板传递变量            
                btFormat.SetNamedSubStringValue("t1", dataGridView1.Rows[i].Cells[6].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("t2", dataGridView1.Rows[i].Cells[7].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("pro1", dataGridView1.Rows[i].Cells[3].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("len", dataGridView1.Rows[i].Cells[4].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("speed1", dataGridView1.Rows[i].Cells[8].Value.ToString()); //向bartender模板传递变量
                btFormat.SetNamedSubStringValue("xmd", dataGridView1.Rows[i].Cells[9].Value.ToString()); //向bartender模板传递变量
                btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
            }

            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

            Class1.killbartender();

        }

        private void pro1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        { //打印单独标签

            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\yuansi", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt16(textBox2.Text);  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
            

            int s = dataGridView1.RowCount;
            for (int i = 0; i <= s - 1; i++)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    btFormat.SetNamedSubStringValue("batch", dataGridView1.Rows[i].Cells[0].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("desc1", dataGridView1.Rows[i].Cells[2].Value.ToString()); //向bartender模板传递变量            
                    btFormat.SetNamedSubStringValue("t1", dataGridView1.Rows[i].Cells[6].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("t2", dataGridView1.Rows[i].Cells[7].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("pro1", dataGridView1.Rows[i].Cells[3].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("len", dataGridView1.Rows[i].Cells[4].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("speed1", dataGridView1.Rows[i].Cells[8].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("xmd", dataGridView1.Rows[i].Cells[9].Value.ToString()); //向bartender模板传递变量

                    btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
                }
                

            }

            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

            Class1.killbartender();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            if (quitflag == "0")
            {
                quitflag = "2";
            }
        }

        private void groupBox1_Leave(object sender, EventArgs e)
        {
            if (quitflag == "2")
            {
                quitflag = "0";
            }
        }

        private void pro2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {
            dateTimePicker3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            lencalc();
            
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            lencalc();
        }

        private void lencalc()
        {
            DateTime dt1 = Convert.ToDateTime(dateTimePicker3.Value);

            if (textBox1.Text != "")
            {
                DateTime dt = dt1.AddMinutes(Convert.ToDouble(textBox1.Text));
                dateTimePicker4.Text = dt.ToString();
            }
            else
            {
                dateTimePicker4.Text = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            }

            if(textBox1.Text!="" && speedall.Text!="" && kz.Text!="")
            {
                lenall.Text = (Convert.ToDecimal(speedall.Text) * Convert.ToDecimal(textBox1.Text)).ToString();
                pro2.Text = (Convert.ToDecimal(lenall.Text) * Convert.ToDecimal(kz.Text)).ToString("#0.00");
            }

        }

        private void speedall_TextChanged(object sender, EventArgs e)
        {
            lencalc();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lencalc();
        }

        private void lenall_TextChanged(object sender, EventArgs e)
        {
            if (lenall.Text != "")
            {
                pro2.Text = (Convert.ToDecimal(lenall.Text) * Convert.ToDecimal(kz.Text)).ToString("#0.00");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("断丝次数将被归0,状态变为停止,确认初始化所有纺位?", "初始化", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("update wsdevice set status1='停止',t1='{0}',t2='{0}',speed1='90',duansi='0',duansimin='0' where workshop='{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:00:00"),Class1.workshop);
                int c = Class1.ExcuteScal(sql);
                if (c >= 1)
                {
                    readlistviewstatus();
                }
            }

        }

        private void label31_Click(object sender, EventArgs e)
        {
            dateTimePicker5.Text= DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
        }

        private void label10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请输入1号纺位的流水号, 后续纺位在此基础上累加");
            if (MessageBox.Show("是否从1开始计数?","", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                l.Text = "0001";

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("断丝次数将被归0,状态变为停止,确认初始化纺位"+fw.Text+"?", "初始化", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = string.Format("update wsdevice set status1='停止',t1='{0}',t2='{0}',speed1='90',duansi='0',duansimin='0' where workshop='{1}' and devicenum='{2}'", DateTime.Now.ToString("yyyy-MM-dd HH:00:00"), Class1.workshop, fw.Text.Trim());
                int c = Class1.ExcuteScal(sql);
                if (c >= 1)
                {
                    readlistviewstatus();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("断丝位置" + fw.Text + ",本次断丝时间(分):"+dssj.Text+"?", "断丝", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dscs.Text = (Convert.ToInt32(dscs.Text) + 1).ToString();
                string sql = string.Format("update wsdevice set duansi=duansi+1,duansimin=duansimin+'{1}' where workshop='{2}' and devicenum='{3}'", dscs.Text, dssj.Text, Class1.workshop, fw.Text.Trim());
                int c = Class1.ExcuteScal(sql);
                if (c == 1)
                {
                    DateTime dt10 = Convert.ToDateTime(dateTimePicker1.Value);
                    DateTime dt50 = Convert.ToDateTime(dateTimePicker5.Value);
                    double ts = ((TimeSpan)(dt50 - dt10)).TotalMinutes;
                    string dslocaccu = (Convert.ToDecimal(speed1.Text) * Convert.ToDecimal(ts)).ToString();

                    string sql2 = string.Format("insert into duansi(batch,dstime,dscs,dssj,dslocaccu) values('{0}','{1}','{2}','{3}','{4}')", batch.Text, dateTimePicker5.Text, dscs.Text, dssj.Text, dslocaccu);
                    int c2=Class1.ExcuteScal(sql2);
                    if (c2 == 1)
                    {
                        if (MessageBox.Show("是否打印断丝标签?", "断丝", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            BarTender.Application btApp;
                            BarTender.Format btFormat;
                            btApp = new BarTender.Application();
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\duansi", false, "");
                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
                            btFormat.SetNamedSubStringValue("cishu", dscs.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("fangwei", fw.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("time1", dateTimePicker5.Text); //向bartender模板传递变量 
                            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
                            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
                            Class1.killbartender();
                        }

                        freshduansi();
                        dtcalc1();
                    }
                    readlistviewstatus();
                    loadfwinfo(fw.Text);
                    
                }
            }
        }

        private void freshduansi()
        {
            if (batch.Text != "")
            {
                string sql = string.Format("select dscs as #,dstime as 发生时间,dssj as 损失时间,(select sum(dssj) from duansi  where batch='{0}' group by batch) as T from duansi where batch='{0}' order by dscs", batch.Text);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = Class1.GetAllDataSet(sql);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                    label35.Text = dt.Rows[0][3].ToString();
                    label39.Text = (Convert.ToDecimal(label35.Text) * Convert.ToDecimal(speed1.Text)).ToString();
                }
                else
                {
                    dataGridView2.DataSource = null;
                    label35.Text = "0";
                    label39.Text = "0";

                }

            }
        }


        private void dssj_TextChanged(object sender, EventArgs e)
        {
            if (dssj.Text != "" && speed1.Text != "")
            {
                dslen.Text = (Convert.ToDecimal(dssj.Text) * Convert.ToDecimal(speed1.Text)).ToString();
                dtcalc1();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("断丝位置" + fw.Text + "复位,清空该批次的断丝记录?", "复位", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                dscs.Text = (Convert.ToInt32(dscs.Text) + 1).ToString();
                string sql = string.Format("update wsdevice set duansi='0',duansimin='0' where workshop='{0}' and devicenum='{1}'",Class1.workshop, fw.Text.Trim());
                int c = Class1.ExcuteScal(sql);
                if (c == 1)
                {
                    string sql2 = string.Format("delete from duansi where batch='{0}'", batch.Text);
                    int c2 = Class1.ExcuteScal(sql2);
                    readlistviewstatus();
                    loadfwinfo(fw.Text);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            dateTimePicker2.Text= DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("c:\\data\\生产记录"+tpno.Text+".txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            int s = dataGridView1.RowCount;
            string sql = string.Format("select T1.batch as 批次,rtrim(T1.material) as 物料,T2.desc1,T1.pro1 as 重量,T1.len as 长度,rtrim(T1.qa) as 质量,T1.t1 as 开始时间,T1.t2 as 结束时间 from stock T1 left join masterdata T2 on T1.material=T2.itemcode  where tpno='{0}' order by batch", tpno.Text.Trim());
            sw.WriteLine("托盘号: "+tpno.Text+" \r\n正品重量(KG): "+weightcount.Text+" \r\n废品重量(KG): " + feicount.Text + " \r\n正品长度(M): " + lencount.Text + " \r\n物料明细: ");
            sw.WriteLine("批次;物料;描述;重量;长度;质量;开始时间;结束时间");
            for (int i = 0; i <=s - 1; i++)
            {
                sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value+";"+ dataGridView1.Rows[i].Cells[1].Value + ";"+dataGridView1.Rows[i].Cells[2].Value + ";"+dataGridView1.Rows[i].Cells[3].Value + ";"+dataGridView1.Rows[i].Cells[4].Value + ";"+dataGridView1.Rows[i].Cells[5].Value + ";"+dataGridView1.Rows[i].Cells[6].Value + ";" + dataGridView1.Rows[i].Cells[7].Value);  
            }
            sw.Close();
            fs.Close();
            MessageBox.Show("报告位置 c:\\data\\生产记录"+tpno.Text+".txt");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("请输入抽样时间日期", "抽样", "", -1, -1);
            string batchadd = "";



            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\songjian", false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
            for (int i = 1; i <= 28; i++)
            {

                string fwl = (1000 + Convert.ToInt32(l.Text) + i - 1).ToString();
                batchadd = batchstart.Text + "-" + p.Text + "-" + c.Text + "-" + fwl.ToString().Substring(1, 4) + "-" + sp2.Text + "-" + i.ToString() + "#" + testm.Text.Trim();
                //dayinbiaoqian

                btFormat.SetNamedSubStringValue("batch", batchadd); //向bartender模板传递变量
                btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
            }

            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

            Class1.killbartender();


           

        }


        private void dtcalc1()
        {
            DateTime dt10 = Convert.ToDateTime(dateTimePicker1.Value);
            DateTime dt20 = Convert.ToDateTime(dateTimePicker2.Value);
            double ts = ((TimeSpan)(dt20 - dt10)).TotalMinutes;
            len.Text = (Convert.ToDecimal(speed1.Text) * Convert.ToInt32(ts) - Convert.ToDecimal(label39.Text)).ToString();
            pro1.Text = (Convert.ToDecimal(len.Text) * Convert.ToDecimal(kz.Text)).ToString("#0.00");
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dtcalc1();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (Class1.curuser == "admin")
            {
                if (MessageBox.Show("确认删除记录?", "删除", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Selected == true)
                        {
                            string sql0 = string.Format("delete from stock where batch='{0}' and sloc='{1}'", dataGridView1.Rows[i].Cells[0].Value.ToString(), Class1.workshop);
                            int c = Class1.ExcuteScal(sql0);
                            if (c == 1)
                            {
                                if (MessageBox.Show("是否删除断丝记录?", "删除", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    string sql1 = string.Format("delete from duansi where batch='{0}'", dataGridView1.Rows[i].Cells[0].Value.ToString());
                                    c = Class1.ExcuteScal(sql1);
                                }

                            }
                        }


                    }
                    flashtplist();
                }

            }
            else
            {
                MessageBox.Show("请登录管理员账号删除");
            }

               
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt30 = Convert.ToDateTime(dateTimePicker3.Value);
            DateTime dt40 = Convert.ToDateTime(dateTimePicker4.Value);
            double ts = ((TimeSpan)(dt40 - dt30)).TotalMinutes;
            textBox1.Text = ts.ToString();
            lenall.Text = (Convert.ToDecimal(speedall.Text) * Convert.ToDecimal(ts)).ToString();
            pro2.Text = (Convert.ToDecimal(lenall.Text) * Convert.ToDecimal(kz.Text)).ToString("#0.00");
        }

        private void yuansi_FormClosing(object sender, FormClosingEventArgs e)
        {
            Class1.killapp();
            Application.Exit();
        }

        private void c_Click(object sender, EventArgs e)
        {

        }

        private void p_Click(object sender, EventArgs e)
        {

        }

        private void l_Click(object sender, EventArgs e)
        {

        }

        private void batchstart_Click(object sender, EventArgs e)
        {
            batchstart.Focus();
            batchstart.Select(0, batchstart.Text.Length);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Selected == true)
                {
                    string dscs = dataGridView2.Rows[i].Cells[0].Value.ToString().Trim();
                    string dstime = dataGridView2.Rows[i].Cells[1].Value.ToString().Trim();
                    string dssj = dataGridView2.Rows[i].Cells[2].Value.ToString().Trim();
                    string batchx = batch.Text;
                    string sql = string.Format("delete from duansi where dscs='{0}' and dstime='{1}' and dssj='{2}' and batch='{3}'",dscs,dstime,dssj,batchx);
                    
                    int c = Class1.ExcuteScal(sql);
                    if (c == 1)
                    {
                        string sqlx = string.Format("update wsdevice set duansi=duansi-1,duansimin=duansimin-{0} where workshop='{1}' and devicenum='{2}'",dssj,Class1.workshop, fw.Text);
                        
                        int cc = Class1.ExcuteScal(sqlx);
                        freshduansi();
                        readlistviewstatus();
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string pro1 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim();
            string len = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Trim();
            string t1 = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString().Trim();
            string t2 = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString().Trim();

            string batch1 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();


            string sql1 = string.Format("update stock set pro1='{0}',len='{1}',t1='{2}',t2='{3}',pro1len='{1}' where batch='{4}'", pro1,len,t1,t2,batch1);
            //MessageBox.Show(sql1);
            int c2 = Class1.ExcuteScal(sql1);
            if (c2 == 0)
            {
                MessageBox.Show("更新失败");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Selected == true)
                {
                    BarTender.Application btApp;
                    BarTender.Format btFormat;
                    btApp = new BarTender.Application();
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\duansi", false, "");
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                    btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
                    btFormat.SetNamedSubStringValue("cishu", dataGridView2.Rows[i].Cells[0].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("fangwei", fw.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("time1", dataGridView2.Rows[i].Cells[1].Value.ToString()); //向bartender模板传递变量 
                    btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
                    btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
                    Class1.killbartender();
                }
            }
            
        }

        private void batchstart_TextChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Hide();
            dstrans dst = dstrans.ChildFromInstanc;
            if (dst != null)
            {
                dst.Owner = this;
                dst.Show();

            }
        }

        private void l_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
