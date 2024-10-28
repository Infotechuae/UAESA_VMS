using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text;
//using System.Configuration;
using System.IO;

using Oracle.DataAccess.Client;
namespace DAL
{
    /// <summary>
    /// Summary description for DALORACLE.
    /// </summary>
    public class DALORACLE
    {
        private OracleTransaction m_Trans;
        private OracleConnection m_Connection;
        public string[] Tab_Select = new string[10];
        public static string CONFIG_FILE = @"\EYMatrix.ini";
        #region
        public DALORACLE()
        {
        }
        //Oracle COnnection
        public static OracleConnection getConnection()
        {

           string _connectionString;
            string FName = CONFIG_FILE;
            OracleConnection _connection = null;
            TextReader tr = null;
            try
            {
                _connectionString = "";// System.Configuration.ConfigurationSettings.AppSettings["Connection"];
               // tr = new StreamReader(Application.LocalUserAppDataPath + FName);
               // _connectionString = "";//tr.ReadLine();

                //_connectionString = "Data Source=11g;User ID=ecash_new;Password= ecash_new";
                _connection = new OracleConnection(_connectionString);
                _connection.Open();

            }
            catch (Exception ex)
            {
                _connection.Dispose();
                _connection.Close();
                throw ex;

            }
            finally
            {
                tr.Dispose();
                tr.Close();
                tr = null;
            }
            return _connection;
        }

        //Checking COnnection String 
        public static OracleConnection getConnection(String _connectionString)
        {
            OracleConnection _connection = null;
            try
            {
                _connection = new OracleConnection(_connectionString);
                _connection.Open();

            }
            catch (Exception ex)
            {
                _connection.Dispose();
                _connection.Close();
                throw ex;
            }
            return _connection;

        }

        # endregion


        public void ExecuteQuery(string _query, string _connectionString)
        {
            OracleConnection _conn = null;
            OracleCommand _command = null;
            string qry = "";
            //string strInsert = "Insert into tablefilter (FROMDATE,TODATE)VALUES('" + dtTransFrom.Value.Date.ToString("dd-MMM-yyyy") + "','" + dtTransTo.Value.Date.ToString("dd-MMM-yyyy") + "')";
            //string strDelete = "DELETE FROM TABLEFILTER";
            try
            {
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection( _connectionString);
                _command = new OracleCommand();
                _command.Connection = _conn;
                _command.CommandText = _query;
                _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                _command.Dispose();
                _command = null;
                _conn.Dispose();
                _conn.Close();
            }
        }
        public DataSet GetOracleDS(string sqlstr, string ls_table, string _connectionString)
        {
            //Get or Retrive the dataset from a particular table in MSSQL and fill it in dataset 
            OracleDataAdapter adapter = new OracleDataAdapter();
            OracleConnection _conn = null;
            System.Data.DataSet DBSds = null;
            try
            {
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection( _connectionString);
                adapter.SelectCommand = new OracleCommand(sqlstr, _conn);
                adapter.Fill(DBSds, ls_table);


              //  Tab_Select[DBSds.Tables.Count - 1] = sqlstr;
               
               
            }
            catch (Exception ex)
            {
                
                DBSds = null;
               // throw (ex);
            }

            finally
            {
                adapter.Dispose();
                //cmd.Dispose();
                adapter = null;
               // cmd = null;
                _conn.Dispose();
                _conn.Close();

            }
            return DBSds;
        }
        public  DataTable fetchRecords(String _query, string _connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            OracleConnection _conn = new OracleConnection();

            _conn = DALORACLE.getConnection(_connectionString);
            OracleCommand _command = new OracleCommand();
            _command.Connection = _conn;
            _command.CommandText = _query;
            OracleDataAdapter _adapter = new OracleDataAdapter(_command);
            DataTable _dataTable = null;
            _dataTable = new DataTable("_Tab1");

            try
            {
                _adapter.Fill(_dataTable);
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("fetch" + e.ToString());

            }
            finally
            {
                _command.Dispose();
                _adapter.Dispose();
                if (_conn != null && _conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _conn.Dispose();

                }
            }
            return _dataTable;
        }


        //public DataSet GetDataset(string DataSetName, string TableName, string SPName, String[,] _params)
        //{

