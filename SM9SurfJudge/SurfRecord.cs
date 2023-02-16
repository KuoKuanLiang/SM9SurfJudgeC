using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM9SurfJudge
{
    public class SurfRecord
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        /// 插入数据库后的pt_result_id
        public int pt_result_id;

        /// <summary>
        /// 流水碼
        /// </summary>
        public int SerialNoByDay;  

        /// <summary>
        /// 粗糙度儀編碼
        /// </summary>
        public string judge_machine_no;

        /// <summary>
        /// 標準
        /// </summary>
        public string STD; 

        /// <summary>
        /// 樣本數
        /// </summary>
        public string N;

        /// <summary>
        /// λC (ex => 0.1in )
        /// </summary>
        public string Lambda;

        /// <summary>
        /// λS (ex => 320µin )
        /// </summary>
        public string LambdaS;

        /// <summary>
        /// 單紋路_RA
        /// </summary>
        public string SingleRA;

        /// <summary>
        /// 單紋路_RZ
        /// </summary>
        public string SingleRZ;

        /// <summary>
        /// 多紋路_RA
        /// </summary>
        public string MultipleRA;

        /// <summary>
        /// 多紋路_RZ
        /// </summary>
        public string MultipleRZ;

        /// <summary>
        /// 整體波形最左邊裁切點
        /// </summary>
        public string CutIndexL;

        /// <summary>
        /// 整體波形最右邊裁切點
        /// </summary>
        public string CutIndexR;

        /// <summary>
        /// 紋路裁取寬度(一個紋路的總寬)
        /// </summary>
        public string CutOffnum;

        /// <summary>
        /// 原始的曲線P的點群座標
        /// </summary>
        public List<MECPoint> points=new List<MECPoint>();

        /// <summary>
        /// 裁切、轉換完的曲線R
        /// </summary>
        public List<MECPoint> FinalPoints = new List<MECPoint>();

        /// <summary>
        /// 各個區段的裁切點紀錄
        /// </summary>
        public List<CropRecord> CropRecords;//如果可以紀錄索引的話，會簡單很多

        /// <summary>
        /// 資料庫寫入時間
        /// </summary>
        public string UpdateTime;  //判定插入时间

    }

    /// <summary>
    /// 點X、Z值
    /// </summary>
    public class MECPoint
    {
        /// <summary>
        /// X座標
        /// </summary>
        public decimal X { get; set; }
        /// <summary>
        /// Z座標
        /// </summary>
        public decimal Z { get; set; }
        public MECPoint()
        {
            X = 0.0m;
            Z = 0.0m;
        }
        public MECPoint(decimal _X, decimal _Z)
        {
            X = _X;
            Z = _Z;
        }
    }

    /// <summary>
    /// 單個波形資料
    /// </summary>
    public class CropRecord
    {
        /// <summary>
        /// 左裁切_座標
        /// </summary>
        public MECPoint LcropPoint { get; set; }
        
        /// <summary>
        /// 右裁切_座標
        /// </summary>
        public MECPoint RcropPoint { get; set; }

        public CropRecord()
        {
            LcropPoint = null;
            RcropPoint = null;
        }
        public CropRecord( MECPoint _LcropPoint,MECPoint _RcropPoint )
        {
            LcropPoint = _LcropPoint;
            RcropPoint = _RcropPoint;
        }
    }
}
