using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace tzgw
{
    class Class1
    {
        public static string sqlstr = "";

        //public static string sqlstr = "server=192.168.50.200\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2!!";
        //public static string sqlstr = "server=.\\SQLEXPRESS;Database=gwtz;User ID=sa;Password=sa2sa2";
        public static string itemyuansi1 = ""; //记录itemselect选择的物料号
        public static string itemtansi1 = "";
        public static string itemmu1 = ""; //addyuansi
        public static string fromform = "";//记录itemselect来源界面

        public static string companyinfo = "威海市川铭自动化科技有限公司     联系电话:13863094191";

        public static string frombatchtxt = ""; //记录去修改批次时的批次
        public static string fromboxbatch = ""; //记录去选卷时的批次
        public static string backboxbatch = ""; //记录选完返回的箱号
        public static string rg1 = "";
        public static string rg2 = "";
        public static string rg3 = "";
        public static string rg4 = "";
        public static string rg5 = "";
        public static string rg6 = "";
        public static string rg7 = "";
        public static string rg8 = "";
        public static string rg9 = "";
        public static string rg10 = "";
        public static string rg11 = "";
        public static string rg12 = "";
        public static string rg13 = "";
        public static string rg14 = "";
        public static string rg15 = "";
        public static string rg16 = "";
        public static string rg17 = "";
        public static string rg18 = "";
        public static string rg19 = "";
        public static string rg20 = "";
        public static string rg21 = "";
        public static string rg22 = "";

        public static string workshop = "";
        public static string curuser = "";
        public static string shift1 = "";

        public static int ExcuteScal(string sql)
        {
            
            SqlConnection sqlCon = new SqlConnection(Class1.sqlstr);
            int i = 0;
            try
            {
                SqlCommand sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCon.Open();
                i = sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                i = 0;
                //throw new Exception("failed to connect to server", ex);
            }
            finally
            {
                sqlCon.Close();
            }
            return i;
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static DataSet GetAllDataSet(string sql)
        {

            SqlConnection sqlCon = new SqlConnection(Class1.sqlstr);
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(sql, sqlCon);
                sqlCon.Open();
                sqlDa.Fill(ds);
            }
            catch (SqlException ex)
            {
                //throw new Exception("failed to connect to server", ex);
            }
            finally
            {
                sqlCon.Close();
            }
            return ds;
        }


        public static bool isNumeric(string value)
        {
            return Regex.IsMatch(value, "^\\d+$");
        }


        public static void killbartender()
        {

            Process[] KillmyProcess = Process.GetProcesses();
            for (int i = 0; i < KillmyProcess.Length; i++)
            {
                if (KillmyProcess[i].ProcessName.Length >= 7)
                {
                    if (KillmyProcess[i].ProcessName.ToString().Substring(0, 7) == "bartend" || KillmyProcess[i].ProcessName.ToString().Substring(0, 7) == "BarTend")
                    {
                        KillmyProcess[i].Kill();
                    }
                }
                
            }
        }


        public static void killapp()
        {

            Process[] KillmyProcess = Process.GetProcesses();
            for (int i = 0; i < KillmyProcess.Length; i++)
            {
                if (KillmyProcess[i].ProcessName.ToString().Substring(0) == "tzgw")
                {
                    KillmyProcess[i].Kill();
                }
            }
        }
    }
}