        //    OracleCommand oCmd = new OracleCommand(SPName);
        //    OracleConnection _conn = new OracleConnection();
        //    OracleDataAdapter oAdp = null;

        //    OracleParameter[] _Oracleparam = new OracleParameter[_params.GetLength(0)];
        //    for (int i = 0; i < _params.GetLength(0); i++)
        //    {
        //        _Oracleparam[i] = new OracleParameter(_params[i, 0], _params[i, 1]);

        //    }
        //    int _paramsLength = _Oracleparam.Length;
        //    DataSet Ds;
        //    try
        //    {
        //        _conn = DALORACLE.getConnection();
        //        oCmd.Connection = _conn;
        //        for (int i = 0; i < _paramsLength; i++)
        //        {
        //            //Add values as parameters
        //            oCmd.Parameters.Add(_Oracleparam[i]);
        //            oCmd.Parameters[oCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
        //        }

        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.Parameters.Add(new OracleParameter(TableName, OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
        //        Ds = new DataSet(DataSetName);
        //        oAdp = new OracleDataAdapter(oCmd);
        //        oAdp.Fill(Ds, TableName);

        //    }
        //    catch (Exception ex)
        //    {
        //        Ds = null;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        _conn.Dispose();
        //        _conn.Close();
        //        oCmd.Dispose();
        //        oAdp.Dispose();
        //    }
        //    return Ds;
        //}



        public  DataSet fetchRecordsDs(String _query, string _connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            OracleConnection _conn = new OracleConnection();
            DataSet _dataSet = null;
            try
            {
                _conn = DALORACLE.getConnection(_connectionString);
                OracleCommand _command = new OracleCommand();
                _command.InitialLONGFetchSize = 10000000;
                //_command.AddRowid = true;
                _command.Connection = _conn;
                _command.CommandText = _query;
                OracleDataAdapter _adapter = new OracleDataAdapter(_command);
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
        public DataSet GetDataset(string DataSetName, string TableName, string PROC_NAME, string _connectionString)
        {
            OracleCommand cmd = null;
            OracleDataAdapter dbAdapter = null;
            OracleConnection _conn = null;

            System.Data.DataSet ds = null;
            try
            {
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection(_connectionString);
                ds = new DataSet(DataSetName);
                cmd = new OracleCommand(PROC_NAME, _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter(TableName, OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                dbAdapter = new OracleDataAdapter();
                dbAdapter.SelectCommand = cmd;
                dbAdapter.Fill(ds, TableName);

            }
            catch (Exception e)
            {
                ds = null;
                throw e;
            }
            finally
            {
                dbAdapter.Dispose();
                cmd.Dispose();
                dbAdapter = null;
                cmd = null;
                _conn.Dispose();
                _conn.Close();

            }
            return ds;
        }

        public bool UpdoracleDs(string Tablename, DataSet DBSds, string _connectionString)
        {
            // Save or Update  one table  in Oracle 

            OracleCommandBuilder OraCB = null;
            OracleConnection _conn = null;
            OracleTransaction trans = null;
            OracleDataAdapter dbAdapter = null;
            bool bStatus = false;
            try
            {
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection(_connectionString);
                trans = _conn.BeginTransaction(IsolationLevel.ReadCommitted);

                dbAdapter = new OracleDataAdapter();
                dbAdapter.SelectCommand = new OracleCommand("select * from " + Tablename, _conn);//, trans);
                OraCB = new OracleCommandBuilder(dbAdapter);
                dbAdapter.Update(DBSds, Tablename);
                trans.Commit();
                bStatus = true;
            }

            catch (OracleException ex)
            {
                throw ex;
                trans.Rollback();
                trans.Dispose();

            }
            finally
            {
                _conn.Dispose();
                _conn.Close();
                dbAdapter.Dispose();
                OraCB.Dispose();
                dbAdapter = null;
                OraCB = null;
            }
            return bStatus;
        }
        public DataSet GetDataset(string DataSetName, string TableName, string strORACLE, CommandType cmdType, string _connectionString)
        {
            OracleCommand cmd = null;
            OracleDataAdapter dbAdapter = null;
            OracleConnection _conn = null;
            System.Data.DataSet ds = null;
            try
            {
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection(_connectionString);
                ds = new DataSet(DataSetName);
                cmd = new OracleCommand(strORACLE, _conn);
                cmd.CommandType = cmdType;

                dbAdapter = new OracleDataAdapter(cmd);
                dbAdapter.Fill(ds, TableName);

            }
            catch (Exception e)
            {
                ds = null;
                throw e;
            }
            finally
            {
                dbAdapter.Dispose();
                cmd.Dispose();
                _conn.Dispose();
                dbAdapter = null;
                cmd = null;
            }
            return ds;
        }


        public DataSet GetDataset(string DataSetName, string TableName, string SPName, String[,] _params, string _connectionString)
        {

            OracleCommand oCmd = new OracleCommand(SPName);
            OracleConnection _conn = new OracleConnection();
            OracleDataAdapter oAdp = null;

            OracleParameter[] _Oracleparam = new OracleParameter[_params.GetLength(0)];
            for (int i = 0; i < _params.GetLength(0); i++)
            {
                _Oracleparam[i] = new OracleParameter(_params[i, 0], _params[i, 1]);

            }
            int _paramsLength = _Oracleparam.Length;
            DataSet Ds;
            try
            {
                _conn = DALORACLE.getConnection(_connectionString);
                oCmd.Connection = _conn;
                for (int i = 0; i < _paramsLength; i++)
                {
                    //Add values as parameters
                    oCmd.Parameters.Add(_Oracleparam[i]);
                    oCmd.Parameters[oCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
                }

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new OracleParameter(TableName, OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                Ds = new DataSet(DataSetName);
                oAdp = new OracleDataAdapter(oCmd);
                oAdp.Fill(Ds, TableName);

            }
            catch (Exception ex)
            {
                Ds = null;
                throw ex;
            }
            finally
            {
                _conn.Dispose();
                _conn.Close();
                oCmd.Dispose();
                oAdp.Dispose();
            }
            return Ds;
        }

        public DataSet GetDBSchema(string strSQL, string TableName, string Objtype, string _connectionString)//CommandType cmdType
        {
            OracleCommand cmd;
            OracleDataAdapter dbAdapter;
            OracleConnection _conn = new OracleConnection();
            DataSet Ds = null;
            try
            {
                _conn = DAL.DALORACLE.getConnection(_connectionString);
                Ds = new DataSet();
                cmd = new OracleCommand(strSQL, _conn);
                dbAdapter = new OracleDataAdapter();
                dbAdapter.SelectCommand = cmd;
                dbAdapter.Fill(Ds, TableName);

            }
            catch (Exception e)
            {
                Ds = null;
                throw e;
            }
            finally
            {
                _conn.Dispose();
                _conn.Close();
                dbAdapter = null;
                cmd = null;
            }
            return Ds;
        }


        public bool StartTransaction()
        {
            try
            {
                if (m_Connection != null)
                {
                    m_Trans = m_Connection.BeginTransaction();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        public bool CommitTransaction()
        {
            try
            {
                if (m_Trans != null)
                {
                    m_Trans.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        public bool RollBackTransaction()
        {
            try
            {
                if (m_Trans != null)
                {
                    m_Trans.Rollback();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }



        ~DALORACLE()
        {

        }

        public OracleTransaction Transaction
        {
            get
            {
                return m_Trans;
            }
        }

        #region Stored Procedure Handlers

        public static int[] executeSP(String _spName, String[,] _params,string _connectionString)
        //accepts stored procedure name a string & values as string array
        {
            try
            {
                //int[] returnValue;
                OracleParameter[] _Oracleparam = new OracleParameter[_params.GetLength(0)];
                for (int i = 0; i < _params.GetLength(0); i++)
                {

                    _Oracleparam[i] = new OracleParameter(_params[i, 0], _params[i, 1]);

                }
                int[] returnValue = executeSP(_spName, _Oracleparam, _connectionString);
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //accepts stored procedure name a string & values as Oracleparameters
        private static int[] executeSP(String _spName, OracleParameter[] _params, string _connectionString)
        {
            OracleConnection _conn = null;
            OracleTransaction _trans = null;
            int[] returnValue;
            try
            {
                _conn = DALORACLE.getConnection(_connectionString);
                _trans = _conn.BeginTransaction();
                // begins a transaction and 
                //calls executeSP with procedurename,values & transaction

                returnValue = executeSP(_spName, _params, _trans);
                //if executed succefully commits 
                //else raises exception and rolls back the transaction
                _trans.Commit();


            }
            catch (Exception e)
            {

                if (_trans != null)
                {
                    _trans.Rollback();
                }
                throw e;

            }
            finally
            {
                if (_conn != null && _conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                    _trans.Dispose();
                    _conn.Dispose();
                }
            }
            return returnValue;
        }

        private static int[] executeSP(String _spName, OracleParameter[] _params, OracleTransaction _trans)
        {

            OracleCommand _command = null;
            int[] valuesReturned; // declare valuesRetuned as an int array to store 
            try
            {
                _command = new OracleCommand();
                _command.Connection = _trans.Connection;
                //_command.Transaction = _trans;
                _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
                _command.CommandText = _spName; //sets command text as procedurename to be executed

                //An additional parameter is added
                //which fetches the value returned by the procedure
                //which is usally the new id when a new record is inserted
                //this id is used incase if we have transaction/detail tables
                //as a foreign key
                OracleParameter returnParam = _command.Parameters.Add("v_Newid", OracleDbType.Int32, 8);
                int _paramsLength = _params.Length;
                for (int i = 0; i < _paramsLength; i++)
                {
                    //Add values as parameters
                    _command.Parameters.Add(_params[i]);
                    _command.Parameters[_command.Parameters.Count - 1].Direction = ParameterDirection.Input;
                }
                returnParam.Value = 0;
                returnParam.Direction = ParameterDirection.InputOutput;

                valuesReturned = new int[2]; //records effected & New ID returned during insertion


                //stores recordsAffected variable recordsAffected
                int recordsAffected = _command.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));

                valuesReturned[0] = newIDReturned;
                valuesReturned[1] = recordsAffected;

            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                _command.Dispose();
            }
            return valuesReturned;
        }

        public static int doDelete(String _spName, int ID, string tbname, string _connectionString)
        {
            int valuesReturned = 0;
            OracleConnection _conn = null;
            OracleCommand _command = null;
            try
            {


                _command = new OracleCommand();
                _conn = new OracleConnection();
                _conn = DALORACLE.getConnection(_connectionString);

                OracleParameter returnParam = _command.Parameters.Add("v_Newid", OracleDbType.Int32, 8);
                _command.Parameters.Add(new OracleParameter("ID", OracleDbType.Int32, 8));
                _command.Parameters.Add(new OracleParameter("tbname", OracleDbType.NChar, 50));
                _command.Parameters[1].Value = ID;
                _command.Parameters[2].Value = tbname;
                _command.Connection = _conn;
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = _spName;
                valuesReturned = _command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Dispose();
                _conn.Close();
                _command.Dispose();
            }
            return valuesReturned;

        }


        //-----------------------------------------------------------------------------------------------
        // "executePicSP" IS  NOT A GENERALISED FUNCTION
        //Specifically For Picture_Mst Finger Template Updation[guiFinger].

        public static int executePicSP(String[,] _params, Byte[] PICDATA, Byte[] PICIMAGE, string _connectionString)
        {

            OracleConnection Ocn = null;
            OracleConnection Ocnblob = null;
            OracleCommand Cmd = null;
            Oracle.DataAccess.Types.OracleBlob BLOBO = null;
            int[] valuesReturned;
            try
            {


                Ocn = DALORACLE.getConnection(_connectionString);
                Cmd = new OracleCommand("MEMBER_PIC_UPDATE", Ocn);
                OracleParameter returnParam = Cmd.Parameters.Add("v_Newid", OracleDbType.Int32, 8);

                Cmd.Parameters.Add(new OracleParameter("v_CARD_MEMBERID", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_IMAGELEN", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_TEMPLATELENGTH", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_PICTURE", OracleDbType.Blob, Convert.ToInt32(_params[2, 1])));
                Cmd.Parameters.Add(new OracleParameter("v_IMAGE", OracleDbType.Blob, Convert.ToInt32(_params[1, 1])));
                Cmd.Parameters.Add(new OracleParameter("v_TYPEFIRST", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_TYPESECOND", OracleDbType.Int32, 8));

                // declare valuesRetuned as an int array to store 
                valuesReturned = new int[2];
                //records effected & New ID returned during insertion


                // Ocnblob = DALORACLE.getConnection();
                BLOBO = new Oracle.DataAccess.Types.OracleBlob(Ocn);
                // BLOBO.Erase();
                BLOBO.Write(PICIMAGE, 0, PICIMAGE.Length);



                Cmd.Parameters[1].Value = _params[0, 1];
                Cmd.Parameters[2].Value = _params[1, 1];
                Cmd.Parameters[3].Value = _params[2, 1];
                Cmd.Parameters[4].Value = PICDATA;
                Cmd.Parameters[5].Value = BLOBO;
                Cmd.Parameters[6].Value = _params[3, 1];
                Cmd.Parameters[7].Value = _params[4, 1];

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "MEMBER_PIC_UPDATE";

                returnParam.Value = 0;
                returnParam.Direction = ParameterDirection.InputOutput;

                //stores recordsAffected variable recordsAffected
                int recordsAffected = Cmd.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));
                valuesReturned[0] = newIDReturned;
                valuesReturned[1] = recordsAffected;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cmd.Dispose();
                BLOBO.Dispose();
                BLOBO.Close();
                //    Ocnblob.Close();
                //    Ocnblob.Dispose();
                Ocn.Close();
                Ocn.Dispose();
            }
            return valuesReturned[0];

        }







        //New Image Updation in Picture_Mst FromBM Also.

        public static int executePicBMSP(String[,] _params, Byte[] PICIMAGE, string _connectionString)
        {

            OracleConnection Ocn = null;
            OracleCommand Cmd = null;
            Oracle.DataAccess.Types.OracleBlob BLOBO = null;
            int[] valuesReturned = null;
            try
            {


                Ocn = DALORACLE.getConnection(_connectionString);
                Cmd = new OracleCommand("member_bmpic_update", Ocn);
                OracleParameter returnParam = Cmd.Parameters.Add("v_Newid", OracleDbType.Int32, 8);
                Cmd.Parameters.Add(new OracleParameter("v_CARD_MEMBERID", OracleDbType.Varchar2, 8));
                Cmd.Parameters.Add(new OracleParameter("v_IMAGELEN", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_TYPE", OracleDbType.Int32, 8));
                Cmd.Parameters.Add(new OracleParameter("v_IMAGE", OracleDbType.Blob, Convert.ToInt32(_params[1, 1])));
                Cmd.Parameters.Add(new OracleParameter("v_MEMBERID", OracleDbType.Int32, 8));

                // declare valuesRetuned as an int array to store 
                valuesReturned = new int[2]; //records effected & New ID returned during insertion

                BLOBO = new Oracle.DataAccess.Types.OracleBlob(Ocn);
                BLOBO.Erase();
                BLOBO.Write(PICIMAGE, 0, PICIMAGE.Length);


                Cmd.Parameters[1].Value = _params[0, 1];
                Cmd.Parameters[2].Value = _params[1, 1];
                Cmd.Parameters[3].Value = _params[2, 1];
                Cmd.Parameters[4].Value = BLOBO;
                Cmd.Parameters[5].Value = _params[3, 1];

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "member_bmpic_update";

                //returnParam.Value = 0;
                //returnParam.Direction = ParameterDirection.InputOutput;

                //string query = "INSERT INTO PICTURE_MST(ID,LENGTH,TYPE,PICTURE) VALUES(" + _params[0, 1] + "," + _params[1, 1] + "," + _params[2, 1] + "," + " :IMAGEPARM)";
                //OracleParameter ORP = new OracleParameter();
                //ORP.OracleDbType = OracleDbType.Blob;
                //ORP.ParameterName = "IMAGEPARM";
                //ORP.Value = BLOBO;
                //Cmd = new OracleCommand(query, Ocn);
                //Cmd.Parameters.Add(ORP);
                //Cmd.ExecuteNonQuery();
                //stores recordsAffected variable recordsAffected
                int recordsAffected = Cmd.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                //int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));
                //valuesReturned[0] = newIDReturned;
                //valuesReturned[1] = recordsAffected;
            }
            catch (Exception ex)
            {
                //throw ex;
              //  System.Windows.Forms.MessageBox.Show(ex.ToString());
                //System.Windows.Forms.MessageBox.Show(str);     
                //System.Windows.Forms.MessageBox.Show(str1);     
            }
            finally
            {
                Cmd.Dispose();
                //BLOBO.Dispose();
                //BLOBO.Close();
                Ocn.Close();
                Ocn.Dispose();
            }
            return 1;
        }

        //public static int executePicBMSP(String[,] _params, Byte[] PICIMAGE)
        //{

        //    OracleConnection Ocn = null;
        //    OracleConnection Ocnblob = null;
        //    OracleCommand Cmd = null;
        //    Oracle.DataAccess.Types.OracleBlob BLOBO = null;
        //    int[] valuesReturned;
        //    try
        //    {

        //        Ocn = DALORACLE.getConnection();
        //        Cmd = new OracleCommand("member_bmpic_update", Ocn);

        //        OracleParameter returnParam = Cmd.Parameters.Add("v_Newid", OracleDbType.Int32, 8);

        //        Cmd.Parameters.Add(new OracleParameter("v_CARD_MEMBERID", OracleDbType.Varchar2, 8));
        //        Cmd.Parameters.Add(new OracleParameter("v_IMAGELEN", OracleDbType.Int32, 8));

        //        Cmd.Parameters.Add(new OracleParameter("v_TYPE", OracleDbType.Int32, 8));
        //        Cmd.Parameters.Add(new OracleParameter("v_IMAGE", OracleDbType.Blob, Convert.ToInt32(_params[1, 1])));
        //        Cmd.Parameters.Add(new OracleParameter("v_MEMBERID", OracleDbType.Varchar2,8));
        //        //Cmd.Parameters.Add(new OracleParameter("v_TYPESECOND", OracleDbType.Int32, 8));
        //        //Cmd.Parameters.Add(new OracleParameter("v_TEMPLATELENGTH", OracleDbType.Int32, 8));
        //        //Cmd.Parameters.Add(new OracleParameter("v_PICTURE", OracleDbType.Blob, Convert.ToInt32(_params[2, 1])));
        //        // declare valuesRetuned as an int array to store 
        //        valuesReturned = new int[2]; //records effected & New ID returned during insertion


        //        Ocnblob = DALORACLE.getConnection();
        //        BLOBO = new Oracle.DataAccess.Types.OracleBlob(Ocnblob);

        //        //BLOBO.Erase();

        //        BLOBO.Write(PICIMAGE, 0, PICIMAGE.Length);



        //        Cmd.Parameters[1].Value = _params[0, 1];
        //        Cmd.Parameters[2].Value = _params[1, 1];
        //        Cmd.Parameters[3].Value = _params[2, 1];
        //        Cmd.Parameters[4].Value = BLOBO;
        //        Cmd.Parameters[5].Value = _params[3, 1];
        //       // Cmd.Parameters[6].Value = _params[4, 1];

        //        Cmd.CommandType = CommandType.StoredProcedure;
        //        Cmd.CommandText = "member_bmpic_update";

        //        returnParam.Value = 0;
        //        returnParam.Direction = ParameterDirection.InputOutput;


        //        //stores recordsAffected variable recordsAffected
        //        int recordsAffected = Cmd.ExecuteNonQuery();

        //        //stores new ID to variable newIDReturned incase of insertion
        //        //int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));
        //        int newIDReturned = 0;
        //        valuesReturned[0] = newIDReturned;
        //        valuesReturned[1] = recordsAffected;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        Cmd.Dispose();
        //        Ocn.Close();
        //        Ocn.Dispose();
        //        BLOBO.Close();
        //        BLOBO.Dispose();
        //        Ocnblob.Close();
        //        //Ocnblob.Dispose();


        //    }
        //    return valuesReturned[0]; 
        //}

















        public static int executeACL(String Spname,string _connectionString)
        {

            OracleConnection Ocn = null;
            OracleCommand Cmd = null;
            try
            {
                Ocn = DALORACLE.getConnection(_connectionString);
                Cmd = new OracleCommand(Spname, Ocn);



                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = Spname;



                //stores recordsAffected variable recordsAffected
                int recordsAffected = Cmd.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                return 1;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cmd.Dispose();
                Ocn.Close();
                Ocn.Dispose();
            }

        }





        #endregion Stored Procedure Handlers
    }


}