using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM9SurfJudge
{
    class Algorithm
    {
        public SurfRecord AlgorithmStep(ref SurfRecord surfrecord)
        {
            //__________________________________________________________________________________________________________

            // 接收完整點資料// X(in),Z(uin)
            List<MECPoint> pointsList = surfrecord.points;

            //複製一份
            List<MECPoint> orgList = new List<MECPoint>();
            pointsList.ForEach(a => orgList.Add( new MECPoint { X = a.X, Z = a.Z } ) );

            //__________________________________________________________________________________________________________

            // 裁切掉溝

            // 裁切掉溝，並建立分段陣列 (未連接)
            List<List<MECPoint>> partsList = new List<List<MECPoint>>();
            decimal scope = Convert.ToDecimal(surfrecord.CutOffnum);
            partsList = cutoff(orgList, scope);

            //__________________________________________________________________________________________________________

            // 只取z值(uin)，建立陣列，給P轉R的方法使用，並記錄裁切點

            // 取z值(uin)
            List<decimal> ZList = new List<decimal>();
            partsList.ForEach(a => a.ForEach( b => ZList.Add(b.Z) ) );

            // 紀錄裁切點
            List<CropRecord> cropList = new List<CropRecord>();
            partsList.ForEach(a => cropList.Add(new CropRecord { LcropPoint = a.First(), RcropPoint = a.Last() })) ;
            surfrecord.CropRecords = cropList;

            //__________________________________________________________________________________________________________

            // P轉R

            // 計算 K 值 ： | 2 * k + 1 - Cn * Nc | = 最小
            // Nc：每一個Lc中有幾個點
            // Cn：固定的常數 0.2275

            int Nc = pointsList.Count() / Convert.ToInt16( surfrecord.N );
            int K = Convert.ToInt16( Math.Round( Math.Abs( ( Nc * 0.2275m - 1 ) / 2 ) ) );  //K = 284
            
            //P轉R公式
            decimal[] R = changeF(ZList, K);

            //__________________________________________________________________________________________________________

            // 重新加上X值，並連接起來

            List<MECPoint> RList = new List<MECPoint>();
            for (int i = 0; i < R.Length; i++)
            {
                RList.Add(new MECPoint { X = pointsList[i].X, Z = R[i] });
            }

            //__________________________________________________________________________________________________________

            // 取得 λc 的值

            // λc 的種類
            var tupleList = new List<(string,decimal)>{("0.003in", 0.003m),("0.01in", 0.01m),("0.03in", 0.03m), ("0.1in", 0.1m), ("0.3in", 0.3m), ("1in", 1m) };
            string strLc = surfrecord.Lambda;
            // 取得 λc 的值
            decimal Lc = tupleList.Find(a => a.Item1 == strLc).Item2;

            //__________________________________________________________________________________________________________

            // 裁切最左右不要的點

            //最左裁切點
            int Sr = 0;
            // 如果1個 Lc大於 第一分段的最後一點的下一個點， 那最左裁切點為 第一分段的最後一點的下一個點
            if (RList.FindIndex(a => a.X >= Lc) >= partsList.First().Count())
            {
                Sr = partsList.First().Count();
            }else
            {
                Sr = RList.FindIndex(a => a.X >= Lc);
            }

            //------------------------------------------

            // 暫時的最左邊裁切點
            int tempEr = RList.FindIndex(a => a.X >= RList[Sr].X + Lc * 1);
            //最左邊裁切點
            int Er = tempEr;
            int number = 1;
            // 從最左裁切點往右，看可以容納幾個 Lc，就處理幾個 Lc，並且 最右邊被裁切掉的那些點數量必須大於k+1
            while (tempEr != -1 && RList.Count() - tempEr > K)
            {
                Er = tempEr;
                tempEr = RList.FindIndex(a => a.X >= RList[Sr].X + Lc * number);
                number++;
            }

            //------------------------------------------

            // 裁切掉
            RList = RList.GetRange(Sr, Er - Sr);

            // 紀錄最左右要裁切的點
            surfrecord.CutIndexL = Sr.ToString();
            surfrecord.CutIndexR = Er.ToString();
            
            //______________________________________________________________________________________________

            // 移動中心線，使中心線接近 Y = 0 

            // Z的算術平均數
            decimal avgR = RList.Average(a => a.Z);
            //每個點的Z減去算術平均數
            RList.ForEach(a => a.Z = a.Z- avgR);

            //紀錄最後轉出的曲線R
            surfrecord.FinalPoints = RList;

            //______________________________________________________________________________________________

            // 計算RARZ

            // 此計算公式需要 X(in)、Z(uin)
            Tuple<decimal, decimal> obj = rarz(RList, Lc);
            // 紀錄rarz
            surfrecord.MultipleRA = obj.Item1.ToString();
            surfrecord.MultipleRZ = obj.Item2.ToString();

            //______________________________________________________________________________________________
            
            return surfrecord;
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// 裁切掉溝，並回傳隔開的紋路區段陣列資料
        /// </summary>
        /// <param name="list">X(in)、Z(uin)</param>
        /// <returns></returns>
        public List<List<MECPoint>> cutoff( List<MECPoint> lineList , decimal scope)
        {
            //要回傳的陣列
            List<List<MECPoint>> cutoffList = new List<List<MECPoint>>();

            //____________________________________________________________________

            //備份原始波形
            List<MECPoint> backupList = new List<MECPoint>();
            lineList.ForEach(a => backupList.Add(new MECPoint { X = a.X, Z = a.Z }));

            //____________________________________________________________________

            //去除0以下的點
            lineList.RemoveAll(a => a.Z < 0);

            //____________________________________________________________________

            //建立分段的陣列資料
            List<MECPoint> tempList = new List<MECPoint>();
            for (int i = 0; i < lineList.Count() - 1; i++)
            {
                tempList.Add(new MECPoint { X = lineList[i].X, Z = lineList[i].Z });
                //如果兩個點之間超過Lc，那就加入分段
                if (lineList[i + 1].X - lineList[i].X > backupList[2].X)
                {
                    //重整陣列
                    cutoffList.Add(new List<MECPoint>(tempList));
                    tempList.Clear();
                }
                //將最後一段也加入
                if (i == lineList.Count() - 2)
                {
                    tempList.Add(new MECPoint { X = lineList[i + 1].X, Z = lineList[i + 1].Z });
                    cutoffList.Add(new List<MECPoint>(tempList));
                }
            }

            //_____________________________________________________________________________________________

            //處理中間的分段 

            //向左、右各加上(scope / 2)(in)的距離(此距離已經有再往內縮一些，使裁切點更接近ttl的裁切點)，並找出最接近的點
            for (int i = 1; i < cutoffList.Count() - 1; i++)
            {
                //第一、最後的點x相加/2，得到中間值
                decimal min = (cutoffList[i].First().X + cutoffList[i].Last().X) / 2;
                //左
                int Li = cutoffList[i].FindIndex(a => a.X >= min - (scope / 2) );
                //右
                int Ri = cutoffList[i].FindLastIndex(a => a.X <= min + (scope / 2) );

                //更新分段的點資料
                cutoffList[i] = cutoffList[i].GetRange(Li, Ri - Li);
            }


            //_____________________________________________________________________________________________

            //處理第一、最後的分段

            // 每個分段之間的距離總和(最後會平均)
            decimal dis = 0;
            // 累加次數
            int disnum = 0;
            //取得每個分段之間的平均距離。(可以想成溝距)
            for (int i = 1; i < cutoffList.Count() - 2; i++)
            {
                dis = dis + cutoffList[i + 1].First().X - cutoffList[i].Last().X;
                disnum++;
            }
            //平均
            dis = dis / disnum;

            // 先處理最後分段，因刪除時邏輯比較方便，並且最後、第一分段之間的邏輯，算是相反過來的
            //------------------------------------------------------------------------------------

            // 最後分段

            // 預計左邊起點 值 (從倒數第二分段的最後一個點 + dis)
            decimal lastnumL = cutoffList[cutoffList.Count() - 2].Last().X + dis;
            // 右邊起點 索引 (預設是最後一段的最後一個點)
            int lastintR = cutoffList.Last().Count() - 1;
            // 如果 預計左邊起點 小於最後一個點，那就開始找右邊起點
            if (lastnumL < cutoffList.Last().Last().X)
            {
                // 左邊起點 索引 
                int lastintL = cutoffList.Last().FindIndex(a => a.X >= lastnumL);

                // 左邊起點實際的值
                decimal lastfindnumL = cutoffList.Last()[lastintL].X;
                // 如果左邊起點 + scope 的值，小於最後一個點，那就找出最接近的點
                if (lastfindnumL + scope < cutoffList.Last().Last().X)
                {
                    lastintR = cutoffList.Last().FindLastIndex(a => a.X <= lastfindnumL + scope);
                }
                // 如果大於的話，那表示最後一個點就是右邊起點，也就是預設值

                // 更新分段的點資料
                cutoffList[cutoffList.Count() - 1] = cutoffList.Last().GetRange(lastintL, lastintR - lastintL);
            }
            // 如果 大於，那此分段要刪除
            else
            {
                cutoffList.RemoveAt(cutoffList.Count() - 1);
            }

            //------------------------------------------------------------------------------------

            // 第一分段

            // 預計右邊起點 值 (從第二個分段的第一個點 - dis)
            decimal firstnumR = cutoffList[1].First().X - dis;
            // 左邊起點 索引 (預設是第一分段的第一個點)
            int firstintL = 0;
            // 如果 預計右邊起點 大於第一個點，那就開始找左邊起點
            if (firstnumR > cutoffList.First().First().X)
            {
                // 右邊起點 索引
                int firstintR = cutoffList.First().FindLastIndex(a => a.X <= firstnumR);

                // 右邊起點實際的值
                decimal firstfindnumR = cutoffList.First()[firstintR].X;
                // 如果右邊起點 - scope 的值，大於第一個點，那就找出最接近的點
                if (firstfindnumR - scope > cutoffList.First().First().X)
                {
                    firstintL = cutoffList.First().FindIndex(a => a.X >= firstfindnumR - scope);
                }
                // 如果小於的話，那表示第一個點就是左邊起點，也就是預設值

                // 更新分段的點資料
                cutoffList[0] = cutoffList.First().GetRange(firstintL, firstintR - firstintL);
            }
            // 如果 大於，那此分段要刪除
            else
            {
                cutoffList.RemoveAt(0);
            }


            return cutoffList;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// P 轉 R 公式 
        /// </summary>
        /// <param name="X">單位 uin</param>
        /// <param name="K"> 絕對值( 2 * k + 1 - Cn * Nc ) = 最小 </param>
        /// <returns></returns>
        public decimal[] changeF(List<decimal> X, int K)
        {
            decimal[] R = new decimal[X.Count()];// 粗糙度輪廓
            decimal[] M = new decimal[X.Count()];// 過濾平均線

            //______________________________________________________________________________________________

            for (int i = 0; i < X.Count(); i++)
            {
                M[i] = X[i];
            }

            //______________________________________________________________________________________________

            for (int n = 1; n <= 4; n++)
            {

                int J = K + 1;
                R[J] = 0;
                for (int a = J - K - 1; a < J + K; a++) // 0 ~ 最後一個點
                {
                    R[J] = R[J] + M[a];
                }
                for (J = (K + 1); J < X.Count() - (K + 1); J++) // 第K點 ~ 最後一個點減去(K+1)個點 (左右兩邊 (K+1) 個點不處理) ==> 是為了讓下方 M[J + K] - M[J - K - 1] 不會報錯
                {
                    R[J] = R[J - 1] + M[J + K] - M[J - K - 1];
                }

                //------------------------------------------------------

                J = K + 1;
                M[J] = 0;
                for (int c = J - K - 1; c < J + K; c++) // 0 ~ 最後一個點
                {
                    M[J] = M[J] + R[c];
                }
                for (J = (K + 1); J < X.Count() - (K + 1); J++) // 第K點 ~ 最後一個點減去(K+1)個點 (左右兩邊 (K+1) 個點不處理) ==> 是為了讓下方  R[J + K] - R[J - K - 1] 不會報錯
                {
                    M[J] = M[J - 1] + R[J + K] - R[J - K - 1];
                }

            }

            //______________________________________________________________________________________________

            for (int i = 0; i < X.Count() - 0; i++)
            {
                M[i] = M[i] / (2 * K + 1) / (2 * K + 1) / (2 * K + 1) / (2 * K + 1);
                M[i] = M[i] / (2 * K + 1) / (2 * K + 1) / (2 * K + 1) / (2 * K + 1);
                R[i] = X[i] - M[i];
            }

            return R;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// rarz計算公式
        /// </summary>
        /// <param name="connectList">X(in)、Z(uin)</param>
        /// <param name="Lc">λc(in)</param>
        /// <returns></returns>
        public Tuple<decimal, decimal> rarz(List<MECPoint> connectList, decimal Lc)
        {
            int startCutOff = 0;
            int endCutOff = 0;
            int indexCutOff = 0;
            decimal sumAGE = 0;
            decimal sumDIF = 0;
            while (endCutOff != connectList.Count - 1)
            {
                indexCutOff++;

                //此區段的最後一個X
                //sj410的要設定如下
                //0.0985 -> λc = 0.1
                //0.03152 -> λc = 0.03
                //sv3100的要設定如下
                //0.1 -> λc = 0.1
                //0.03 -> λc = 0.03
                decimal temp = connectList[0].X + (Lc * indexCutOff);

                //大於等於此區段最後一個X的點的索引
                endCutOff = connectList.FindIndex(a => a.X >= temp);
                //應該要尋找最近的?

                //-1表示已經到點資料的結尾了
                if (endCutOff == -1)
                {
                    //=最後一個點的索引
                    endCutOff = connectList.Count - 1;
                }

                //--------------------------------
                //區段RA
                decimal sum = 0;
                //加總此區間的 Z 值
                for (int i = startCutOff; i <= endCutOff; i++)
                {
                    sum = sum + Math.Abs(connectList[i].Z);
                }
                //計算此區間算術平均
                decimal age = sum / (endCutOff - startCutOff + 1);
                //將每個區間的算術平均加總
                sumAGE = sumAGE + age;

                //----------------------------------
                //區段RZ
                //此區間最大值
                decimal max = connectList.GetRange(startCutOff, endCutOff - startCutOff + 1).Max(a => a.Z);
                //此區間最小值
                decimal min = connectList.GetRange(startCutOff, endCutOff - startCutOff + 1).Min(a => a.Z);
                //此區間最大最小的差值
                decimal difference = Math.Abs(max - min);
                //將每個區間的最大最小差值給加總
                sumDIF = sumDIF + difference;

                //------------------------------------

                startCutOff = endCutOff + 1;
                //Console.WriteLine("RA " + age + "/// RZ " + difference);
            }
            //答案
            decimal RA = sumAGE / indexCutOff;
            decimal RZ = sumDIF / indexCutOff;

            return Tuple.Create(RA, RZ);
        }

    }
}
