
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace DAL
{
    public class Utils
    {
        //Fetching Records having ID and Prcedure Name

        const string PROC_NAME = "GENERAL_SP_READ";

        public static DataSet fetchRecordsDS(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
        {

            string m_TableName = "GeneralDT";
            string m_DataSetName = "GeneralDS";
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[3, 2];

                _paramArray[0, 0] = "v_ColumnName";
                _paramArray[0, 1] = COLNAME;

                _paramArray[1, 0] = "v_tbName";
                _paramArray[1, 1] = TBNAME;

                _paramArray[2, 0] = "v_WhereClause";
                _paramArray[2, 1] = WHERECLAUSE;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, PROC_NAME, _paramArray, connectionString);
            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }

        //public static int Update(string tbname, string cols, string whereclause)
        //{
        //    int[] valuesReturned;
        //    int newId = 0;
        //    try
        //    {

        //        string[,] _param;

        //        _param = new string[3, 2];

        //        //tbname = "member_mst";
        //        //cols = "member_pin=123";
        //        //whereclause = "member_id=3276";
        //        _param[0, 0] = "v_tbname";
        //        _param[0, 1] = tbname;

        //        _param[1, 0] = "v_colnames";
        //        _param[1, 1] = cols;

        //        _param[2, 0] = "v_whereclause";
        //        _param[2, 1] = whereclause;

        //        valuesReturned = DAL.DALSQL.executeSP("UPDATEBYCOLUMN", _param);
        //        newId = valuesReturned[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return newId;
        //}
        public static DataTable fetchRecords(string COLNAME, string TBNAME, string WHERECLAUSE,string connectionString)
        {

            string m_TableName = "GeneralDT";
            string m_DataSetName = "GeneralDS";
            string[,] _paramArray;
          

            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[3, 2];

                _paramArray[0, 0] = "v_ColumnName";
                _paramArray[0, 1] = COLNAME;

                _paramArray[1, 0] = "v_tbName";
                _paramArray[1, 1] = TBNAME;

                _paramArray[2, 0] = "v_WhereClause";
                _paramArray[2, 1] = WHERECLAUSE;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, PROC_NAME, _paramArray, connectionString);
            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds.Tables[0];
        }
        public static bool UpdDataDs(string connectionString, string Tablename, DataSet DBSds)
        {
            // Save or Update  one table  in Access database
            SqlConnection conn = new SqlConnection();



            SqlCommandBuilder SqlCB;
            SqlTransaction trans = null;
            SqlDataAdapter da;
            try
            {
                conn = DAL.DALSQL.getConnection(connectionString);
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                //    for (int i = 0; i <= Tab_Select.GetUpperBound(0); i++)
                //  {

                //     if (Tab_Select[i] != null)
                //     {
                //da = new OleDbDataAdapter();
                //da.SelectCommand = new OleDbCommand (Tab_Select[i], conn, trans);
                //custCB = new OleDbCommandBuilder(da);
                //da.Update(DBSds, DBSds.Tables[i].TableName);
                da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("select * from " + Tablename, conn, trans);

                SqlCB = new SqlCommandBuilder(da);

                da.Update(DBSds, Tablename);

                trans.Commit();
                conn.Close();
                conn.Dispose();
                return true;
            }

            catch (SqlException ex)
            {
                //  Console.WriteLine(ex.Message + "   Transaction Rollback, Try again ");
                trans.Rollback();
                throw ex;
                conn.Close();
                conn.Dispose();
                return false;
            }

        }

        public static DataTable fetchQueryRecords(string  strQury, string connectionString)
        {

            string m_TableName = "GeneralQDT";
            string m_DataSetName = "GeneralQDS";
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[1, 2];

                _paramArray[0, 0] = "v_QUERY";
                _paramArray[0, 1] = strQury;

               
                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, "GENERAL_QUERY_READ", _paramArray, connectionString);
            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds.Tables[0];
        }

        public static DataTable fetchQueryRecordsSP(string strValue,string ModuleCode,string StrSPname, string connectionString)
        {

            string m_TableName = StrSPname+"DT";
            string m_DataSetName = StrSPname+"DS";
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[2, 2];

                _paramArray[0, 0] = "StrVal";
                _paramArray[0, 1] = strValue;

                _paramArray[1, 0] = "ModuleCode";
                _paramArray[1, 1] = ModuleCode;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

              //  ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds.Tables[0];
        }
        public static DataSet fetchDSQueryRecordsSP(string strValue, string ModuleCode, string StrSPname, string connectionString)
        {

            string m_TableName = ModuleCode + "DT";
            string m_DataSetName = ModuleCode ;
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[2, 2];

                _paramArray[0, 0] = "StrVal";
                _paramArray[0, 1] = strValue;

                _paramArray[1, 0] = "ModuleCode";
                _paramArray[1, 1] = ModuleCode;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

            //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }
        public static DataSet fetchDSQueryRecordsSP(string strValue, string strValue1, string ModuleCode, string StrSPname, string connectionString)
        {

            string m_TableName = ModuleCode + "DT";
            string m_DataSetName = ModuleCode;
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                if (strValue.Length == 0)
                    strValue = strValue1;
                _paramArray = new string[3, 2];

                _paramArray[0, 0] = "StrVal";
                _paramArray[0, 1] = strValue;
                _paramArray[1, 0] = "StrVal1";
                _paramArray[1, 1] = strValue1;

                _paramArray[2, 0] = "ModuleCode";
                _paramArray[2, 1] = ModuleCode;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

                //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }
        public static DataSet fetchDSRecordsSP(string strValue, string ModuleCode,string UserName, string StrSPname, string connectionString)
        {

            string m_TableName = ModuleCode + "DT";
            string m_DataSetName = ModuleCode;
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[3, 2];

                _paramArray[0, 0] = "StrVal";
                _paramArray[0, 1] = strValue;

                _paramArray[1, 0] = "ModuleCode";
                _paramArray[1, 1] = ModuleCode;

                _paramArray[2, 0] = "UserName";
                _paramArray[2, 1] = UserName;

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

                //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }
        public static DataSet fetchDSQueryRecordsSP(string strValue, string ModuleCode, string FrmDate, string ToDate, string StrSPname, string connectionString)
        {

            string m_TableName = ModuleCode + "DT";
            string m_DataSetName = ModuleCode;
            string[,] _paramArray;


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                _paramArray = new string[4, 2];

                _paramArray[0, 0] = "StrVal";
                _paramArray[0, 1] = strValue;

                _paramArray[1, 0] = "ModuleCode";
                _paramArray[1, 1] = ModuleCode;

                _paramArray[2, 0] = "FromDate";
                _paramArray[2, 1] = FrmDate;

                _paramArray[3, 0] = "ToDate";
                _paramArray[3, 1] = ToDate;
                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

                //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }
        public static DataSet fetchDSRecordsSP(string strValue, string[,] _paramArray, string StrSPname, string connectionString)
        {

            string m_TableName = StrSPname + "DT";
            string m_DataSetName = StrSPname;



            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {


                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);

                //   ds.WriteXmlSchema("C:\\" + m_TableName + ".xsd");

            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds;
        }
        public static DataTable fetchQuerySP(string[,] _paramArray, string StrSPname, string connectionString)
        {
          

            string m_TableName = StrSPname + "DT";
            string m_DataSetName = StrSPname + "DS";
            


            DAL.DALSQL objDal;
            DataSet ds = null;
            try
            {
                

                objDal = new DAL.DALSQL();
                ds = objDal.GetDataset(m_DataSetName, m_TableName, StrSPname, _paramArray, connectionString);
            }
            catch (Exception ex)
            {
                ds = null;
                throw (ex);
            }
            finally
            {
                objDal = null;
            }
            return ds.Tables[0];
        }
        public static int fetchValues(string COLNAME, string TBNAME, string WHERECLAUSE,string connectionString)
        {

            DataTable dt = null;
            int result=0;
            try
            {
                dt = new DataTable();
                dt = fetchRecords(COLNAME, TBNAME, WHERECLAUSE, connectionString);
                if(dt.Rows.Count > 0) 
                    if(!(dt.Select()[0].ItemArray[0].Equals(System.DBNull.Value))) 
                result = Convert.ToInt32(dt.Select()[0].ItemArray[0]);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
            return result;
        }
        public static string fetchString(string COLNAME, string TBNAME, string WHERECLAUSE, string connectionString)
        {

            DataTable dt = null;
            string result="";
            try
            {
                dt = new DataTable();
                dt = fetchRecords(COLNAME, TBNAME, WHERECLAUSE, connectionString);
                if (dt.Rows.Count > 0)
                result = dt.Select()[0].ItemArray[0].ToString() ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
            return result;

        }

        public static DataTable fetchRecords(String _query, string connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            SqlConnection _conn = new SqlConnection();

            _conn = DAL.DALSQL.getConnection(connectionString);
            SqlCommand _command = new SqlCommand();
            _command.Connection = _conn;
            _command.CommandText = _query;
            SqlDataAdapter _adapter = new SqlDataAdapter(_command);
            DataTable _dataTable = null;
            _dataTable = new DataTable("_Tab1");

            try
            {
                _adapter.Fill(_dataTable);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _command.Dispose();
                if (_conn != null && _conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();

                }
            }
            return _dataTable;
        }

        public static DataSet fetchRecordsDs(String _query, string connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            SqlConnection _conn = new SqlConnection();
            DataSet _dataSet=null;
            try
            {
                _conn = DAL.DALSQL.getConnection(connectionString);
                SqlCommand _command = new SqlCommand();
            _command.Connection = _conn;
            _command.CommandText = _query;
            SqlDataAdapter _adapter = new SqlDataAdapter(_command);
            _dataSet = new DataSet("_DSet");
            _adapter.Fill(_dataSet);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
              
               // _command.Dispose();
                 _conn.Close();
                _conn.Dispose();
               
            }
            return _dataSet;
          
        }


        public static int fetchValues(String _query,string connectionString)
        {
            int  Val=1;
            SqlConnection _conn = new SqlConnection();
            try
            {
                _conn = DAL.DALSQL.getConnection(connectionString);
                SqlCommand orCmd = new SqlCommand(_query, _conn);
            Val = Convert.ToInt32(orCmd.ExecuteScalar());
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
               // orCmd.Dispose();
                _conn.Close();
                _conn.Dispose();
             }
             return Val;     
        }

        public static string fetchDataValues(String _query, string connectionString)
        {
            String Val = "0";
            SqlConnection _conn = new SqlConnection();
            try
            {
                _conn = DAL.DALSQL.getConnection(connectionString);
                SqlCommand orCmd = new SqlCommand(_query, _conn);
                Val = Convert.ToString( orCmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                // orCmd.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
            return Val;
        }


    }
}
