using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace RZXK.Common
{
    public class DataClass_Mysql
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataClass_Mysql()
        {
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionStringSQL
        {
            get
            {
                return Convert.ToString(ConfigurationManager.ConnectionStrings["MySqlStr"]);
            }
        }
        /// <summary>
        /// 返回连接对象
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionStringSQL);
        }

        /// <summary>
        /// 根据条件返回一下Reader
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string strSQL)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;


            try
            {
                conn = GetConnection();
                conn.Open();
                cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandTimeout = 600000;
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (System.Exception e)
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                throw (e);
            }

        }
        /// <summary>
        /// 根据查询条件，返回一个scalar集,返回第一行,第一列数据
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string strSQL)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;

            object oRet = null;

            try
            {
                conn = GetConnection();
                conn.Open();
                cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandTimeout = 600000;
                oRet = cmd.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                throw (e);
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (cmd != null)
                    cmd.Dispose();
            }
            return oRet;
        }

        /// <summary>
        /// 返回一个dataset对象
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="strConnectionString"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string strSQL, string strConnectionString)
        {
            MySqlConnection conn = null;
            MySqlDataAdapter dapt = null;
            DataSet ds = new DataSet();

            try
            {
                conn = new MySqlConnection(strConnectionString);
                conn.Open();
                dapt = new MySqlDataAdapter(strSQL, conn);
                dapt.Fill(ds, "table1");
            }
            catch (System.Exception e)
            {
                throw (e);
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (dapt != null)
                    dapt.Dispose();
            }

            return ds;


        }

        /// <summary>
        /// 返回一个dataset
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string strSQL)
        {
            return ExecuteDataSet(strSQL, ConnectionStringSQL);
        }

        /// <summary>
        /// 返回一个dataset带事务处理
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetWithTrans(string strSQL)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter dapt = null;
            MySqlTransaction tran = null;

            DataSet ds = new DataSet();

            try
            {
                conn = GetConnection();
                conn.Open();
                tran = conn.BeginTransaction();
                cmd = new MySqlCommand(strSQL, conn, tran);
                cmd.CommandTimeout = 600000;
                dapt = new MySqlDataAdapter(cmd);
                dapt.Fill(ds, "table1");
                tran.Commit();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                tran.Rollback();

                ds.Tables.Clear();
                ds.Tables.Add("Err");
                DataTable dt = ds.Tables[0];
                dt.Columns.Add("ErrNumber", typeof(System.Int32));
                dt.Columns.Add("ErrMsg", typeof(string));
                DataRow row = dt.NewRow();
                row["ErrNumber"] = e.ErrorCode;
                row["ErrMsg"] = e.Message;

                dt.Rows.Add(row);
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (cmd != null)
                    cmd.Dispose();
                if (dapt != null)
                    dapt.Dispose();
                if (tran != null)
                    tran.Dispose();
            }
            return ds;

        }

        /// <summary>
        /// 返回一个datatable表
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strSQL, ref MySqlConnection conn)
        {
            MySqlDataAdapter dapt = null;
            DataTable dt = new DataTable();

            try
            {
                dapt = new MySqlDataAdapter(strSQL, conn);
                dapt.Fill(dt);
            }
            catch (System.Exception e)
            {
                throw (e);
            }
            finally
            {
                if (dapt != null)
                    dapt.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// 根据条件，返回一个数据表！
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strSQL)
        {
            MySqlConnection conn = null;

            DataTable dt = null;

            try
            {
                conn = GetConnection();
                conn.Open();
                dt = ExecuteDataTable(strSQL, ref conn);
            }
            catch (System.Exception e)
            {
                throw (e);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return dt;
        }


        /// <summary>
        /// 执行SQL语句，返回结果数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string strSQL)
        {
            string ErrMsg = string.Empty;
            return EexcuteNoQuery(strSQL, ref ErrMsg);
        }

        /// <summary>
        /// 执行SQL语句，返回结果数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static int EexcuteNoQuery(string strSQL, ref string Msg)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;

            int i = 0;
            try
            {
                conn = GetConnection();
                conn.Open();
                cmd = new MySqlCommand(strSQL, conn);

                cmd.CommandTimeout = 600000;
                i = cmd.ExecuteNonQuery();

            }
            catch (System.Data.SqlClient.SqlException e)
            {
                i = Math.Abs(e.ErrorCode) * -1;
                Msg = e.Message;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (cmd != null)
                    cmd.Dispose();
            }
            return i;
        }


        /// <summary>
        /// 执行SQL语句，带事务处理。返回结果数
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static int ExecuteNoQueryWithTrans(string strSQL)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;

            MySqlTransaction tran = null;
            int i = 0;

            try
            {
                conn = GetConnection();
                conn.Open();
                tran = conn.BeginTransaction();
                cmd = new MySqlCommand(strSQL, conn, tran);
                cmd.CommandTimeout = 600000;
                i = cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                i = Math.Abs(e.ErrorCode) * -1;
                tran.Rollback();
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (cmd != null)
                    cmd.Dispose();
                if (tran != null)
                    tran.Dispose();
            }
            return i;
        }

        /// <summary>
        /// 带分页功能的结果集，返回一个dataset
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="PrimaryKey"></param>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="SortExpression"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public static DataSet GetSqlResult(string strSQL, string PrimaryKey, int PageNo, int PageSize, string SortExpression, ref int RecordCount)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter dapt = null;
            try
            {
                conn = new MySqlConnection(DataClass_Mysql.ConnectionStringSQL);

                cmd = new MySqlCommand("GetPageResult", conn);
                cmd.CommandTimeout = 600000;
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlParameter pSql = cmd.Parameters.Add("@sql", MySqlDbType.VarChar, 4000);
                pSql.Value = strSQL;
                MySqlParameter pPKey = cmd.Parameters.Add("@PKey", MySqlDbType.VarChar, 50);
                pPKey.Value = PrimaryKey;
                MySqlParameter pPageNo = cmd.Parameters.Add("@PageNo", MySqlDbType.Int32, 4);
                pPageNo.Value = PageNo;
                MySqlParameter pPageSize = cmd.Parameters.Add("@PageSize", MySqlDbType.Int32, 4);
                pPageSize.Value = PageSize;
                MySqlParameter pSort = cmd.Parameters.Add("@sort", MySqlDbType.VarChar, 50);
                pSort.Value = SortExpression;
                MySqlParameter pRecordCount = cmd.Parameters.Add("@RecordCount", MySqlDbType.Int32, 4);
                //pRecordCount.Value = SortExpression;
                pRecordCount.Direction = ParameterDirection.Output;
                dapt = new MySqlDataAdapter(cmd);
                conn.Open();
                DataSet ds = new DataSet();
                dapt.Fill(ds, "Table1");
                RecordCount = (int)pRecordCount.Value;
                return ds;

            }
            catch (Exception e)
            {
                throw (e);
                //return null;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (cmd != null)
                    cmd.Dispose();
                if (dapt != null)
                    dapt.Dispose();
            }

        }




    }
}
