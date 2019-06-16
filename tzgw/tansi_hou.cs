using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace tzgw
{
    public partial class tansi_hou : Form
    {
        public tansi_hou()
        {
            
            InitializeComponent();
            this.Text = Class1.workshop;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            flashdataview();
            readtspc(); //读取碳丝原料批次
            InitprinterComboBox();
            this.Text = Class1.workshop+" "+Class1.companyinfo;
            //DataGridView dataGridView2 = new DataGridView();
            end1.Text = "下卷" + " ("+ Class1.shift1+")";
            start1.Text = "上卷" + " (" + Class1.shift1 + ")";
            comboBox6.Text = Class1.shift1;
            
                label40.Text = "上浆剂批次";
                label48.Text = "碳酸氢铵批次";
            if (Class1.workshop == "ts7")
            {
                prttj.Text = "温度≤24℃ 湿度≤65%";
            }
            else
            {
                prttj.Text = "温度≤30℃ 湿度≤65%";
            }
        }

        public void backbox()
        {
            boxno.Text = Class1.backboxbatch;
            string[] sArray = Class1.backboxbatch.Split('-');
            prtxh.Text = sArray[sArray.Length - 1].ToString();
        }
        private void InitprinterComboBox()
        {
            List<String> list = LocalPrinter.GetLocalPrinters(); //获得系统中的打印机列表
            foreach (String s in list)
            {
                printerlist.Items.Add(s); //将打印机名称添加到下拉框中
            }

            printerlist.Text = dftprinter.GetDefaultPrinter();
        }
        private void readboxlist(string item)
        {
            string sql1 = string.Format("select boxnm from box where workshop='{0}' and itemcode<>'' and itemcode='{1}' ", Class1.workshop, item);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql1);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                comboBox2.Items.Clear();
                comboBox1.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(dt.Rows[i][0].ToString());
                    comboBox2.Items.Add(dt.Rows[i][0].ToString());
                }
                comboBox2.Text = dt.Rows[0][0].ToString();
            }
        }

        private void readtspc()
        {
            string sql1 = string.Format("select batch1,batch2 from lastbatch where workshop='{0}' and itemcode='{1}'", Class1.workshop, mt.Text);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql1);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                rmbatch1.Text = dt.Rows[0][0].ToString();
                rmbatch2.Text = dt.Rows[0][1].ToString();

            }
            else
            {
                rmbatch1.Text = "";
                rmbatch2.Text = "";

            }

        }


        private delegate void SetDGVSource(DataTable dt);//添加设置DataGridView的DataSource的代理  
        public static void SetDGVSourceFunction1(DataTable dt)
        {
            if (dataGridView3.InvokeRequired)
            {
                SetDGVSource delegateSetSource1 = new SetDGVSource(SetDGVSourceFunction1);
                dataGridView3.Invoke(delegateSetSource1, new object[] { dt });
            }
            else
            {
                dataGridView3.DataSource = dt;
            }
        
        } //状态清单

        private delegate void SetDGVSource2(DataTable dt);//添加设置DataGridView的DataSource的代理  
        public static void SetDGVSourceFunction2(DataTable dt)
        {
            if (dataGridView2.InvokeRequired)
            {
                SetDGVSource2 delegateSetSource2 = new SetDGVSource2(SetDGVSourceFunction2);
                dataGridView2.Invoke(delegateSetSource2, new object[] { dt });

               

            }
            else
            {
                dataGridView2.DataSource = dt;
            }

        }// box 清单

        private delegate void SetDGVSource0(DataTable dt);//添加设置DataGridView的DataSource的代理  
        public static void SetDGVSourceFunction0(DataTable dt)
        {
            if (dataGridView1.InvokeRequired)
            {
                SetDGVSource0 delegateSetSource0 = new SetDGVSource0(SetDGVSourceFunction0);
                dataGridView1.Invoke(delegateSetSource0, new object[] { dt });
            }
            else
            {
                dataGridView1.DataSource = dt;
            }


           

        }// box 状态 清单



        private static tansi_hou childFromInstanc;
        public static tansi_hou ChildFromInstanc
        {
            get
            {
                if (childFromInstanc == null || childFromInstanc.IsDisposed)
                {
                    childFromInstanc = new tansi_hou();
                }

                return childFromInstanc;
            }
        }


        private void gw_TextChanged(object sender, EventArgs e)
        {
            yspc.Text = "";

            flashdataview();
            //0 纺位 1 当前状态-前 2 当前状态-后 3 mt 生产物料 4 上个成品批次 5 原丝批次 6 原丝描述 7 上卷时间 8 下卷时间 9 大批次 10 上次质检 11 后端速度
            if (gw.Text != "")
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    if (dataGridView3.Rows[i].Cells[0].Value.ToString().Trim() == gw.Text)
                    {
                        dataGridView3.FirstDisplayedScrollingRowIndex = i;
                        yspc.Text = dataGridView3.Rows[i].Cells[5].Value.ToString().Trim();

                        if (dataGridView3.Rows[i].Cells[2].Value.ToString().Trim() == "停止")
                        {
                            btstu.Text = "开始";

                            //dt1.Enabled = true;
                            if (dt1lock.Checked != true)
                            {
                                dt1.Text = dataGridView3.Rows[i].Cells[8].Value.ToString().Trim();
                            }

                            //dt2.Enabled = false;
                            start1.BackColor = Color.Lime;
                            end1.BackColor = SystemColors.Control;
                        }//为空表示停止状态 只刷新上卷时间， 下卷按钮和下卷时间为灰 
                        else
                        {

                            btstu.Text = "停止";

                            //dt1.Enabled = true;
                            if (dt1lock.Checked != true)
                            {
                                dt1.Text = dataGridView3.Rows[i].Cells[7].Value.ToString().Trim();
                            }

                            //dt2.Enabled = true;
                            if (dt2lock.Checked != true)
                            {
                                dt2.Text = DateTime.Now.ToString();
                            }

                            start1.BackColor = SystemColors.Control;
                            end1.BackColor = Color.Lime;
                        }

                        if (dlfw.Checked == true)
                        {
                            rollbatch.Text = infobatchbig.Text.ToString().Trim();
                            mt.Text = infowl.Text.ToString().Trim();
                        }
                        else
                        {
                            rollbatch.Text = dataGridView3.Rows[i].Cells[9].Value.ToString().Trim();
                            mt.Text = dataGridView3.Rows[i].Cells[3].Value.ToString().Trim();
                        }
                        spd1.Text = dataGridView3.Rows[i].Cells[11].Value.ToString().Trim();
                        qa.Text = dataGridView3.Rows[i].Cells[10].Value.ToString().Trim();
                        kz.Text = dataGridView3.Rows[i].Cells[12].Value.ToString().Trim();
                        boxno.Text = Class1.workshop+"-"+DateTime.Now.ToString("yyyyMMdd");
                        
                    }
                }//原丝批次 工位全部信息

                

                flashdataview0();

                freshbatchnext();

            }








        }

        private void flashdataview0()
        {
            DataTable dt2 = new DataTable();
            SetDGVSourceFunction0(dt2);

            string sql1 = "";

            if (dlfw.Checked == true)
            {
                sql1 = string.Format("select T1.boxnm as 名称,T1.r1 as 下限,T1.r2 as 上限,T1.reqnum as 需求,T1.maxcon as 容量,'0' as 库存,T1.comm2 as 上卷 from box T1 where T1.workshop='{0}' and T1.itemcode='{1}'", Class1.curuser, mt.Text.Trim());
            }
            else
            {
                sql1 = string.Format("select T1.boxnm as 名称,T1.r1 as 下限,T1.r2 as 上限,T1.reqnum as 需求,T1.maxcon as 容量,'0' as 库存,T1.comm2 as 上卷 from box T1 where T1.workshop='{0}' and T1.itemcode='{1}'", Class1.workshop, mt.Text.Trim());
            }

            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                SetDGVSourceFunction0(dt);

            }
        }


        private void flashdataview()
        {

            DataTable dt2 = new DataTable();
            SetDGVSourceFunction1(dt2);

            string sql1 = "";

            sql1 = string.Format("select cast(T1.devicenum as int) as 纺位,T1.status1 as 机头状态,T1.status2 as 机尾状态,T1.itemrun 生产物料, isnull(dateadd(mi,T4.lilun,T1.t3),dateadd(mi,T4.lilun,getdate())) as 理论下卷时间, T1.batchcus as 原丝批次, T2.len as 原丝剩余,T1.t3 as 上卷时间, T1.t4 as 下卷时间,T1.batchbig as 当前批次,T1.lastqa as 上次质检,T1.speed2 as 后端速度,T1.xmd as 线密度 from wsdevice T1 left join stock T2 on T1.batchcus = T2.batch left join masterdata T3 on T2.material = T3.itemcode left join masterdata T4 on T1.itemrun=T4.itemcode where workshop = '{0}' and 1=1", Class1.workshop);

            if (workonly.Checked == true)
            {
                sql1 = sql1 + string.Format(" and status2='运行'");
            }

            sql1 = sql1 + string.Format(" order by T1.status2 desc,理论下卷时间,T1.itemrun,纺位");

            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql1);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                SetDGVSourceFunction1(dt);


                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    if (dataGridView3.Rows[i].Cells[2].Value.ToString().Trim()=="运行" & Convert.ToDateTime(dataGridView3.Rows[i].Cells[4].Value) <= DateTime.Now)
                    {
                        dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }

                }
            }




        }



        private void button3_Click(object sender, EventArgs e)
        {
            flashdataview();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            clear1();

        }

        private void clear1()
        {
            gw.Text = "";
            yspc.Text = "";
            mt.Text = "";
            spd1.Text = "";
            newbatch.Text = "";
            rollbatch.Text = "";
            if (dt1lock.Checked != true)
            {
                dt1.Text = DateTime.Now.ToString();
            }

            if (dt2lock.Checked != true)
            {
                dt2.Text = DateTime.Now.ToString();
            }

            boxno.Text = Class1.workshop+"-"+DateTime.Now.ToString("yyyyMMdd");
            qa.Text = "";

            sboxno.Text = "";

            comboBox1.Text = "";
            gw.Focus();

            if (keepw.Checked == false)
            {
                fweight.Text = "";
                flen.Text = "";
                weight.Text = "0";
                len.Text = "0";
            }


            start1.BackColor = SystemColors.Control;
            end1.BackColor = SystemColors.Control;
            start1.ForeColor = Color.Black;
            end1.ForeColor = Color.Black;
        }

        private void workonly_CheckStateChanged(object sender, EventArgs e)
        {
            flashdataview();
        }


      

        private void button6_Click(object sender, EventArgs e)
        {
            Class1.killbartender();
            try
            {
                serialPort.Close();
            }
            finally
            {

                mainmenu mn = (mainmenu)this.Owner;
                this.Owner.Show();
                this.Dispose();
            }


        }


        private void button4_Click(object sender, EventArgs e)
        {
            string allowprint = "1";
            string allowinsert = "1";
            if (inforg.Checked == true)
            {
                if (Convert.ToInt32(gw.Text) >= Convert.ToInt32(infof1.Text) & Convert.ToInt32(gw.Text) <= Convert.ToInt32(infof2.Text))
                {
                    allowprint = "1";
                }
                else
                {
                    allowprint = "0";
                    MessageBox.Show("工位不在限定范围内");
                }
            }
            else
            {
                allowprint = "1";
            }
                

            if (allowprint == "1" & gw.Text != "" & newbatch.Text.Length==11 & rollbatch.Text != "" & flen.Text != "" & fweight.Text != "" & cls.Text != "")
                {

                if (boxno.Text.Substring(0, 1) != "t")
                {
                    if (MessageBox.Show("默认下卷箱号错误，请确认是否向此箱加入此卷?", "默认箱号", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        allowinsert = "0";
                    }
                }
                
                if (allowinsert == "1")
                {
                    string sql = string.Format("insert into stock(branch,sloc,material,batch,displaybatch,stockin,t1,t2,boxno,qa,pro1,len,rmbatch,rmbatch1,batchbig,c5,qatype,qa2,qa2r,tpno,shift1,rmbatch2) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','','{19}','{20}')", "tz", Class1.workshop, mt.Text, newbatch.Text, newbatch.Text, DateTime.Now, dt1.Text, dt2.Text, boxno.Text.Trim(), qa.Text, fweight.Text.ToString(), flen.Text.ToString(), yspc.Text.Trim(), rmbatch1.Text, rollbatch.Text, Class1.workshop + "-" + gw.Text, cls.Text, qa2.Text, qa2r.Text, comboBox6.Text, rmbatch2.Text);
                    int c1 = Class1.ExcuteScal(sql);
                    if (c1 == 1)
                    {
                        lastbox1.Text = sboxno.Text; //保留箱号


                        if (ptoff.Checked == false)
                        {
                            BarTender.Application btApp;
                            BarTender.Format btFormat;
                            btApp = new BarTender.Application();
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                            if (comboBox4.Text == "A")
                            {
                                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                            }
                            if (comboBox4.Text == "M")
                            {
                                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");
                            }
                            if (comboBox4.Text == "F")
                            {
                                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiF", false, "");
                            }
                            if (comboBox4.Text == "L")
                            {
                                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiL", false, "");
                            }

                            btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数
                            btFormat.SetNamedSubStringValue("desc1", desc2.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("batch", newbatch.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("batch0", rollbatch.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("pro1", fweight.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("len", flen.Text); //向bartender模板传递变量

                            btFormat.SetNamedSubStringValue("date1", prtsj.Text); //向bartender模板传递变量
                            btFormat.SetNamedSubStringValue("c5", Class1.workshop + "-" + gw.Text); //向bartender模板传递变量

                            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性
                            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签
                            Class1.killbartender();
                        }
                        string sql3 = "";
                        if (dlfw.Checked == true)
                        {
                            sql3 = string.Format("update box set comm2='{0}' where workshop='{1}' and itemcode='{2}' and boxnm='{3}'", newbatch.Text, Class1.curuser, mt.Text, cls.Text);

                        }
                        else
                        {
                            sql3 = string.Format("update box set comm2='{0}' where workshop='{1}' and itemcode='{2}' and boxnm='{3}'", newbatch.Text, Class1.workshop, mt.Text, cls.Text);

                        }
                        int c3 = Class1.ExcuteScal(sql3);
                        if (c3 == 1)
                        {
                            flashdataview0();
                        }


                        if (checkBox5.Checked == false)
                        {
                            if (upc.Checked == false)
                            { //连续上卷
                                string sql2 = string.Format("update wsdevice set batchrun='{0}',status2='停止',t4='{4}' where workshop='{1}' and devicenum='{2}'", newbatch.Text, Class1.workshop, gw.Text, mt.Text, dt2.Text);
                                int c2 = Class1.ExcuteScal(sql2);
                                if (c2 == 1)
                                {
                                    flashdataview();
                                    flashdataview2();
                                    string gw1 = gw.Text;
                                    clear1();
                                    gw.Text = gw1;
                                }

                            }
                            else
                            {
                                string sql2 = string.Format("update wsdevice set batchrun='{0}',status2='运行',t3='{4}' where workshop='{1}' and devicenum='{2}'", newbatch.Text, Class1.workshop, gw.Text, mt.Text, dt2.Text);
                                int c2 = Class1.ExcuteScal(sql2);
                                if (c2 == 1)
                                {
                                    flashdataview();
                                    flashdataview2();
                                    clear1();
                                }
                            }

                        }
                        else
                        {
                            flashdataview();
                            flashdataview2();
                            clear1();
                        }


                    }
                    else
                    {
                        MessageBox.Show("添加失败,请确认卷号是否重复?");
                    }
                }


                }
                else
                {
                    MessageBox.Show("请确认输入以下内容：批次，卷号(11位)，重量，长度， 类型");
                }
            

            
        }



        private void boxno_TextChanged(object sender, EventArgs e)
        {
            string[] sArray = boxno.Text.Split('-');
            prtxh.Text = sArray[sArray.Length-1].ToString();
            
            flashdataview2();
            dataGridView2.ReadOnly = false;
            foreach (DataGridViewColumn c in dataGridView2.Columns)
            {
                if (c.Index == 0 | c.Index == 1)
                {
                    c.ReadOnly = true;
                }
            }


        }


        private void flashdataview2()
        {
            if (boxno.Text != "" & boxno.Text.Length>=Convert.ToInt16(textBox1.Text))
            {
                string sql = string.Format("select row_number() over (order by T1.batch) as #,T1.batch as 卷号,T1.t1 as 开始时间,T1.t2 as 结束时间,T1.c5 as 工位,T1.pro1 as 重量,T1.batchbig as 批号,T1.material as 物料,T2.desc1 as 描述,T1.len as 长度,T1.qatype as 类别,T1.stockin as 完工时间 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where T1.boxno='{0}'", boxno.Text);
                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql);
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = ds.Tables[0];
                boxqty.Text = (dt.Rows.Count).ToString();

                if (dt.Rows.Count > 0)
                {
                    
                    DateTime dtn = DateTime.Now;
                    DateTime dtn0 = DateTime.Now;
                    DateTime dtn00 = DateTime.Now;
                    string cb = "";

                    double wn2 = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToDateTime(dt.Rows[i][3]) < dtn)
                        {
                            //MessageBox.Show(Convert.ToDateTime(dt.Rows[i][3]).ToString());
                            dtn = Convert.ToDateTime(dt.Rows[i][3]);
                            cb = dt.Rows[i][10].ToString();
                        }
                        
                        wn2 = wn2 + Convert.ToDouble(dt.Rows[i][5]);
                        
                        dtn0= Convert.ToDateTime(dt.Rows[dt.Rows.Count-1][3]);
                        dtn00 = Convert.ToDateTime(dt.Rows[0][3]);

                    }
                    if (comboBox5.Text == "最早结束")
                    {
                        prtsj.Text = dtn.ToString("yy.MM.dd");
                        // MessageBox.Show("1");
                        comboBox1.Text = cb;
                    }
                    if (comboBox5.Text == "最后一行")
                    {
                        prtsj.Text = dtn0.ToString("yy.MM.dd");
                        comboBox1.Text = dt.Rows[dt.Rows.Count-1][10].ToString();
                    }
                    if (comboBox5.Text == "首行")
                    {
                        prtsj.Text = dtn00.ToString("yy.MM.dd");
                        comboBox1.Text = dt.Rows[0][10].ToString();
                    }

                    if (checkBox4.Checked == false)
                    {
                        prtjz.Text = wn2.ToString("#0.00") + " kg";
                    }
                    else
                    {
                        prtjz.Text =(wn2/dt.Rows.Count).ToString("#0.00") + "kg X "+ dt.Rows.Count.ToString();
                    }



                    string itm = dt.Rows[0][7].ToString().Trim();
                    string sqlx = string.Format("select sp4,sp5,desc1 from masterdata where itemcode='{0}'", itm);
                    DataSet dsx = new DataSet();
                    dsx = Class1.GetAllDataSet(sqlx);
                    DataTable dtx = new DataTable();
                    dtx = dsx.Tables[0];
                    if (dtx.Rows.Count > 0)
                    {
                        prtgg.Text = dtx.Rows[0][2].ToString().Trim();
                        prtrq.Text = dtx.Rows[0][0].ToString().Trim();
                        prtbz.Text = dtx.Rows[0][1].ToString().Trim();
                    }

                    SetDGVSourceFunction2(dt);

                }
                else
                {
                    comboBox1.Text = "";
                    prtsj.Text = "";
                    prtjz.Text = "";

                    prtgg.Text = "";
                    prtrq.Text = "";
                    prtbz.Text = "";
                    SetDGVSourceFunction2(dt2);
                }

            }
            else
            {
                comboBox1.Text = "";
                prtsj.Text = "";
                prtjz.Text = "";
                prtgg.Text = "";
                prtrq.Text = "";
                prtbz.Text = "";
            }
            


        }
        SerialPort serialPort = new SerialPort();
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text == "连接电子秤")
            {
                //设置参数
                serialPort.PortName = "COM1"; //通信端口
                serialPort.BaudRate = 9600; //串行波特率
                serialPort.DataBits = 8; //每个字节的标准数据位长度
                serialPort.StopBits = StopBits.One; //设置每个字节的标准停止位数
                serialPort.Parity = Parity.None; //设置奇偶校验检查协议
                serialPort.ReadTimeout = 3000; //单位毫秒
                serialPort.WriteTimeout = 3000; //单位毫秒
                                                //串口控件成员变量，字面意思为接收字节阀值，
                                                //串口对象在收到这样长度的数据之后会触发事件处理函数
                                                //一般都设为1
                serialPort.ReceivedBytesThreshold = 1;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(CommDataReceived); //设置数据接收事件（监听）

                try
                {
                    serialPort.Open(); //打开串口
                }
                catch (Exception ex)
                {
                    MessageBox.Show("提示信息", "串行端口打开失败！具体原因：" + ex.Message);
                    System.Environment.Exit(0); //彻底退出应用程序   
                }

                button1.Text = "断开电子秤";
            }
            else
            {
                try
                {
                    serialPort.Close();
                }
                finally
                {
                    weight.Text = "";
                    len.Text = "";
                }

                button1.Text = "连接电子秤";
            }

        }

        public string msg = "";

        private List<byte> buffer = new List<byte>(4096);
        public void CommDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //Comm.BytesToRead中为要读入的字节长度

                int len1 = serialPort.BytesToRead;
                //MessageBox.Show(len.ToString());
                Byte[] readBuffer = new Byte[len1];
                serialPort.Read(readBuffer, 0, len1); //将数据读入缓存
                //buffer.Clear();
                buffer.AddRange(readBuffer);
                //处理readBuffer中的数据，自定义处理过程
                while (buffer.Count > 19)
                {
                    if (buffer[0] == 0x53)
                    {
                        if (buffer[1] == 0x54 && buffer[17] == 0x0D && buffer[18] == 0x0A)
                        {
                            msg = "";
                            for (int j = 0; j <= 18; j++)
                            {
                                if (buffer[j] == 0x2e || buffer[j] == 0x30 || buffer[j] == 0x31 || buffer[j] == 0x32 || buffer[j] == 0x33 || buffer[j] == 0x34 || buffer[j] == 0x35 || buffer[j] == 0x36 || buffer[j] == 0x37 || buffer[j] == 0x38 || buffer[j] == 0x39)
                                {
                                    msg = msg + Char.ConvertFromUtf32(Convert.ToInt32(buffer[j]));
                                }


                            }

                            BeginInvoke(new MethodInvoker(delegate
                        {
                            try
                            {
                                weight.Text = "";
                                string[] split = msg.Split(new Char[] { ',' });

                                string readfrom = split[0].ToString();
                                if (pz.Text != "")
                                {
                                    weight.Text = (Convert.ToDecimal(readfrom.Trim()) - Convert.ToDecimal(pz.Text.Trim())).ToString();
                                    if (kz.Text != "")
                                    {
                                        len.Text = Convert.ToInt32(((Convert.ToDecimal(weight.Text.Trim())) / Convert.ToDecimal(kz.Text.Trim()))).ToString();
                                    }
                                    else
                                    {
                                        len.Text = "0";
                                    }
                                }


                            }
                            finally
                            {

                            }

                        }));

                        }
                        buffer.RemoveRange(0, 18);
                    }
                    else
                    {
                        buffer.RemoveAt(0);
                    }




                }

                //serialPort.DiscardInBuffer();

            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show("提示信息", "接收返回消息异常！具体原因：" + ex.Message);
            //}
            finally
            {

            }

        }



        private void weight_TextChanged(object sender, EventArgs e)
        {
            if (weight.Text != "")
            {
                len.Text = Convert.ToInt32(Math.Round(Convert.ToDouble(weight.Text.Trim()),2) / Convert.ToDouble(kz.Text.Trim())).ToString();
                wuchatest();
            }

        }


        private void wuchatest()
        {

            if (checkBox1.Checked == true)
            {
                int flag = 0;
                if (weight.Text != "")
                {
                    if(f10.Text != "" && f11.Text != "")
                    {
                        if (Convert.ToDecimal(weight.Text) >= Convert.ToDecimal(f10.Text) && Convert.ToDecimal(weight.Text) <= Convert.ToDecimal(f11.Text))
                        {

                            flag = 1;
                        }
                    }
                    if (f20.Text != "" && f21.Text != "")
                    {
                        if (Convert.ToDecimal(weight.Text) >= Convert.ToDecimal(f20.Text) && Convert.ToDecimal(weight.Text) <= Convert.ToDecimal(f21.Text))
                        {

                            flag = 2;

                        }
                    }
  

                }

                if (flag == 0)
                {
                    fweight.Text = weight.Text.ToString();
                    flen.Text = len.Text.ToString();
                }
                if (flag == 1)
                {
                    fweight.Text = f1.Text.ToString();
                    flen.Text = l1.Text.ToString();
                }
                if (flag == 2)
                {
                    fweight.Text = f2.Text.ToString();
                    flen.Text = l2.Text.ToString();
                }

            }
            else
            {
                fweight.Text = weight.Text.ToString();
                flen.Text = len.Text.ToString();
            }
        }

        private void newbatch_TextChanged(object sender, EventArgs e)
        {

                string year = "";
                string month = "";
                string day = "";
                if (newbatch.Text.Length > 6)
                {
                    string[] Arr = dt1.Text.Split(' ');
                    string[] Arr2 = dt2.Text.Split(' ');
                    string left6 = newbatch.Text.Remove(6);
                    //MessageBox.Show(left6);
                    year = left6.Substring(0,2);
                    month = left6.Substring(2, 2);
                    day = left6.Substring(4, 2);

                if (checkBox5.Checked==true)
                {
                    dt1.Text = "20" + year + "/" + month + "/" + day + " " + Arr[1];
                    dt2.Text = "20" + year + "/" + month + "/" + day + " " + Arr2[1];
                }



                prtsj.Text = year + "." + month + "." + day;
                }
            
            
        }




        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

            string prttype = "";
            int prttypestart = 0;
            int prttypeend = 0;


            if (checkBox6.Checked == true)
            {
                int prtline2 = 1;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Selected == true)
                    {
                        prtline2 = i + 1;
                    }
                }
                prttypestart = Convert.ToInt32(Interaction.InputBox("请选择打印-起始-行数", "打印", prtline2.ToString(), -1, -1));
                prttypeend = Convert.ToInt32(Interaction.InputBox("请选择打印-终止-行数", "打印", (dataGridView2.Rows.Count-1).ToString(), -1, -1));

                BarTender.Application btApp;
                BarTender.Format btFormat;
                btApp = new BarTender.Application();
                btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                if (comboBox4.Text == "A")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                }
                if (comboBox4.Text == "M")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");
                }
                if (comboBox4.Text == "F")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiF", false, "");
                }
                if (comboBox4.Text == "L")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiL", false, "");
                }
                if (comboBox4.Text == "S")
                {
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiS", false, "");
                }

                btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数


                for (int i = prttypestart-1; i < prttypeend; i++)
                {
                    btFormat.SetNamedSubStringValue("desc1", dataGridView2.Rows[i].Cells[8].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("batch", dataGridView2.Rows[i].Cells[1].Value.ToString()); //向bartender模板传递变量            
                    btFormat.SetNamedSubStringValue("batch0", dataGridView2.Rows[i].Cells[6].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("pro1", dataGridView2.Rows[i].Cells[5].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("len", dataGridView2.Rows[i].Cells[9].Value.ToString()); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("date1", prtsj.Text); //向bartender模板传递变量
                    btFormat.SetNamedSubStringValue("c5", dataGridView2.Rows[i].Cells[4].Value.ToString()); //向bartender模板传递变量


                    btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性

                }

                btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

                Class1.killbartender();
            }
            else
            {
                int prtline = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Selected == true)
                    {
                        prtline = i + 1;
                    }
                }
                prttype = Interaction.InputBox("请选择打印行数,0-全部", "打印", prtline.ToString(), -1, -1);
                if (prttype == "0")
                {
                    BarTender.Application btApp;
                    BarTender.Format btFormat;
                    btApp = new BarTender.Application();
                    btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                    if (comboBox4.Text == "A")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                    }
                    if (comboBox4.Text == "M")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");
                    }
                    if (comboBox4.Text == "F")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiF", false, "");
                    }
                    if (comboBox4.Text == "L")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiL", false, "");
                    }
                    if (comboBox4.Text == "S")
                    {
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiS", false, "");
                    }

                    btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                    btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

                    int s = dataGridView2.RowCount;
                    for (int i = 0; i < s - 1; i++)
                    {
                        btFormat.SetNamedSubStringValue("desc1", dataGridView2.Rows[i].Cells[8].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("batch", dataGridView2.Rows[i].Cells[1].Value.ToString()); //向bartender模板传递变量            
                        btFormat.SetNamedSubStringValue("batch0", dataGridView2.Rows[i].Cells[6].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("pro1", dataGridView2.Rows[i].Cells[5].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("len", dataGridView2.Rows[i].Cells[9].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("date1", prtsj.Text); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("c5", dataGridView2.Rows[i].Cells[4].Value.ToString()); //向bartender模板传递变量


                        btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性

                    }

                    btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

                    Class1.killbartender();
                }
                else
                {
                    int s = dataGridView2.RowCount;
                    if ((Convert.ToInt16(prttype) - 1) >= 0 && (Convert.ToInt16(prttype) - 1) < s - 1)
                    {
                        int r = Convert.ToInt16(prttype) - 1;
                        BarTender.Application btApp;
                        BarTender.Format btFormat;
                        btApp = new BarTender.Application();
                        btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                        if (comboBox4.Text == "A")
                        {
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansi", false, "");
                        }
                        if (comboBox4.Text == "M")
                        {
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiM", false, "");
                        }
                        if (comboBox4.Text == "F")
                        {
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiF", false, "");
                        }
                        if (comboBox4.Text == "L")
                        {
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiL", false, "");
                        }
                        if (comboBox4.Text == "S")
                        {
                            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\tansiS", false, "");
                        }
                        btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;  //设置同序列打印的份数
                        btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

                        btFormat.SetNamedSubStringValue("desc1", dataGridView2.Rows[r].Cells[8].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("batch", dataGridView2.Rows[r].Cells[1].Value.ToString()); //向bartender模板传递变量            
                        btFormat.SetNamedSubStringValue("batch0", dataGridView2.Rows[r].Cells[6].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("pro1", dataGridView2.Rows[r].Cells[5].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("len", dataGridView2.Rows[r].Cells[9].Value.ToString()); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("date1", prtsj.Text); //向bartender模板传递变量
                        btFormat.SetNamedSubStringValue("c5", dataGridView2.Rows[r].Cells[4].Value.ToString()); //向bartender模板传递变量



                        btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性

                        btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

                        Class1.killbartender();
                    }
                }

            }




        }



        private void freshbox(string boxno1)
        {
            if (boxno1 != "")
            {
                string sql = string.Format("select row_number() over (order by T1.batch) as #,T1.batch as 卷号,T1.batchbig as 批号,T1.material as 物料,T2.desc1 as 描述,T1.pro1 as 重量,cast(T1.res/convert(float,T2.sp1) as decimal(18,2)) as 长度 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where T1.boxno='{0}'", boxno1);
                DataSet ds = new DataSet();
                ds = Class1.GetAllDataSet(sql);
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    SetDGVSourceFunction2(dt);
                }
                else
                {
                    SetDGVSourceFunction2(dt2);
                }

            }
        }

        private void tansi_hou_Load(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void mt_TextChanged_1(object sender, EventArgs e)
        {
            string sql = string.Format("select desc1,sp1,sp3,sp4,sp5,printername,lilun from masterdata where itemcode='{0}'", mt.Text.Trim());
            DataSet ds = new DataSet();
            ds = Class1.GetAllDataSet(sql);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                desc2.Text = ds.Tables[0].Rows[0][0].ToString().Trim();
                
                //kz.Text = ds.Tables[0].Rows[0][1].ToString().Trim();
                gy.Text = ds.Tables[0].Rows[0][2].ToString().Trim();
                printerlist.Text= ds.Tables[0].Rows[0][5].ToString().Trim();
                if (dftprinter.SetDefaultPrinter(printerlist.Text) != true)
                {
                    MessageBox.Show("设置默认打印机失败!");
                }
                lilun.Text= ds.Tables[0].Rows[0][6].ToString().Trim();
            }
            else
            {
                desc2.Text = "";

                //kz.Text = "1";
                gy.Text = "1";
                lilun.Text = "0";
            }

            string sql2 = string.Format("select xiaxian,shangxian,fweight,flen from wucha where itemcode='{0}'", mt.Text.Trim());
            DataSet ds2 = new DataSet();
            ds2 = Class1.GetAllDataSet(sql2);
            DataTable dt2 = new DataTable();
            dt2 = ds2.Tables[0];
            if (dt2.Rows.Count>0)
            {

                f10.Text = dt2.Rows[0][0].ToString();
                f11.Text = dt2.Rows[0][1].ToString();
                f1.Text = dt2.Rows[0][2].ToString();
                l1.Text = dt2.Rows[0][3].ToString();
               
                if (dt2.Rows.Count > 1)
                {
                    f20.Text = dt2.Rows[1][0].ToString();
                    f21.Text = dt2.Rows[1][1].ToString();
                    f2.Text = dt2.Rows[1][2].ToString();
                    l2.Text = dt2.Rows[1][3].ToString();
                }
            }
            else
            {
                f10.Text = "";
                f11.Text = "";
                f1.Text = "";
                l1.Text = "";
                f20.Text = "";
                f21.Text = "";
                f2.Text = "";
                l2.Text = "";
            }

            readboxlist(mt.Text.Trim());

        }



        private void start1_Click(object sender, EventArgs e)
        {
            if (gw.Text != "" && dt1.Text != "")
            {
                string sql = string.Format("update wsdevice set t3='{0}',status2='运行' where devicenum='{1}' and workshop='{2}'", dt1.Text, gw.Text.Trim(), Class1.workshop);
                int c = Class1.ExcuteScal(sql);
                if (c == 1)
                {
                    gw.Text = "";
                    clear1();
                }
            }
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            if (dt1lock.Checked != true)
            {
                dt1.Text = DateTime.Now.ToString();
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (dt2lock.Checked != true)
            {
                dt2.Text = DateTime.Now.ToString();
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            wuchatest();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            wuchatest();
        }

        private void sboxno_TextChanged(object sender, EventArgs e)
        {
            boxno.Text = rollbatch.Text.Trim() + "-" + sboxno.Text.Trim();
            prtxh.Text = sboxno.Text;
        }

        private void fweight_TextChanged(object sender, EventArgs e)
        {
            if (fweight.Text != "")
            {
                comboBox1.Text = "";
                cls.Text = "";
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    decimal d1 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value.ToString().Trim());
                    decimal d2 = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString().Trim());

                    if (Convert.ToDecimal(fweight.Text) >= d1 && Convert.ToDecimal(fweight.Text) <= d2)
                    {
                        comboBox1.Text = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                        cls.Text=dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();

                        /* 当下M或L 更改类别首字母，但会造成生成卷号的问题，待确认
                         if (checkBox7.Checked == true)
                         {
                            if (cls.Text.Length >= 1)
                            {
                                cls.Text = cls.Text.Remove(0, 1);
                                cls.Text = cls.Text.Insert(0, comboBox4.Text);
                            }
                        }
                        */
                    }

                    if (mt.Text != "" & rollbatch.Text != "")
                    {
                        string sql = string.Format("select count(batch) from stock where qatype='{0}' and batchbig='{2}' and material='{1}'", dataGridView1.Rows[i].Cells[0].Value.ToString().Trim(),mt.Text.Trim(),rollbatch.Text.Trim());
                        
                        DataSet ds0 = new DataSet();
                        ds0 = Class1.GetAllDataSet(sql);
                        DataTable dt0 = new DataTable();
                        dt0 = ds0.Tables[0];
                        //MessageBox.Show(sql);
                        if (dt0.Rows.Count > 0)
                        {
                            dataGridView1.Rows[i].Cells[5].Value = dt0.Rows[0][0].ToString();

                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[5].Value = "0";
                        }
                    }

                }
            }
        }






        private void mt_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
            string sqlitm = string.Format("select 1 from masterdata where itemcode='{0}'",infowl.Text);
            //before setup check the itemcode exist
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sqlitm);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (MessageBox.Show("确认设置纺位信息?", "初始化", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string sql0 = string.Format("update lastbatch set batch1='{0}',batch2='{3}' where workshop='{1}' and itemcode='{2}'", rmbatch1.Text.Trim(), Class1.workshop, infowl.Text, rmbatch2.Text.Trim());
                    int c2 = Class1.ExcuteScal(sql0);
                    string sql = "";
                    if (range1.BackColor == Color.Lime)
                    {
                        if (checkBox2.Checked == true)
                        {
                            sql = string.Format("update wsdevice set status2='运行',itemrun='{0}',batchbig='{1}',speed2='{2}',xmd='{6}' where workshop='{3}' and devicenum>='{4}' and devicenum<='{5}'", infowl.Text, infobatchbig.Text, infospeed2.Text, Class1.workshop, infof1.Text, infof2.Text, xmd.Text.Trim());
                        }
                        else
                        {
                            sql = string.Format("update wsdevice set status2='停止',itemrun='{0}',batchbig='{1}',speed2='{2}',xmd='{6}' where workshop='{3}' and devicenum>='{4}' and devicenum<='{5}'", infowl.Text, infobatchbig.Text, infospeed2.Text, Class1.workshop, infof1.Text, infof2.Text, xmd.Text.Trim());

                        }
                    }
                    if (range2.BackColor == Color.Lime)
                    {
                        if (checkBox2.Checked == true)
                        {
                            sql = string.Format("update wsdevice set status2='运行',itemrun='{0}',batchbig='{1}',speed2='{2}',xmd='{5}' where workshop='{3}' and devicenum in ({4})", infowl.Text, infobatchbig.Text, infospeed2.Text, Class1.workshop, infof3.Text, xmd.Text.Trim());
                        }
                        else
                        {
                            sql = string.Format("update wsdevice set status2='停止',itemrun='{0}',batchbig='{1}',speed2='{2}',xmd='{5}' where workshop='{3}' and devicenum in ({4})", infowl.Text, infobatchbig.Text, infospeed2.Text, Class1.workshop, infof3.Text, xmd.Text.Trim());
                        }
                    }
                    int c = Class1.ExcuteScal(sql);
                    if (c > 0)
                    {
                        flashdataview();
                        clear1();
                    }
                    if (comboBox2.Text != "")
                    {
                        string sql3 = string.Format("update box set reqnum='{0}' where itemcode='{1}' and boxnm='{2}'", infoqty.Text, infowl.Text, comboBox2.Text);
                        int c3 = Class1.ExcuteScal(sql3);
                        if (c3 == 1)
                        {
                            flashdataview0();
                        }
                    }


                }
            }
            else
            {
                MessageBox.Show("物料不存在");
            }
            

        }

        private void infowl_TextChanged(object sender, EventArgs e)
        {
            readboxlist(infowl.Text);
            string SQL1 = string.Format("select sp1,desc1 from masterdata where itemcode='{0}'",infowl.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(SQL1);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                xmd.Text = dt.Rows[0][0].ToString().Trim();
                desc6.Text= dt.Rows[0][1].ToString().Trim();
            }
        }


        public void itemcodeload()
        {
            infowl.Text = "";
            infowl.Text = Class1.itemtansi1;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string s = "";
            if (boxno.Text.Length >= 8)
            {
                int k = boxno.Text.IndexOf('-');
                if (k > 0)
                {
                    s = boxno.Text.Substring(0, k);
                }
                else
                {
                    s = boxno.Text.Substring(0, 8);
                }
                //MessageBox.Show(s);

            }
            BarTender.Application btApp;
            BarTender.Format btFormat;
            btApp = new BarTender.Application();
            
            btFormat = btApp.Formats.Open(@"C:\data\tzgw-labeltemplete\"+ comboBox3.Text, false, "");
            btFormat.PrintSetup.IdenticalCopiesOfLabel = 2;  //设置同序列打印的份数
            btFormat.PrintSetup.NumberSerializedLabels = 1;  //设置需要打印的序列数

            btFormat.SetNamedSubStringValue("desc1", prtgg.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("boxno", prtxh.Text); //向bartender模板传递变量            
            btFormat.SetNamedSubStringValue("batch", boxno.Text); //向bartender模板传递变量
            

            btFormat.SetNamedSubStringValue("batchbig", s); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("date1", prtsj.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("savedate", prtrq.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("nweight", prtjz.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("bz", prtbz.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("tj", prttj.Text); //向bartender模板传递变量

            btFormat.SetNamedSubStringValue("boxqty", boxqty.Text); //向bartender模板传递变量
            btFormat.SetNamedSubStringValue("typea", comboBox1.Text); //向bartender模板传递变量

            string qr = "名称:碳纤维 牌号规格:" + prtgg.Text + " 箱号:" + prtxh.Text + " 批号:" + s + " 生产日期:" + prtsj.Text + " 贮存日期:" + prtrq.Text + " 储存条件:" + prttj.Text + " 净重:" + prtjz.Text + " x " + boxqty.Text + " 执行标准:" + prtbz.Text;
            btFormat.SetNamedSubStringValue("QR", qr); //向bartender模板传递变量
            btFormat.PrintOut(false, false); //第二个false设置打印时是否跳出打印属性


            btFormat.Close(BarTender.BtSaveOptions.btSaveChanges); //退出时是否保存标签

            Class1.killbartender();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dftprinter.SetDefaultPrinter(printerlist.Text))
            {
                MessageBox.Show("设置默认打印机成功,当前默认打印机为"+ printerlist.Text);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (infobatchbig.Text != "")
            {
                Class1.fromboxbatch = infobatchbig.Text + "-" + cls.Text;
            }
            if (rollbatch.Text != "")
            {
                Class1.fromboxbatch = rollbatch.Text+ "-"+cls.Text;
            }

            boxpick bp = boxpick.ChildFromInstanc;
            if (bp != null)
            {
                bp.Owner = this;
                bp.Show();
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            songjian sj1 = songjian.ChildFromInstanc;
            if (sj1 != null)
            {
                sj1.Owner = this;
                sj1.init1(boxno.Text);
                sj1.Show();
                
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("c:\\data\\生产记录" + boxno.Text + ".txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            int s = dataGridView2.RowCount;
            string sql = string.Format("select row_number() over (order by T1.batch) as #,T1.batch as 卷号,T1.t1 as 开始时间,T1.t2 as 结束时间,T1.c5 as 工位,T1.pro1 as 重量,T1.batchbig as 批号,T1.material as 物料,T2.desc1 as 描述,T1.len as 长度,T1.qatype as 类别 from stock T1 left join masterdata T2 on T1.material=T2.itemcode where T1.boxno='{0}'", boxno.Text);
            sw.WriteLine("箱号: " + boxno.Text + " \r\n类别: " + comboBox1.Text + " \r\n重量(KG): " + prtjz.Text + " \r\n生产日期: " + prtsj.Text + " \r\n物料明细: ");
            sw.WriteLine("#;卷号;开始时间;结束时间;工位;重量;批号;物料;描述;长度;类别;");
            for (int i = 0; i <= s - 1; i++)
            {
                sw.WriteLine(dataGridView2.Rows[i].Cells[0].Value + ";" + dataGridView2.Rows[i].Cells[1].Value + ";" + dataGridView2.Rows[i].Cells[2].Value + ";" + dataGridView2.Rows[i].Cells[3].Value + ";" + dataGridView2.Rows[i].Cells[4].Value + ";" + dataGridView2.Rows[i].Cells[5].Value + ";" + dataGridView2.Rows[i].Cells[6].Value + ";" + dataGridView2.Rows[i].Cells[7].Value + ";" + dataGridView2.Rows[i].Cells[8].Value + ";" + dataGridView2.Rows[i].Cells[9].Value + ";" + dataGridView2.Rows[i].Cells[10].Value);
            }
            sw.Close();
            fs.Close();
            MessageBox.Show("报告位置 c:\\data\\生产记录" + boxno.Text + ".txt");
        }

        private void yspc_TextChanged(object sender, EventArgs e)
        {
            string sql = string.Format("select top 1 date1 from yuansi where batch='{0}' and devicenum='{1}' and workshop='{2}' order by date1 desc",yspc.Text.Trim(),gw.Text,Class1.workshop);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                label19.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                label19.Text = "";
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cls_TextChanged(object sender, EventArgs e)
        {
            freshbatchnext();
        }

        private void freshbatchnext()
        {

            if (cls.Text != "")
            {

                string cg = "X";
                if (Class1.workshop == "ts8")
                {
                    cg = "4";
                }
                if (Class1.workshop == "ts7")
                {
                    cg = "2";
                }
                if (Class1.workshop == "ts02")
                {
                    cg = "3";
                }
                if (Class1.workshop == "ts01")
                {
                    cg = "0";
                }
                if (Class1.workshop == "ts2")
                {
                    cg = "1";
                }
                string bc = DateTime.Now.ToString("yyMMdd") + cg + "0001";

 
                    string sqlbatch = string.Format("select top 1 batch from stock where left(batch,7)='{0}' and qatype='{1}' order by batch desc", bc.Remove(7), cls.Text.Trim());
                    int flag = 0;
                    string batchn1 = DateTime.Now.ToString("yyMMdd") + cg + "0000";
                    string batchn2 = DateTime.Now.ToString("yyMMdd") + cg + "0000";
                    string batchn3 = DateTime.Now.ToString("yyMMdd") + cg + "0000";
                    DataSet ds = new DataSet();
                    ds = Class1.GetAllDataSet(sqlbatch);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {

                        batchn1 = (Convert.ToInt64(dt.Rows[0][0]) + 1).ToString();
                    }

                        for (int s = 0; s < dataGridView1.Rows.Count - 1; s++)
                        {
                            //MessageBox.Show(dataGridView1.Rows[s].Cells[0].ToString().Trim());
                            if (dataGridView1.Rows[s].Cells[0].Value.ToString().Trim() == cls.Text)
                            {
                                batchn2 = dataGridView1.Rows[s].Cells[6].Value.ToString().Trim();
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            batchn3 = DateTime.Now.ToString("yyMMdd") + cg + "0000";
                        }

                        //  
                    //成品批次生成 年年月月日日+产线+0001流水

                    long x = Convert.ToInt64(batchn1);
                    long y= Convert.ToInt64(batchn2)+1;
                    long z= Convert.ToInt64(batchn3);

                if (checkBox3.Checked == true)
                {
                    newbatch.Text = y.ToString();
                }
                else
                {
                    if (x >= y)
                    {
                        if (x >= z)
                        {
                            newbatch.Text = batchn1;
                        }
                        else
                        {
                            newbatch.Text = batchn3;
                        }

                    }
                    else
                    {
                        if (y >= z)
                        {
                            newbatch.Text = y.ToString();
                        }
                        else
                        {
                            newbatch.Text = batchn3;
                        }
                    }

                }





            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (infowl.Text != "")
            {
                 for (int c = 0; c < comboBox2.Items.Count; c++)
                {
                    string cb2x = comboBox2.Items[c].ToString().Trim();
                    string sql1 = "";
                    if (dlfw.Checked == true)
                    {
                        sql1 = string.Format("select top 1 comm2 from box where workshop='{0}' and itemcode='{1}' and boxnm='{2}'", Class1.curuser, infowl.Text, cb2x);
                    }
                    else
                    {
                        sql1 = string.Format("select top 1 comm2 from box where workshop='{0}' and itemcode='{1}' and boxnm='{2}'", Class1.workshop, infowl.Text, cb2x);
                    }
                    string start1 = "";
                    
                    //MessageBox.Show(sql1);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = Class1.GetAllDataSet(sql1);
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString().Trim() != "")
                        {
                            start1 = dt.Rows[0][0].ToString().Trim();

                        }
                        else
                        {
                            string cg = "X";
                            if (Class1.workshop == "ts8")
                            {
                                cg = "4";
                            }
                            if (Class1.workshop == "ts7")
                            {
                                cg = "2";
                            }
                            if (Class1.workshop == "ts02")
                            {
                                cg = "3";
                            }
                            if (Class1.workshop == "ts01")
                            {
                                cg = "0";
                            }
                            if (Class1.workshop == "ts2")
                            {
                                cg = "1";
                            }
                            start1 = DateTime.Now.ToString("yyMMdd") + cg + "0001";
                        }
                    }
                    else
                    {
                        string cg = "X";
                        if (Class1.workshop =="ts8")
                        {
                            cg = "4";
                        }
                        if (Class1.workshop == "ts7")
                        {
                            cg = "2";
                        }
                        if (Class1.workshop == "ts02")
                        {
                            cg = "3";
                        }
                        if (Class1.workshop == "ts01")
                        {
                            cg = "0";
                        }
                        if (Class1.workshop == "ts2")
                        {
                            cg = "1";
                        }
                        start1 = DateTime.Now.ToString("yyMMdd") +cg+ "0001";
                    }
                    string str = Interaction.InputBox("请输入 "+cb2x+" 开始卷号", "卷号起始", start1, -1, -1);
                    string sql2 = "";
                    if (dlfw.Checked == true)
                    {
                        sql2 = string.Format("update box set comm2='{3}' where workshop='{0}' and itemcode='{1}' and boxnm='{2}'", Class1.curuser, infowl.Text, cb2x, str);
                    }
                    else
                    {
                        sql2 = string.Format("update box set comm2='{3}' where workshop='{0}' and itemcode='{1}' and boxnm='{2}'", Class1.workshop, infowl.Text, cb2x, str);

                    }
                    //MessageBox.Show(sql2);
                    int cc=Class1.ExcuteScal(sql2);
                    if (cc == 1)
                    {
                        flashdataview0();
                    }
                    else
                    {

                        string sql3 = string.Format("insert into box(workshop,itemcode,boxid,boxnm,r1,r2,boxlv,reqnum,maxcon,comm2) select '{0}','{1}',boxid,boxnm,r1,r2,boxlv,'1','1','{3}' from box where workshop='{4}' and itemcode='{1}' and boxnm='{2}'", Class1.curuser, infowl.Text, cb2x, str, Class1.workshop);
                        //MessageBox.Show(sql3);
                        int cc2 = Class1.ExcuteScal(sql3);
                        if (cc2 == 1)
                        {
                            flashdataview0();
                        }
                    }

                }
            }
        }

        private void dt1_MouseMove(object sender, MouseEventArgs e)
        {
            //dt1.Focus();//获取焦点，否则选中文本不起作用
            
        }

        private void weight_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void dt2_MouseMove(object sender, MouseEventArgs e)
        {
            //dt2.Focus();

        }

        private void len_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void gw_MouseMove(object sender, MouseEventArgs e)
        {
  
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Class1.frombatchtxt = newbatch.Text;
            this.Hide();
            batchchange bcc = batchchange.ChildFromInstanc;
            if (bcc != null)
            {
                bcc.Owner = this;
                
                bcc.Show();
            }
        }

        private void gw_MouseClick(object sender, MouseEventArgs e)
        {
            gw.Focus();
            gw.Select(0,gw.Text.Length);
           
        }

        private void weight_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void tansi_hou_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                serialPort.Close();
            }
            catch
            { }
            Class1.killbartender();
            Class1.killapp();
            Application.Exit();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Class1.fromform = "ts";
            this.Hide();
            itemselect ist = itemselect.ChildFromInstanc;
            if (ist != null)
            {
                ist.Owner = this;
                ist.Show();
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string t1 = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            string t2 = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
            string c5 = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString().Trim();
            string pro1 = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
            string batchbig = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
            string material = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString().Trim();
            
            string len1 = dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString();
            string qatype = dataGridView2.Rows[e.RowIndex].Cells[10].Value.ToString();
            string stockin = dataGridView2.Rows[e.RowIndex].Cells[11].Value.ToString();

            string batch1 = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();


            string sql1 = string.Format("update stock set t1='{0}',t2='{1}',c5='{2}',pro1='{3}',batchbig='{4}',material='{5}',len='{6}',qatype='{7}',stockin='{8}' where batch='{9}' and sloc='{10}'",t1,t2,c5,pro1,batchbig,material,len1,qatype,stockin,batch1,Class1.workshop);
            //MessageBox.Show(sql1);
            int c2 = Class1.ExcuteScal(sql1);
            if (c2 == 0)
            {
                MessageBox.Show("更新失败,只能修改库位为本车间的物料");
            }
        }

        private void infof1_Enter(object sender, EventArgs e)
        {
            focusr1();
        }

        private void infof3_TextChanged(object sender, EventArgs e)
        {
            focusr2();
        }

        private void infof1_TextChanged(object sender, EventArgs e)
        {
            focusr1();
        }



        private void focusr1()
        {
            range1.BackColor = Color.Lime;
            range2.BackColor = SystemColors.Control;
            infof1.BackColor = Color.Lime;
            infof2.BackColor = Color.Lime;
            infof3.BackColor = SystemColors.Control;
        }

        private void focusr2()
        {

            range2.BackColor = Color.Lime;
            range1.BackColor = SystemColors.Control;
            infof1.BackColor = SystemColors.Control;
            infof2.BackColor = SystemColors.Control;
            infof3.BackColor = Color.Lime;

        }

        private void infof3_Enter(object sender, EventArgs e)
        {
            focusr2();
        }

        private void infof2_TextChanged(object sender, EventArgs e)
        {
            focusr1();
        }


        public void backitm()
        {

            flashdataview2();

            //查找和修改返回
            string sql = string.Format("select boxno from stock where batch='{0}'", newbatch.Text);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = Class1.GetAllDataSet(sql);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                boxno.Text = dt.Rows[0][0].ToString().Trim();
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    dataGridView2.Rows[i].Selected = false;
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString().Trim() == newbatch.Text)
                    {

                        dataGridView2.FirstDisplayedScrollingRowIndex = i;
                        dataGridView2.Rows[i].Selected = true;
                    }
                }
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            backitm();
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            pregen pg = pregen.ChildFromInstanc;
            if (pg != null)
            {
                pg.Owner = this;
                pg.Show();
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                gw.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            }
        }

        private void btstu_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (gw.Text != "")
            {
                if (btstu.Text == "开始")
                {
                    sql = string.Format("update wsdevice set status2='运行' where workshop='{0}' and devicenum='{1}'",Class1.workshop, gw.Text.Trim());
                }
                if (btstu.Text == "停止")
                {

                    sql = string.Format("update wsdevice set status2='停止' where workshop='{0}' and devicenum='{1}'", Class1.workshop, gw.Text.Trim());
                }
                int c = Class1.ExcuteScal(sql);
                if (c == 1)
                {
                    string tmp = gw.Text;
                    gw.Text = "";
                    gw.Text = tmp;
                }
            }
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {

            if (dt1.Value < Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd 00:00")))
            {
                label3.BackColor = Color.Red;
            }

            if (Convert.ToDateTime(dt1.Value.ToString("yyyy/MM/dd HH:mm")) > DateTime.Now)
            {
                label3.BackColor = Color.LightBlue;
            }

            if (dt1.Value >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00")) & Convert.ToDateTime(dt1.Value.ToString("yyyy/MM/dd HH:mm")) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm")))
            {
                label3.BackColor = Color.Yellow;
            }

            if (dt1.Value.ToString("yyyy/MM/dd HH:mm") == DateTime.Now.ToString("yyyy/MM/dd HH:mm"))
            {
                label3.BackColor = Color.LawnGreen;
            }

        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            if (dt2.Value < Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd 00:00")))
            {
                label4.BackColor = Color.Red;
            }

            if (Convert.ToDateTime(dt2.Value.ToString("yyyy/MM/dd HH:mm")) > DateTime.Now)
            {
                label4.BackColor = Color.LightBlue;
            }

            if (dt2.Value >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00")) & Convert.ToDateTime(dt2.Value.ToString("yyyy/MM/dd HH:mm")) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm")))
            {
                label4.BackColor = Color.Yellow;
            }

            if (dt2.Value.ToString("yyyy/MM/dd HH:mm") == DateTime.Now.ToString("yyyy/MM/dd HH:mm"))
            {
                label4.BackColor = Color.LawnGreen;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.Hide();
            aftergen ag = aftergen.ChildFromInstanc;
            if (ag != null)
            {
                ag.Owner = this;
                ag.Show();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Hide();
            tppick2 tpp = tppick2.ChildFromInstanc;
            if (tpp != null)
            {
                tpp.Owner = this;
                tpp.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (gw.Text != "")
            {
                this.Hide();
                popgw pw = popgw.ChildFromInstanc;
                if (pw != null)
                {
                    pw.Owner = this;
                    pw.init1(Class1.workshop + "-" + gw.Text);
                    pw.Show();
                }
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            quickrec rec1 = quickrec.ChildFromInstanc;
            if (rec1 != null)
            {
                rec1.init1();
                rec1.Owner = this;
                rec1.Show();

            }
        }

        public void inputgw(string gw1, string time1,string fx)
        {
            if (fx.Substring(0, 1) == "上")
            {
                dt1lock.Checked = true;
                dt1.Text= DateTime.Now.ToString("yyyy/MM/dd") +" "+ time1;
                gw.Text = gw1;
                start1.ForeColor = Color.Red;
                end1.ForeColor = Color.Black;
            }

            if (fx.Substring(0, 1) == "下")
            {
                dt2lock.Checked = true;
                dt2.Text = DateTime.Now.ToString("yyyy/MM/dd") + " " + time1;
                gw.Text = gw1;
                end1.ForeColor = Color.Red;
                start1.ForeColor = Color.Black;
            }

            dt1lock.Checked = false;
            dt2lock.Checked = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void prtsj_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void prtxh_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            flashdataview2();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void comboBox5_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            flashdataview2();
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (comboBox5.Text == "自由选择")
            {
                DateTime dtx = Convert.ToDateTime(dataGridView2.Rows[e.RowIndex].Cells[3].Value);
                prtsj.Text = dtx.ToString("yy.MM.dd");
                comboBox1.Text = dataGridView2.Rows[e.RowIndex].Cells[10].Value.ToString();
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            start1.Text = "上卷" + " (" + comboBox6.Text + ")";
            end1.Text = "下卷" + " (" + comboBox6.Text + ")";
        }

        private void dlfw_CheckedChanged(object sender, EventArgs e)
        {
            if (dlfw.Checked == true)
            {
                infowl.BackColor = Color.LightGreen;
                infobatchbig.BackColor = Color.LightGreen;
                checkBox3.Checked = true;
            }
            else
            {
                infowl.BackColor = SystemColors.Control;
                infobatchbig.BackColor = SystemColors.Control;
                checkBox3.Checked = false;

            }
        }
    }




}
