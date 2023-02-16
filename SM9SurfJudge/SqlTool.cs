using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM9SurfJudge
{
    class SqlTool
    {
        //ExceptionHandler物件
        ExceptionHandler ExHdr;

        /// <summary>
        /// 连接数据库一些声明函数以及方法
        /// </summary>
        private NpgsqlConnection conn = null;

        public String connectionString = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlTool()
        {
            //数据库连接句柄 postgresql
            this.connectionString = @"
                Host=" + Main.para_s[2].Trim() + @";
                Port=" + Main.para_s[4].Trim() + @";
                Username=postgres;
                Password=3387523;
                Database=SM9SurfJudgeDB;";

            //连线
            this.conn = new NpgsqlConnection(this.connectionString);

            //开启资料库连线,考虑到轮询会一直使用。不需要重复打开.可能影响启动速度看要不要后期考虑删除
            //this.conn.Open();

            //建立Exception Handler物件
            this.ExHdr = new ExceptionHandler(Application.StartupPath);
        }

        public NpgsqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 执行传入的sql语句，返回影响的int行数，一般用于insert,delete,update
        /// </summary>
        /// <param name="mysql">处理SQL</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string mysql)
        {
            int result;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(mysql, GetConn());
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception EX)
            {
                ExHdr.RecordError(EX.ToString());
                result = 0;
            }
            finally
            {
                //先写，看情况这段要不要删除
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;

        }

        /// <summary>
        /// 执行传入的SQL语句，返回一个DataTable数据集
        /// </summary>
        /// <param name="mysql">处理SQL返回</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteQuery(string cmdText)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cmdText, GetConn());
                NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception EX)
            {
                ExHdr.RecordError(EX.ToString());
                return null;
            }

        }

        /// <summary>
        /// 执行传入的sql语句，用于特殊insert返回pt_result_id
        /// </summary>
        /// <param name="mysql">处理SQL</param>
        /// <returns>新增的pt_result_id</returns>
        public int ExecuteScalar(string mysql)
        {
            int res;
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(mysql, GetConn());
                res = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception EX)
            {
                res = 0;
                ExHdr.RecordError(EX.ToString());
            }
            finally
            {
                //先写，看情况这段要不要删除
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return res;

        }

        //从table pt_result中捞取当天serialNO最大流水码。假如没有即认定为从1开始
        public int GetSerialNoByDay()
        {
            int seq = 0;
            string dts = DateTime.Now.ToString("yyyy.MM.dd 00:00:00");
            string dte = DateTime.Now.ToString("yyyy.MM.dd 24:00:00");
            string mysql = "select max(serial_no_by_day) from pt_result where update_time BETWEEN '" + dts + "' AND '" + dte + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(mysql, GetConn());
            object obj = cmd.ExecuteScalar();
            if (obj != null && obj.ToString()!="")
            {
                seq = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return seq+1;
        }

    }
}
