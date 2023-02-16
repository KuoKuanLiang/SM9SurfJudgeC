using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SM9SurfJudge
{
    /// <summary>
    /// 各种操作的类
    /// </summary>
    public class Comm
    {
        // 宣告連接埠
        public static SerialPort ComObj = new SerialPort();

        //ExceptionHandler物件
        static ExceptionHandler ExHdr = new ExceptionHandler(Application.StartupPath);

        public static int CheckMachinenoAndMac(string MachineNo)
        {
            
            SqlTool sqltool = new SqlTool();
            List<string> macs = new List<string>();
            macs = Comm.GetMacByWMI();
            int checkok = 0;   //不存在
            foreach (var mac in macs)
            {
                //既然都要找资讯人工确认，不要判断太多简单判断即可
                string checksql = "select * from JUDGE_MACHINE_LIST where JUDGE_MACHINE_NO='" + MachineNo + "' and MAC_ADDRESS='" + mac + "'";
                int res = (sqltool.ExecuteQuery(checksql)).Rows.Count;
                if (res > 0)
                {
                    checkok = 1;
                }
            }
            return checkok;

        }
        ///<summary>
        /// 通过WMI读取系统信息里的网卡MAC
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }
        /// <summary>
        /// 检测是不是数字带浮点的
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumberF(string strNumber)
        {
            Regex regex = new Regex(@"^\d+(\.\d)?$");
            return regex.IsMatch(strNumber);

        }

        /// <summary>
        /// 读取xml文件资料
        /// </summary>
        /// <param name="path">读取参数xml文件路径</param>
        /// <param name="fristNode">总节点</param>
        /// <returns></returns>
        public static string[] ReadXml(string path, string fristNode)
        {
            //初始化调用xml
            XmlDocument xd = new XmlDocument();

            //家在xml文件
            xd.Load(path);

            //声明存放参数
            string[] str = new string[xd.SelectSingleNode(fristNode).ChildNodes.Count];

            //获取所有参数
            for (int i = 0; i < xd.SelectSingleNode(fristNode).ChildNodes.Count; i++)
            {
                XmlElement xe = (XmlElement)xd.SelectSingleNode(fristNode).ChildNodes[i];
                str[i] = xe.InnerText;
            }

            //将路径字串最后的反斜杠移除
            str[0] = removeSlashAtTheEnd(str[0]);
            str[1] = removeSlashAtTheEnd(str[1]);
            str[2] = removeSlashAtTheEnd(str[2]);

            return str;
        }

        /// <summary>
        /// 删除路径字串中的斜线结尾
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String removeSlashAtTheEnd(String str)
        {
            str = str.Trim();

            if (str.Substring(str.Length - 1) == "\\")
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }

        /// <summary>
        /// 修改XML资料
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fristNode"></param>
        /// <param name="value"></param>
        public static void ModifyXml(string path, string fristNode, string[] value)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            for (int i = 0; i < value.Length; i++)
            {
                XmlElement xe = (XmlElement)xd.SelectSingleNode(fristNode).ChildNodes[i];
                xe.InnerText = value[i];
            }
            xd.Save(path);
        }

        /// <summary>
        /// 輸入指令到粗糙儀
        /// </summary>
        /// <param name="writeStr"></param>
        /// <returns></returns>
        public static string InputStr(string writeStr)
        {
            string responseStr = "";
            try
            {
                //將要發送的報文，轉成Byte Aaary
                byte[] SendStringByteAry = Encoding.ASCII.GetBytes(writeStr);

                //在最後加上ASCII的CR字元 (CR字元在C#中就是『\r』, ASCII碼是13)
                List<byte> SendStringByteLst = SendStringByteAry.ToList();
                SendStringByteLst.Add(13);
                SendStringByteAry = SendStringByteLst.ToArray();

                //發送報文
                ComObj.Write(SendStringByteAry, 0, SendStringByteAry.Length);

                //接收報文
                responseStr = ReceiveData();


            }
            catch (Exception EX)
            {
                if (EX.ToString().Contains("拒絕存取通訊埠") != true)
                {
                    MessageBox.Show("請確認已連接粗糙度儀！");
                }

                ExHdr.RecordError(EX.ToString());
            }
            return responseStr;
        }

        /// <summary>
        /// 取得粗糙儀回傳的資訊
        /// </summary>
        /// <returns></returns>
        public static string ReceiveData()
        {
            //目前己收到的內容
            string receivedString = "";

            //Buffer
            byte[] buffer = null;

            //讀到字符是ASCII的CR字元(CR字元在C#中就是『\r』, ASCII碼是13) 時結束監聽
            //即內容是符合以下條件時, 繼續監聽
            //buffer內容是null
            //或是 buffer的長度是0
            //或是 buffer的最後一個字符不是
            while (buffer == null || Encoding.Default.GetString(buffer).Length == 0 ||
                   Encoding.Default.GetString(buffer).Substring(Encoding.Default.GetString(buffer).Length - 1, 1) != "\r")
            {
                //清空Buffer
                buffer = null;

                //檢查目前在暫存區中的資料有多少Byte
                int bytes = ComObj.BytesToRead;

                //宣告一個此暫存區Byte數量的Array
                buffer = new byte[bytes];

                //由暫存區中讀取資料到此buffer Array
                ComObj.Read(buffer, 0, bytes);

                //將讀到的內容轉成字串.
                receivedString = receivedString + Encoding.ASCII.GetString(buffer);
            }

            //回傳接收到的內容
            return receivedString;
        }



    }
}