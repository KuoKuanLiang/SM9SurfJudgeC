using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SM9SurfJudge
{
    public partial class Main : Form
    {
        
        ///// <summary>
        ///// 声明主线程
        ///// </summary>
        //private Thread ExcuteThread;
        public static string[] para_s;
        public static string[] para_p;

        //委托声明GridShow使用
        public delegate void StartFunc(ref SurfRecord surfrecord);

        //ExceptionHandler物件
        ExceptionHandler ExHdr;

        //定义postgresql连接
        SqlTool sqltool;

        //Lock锁
        private object lockObject = new object();

        //数据物件
        public SurfRecord surfrecord = new SurfRecord();
        
        /// <summary>
        /// 资料SHOWER属性
        /// </summary>
        public int pt_result_id; //GridShow 的ID

        public Shower instance;
        public Shower Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new Shower();
                }
                return instance;
            }
        }

        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 視窗功能的處理(關閉)
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            //當使用者選擇 [視窗]) (功能表中的命令時，如果使用者選擇 [最大化] 按鈕、[最小化] 按鈕、[還原] 按鈕或 [關閉] 按鈕，就會收到此訊息
            const int WM_SYSCOMMAND = 0x0112;
            //關閉視窗
            const int SC_CLOSE = 0xF060;

            //當使用者選擇 [視窗] 並且 右上的關閉按鈕被點擊
            if (msg.Msg == WM_SYSCOMMAND && ((int)msg.WParam == SC_CLOSE))
            {              
                //關閉Com口
                Comm.ComObj.Close();

                this.Close();
                this.Dispose();
            }
            base.WndProc(ref msg);
        }

        /// <summary>
        /// 快捷鍵啟用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        /// <summary>
        /// 快捷鍵未啟用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.KeyPreview = false;
        }

        //快捷鍵
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            //如果目前是塊界啟用狀態
            if (radioButton1.Checked == true)
            {
                //空白鍵被按得時候，模擬單擊開始量測與判定按鈕
                if (e.KeyCode == Keys.Space)
                {
                    //PerformClick會檢視.Enabled是否為true
                    button2.PerformClick();//button2_Click(sender,e);
                }
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            //視窗標題顯示目前版本號
            this.Text = this.Text + " (Ver : " +
                FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString() + ")";

            //建立Exception Handler物件
            this.ExHdr = new ExceptionHandler(Application.StartupPath);

            //从参数文件ParaFiles-S.XML里获取参数值
            para_s = Comm.ReadXml(Application.StartupPath + "\\ParaFile-S.XML", "Para");

            //数据库连接句柄
            sqltool = new SqlTool();

            // 中斷按鈕不可使用
            button3.Enabled = false;

            //打開Com口
            int comm = 1;
            try
            {
                if (Comm.ComObj.IsOpen == false)
                {
                    //RS-232串口連線
                    Comm.ComObj.PortName = "COM1";
                    Comm.ComObj.BaudRate = 57600;//58700
                    Comm.ComObj.StopBits = StopBits.One;
                    Comm.ComObj.Parity = Parity.None;
                    Comm.ComObj.DataBits = 8;
                    Comm.ComObj.Open();
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("請確認已連接粗糙度儀！");
                ExHdr.RecordError(EX.ToString());
                comm = 0;
            }

            
            if (comm == 1)
            {
                //粗糙度儀編碼
                string MachineNo = para_s[0];

                //check mac
                if (Comm.CheckMachinenoAndMac(MachineNo) < 1)
                {
                    MessageBox.Show("⚠『粗糙度儀編號或MAC地址不存在資料庫中，請更改系統參數設定，並由資訊部門協助确认，才能正常使用』", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //主動跳出設定視窗讓使用者設定
                    系统参数设定ToolStripMenuItem_Click(sender, e);

                    //關閉主畫面，請重新打開程式
                    this.Close();
                    this.Dispose();
                }
            }
            else
            {
                this.Close();
                this.Dispose();
            }
            

        }

        private void 系统参数设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParaSet ps = new ParaSet();
            ps.parastr = para_s;
            ps.Owner = this;
            ps.ShowDialog();
        }

        private void 产品量测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductParaSet pp = new ProductParaSet();
            pp.parastr_pp = para_s;
            pp.Owner = this;
            pp.ShowDialog();
        }

        //=============================================================================================================================================

        /// <summary>
        /// 回針按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Thread threadCTRTN = new Thread(CTRTN);
                threadCTRTN.Start();
            }
            catch (Exception ex)
            {
                ExHdr.RecordError(ex.ToString());
            }
        }

        /// <summary>
        /// 輸入指令到粗糙度儀使其回針
        /// </summary>
        private void CTRTN()
        {
            //PSA：目前側針高度
            string returnRDPSA = Comm.InputStr("RDPSA");
            double num = double.Parse(returnRDPSA.Substring(2, returnRDPSA.Length - 2));
            //目前高度有 小於或等於 測針未碰觸物體的高度值+5
            double nqwe = double.Parse(para_s[3].Trim()) + 5;
            if (num <= nqwe)
            {
                // CTRTN：回針
                string returnCTRTN = Comm.InputStr("CTRTN");
                if (returnCTRTN.Contains("NG") == true)
                {
                    MessageBox.Show("回針失敗！錯誤碼：" + returnCTRTN + " ！請聯繫資訊部門解決狀況！");
                }
                else if (returnCTRTN.Contains("OK") == true)
                {
                    textBox1.Clear();
                    textBox1.Text = "回針中......";

                    //回針時開始、中斷、回針按鈕皆不能使用
                    button1.Enabled = false;
                    button2.Enabled = false;

                    // 在回針結束的時候，粗糙度儀的狀態會變為OK000
                    while (true)
                    {
                        if (Comm.InputStr("RDSTU00").Contains("OK000") == true)
                        {
                            textBox1.Clear();
                            textBox1.Text = "完成回針";

                            button1.Enabled = true;
                            button2.Enabled = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("目前側針有接觸物體！無法回針！請抬升測針！");
            }

        }

        //=============================================================================================================================================

        /// <summary>
        /// 中斷量測按鈕
        /// </summary>
        bool button3Click = false;
        private void button3_Click(object sender, EventArgs e)
        {
            button3Click = true;
        }

        //=============================================================================================================================================

        /// <summary>
        /// 開始量測與判定按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool button2Click = false;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2Click == false)
                {
                    button2Click = true;
                    //時間戳記
                    //Console.WriteLine("按下開始按鈕：" + DateTime.Now.ToString());

                    //存取量測參數設定的資料(ISO1997...等)
                    para_p = Comm.ReadXml(para_s[1], "Para");

                    Thread threadCTSTA = new Thread(DataProcess);
                    threadCTSTA.Start();
                }
                
            }
            catch (Exception ex)
            {
                ExHdr.RecordError(ex.ToString());
            }
        }

        //=============================================================================================================================================

        /// <summary>
        /// 主流程
        /// </summary>
        private void DataProcess()
        {
            //設定成手動回針
            Comm.InputStr("WRART0");
            //打開B評估狀態"WRCOB1"
            
            //Step0: 確認B模式是否開啟
            int returnRDCOB = RDCOB();

            //Step1: 检查参数正确性
            int judgePara = 0;
            if (returnRDCOB == 1)
            {
                judgePara = parameter(ref surfrecord);
            }

            //Step2: 下量測指令開始量測
            int Measuret = 0;
            if (judgePara == 1)
            {
                Measuret = CTSTA();
            }

            //Step3: 取點資料，並判斷是否合格
            int analysis = 0;
            if (Measuret == 1)
            {
                analysis = Analysis(ref surfrecord);
            }

            //Step4: 取RA、RZ值(A評估狀態中，單個紋路的)
            if (analysis == 1)
            {
                GetRARZ(ref surfrecord);
            }

            //Step5: 执行插入数据库的动作，获取id，放入surfrecord
            int disposeData = 0;
            if (surfrecord.SingleRA != null && surfrecord.SingleRZ != null)
            {
                disposeData = DisposeData(ref surfrecord);
            }

            //Step6:处理假如有分析结果加入Grid中
            if (disposeData == 1 )
            {
                addThread(ref surfrecord);

                //放在這裡是為了避免"中斷量測"時會"自動回針"
                //設定成自動回針
                Comm.InputStr("WRART1");
                //關閉B評估狀態
                //Comm.InputStr("WRCOB0");
            }

            
            // 不管成功或失敗，所有東西都重整過
            surfrecord = new SurfRecord();
            textBox1.Clear();
            button1.Enabled = true;
            button2.Enabled = true;
            button2Click = false;
            ParaSetToolStripMenuItem.Enabled = true;
        }

        //=============================================================================================================================================

        /// <summary>
        /// Step0: 確認B模式是否開啟
        /// </summary>
        private int RDCOB()
        {
            int RDCOB = 0;
            
            // 確認 B模式是否開啟
            string returnRDCOB = Comm.InputStr("RDCOB");
            if (returnRDCOB.Contains("OK1") == true)
            {
                RDCOB = 1;
            }
            else if (returnRDCOB.Contains("OK0") == true)
            {
                MessageBox.Show("請開啟B評估狀態！");
                RDCOB = -1;
            }
            else if (returnRDCOB.Contains("NG") == true)
            {
                MessageBox.Show("判斷B模式是否開啟失敗！錯誤碼：" + returnRDCOB + " ！請聯繫資訊部門解決狀況！");
            }
            return RDCOB;
        }

        //=============================================================================================================================================

        /// <summary>
        /// Step1:检查参数正确性，並填入相關的值
        /// </summary>
        /// <returns></returns>
        private int parameter(ref SurfRecord surfrecord)
        {
            int judgePara = 0;

            try
            {
                // 系統設定按鈕不能使用
                ParaSetToolStripMenuItem.Enabled = false;

                // 取得參數條件
                string returnRDCON = Comm.InputStr("RDCON1");//RDCON0
                //時間戳記
                //Console.WriteLine("輸入取得參數設定的資料：" + DateTime.Now.ToString());
                if (returnRDCON.Contains("NG") == true)
                {
                    MessageBox.Show("取得參數失敗！錯誤碼：" + returnRDCON + " ！請聯繫資訊部門解決狀況！");
                }
                else
                {
                    textBox1.Clear();
                    textBox1.Text = "判斷參數設定中......";

                    //時間戳記
                    //Console.WriteLine("開始判斷參數設定資料：" + DateTime.Now.ToString());

                    //-------------------------------------------------------------------

                    // OK 0 3 0 2 2 3 2 05 0.500 2 2 0.003 0.019 //(實際中間是沒有空格的)
                    // 只需要前11個字元OK ~ 05
                    returnRDCON = returnRDCON.Substring(0, 11);
                    // 取得數字部分
                    Regex regex = new Regex(@"\d");
                    var num = regex.Matches(returnRDCON);
                    string[] RDCONarray = new string[4];
                    RDCONarray[0] = num[1].ToString();// 3，標準
                    RDCONarray[1] = num[7].ToString() + num[8].ToString();// 05，樣本數
                    RDCONarray[2] = num[5].ToString();// 3，λc
                    RDCONarray[3] = num[4].ToString();// 2，λs

                    //-------------------------------------------------------------------

                    // 轉換para_p為rs232指令中的代號數
                    string[] para_pnum = new string[4];

                    // 標準 STD
                    List<string> STD = new List<string>{ "JIS1982", "JIS1994", "JIS2001", "ISO1997", "ANSI", "VDA", "Free" };
                    para_pnum[0] = STD.FindIndex( a => a.Equals(para_p[0]) ).ToString();
                    surfrecord.STD = para_p[0];

                    // 樣本數 N(01、02、...、20)
                    para_pnum[1] = para_p[1];
                    surfrecord.N = para_p[1];

                    // λc LambdaC
                    List<string> LambdaC = new List<string> { "0.003in", "0.01in", "0.03in", "0.1in", "0.3in", "1in" };
                    para_pnum[2] = LambdaC.FindIndex( a => a.Equals(para_p[2]) ).ToString();
                    surfrecord.Lambda = para_p[2];

                    // λs LambdaS
                    List<string> LambdaS = new List<string> { "100µin", "320µin", "1000µin" };
                    para_pnum[3] = LambdaS.FindIndex( a => a.Equals(para_p[3]) ).ToString();
                    surfrecord.LambdaS = para_p[3];

                    //-------------------------------------------------------------------

                    //判斷參數條件是否設定正確
                    if ( RDCONarray.SequenceEqual(para_pnum) )
                    {
                        judgePara = 1;
                    }
                    else
                    {
                        MessageBox.Show("目前SJ410粗糙度儀量與設定之量測參數不一致！請重新調整量測參數！");
                        judgePara = -1;
                        return judgePara;
                    }

                    //-------------------------------------------------------------------

                    // 判斷是否可以使用自動回針

                    //OK3001000
                    //取得特定資料
                    string returnRDMCN = Comm.InputStr("RDMCN");

                    //第二個數字是我們需要的「自動返回」
                    returnRDMCN = returnRDMCN.Substring(3, 1);

                    decimal[] LambdaArray = new decimal[] { 0.003152m, 0.00985m, 0.03152m, 0.0985m, 0.3152m, 0.985m };//(in)
                    decimal totalLength = LambdaArray[int.Parse(para_pnum[2])] * int.Parse(para_pnum[1]);//(in) = λc * N
                    //2.257mm = 0.0888582676259in
                    //88998.8 uin = 0.0889988 in ≒ 0.09 in
                    // 量測總長度大於一個面，就不要自動回針
                    if (totalLength > 0.095m && returnRDMCN == "1")
                    {
                        MessageBox.Show("刮取總距離會刮到溝，請勿使用自動回針！");
                        judgePara = -1;
                    }
                }
            }
            catch (Exception EX)
            {
                ExHdr.RecordError(EX.ToString());
            }
            return judgePara;
        }

        //=============================================================================================================================================

        /// <summary>
        /// Step2:輸入指令至粗糙度儀使其開始量測
        /// </summary>
        private int CTSTA()
        {
            int OKNG = 0;
            try
            {
                // 開始量測指令
                string returnCTSTA = Comm.InputStr("CTSTA");

                //時間戳記
                //Console.WriteLine("發送開始的指令：" + DateTime.Now.ToString());

                if (returnCTSTA.Contains("NG") == true)
                {
                    if (returnCTSTA.Contains("NG020") == true)
                    {
                        MessageBox.Show("測針已到最底！請回針再重新量測！");
                    }
                    else
                    {
                        MessageBox.Show("開始失敗！錯誤碼：" + returnCTSTA + " ！請聯繫資訊部門解決狀況！");
                    }
                }
                else if (returnCTSTA.Contains("OK") == true)
                {
                    textBox1.Clear();
                    textBox1.Text = "確認測針是否有觸碰被測物......";

                    System.Threading.Thread.Sleep(2000);

                    // OK001：量測中。(可以用來確認測針是否有在物件上)(或是檢查粗糙度儀有沒有回到主畫面)
                    if (Comm.InputStr("RDSTU00").Contains("OK001") == true)
                    {
                        textBox1.Clear();
                        textBox1.Text = "量測中......";

                        // 開啟中斷按鈕以供點擊
                        button3.Enabled = true;

                        // 其餘按鈕不可使用
                        button1.Enabled = false;
                        button2.Enabled = false;

                        // 檢查是否已經量測完或按下中斷按鈕
                        while (true)
                        {
                            // 每執行1次休息1秒，這樣訊埠'COM1'就能釋放出來讓中斷的指令執行
                            //System.Threading.Thread.Sleep(1000);
                            string returnRDSTU = Comm.InputStr("RDSTU00");
                            // 正常的量測完，沒有回針，執行參數判斷部分
                            if (button3Click == false && returnRDSTU.Contains("OK005") == true)
                            {
                                OKNG = 1;
                                break;
                            }
                            // 正常的量測完，有回針，執行參數判斷部分
                            else if (button3Click == false && returnRDSTU.Contains("OK000") == true)
                            {
                                OKNG = 1;
                                break;
                            }
                            // 測針走到最底，可是還沒刮完
                            else if (button3Click == false && returnRDSTU.Contains("OK004") == true)
                            {
                                MessageBox.Show("測針已到最底！請回針再重新量測！");
                                OKNG = 0;
                                break;
                            }
                            // 量測到一半，按下中斷按鈕
                            else if (button3Click == true && returnRDSTU.Contains("OK001") == true)
                            {
                                //中斷量測指令
                                string returnCTSTP = Comm.InputStr("CTSTP");
                                if (returnCTSTP.Contains("OK") == true)
                                {
                                    textBox1.Clear();
                                    textBox1.Text = "已中斷";

                                    // 中斷按鈕重新變回未點擊狀態
                                    button3Click = false;

                                    // 確保有中斷才離開迴圈
                                    OKNG = 0;
                                    break;
                                }
                            }
                        }

                        // 中斷按鈕重新變為不能使用
                        button3.Enabled = false;
                    }
                    else
                    {
                        //OK000
                        MessageBox.Show("測針未接觸被測物！或粗糙度儀未回到主畫面！");
                        OKNG = 0;
                    }
                }

            }
            catch (Exception EX)
            {
                ExHdr.RecordError(EX.ToString());
            }
            return OKNG;
        }

        //=============================================================================================================================================

        /// <summary>
        /// Step3: 取點資料，並且邏輯判定分析(围绕surfrecord来传递)
        /// </summary>
        /// <returns></returns>
        private int Analysis(ref SurfRecord surfrecord)
        {
            int analysis = 0;
            try
            {
                textBox1.Clear();
                textBox1.Text = "取點資料中......";
            
                // RDEVA000：回傳點的數量。(代表聲明要開始讀取點資料)
                string numPstr = Comm.InputStr("RDEVA100");//RDEVA000
                //過濾出總共點數
                Regex regex = new Regex(@"\d+");
                var numPregex = regex.Matches(numPstr);
                //檢查目前總共要取幾次點資料
                int numPint = int.Parse(numPregex[0].ToString());
                int numP = 0;
                if (numPint % 50 != 0)
                {
                    numP++;
                }
                numP = numP + (numPint / 50);


                //時間戳記
                //Console.WriteLine("開始取點資料：" + DateTime.Now.ToString("hh:mm:ss.fff"));

                //取得點資料，1次可以取得50個點
                string allPointsStr = "";
                for (int i = 1; i <= numP; i++)
                {
                    string returnRDEVA = Comm.InputStr("RDEVA150");//RDEVA050
                    allPointsStr = allPointsStr + returnRDEVA;
                    //Console.WriteLine("取第"+i+ "次：" + DateTime.Now.ToString("hh:mm:ss.fff"));
                }

                //時間戳記
                //Console.WriteLine("完成取點資料，進行點資料整理：" + DateTime.Now.ToString("hh:mm:ss.fff"));

                //檢查目前x間距要為多少(in)
                var tupleList = new List<(string, decimal)> { ("1000µin", 0.0001968503937m), ("320µin", 0.00003937007874m), ("100µin", 0.00001968503937m) };
                decimal numX = tupleList.Find(a => a.Item1 == para_p[3]).Item2;
                
                // 取得並建立點資料
                List<MECPoint> pointsXZ = new List<MECPoint>();
                decimal pointsX = 0M;
                //取得數字部分(um)
                Regex regex2 = new Regex(@"(|-)\d*\.\d*");
                var pointsZ = regex2.Matches(allPointsStr);
                for (int j = 0; j < pointsZ.Count; j++)
                {
                    // um轉uin
                    // 1mm = 1000um => 1um = 0.001mm
                    // 1mm = 0.0393700787in => 0.001mm = 0.0000393700787in
                    // 1in = 1000000uin => 0.0000393700787in = 39.3700787uin
                    // 結論：1um = 39.3700787uin 
                    // X(in),Z(uin)
                    pointsXZ.Add(new MECPoint { X = pointsX, Z = Convert.ToDecimal(pointsZ[j].ToString()) * 39.37007874015748m });
                    pointsX = pointsX + numX;
                }

                //輸入點資料
                surfrecord.points = pointsXZ;

                //------------------------------------------------------------------

                // 點資料判斷
                textBox1.Clear();
                textBox1.Text = "分析數據中......";

                //時間戳記
                //Console.WriteLine("完成點資料整理，開始點資料判斷：" + DateTime.Now.ToString("hh:mm:ss.fff"));

                //每個紋路藥材取的總寬度(in)，目前是0.075
                surfrecord.CutOffnum = para_s[5];

                //點資料判斷
                Algorithm data = new Algorithm();
                surfrecord = data.AlgorithmStep(ref surfrecord);

                //時間戳記
                //Console.WriteLine("完成點資料判斷：" + DateTime.Now.ToString("hh:mm:ss.fff"));

                //有值表示演算法順利完成
                if (surfrecord.MultipleRA != null && surfrecord.MultipleRZ != null)
                {
                    analysis = 1;
                }
                else
                {
                    MessageBox.Show("點資料判斷問題,請聯繫資訊部門解決");
                    analysis = -1;
                }

            }
            catch (Exception EX)
            {
                ExHdr.RecordError(EX.ToString());
            }
            return analysis;
        }

        //=============================================================================================================================================

        /// <summary>
        /// STEP4：取得SJ410中A模式的ra、rz值(單個紋路的)
        /// </summary>
        private void GetRARZ(ref SurfRecord surfrecord)
        {
            // 取A評估狀態的rarz
            textBox1.Clear();
            textBox1.Text = "取得A評估狀態的rarz中......";

            //取得A模式有多少演算數值可讀取
            string returnRDPAR = Comm.InputStr("RDPAR0");
            Regex regex = new Regex(@"\d+");
            var numPARstr = regex.Matches(returnRDPAR);
            int index = int.Parse(numPARstr[0].ToString());

            for (int i = 1; i <= index; i++)
            {
                //取得指定數值的值
                string returnRDRES = Comm.InputStr("RDRES002,0" + i.ToString() + ",00");
                if (returnRDRES.Contains("NG") == true)
                {
                    MessageBox.Show("取得數值失敗！錯誤碼：" + returnRDRES + " ！請聯繫資訊部門解決狀況！");
                }
                else if (returnRDRES.Contains("OK") == true)
                {
                    Regex regex2 = new Regex(@"(|-)\d*\.\d*");
                    var numRDRESstr = regex2.Matches(returnRDRES);

                    //取得Ra值 
                    if (returnRDRES.Contains("Ra") == true)
                    {
                        surfrecord.SingleRA = numRDRESstr[0].ToString();
                    }
                    //取得Rz值 
                    else if (returnRDRES.Contains("Rz") == true)
                    {
                        surfrecord.SingleRZ = numRDRESstr[0].ToString();
                    }
                }
            }

            if (surfrecord.SingleRA == null || surfrecord.SingleRZ == null)
            {
                MessageBox.Show("未順利取得A評估狀態的RA、RZ值！請檢查A評估狀態設定！");
            }

        }

        //=============================================================================================================================================

        /// <summary>
        /// STEP5：新資料新增到資料庫中
        /// </summary>
        /// <param name="surfrecord"></param>
        private int DisposeData(ref SurfRecord surfrecord)
        {
            // 取A評估狀態的rarz
            textBox1.Clear();
            textBox1.Text = "存取至資料庫中......";

            int OKNG = 0;
            try
            {
                //時間戳記
                //Console.WriteLine("開始上傳資料庫：" + DateTime.Now.ToString());
                
                //--------------------------------------------------------------------------
                //調入資料處理

                sqltool = new SqlTool();

                //取得目前今日最大的流水碼
                surfrecord.SerialNoByDay = sqltool.GetSerialNoByDay();

                // 判定時間
                surfrecord.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                
                //點資料處理 surfrecord.points
                string points = "";
                foreach (MECPoint p in surfrecord.points)
                {
                    points += "[" + p.X + "," + p.Z + "]";
                }

                // 單RA判斷結果
                string SingleRA_result = "";
                if (double.Parse(surfrecord.SingleRA) < 180)
                {
                    SingleRA_result = "OK";
                }
                else 
                {
                    SingleRA_result = "NG";
                }

                // 單RZ判斷結果
                string SingleRZ_result = "";
                if (double.Parse(surfrecord.SingleRZ) < 700)
                {
                    SingleRZ_result = "OK";
                }
                else 
                {
                    SingleRZ_result = "NG";
                }

                // 多RA判斷結果
                string MultipleRA_result = "";
                if (double.Parse(surfrecord.MultipleRA) < 175)
                {
                    MultipleRA_result = "OK";
                }
                else
                {
                    MultipleRA_result = "NG";
                }

                // 多RZ判斷結果
                string MultipleRZ_result = "";
                if (double.Parse(surfrecord.MultipleRZ) < 900)
                {
                    MultipleRZ_result = "OK";
                }
                else
                {
                    MultipleRZ_result = "NG";
                }

                //__________________________________________________________________________________
                // 上傳資料到pt_result

                int pt_result_id;
                int pt_result_id_return = 0;
                string returnSql = "insert into pt_result (" +
                                    "judge_machine_no," +
                                    "serial_no_by_day," +
                                    "curve_file," +
                                    "para_l," +
                                    "para_n," +
                                    "para_std," +
                                    "para_ls," +
                                    "update_time," +
                                    "single_ra_value," +
                                    "single_rz_value," +
                                    "single_ra_result," +
                                    "single_rz_result," +
                                    "multiple_ra_value," +
                                    "multiple_rz_value," +
                                    "multiple_ra_result," +
                                    "multiple_rz_result," +
                                    "cut_index_l,"+
                                    "cut_index_r," +
                                    "cutoff_width " +
                                    ")Values( " +
                                    " '" + para_s[0] + "'," +
                                    surfrecord.SerialNoByDay + ",'" +
                                    points + "','" +
                                    surfrecord.Lambda + "','" +
                                    surfrecord.N + "','" +
                                    surfrecord.STD + "','" +
                                    surfrecord.LambdaS + "','" +
                                    surfrecord.UpdateTime + "','" +
                                    surfrecord.SingleRA + "','" +
                                    surfrecord.SingleRZ + "','"+
                                    SingleRA_result + "','" +
                                    SingleRZ_result + "','" +
                                    surfrecord.MultipleRA + "','" +
                                    surfrecord.MultipleRZ + "','" +
                                    MultipleRA_result + "','" +
                                    MultipleRZ_result + "'," +
                                    surfrecord.CutIndexL + "," +
                                    surfrecord.CutIndexR + ",'" +
                                    surfrecord.CutOffnum + "' " +
                                    ") returning pt_result_id";

                pt_result_id = sqltool.ExecuteScalar(returnSql);
                pt_result_id_return = pt_result_id;

                // 記錄儲存置資料庫時所產生的id
                surfrecord.pt_result_id = pt_result_id;

                //__________________________________________________________________________________
                // 上傳資料到single_curve_data

                int up_single_curve_data_return = 0;
                //確保有裁切點
                if (surfrecord.CropRecords.Count() != 0)
                {
                    int i = 1;
                    foreach (CropRecord s in surfrecord.CropRecords)
                    {
                        string up_single_curve_data = "insert into single_curve_data(" +
                                                      "pt_result_id," +
                                                      "segment_no," +
                                                      "l_crop_x," +
                                                      "l_crop_z," +
                                                      "r_crop_x," +
                                                      "r_crop_z" +
                                                      ")Values(" +
                                                      pt_result_id + "," +
                                                      i + "," +
                                                      s.LcropPoint.X + "," +
                                                      s.LcropPoint.Z + "," +
                                                      s.RcropPoint.X + "," +
                                                      s.RcropPoint.Z + ")";
                        up_single_curve_data_return = sqltool.ExecuteNonQuery(up_single_curve_data);
                        i++;
                    }
                }
                else
                {
                    up_single_curve_data_return = 1;
                }

                //__________________________________________________________________________________

                //確認都有正常上傳
                if (up_single_curve_data_return != 0 && pt_result_id_return != 0)
                {
                    OKNG = 1;
                }
                else
                {
                    MessageBox.Show("插入數據庫問題,請聯繫資訊部門解決");
                }
            }
            catch (Exception ex)
            {
                ExHdr.RecordError(ex.ToString());
            }
            return OKNG;
        }

        //=============================================================================================================================================

        /// <summary>
        /// (STEP6前置動作) grid建立新資料時，判斷是否使用委派
        /// </summary>
        /// <param name="surfrecord">记录内容</param>
        private void addThread(ref SurfRecord surfrecord)
        {
            //如果目前執行續在處理別的執行續
            if (grid_Show.InvokeRequired)
            {
                StartFunc func = new StartFunc(addRecord);

                //那就使用非同步執行
                this.BeginInvoke(func, surfrecord);
            }
            else
            {
                addRecord(ref surfrecord);
            }
        }

        //=============================================================================================================================================

        /// <summary>
        /// STEP6：在grid建立新的一筆資料
        /// </summary>
        /// <param name="record">记录内容</param>
        /// <param name="type">显示类型</param>
        private void addRecord(ref SurfRecord surfrecord)
        {
            lock (lockObject)
            {
                //時間戳記
                //Console.WriteLine("開始資料顯示在表單中：" + DateTime.Now.ToString());
                
                //缓冲时间
                System.Threading.Thread.Sleep(100);

                //添加一空白列
                grid_Show.Rows.Add();

                int rowCount = grid_Show.Rows.Count;
                
                //要填入列的內容
                DataGridViewRow rs = grid_Show.Rows[rowCount - 1];

                //----------------------------------------------

                // 第一欄位：唯一id
                rs.Cells[0].Value = surfrecord.pt_result_id;

                // 第二欄位：判定時間
                rs.Cells[1].Value = surfrecord.UpdateTime;

                // 第三欄位：流水碼
                rs.Cells[2].Value = surfrecord.SerialNoByDay;

                // 第四欄位：波形圖
                rs.Cells[3].Value = PointToBmp(ref surfrecord);

                // 第五欄位：多RA
                rs.Cells[4].Value = surfrecord.MultipleRA;

                // 第六欄位：多RA結果
                if (decimal.Parse(surfrecord.MultipleRA) < 175)
                {
                    rs.Cells[5].Value = Properties.Resources.OK;
                }
                else
                {
                    rs.Cells[5].Value = Properties.Resources.NC;
                }

                // 第七欄位：多RZ
                rs.Cells[6].Value = surfrecord.MultipleRZ;

                // 第八欄位：多RZ結果
                if (decimal.Parse(surfrecord.MultipleRZ) < 900)
                {
                    rs.Cells[7].Value = Properties.Resources.OK;
                }
                else
                {
                    rs.Cells[7].Value = Properties.Resources.NC;
                }

                // 第九欄位：單RA
                rs.Cells[8].Value = surfrecord.SingleRA;

                // 第十欄位：單RA結果
                if (double.Parse(surfrecord.SingleRA) < 180)
                {
                    rs.Cells[9].Value = Properties.Resources.OK;
                }
                else
                {
                    rs.Cells[9].Value = Properties.Resources.NC;
                }

                // 第十一欄位：單RZ
                rs.Cells[10].Value = surfrecord.SingleRZ;

                // 第十二欄位：單RZ結果
                if (double.Parse(surfrecord.SingleRZ) < 700)
                {
                    rs.Cells[11].Value = Properties.Resources.OK;
                }
                else
                {
                    rs.Cells[11].Value = Properties.Resources.NC;
                }

                //--------------------------------------------------------

                //使卷軸會自動聚焦在最新的一行資料(相當於自動卷軸)
                grid_Show.FirstDisplayedScrollingRowIndex = grid_Show.Rows.Count - 1;

                //時間戳記
                //Console.WriteLine("完成資料顯示在表單中：" + DateTime.Now.ToString() + "-------------2");

            }
        }

        //=============================================================================================================================================

        /// <summary>
        /// 取得點資料，輸入進圖表，轉成map格式
        /// </summary>
        /// <param name="surfrecord"></param>
        /// <returns></returns>
        private Bitmap PointToBmp(ref SurfRecord surfrecord)
        {
            // 設定使用的線段
            Series series = chart1.Series[0];
            //清除原先線段上的資料
            series.Points.Clear();
            foreach (MECPoint p in surfrecord.FinalPoints)
            {
                series.Points.AddXY(p.X, p.Z);
            }
            
            //_______________________________________________

            // 點資料圖表轉bmp
            MemoryStream ms = new MemoryStream();
            chart1.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bmp = new Bitmap(ms);

            return bmp;
        }

        //=============================================================================================================================================

        /// <summary>
        /// 當grid的波形圖欄位被雙擊時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Show_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //SHOW详细波形图到shower上
            if (this.grid_Show.CurrentRow == null)
            {
                MessageBox.Show("请双击有效记录！");
            }
            else
            {
                lock (new object())
                {
                    Shower qr = this.Instance;
                    try
                    {
                        if (grid_Show.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "Bmp")
                        {
                            //传参 Gridview的ID值
                            pt_result_id = int.Parse(grid_Show.Rows[e.RowIndex].Cells[0].Value.ToString());

                            qr.pt_result_id = pt_result_id;

                            qr.ShowDialog();
                        }

                    }
                    catch (Exception ex)
                    {
                        ExHdr.RecordError(ex.ToString());

                        qr.ShowDialog();
                    }
                }
            }
        }

        //=============================================================================================================================================

    }
}
