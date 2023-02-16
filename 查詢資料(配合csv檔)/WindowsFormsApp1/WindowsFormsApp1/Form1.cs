using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //sql語句
            //select
            //--pt_result_id,
            //--update_time,
            //curve_file,
            //multiple_ra_value,
            //multiple_ra_result,
            //multiple_rz_value,
            //multiple_rz_result,
            //single_ra_value,
            //single_ra_result,
            //single_rz_value,
            //single_rz_result,
            //segment_no,
            //l_crop_x,
            //l_crop_z,
            //r_crop_x,
            //r_crop_z
            //from
            //pt_result
            //left join
            //single_curve_data
            //using (pt_result_id)


            //先把所有點資料(curve_file)先擷取出來(直到 ]", 的逗點，把逗點刪除)
            //後面就以逗點進行分割
            StreamReader sr = new StreamReader(@"D:\Project\SM9SurfJudgeC\查詢資料(配合csv檔)\資料\data-1644287605200.csv");
            string line;
            line = sr.ReadLine();//第一行為欄位標題，不用紀錄
            line = sr.ReadLine();
            
            int segment_no = 1;

            //裁切點
            List<PointXZ> cropList = new List<PointXZ>();

            while (line != null)
            {
                
                if (segment_no == 1 || segment_no == 2)
                {
                    // ,1, 索引+2，得到右邊逗點的索引
                    int start = 0;
                    if (line.IndexOf(",1,") != -1)
                    {
                        start = line.IndexOf(",1,") + 2;
                    }
                    // ,2, 索引+2，得到右邊逗點的索引
                    else
                    {
                        start = line.IndexOf(",2,") + 2;
                    }
                    line = line.Remove(0, start + 1);

                    string[] subs = line.Split(',', '\"');
                    subs = subs.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    //裁切點資料
                    cropList.Add(new PointXZ{X= Convert.ToDecimal(subs[0]),Z = Convert.ToDecimal(subs[1]) });
                    cropList.Add(new PointXZ { X = Convert.ToDecimal(subs[2]), Z = Convert.ToDecimal(subs[3]) });

                    segment_no++;
                }
                else if (segment_no == 3)
                {
                    
                    //只要在第三次的時候取點資料就好
                    int curve_file_end = line.IndexOf("]\",") + 2;//右中括號再往右2(逗點)的索引
                    string curve_file = line.Substring(0,curve_file_end);
                    line = line.Remove(0, curve_file_end + 1);
                    //處理curve_file
                    string[] xz = curve_file.Split(']', ',', '[','\"');
                    //移除陣列中的空值
                    xz = xz.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    //所有點
                    List<PointXZ> pointsList = new List<PointXZ>();
                    for (int i=0;i<xz.Count()-1; i=i+2)
                    {
                        pointsList.Add(new PointXZ { X = Convert.ToDecimal(xz[i]), Z = Convert.ToDecimal(xz[i+1]) });
                    }

                    //-------------------------------------------

                    //剩下的就能用逗點去分段了
                    string[] subs = line.Split(',', '\"');
                    subs = subs.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    
                    //裁切點資料
                    cropList.Add(new PointXZ { X = Convert.ToDecimal(subs[9]), Z = Convert.ToDecimal(subs[10]) });
                    cropList.Add(new PointXZ { X = Convert.ToDecimal(subs[11]), Z = Convert.ToDecimal(subs[12]) });

                    //------------------------------------------
                    //取得裁切後的波形

                    //裁切後的點
                    List<PointXZ> finalList = new List<PointXZ>();
                    for (int i=0; i< cropList.Count-1;i=i+2)
                    {
                        int pointL = pointsList.FindIndex(a => a.X == cropList[i].X);
                        int pointR = pointsList.FindIndex(a => a.X == cropList[i+1].X);
                        finalList.AddRange( pointsList.GetRange(pointL, pointR - pointL + 1) );
                    }
                    //建立圖表char
                    Series series = chart2.Series[0];
                    //清除原先線段上的資料
                    series.Points.Clear();
                    foreach (PointXZ point in finalList)
                    {
                        series.Points.AddXY(point.X, point.Z);
                    }
                    // 點資料圖表轉bmp
                    MemoryStream ms = new MemoryStream();
                    chart2.SaveImage(ms, ChartImageFormat.Bmp);
                    Bitmap bmp = new Bitmap(ms);

                    //----------------------------------------
                    //新增資料

                    dataGridView1.Rows.Add();
                    int rowCount = dataGridView1.Rows.Count;
                    DataGridViewRow rs = dataGridView1.Rows[rowCount - 1];

                    // Column1 波形圖
                    rs.Cells[0].Value = bmp;
                    // Column2 多ra值
                    rs.Cells[1].Value = subs[0];
                    // Column3 多ra結果
                    rs.Cells[2].Value = subs[1];
                    // Column4 多rz值
                    rs.Cells[3].Value = subs[2];
                    // Column5 多rz結果
                    rs.Cells[4].Value = subs[3];
                    // Column6 單ra值
                    rs.Cells[5].Value = subs[4];
                    // Column7 單ra結果
                    rs.Cells[6].Value = subs[5];
                    // Column8 單rz值
                    rs.Cells[7].Value = subs[6];
                    // Column9 單rz結果
                    rs.Cells[8].Value = subs[7];


                    segment_no = 1;
                    cropList = new List<PointXZ>();
                }


                line = sr.ReadLine();
            }
        }

    }
    public class PointXZ
    {
        public decimal X { get; set; }
        public decimal Z { get; set; }

    }
}
