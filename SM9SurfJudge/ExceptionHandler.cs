using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM9SurfJudge
{
    /// <summary>
    /// 异常Exception输出到LOG,便于查询
    /// </summary>
    public class ExceptionHandler
    {
        //log路径
        public String LogPath;

        public ExceptionHandler(String logPath)
        {
            this.LogPath = logPath;
        }

        /// <summary>
        ///输出错误 
        /// </summary>
        /// <param name="str">错误讯息</param>
        public void RecordError(String str)
        {
            lock (new object())
            {
                StreamWriter sw = new StreamWriter(this.LogPath + "\\" + "error.txt", true);
                sw.WriteLine("Error Time(Local Machine):" + DateTime.Now.ToString());
                sw.WriteLine("Error Message:" + str);
                sw.WriteLine("=========================================================================");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
        }
    }
}
