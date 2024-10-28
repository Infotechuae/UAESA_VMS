using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text;
//using System.Configuration;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace DAL
{
    /// <summary>
    /// Summary description for DALORACLE.
    /// </summary>
    public class DALmySql
    {
        private MySqlTransaction m_Trans;
        //private MySqlConnection m_Connection;
        public string[] Tab_Select = new string[10];
        public static string CONFIG_FILE = @"\secupass.ini";
        #region
        public DALmySql()
        {
        }
        //MySql COnnection
        public static MySqlConnection getConnection()
        {

            string _connectionString;
            string FName = CONFIG_FILE;
            MySqlConnection _connection = null;
            TextReader tr = null;
            try
            {
                _connectionString = "";// System.Configuration.ConfigurationSettings.AppSettings["Connection"];
                // tr = new StreamReader(Application.LocalUserAppDataPath + FName);
                // _connectionString = "";//tr.ReadLine();

                //_connectionString = "Data Source=11g;User ID=ecash_new;Password= ecash_new";
                _connection = new MySqlConnection(_connectionString);
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
        public static MySqlConnection getConnection(String _connectionString)
        {
            MySqlConnection _connection = null;
            try
            {
                _connection = new MySqlConnection(_connectionString);
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
            MySqlConnection _conn = null;
            MySqlCommand _command = null;
            string qry = "";
            //string strInsert = "Insert into tablefilter (FROMDATE,TODATE)VALUES('" + dtTransFrom.Value.Date.ToString("dd-MMM-yyyy") + "','" + dtTransTo.Value.Date.ToString("dd-MMM-yyyy") + "')";
            //string strDelete = "DELETE FROM TABLEFILTER";
            try
            {
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);
                _command = new MySqlCommand();
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
        public DataSet GetMySqlDS(string MySqlstr, string ls_table, string _connectionString)
        {
            //Get or Retrive the dataset from a particular table in MSMySql and fill it in dataset 
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlConnection _conn = null;
            System.Data.DataSet DBSds = null;
            try
            {
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);
                adapter.SelectCommand = new MySqlCommand(MySqlstr, _conn);
                adapter.Fill(DBSds, ls_table);


                //  Tab_Select[DBSds.Tables.Count - 1] = MySqlstr;


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
        public DataTable fetchRecords(String _query, string _connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            MySqlConnection _conn = new MySqlConnection();

            _conn = getConnection(_connectionString);
            MySqlCommand _command = new MySqlCommand();
            _command.Connection = _conn;
            _command.CommandText = _query;
            MySqlDataAdapter _adapter = new MySqlDataAdapter(_command);
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

        //    MySqlCommand oCmd = new MySqlCommand(SPName);
        //    MySqlConnection _conn = new MySqlConnection();
        //    MySqlDataAdapter oAdp = null;

        //    MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0)];
        //    for (int i = 0; i < _params.GetLength(0); i++)
        //    {
        //        _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);

        //    }
        //    int _paramsLength = _MySqlparam.Length;
        //    DataSet Ds;
        //    try
        //    {
        //        _conn = DALORACLE.getConnection();
        //        oCmd.Connection = _conn;
        //        for (int i = 0; i < _paramsLength; i++)
        //        {
        //            //Add values as parameters
        //            oCmd.Parameters.Add(_MySqlparam[i]);
        //            oCmd.Parameters[oCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
        //        }

        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.Parameters.Add(new MySqlParameter(TableName, MySqlDbType.RefCursor)).Direction = ParameterDirection.Output;
        //        Ds = new DataSet(DataSetName);
        //        oAdp = new MySqlDataAdapter(oCmd);
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



        public DataSet fetchRecordsDs(String _query, string _connectionString)
        //this is a newly added methods which takes only a query as param
        //this is mainly used for lists
        {
            MySqlConnection _conn = new MySqlConnection();
            DataSet _dataSet = null;
            try
            {
                _conn =  getConnection(_connectionString);
                MySqlCommand _command = new MySqlCommand();
                //_command.InitialLONGFetchSize = 10000000;
                //_command.AddRowid = true;
                _command.Connection = _conn;
                _command.CommandText = _query;
                MySqlDataAdapter _adapter = new MySqlDataAdapter(_command);
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
            MySqlCommand cmd = null;
            MySqlDataAdapter dbAdapter = null;
            MySqlConnection _conn = null;

            System.Data.DataSet ds = null;
            try
            {
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);
                ds = new DataSet(DataSetName);
                cmd = new MySqlCommand(PROC_NAME, _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter(TableName, MySqlDbType.Int16)).Direction = ParameterDirection.Output;
                dbAdapter = new MySqlDataAdapter();
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
            // Save or Update  one table  in MySql 

            MySqlCommandBuilder OraCB = null;
            MySqlConnection _conn = null;
            MySqlTransaction trans = null;
            MySqlDataAdapter dbAdapter = null;
            bool bStatus = false;
            try
            {
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);
                trans = _conn.BeginTransaction(IsolationLevel.ReadCommitted);

                dbAdapter = new MySqlDataAdapter();
                dbAdapter.SelectCommand = new MySqlCommand("select * from " + Tablename, _conn);//, trans);
                OraCB = new MySqlCommandBuilder(dbAdapter);
                dbAdapter.Update(DBSds, Tablename);
                trans.Commit();
                bStatus = true;
            }

            catch (MySqlException ex)
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
        public static bool DelMySqlQuery(string Query, string _connectionString)
        {
            try
            {
                // _connectionString = "Server=111.118.215.174;Database=progdest_seculobi;Password=I-(3]biDCq]a;User ID=progdest_secuadm; port=3306;Persist Security Info= true;Charset=utf8";

                MySqlConnection MyConn2 = new MySqlConnection(_connectionString);
                //This is command class which will handle the query and connection object.  
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  

                while (MyReader2.Read())
                {
                }
                MyConn2.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

        }
        public static bool UpdMySqlQuery(string Query, byte[] imgPhoto,byte[] FrontimagePhoto,byte[] backimgPhoto, string _connectionString)
         {
         try  
    {
       // _connectionString = "Server=111.118.215.174;Database=progdest_seculobi;Password=I-(3]biDCq]a;User ID=progdest_secuadm; port=3306;Persist Security Info= true;Charset=utf8";
            
         MySqlConnection MyConn2 = new MySqlConnection(_connectionString);  
             //This is command class which will handle the query and connection object.  
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyCommand2.Parameters.Add("@imagePhoto", MySqlDbType.Blob);
            MyCommand2.Parameters["@imagePhoto"].Value = imgPhoto;
            MyCommand2.Parameters.Add("@FrontSide", MySqlDbType.Blob);
            MyCommand2.Parameters["@FrontSide"].Value = FrontimagePhoto;
            //MyCommand2.Parameters.Add("@BackSide", MySqlDbType.Blob);
            //MyCommand2.Parameters["@BackSide"].Value = backimgPhoto; 
             MySqlDataReader MyReader2;  
            MyConn2.Open();  
            MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  
            
            while (MyReader2.Read())  
         {                     
            }  
            MyConn2.Close();
            return true;
         }  
       catch (Exception ex)  
        {   
           throw   ex  ;
           return false;
       }
         
}
        public DataSet GetDataset(string DataSetName, string TableName, string strORACLE, CommandType cmdType, string _connectionString)
        {
            MySqlCommand cmd = null;
            MySqlDataAdapter dbAdapter = null;
            MySqlConnection _conn = null;
            System.Data.DataSet ds = null;
            try
            {
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);
                ds = new DataSet(DataSetName);
                cmd = new MySqlCommand(strORACLE, _conn);
                cmd.CommandType = cmdType;

                dbAdapter = new MySqlDataAdapter(cmd);
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

            MySqlCommand oCmd = new MySqlCommand(SPName);
            MySqlConnection _conn = new MySqlConnection();
            MySqlDataAdapter oAdp = null;

            MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0)];
            for (int i = 0; i < _params.GetLength(0); i++)
            {
                _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);

            }
            int _paramsLength = _MySqlparam.Length;
            DataSet Ds;
            try
            {
                _conn = DALmySql.getConnection(_connectionString);
                oCmd.Connection = _conn;
                for (int i = 0; i < _paramsLength; i++)
                {
                    //Add values as parameters
                    oCmd.Parameters.Add(_MySqlparam[i]);
                    oCmd.Parameters[oCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
                }

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new MySqlParameter(TableName, MySqlDbType.Int16)).Direction = ParameterDirection.Output;
                Ds = new DataSet(DataSetName);
                oAdp = new MySqlDataAdapter(oCmd);
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

        //public DataSet GetDBSchema(string strMySql, string TableName, string Objtype, string _connectionString)//CommandType cmdType
        //{
        //    MySqlCommand cmd;
        //    MySqlDataAdapter dbAdapter;
        //    MySqlConnection _conn = new MySqlConnection();
        //    DataSet Ds = null;
        //    try
        //    {
        //        _conn = DAL.DALMySql.getConnection(_connectionString);
        //        Ds = new DataSet();
        //        cmd = new MySqlCommand(strMySql, _conn);
        //        dbAdapter = new MySqlDataAdapter();
        //        dbAdapter.SelectCommand = cmd;
        //        dbAdapter.Fill(Ds, TableName);

        //    }
        //    catch (Exception e)
        //    {
        //        Ds = null;
        //        throw e;
        //    }
        //    finally
        //    {
        //        _conn.Dispose();
        //        _conn.Close();
        //        dbAdapter = null;
        //        cmd = null;
        //    }
        //    return Ds;
        //}


        //public bool StartTransaction()
        //{
        //    try
        //    {
        //        if (m_Connection != null)
        //        {
        //            m_Trans = m_Connection.BeginTransaction();
        //            return true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return false;
        //}

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



        ~DALmySql()
        {

        }

        public MySqlTransaction Transaction
        {
            get
            {
                return m_Trans;
            }
        }

        #region Stored Procedure Handlers

        public static int[] executeSP(String _spName, String[,] _params, string _connectionString)
        //accepts stored procedure name a string & values as string array
        {

            int[] valuesReturned;
            try
            {


                MySqlConnection FileConn;
                FileConn = DALmySql.getConnection(_connectionString);
                MySqlCommand _command = null;
                // declare valuesRetuned as an int array to store 

                _command = new MySqlCommand();
                _command.Connection = FileConn;
                //_command.Transaction = _trans;
                _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
                _command.CommandText = _spName; //sets command text as procedurename to be executed


                MySqlParameter returnParam = _command.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);
                //int _paramsLength = _MySqlparam.Length;

                MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0)];
                for (int i = 0; i < _params.GetLength(0); i++)
                {

                    _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);
                    _command.Parameters.Add(_MySqlparam[i]);
                    _command.Parameters[_command.Parameters.Count - 1].Direction = ParameterDirection.Input;

                }




                returnParam.Value = 0;
                returnParam.Direction = ParameterDirection.ReturnValue;// InputOutput;

                valuesReturned = new int[2]; //records effected & New ID returned during insertion


                //stores recordsAffected variable recordsAffected
                int recordsAffected = _command.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));

                valuesReturned[0] = newIDReturned;
                valuesReturned[1] = recordsAffected;

                FileConn.Close();
                FileConn.Dispose();
                //
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valuesReturned;
            //MySqlConnection _conn = null;
            //MySqlCommand _MySqlcmd;
            //try
            //{
            //   int[] returnValue;
            //    MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0)];
            //    for (int i = 0; i < _params.GetLength(0); i++)
            //    {

            //        _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);

            //    }

            //    _conn = DALMySql.getConnection(_connectionString);
            //    _MySqlcmd = new MySqlCommand(_spName, _conn);
            //    _MySqlcmd.CommandType = CommandType.StoredProcedure;
            //    _MySqlcmd.ExecuteNonQuery();

            //    returnValue = new int[2]; 


            //    return returnValue;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }
        public static int[] executeSP_Photo(String _spName, byte[] PICDATA, String[,] _params, string _connectionString)
        //accepts stored procedure name a string & values as string array
        {

            int[] valuesReturned;
            try
            {


                MySqlConnection FileConn;
                FileConn = DALmySql.getConnection(_connectionString);
                MySqlCommand _command = null;
                // declare valuesRetuned as an int array to store 

                _command = new MySqlCommand();
                _command.Connection = FileConn;
                //_command.Transaction = _trans;
                _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
                _command.CommandText = _spName; //sets command text as procedurename to be executed


                MySqlParameter returnParam = _command.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);

                //int _paramsLength = _MySqlparam.Length;

                MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0) + 1];
                for (int i = 0; i < _params.GetLength(0); i++)
                {

                    _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);
                    _command.Parameters.Add(_MySqlparam[i]);
                    _command.Parameters[_command.Parameters.Count - 1].Direction = ParameterDirection.Input;

                }

                //  _MySqlparam[11] = new MySqlParameter("imgPhoto", PICDATA);
                // _command.Parameters.Add(new MySqlParameter("imgPhoto", MySqlDbType.Image, PICDATA.Length));
                _command.Parameters.AddWithValue("imgPhoto", PICDATA);
                _command.Parameters[_command.Parameters.Count - 1].Direction = ParameterDirection.Input;
                returnParam.Value = 0;
                returnParam.Direction = ParameterDirection.ReturnValue;// InputOutput;

                valuesReturned = new int[2]; //records effected & New ID returned during insertion


                //stores recordsAffected variable recordsAffected
                int recordsAffected = _command.ExecuteNonQuery();

                //stores new ID to variable newIDReturned incase of insertion
                int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));

                valuesReturned[0] = newIDReturned;
                valuesReturned[1] = recordsAffected;

                FileConn.Close();
                FileConn.Dispose();
                //
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valuesReturned;
            //MySqlConnection _conn = null;
            //MySqlCommand _MySqlcmd;
            //try
            //{
            //   int[] returnValue;
            //    MySqlParameter[] _MySqlparam = new MySqlParameter[_params.GetLength(0)];
            //    for (int i = 0; i < _params.GetLength(0); i++)
            //    {

            //        _MySqlparam[i] = new MySqlParameter(_params[i, 0], _params[i, 1]);

            //    }

            //    _conn = DALMySql.getConnection(_connectionString);
            //    _MySqlcmd = new MySqlCommand(_spName, _conn);
            //    _MySqlcmd.CommandType = CommandType.StoredProcedure;
            //    _MySqlcmd.ExecuteNonQuery();

            //    returnValue = new int[2]; 


            //    return returnValue;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }
        //accepts stored procedure name a string & values as MySqlparameters
        private static int[] executeSP(String _spName, MySqlParameter[] _params, string _connectionString)
        {
            MySqlConnection _conn = null;
            MySqlTransaction _trans = null;
            int[] returnValue;
            try
            {
                _conn = DALmySql.getConnection(_connectionString);
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

        private static int[] executeSP(String _spName, MySqlParameter[] _params, MySqlTransaction _trans)
        {

            MySqlCommand _command = null;
            int[] valuesReturned; // declare valuesRetuned as an int array to store 
            try
            {
                _command = new MySqlCommand();
                _command.Connection = _trans.Connection;
                //_command.Transaction = _trans;
                _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
                _command.CommandText = _spName; //sets command text as procedurename to be executed

                //An additional parameter is added
                //which fetches the value returned by the procedure
                //which is usally the new id when a new record is inserted
                //this id is used incase if we have transaction/detail tables
                //as a foreign key
                MySqlParameter returnParam = _command.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);
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
            MySqlConnection _conn = null;
            MySqlCommand _command = null;
            try
            {


                _command = new MySqlCommand();
                _conn = new MySqlConnection();
                _conn = DALmySql.getConnection(_connectionString);

                MySqlParameter returnParam = _command.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);
                _command.Parameters.Add(new MySqlParameter("ID", MySqlDbType.Int16, 8));
                _command.Parameters.Add(new MySqlParameter("tbname", MySqlDbType.String, 50));
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

            MySqlConnection Ocn = null;
            MySqlConnection Ocnblob = null;
            MySqlCommand Cmd = null;

            int[] valuesReturned;
            try
            {


                Ocn = DALmySql.getConnection(_connectionString);
                Cmd = new MySqlCommand("SP_LOCAL_EID_Image", Ocn);
                MySqlParameter returnParam = Cmd.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);
                Cmd.Parameters.Add(new MySqlParameter("U_UID", MySqlDbType.String, 50));
                Cmd.Parameters.Add(new MySqlParameter("EID_UID", MySqlDbType.String, 50));
                Cmd.Parameters.Add(new MySqlParameter("Caption", MySqlDbType.String, 50));
                Cmd.Parameters.Add(new MySqlParameter("PictData", MySqlDbType.Blob, PICDATA.Length));
                //Cmd.Parameters.Add(new MySqlParameter("v_IMAGE", MySqlDbType.Blob, Convert.ToInt32(_params[1, 1])));


                // declare valuesRetuned as an int array to store 
                valuesReturned = new int[2];
                //records effected & New ID returned during insertion


                // Ocnblob = DALORACLE.getConnection();
                //  BLOBO = new MySql.DataAccess.Types.MySqlBlob(Ocn);
                // BLOBO.Erase();
                //  BLOBO.Write(PICIMAGE, 0, PICIMAGE.Length);



                Cmd.Parameters[1].Value = _params[0, 1];
                Cmd.Parameters[2].Value = _params[1, 1];
                Cmd.Parameters[3].Value = _params[2, 1];
                Cmd.Parameters[4].Value = PICDATA;


                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "SP_LOCAL_EID_Image";

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
                //BLOBO.Dispose();
                //BLOBO.Close();
                //    Ocnblob.Close();
                //    Ocnblob.Dispose();
                Ocn.Close();
                Ocn.Dispose();
            }
            return valuesReturned[0];

        }







        //New Image Updation in Picture_Mst FromBM Also.

        //public static int executePicBMSP(String[,] _params, Byte[] PICIMAGE, string _connectionString)
        //{

        //    MySqlConnection Ocn = null;
        //    MySqlCommand Cmd = null;
        //    MySql.DataAccess.Types.MySqlBlob BLOBO = null;
        //    int[] valuesReturned = null;
        //    try
        //    {


        //        Ocn = DALmySql.getConnection(_connectionString);
        //        Cmd = new MySqlCommand("member_bmpic_update", Ocn);
        //        MySqlParameter returnParam = Cmd.Parameters.Add("v_Newid", MySqlDbType.Int16, 8);
        //        Cmd.Parameters.Add(new MySqlParameter("v_CARD_MEMBERID", MySqlDbType.String, 8));
        //        Cmd.Parameters.Add(new MySqlParameter("v_IMAGELEN", MySqlDbType.Int16, 8));
        //        Cmd.Parameters.Add(new MySqlParameter("v_TYPE", MySqlDbType.Int16, 8));
        //        Cmd.Parameters.Add(new MySqlParameter("v_IMAGE", MySqlDbType.Image, Convert.ToInt32(_params[1, 1])));
        //        Cmd.Parameters.Add(new MySqlParameter("v_MEMBERID", MySqlDbType.Int16, 8));

        //        // declare valuesRetuned as an int array to store 
        //        valuesReturned = new int[2]; //records effected & New ID returned during insertion

        //        BLOBO = new MySql.DataAccess.Types.MySqlBlob(Ocn);
        //        BLOBO.Erase();
        //        BLOBO.Write(PICIMAGE, 0, PICIMAGE.Length);


        //        Cmd.Parameters[1].Value = _params[0, 1];
        //        Cmd.Parameters[2].Value = _params[1, 1];
        //        Cmd.Parameters[3].Value = _params[2, 1];
        //        Cmd.Parameters[4].Value = BLOBO;
        //        Cmd.Parameters[5].Value = _params[3, 1];

        //        Cmd.CommandType = CommandType.StoredProcedure;
        //        Cmd.CommandText = "member_bmpic_update";

        //        //returnParam.Value = 0;
        //        //returnParam.Direction = ParameterDirection.InputOutput;

        //        //string query = "INSERT INTO PICTURE_MST(ID,LENGTH,TYPE,PICTURE) VALUES(" + _params[0, 1] + "," + _params[1, 1] + "," + _params[2, 1] + "," + " :IMAGEPARM)";
        //        //MySqlParameter ORP = new MySqlParameter();
        //        //ORP.MySqlDbType = MySqlDbType.Blob;
        //        //ORP.ParameterName = "IMAGEPARM";
        //        //ORP.Value = BLOBO;
        //        //Cmd = new MySqlCommand(query, Ocn);
        //        //Cmd.Parameters.Add(ORP);
        //        //Cmd.ExecuteNonQuery();
        //        //stores recordsAffected variable recordsAffected
        //        int recordsAffected = Cmd.ExecuteNonQuery();

        //        //stores new ID to variable newIDReturned incase of insertion
        //        //int newIDReturned = Convert.ToInt32(Convert.ToString(returnParam.Value));
        //        //valuesReturned[0] = newIDReturned;
        //        //valuesReturned[1] = recordsAffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //      //  System.Windows.Forms.MessageBox.Show(ex.ToString());
        //        //System.Windows.Forms.MessageBox.Show(str);     
        //        //System.Windows.Forms.MessageBox.Show(str1);     
        //    }
        //    finally
        //    {
        //        Cmd.Dispose();
        //        //BLOBO.Dispose();
        //        //BLOBO.Close();
        //        Ocn.Close();
        //        Ocn.Dispose();
        //    }
        //    return 1;
        //}

        //public static int executePicBMSP(String[,] _params, Byte[] PICIMAGE)
        //{

        //    MySqlConnection Ocn = null;
        //    MySqlConnection Ocnblob = null;
        //    MySqlCommand Cmd = null;
        //    MySql.DataAccess.Types.MySqlBlob BLOBO = null;
        //    int[] valuesReturned;
        //    try
        //    {

        //        Ocn = DALORACLE.getConnection();
        //        Cmd = new MySqlCommand("member_bmpic_update", Ocn);

        //        MySqlParameter returnParam = Cmd.Parameters.Add("v_Newid", MySqlDbType.Int1632, 8);

        //        Cmd.Parameters.Add(new MySqlParameter("v_CARD_MEMBERID", MySqlDbType.Varchar2, 8));
        //        Cmd.Parameters.Add(new MySqlParameter("v_IMAGELEN", MySqlDbType.Int1632, 8));

        //        Cmd.Parameters.Add(new MySqlParameter("v_TYPE", MySqlDbType.Int1632, 8));
        //        Cmd.Parameters.Add(new MySqlParameter("v_IMAGE", MySqlDbType.Blob, Convert.ToInt32(_params[1, 1])));
        //        Cmd.Parameters.Add(new MySqlParameter("v_MEMBERID", MySqlDbType.Varchar2,8));
        //        //Cmd.Parameters.Add(new MySqlParameter("v_TYPESECOND", MySqlDbType.Int1632, 8));
        //        //Cmd.Parameters.Add(new MySqlParameter("v_TEMPLATELENGTH", MySqlDbType.Int1632, 8));
        //        //Cmd.Parameters.Add(new MySqlParameter("v_PICTURE", MySqlDbType.Blob, Convert.ToInt32(_params[2, 1])));
        //        // declare valuesRetuned as an int array to store 
        //        valuesReturned = new int[2]; //records effected & New ID returned during insertion


        //        Ocnblob = DALORACLE.getConnection();
        //        BLOBO = new MySql.DataAccess.Types.MySqlBlob(Ocnblob);

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
















        public static int executeACL(String Spname, string _connectionString)
        {

            MySqlConnection Ocn = null;
            MySqlCommand Cmd = null;
            try
            {
                Ocn = DALmySql.getConnection(_connectionString);
                Cmd = new MySqlCommand(Spname, Ocn);



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