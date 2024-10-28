using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL 
{
    /// <summary>
    /// Summary description for DALSQL.
    /// </summary>
    public class DALSQL 
    {

              public DALSQL()
              {
                  //
              }

              public static string ExecuteSProcedure(SqlCommand cmdOrl, SqlConnection _Centralconn)
              {

                  try
                  {
                      cmdOrl.Connection = _Centralconn;
                      cmdOrl.CommandTimeout = 10;


                      string recordsAffected = Convert.ToString(cmdOrl.ExecuteNonQuery());
                      return recordsAffected;
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              }
              public string ExecuteStoredProcedure( SqlCommand cmdSQL,string strConnection)
              {
                  SqlConnection m_Connection = new SqlConnection(strConnection);



                  try
                  {
                      m_Connection.Open();
                      cmdSQL.Connection = m_Connection;
                      cmdSQL.ExecuteNonQuery();
                //cmdSQL.Parameters[0].Value
                cmdSQL.Dispose(); 
                m_Connection.Close();
                      return  "1";
                  }
                  catch (Exception ex)
                  {
                      throw ex;
                  }
              }

        public string ExecuteStoredProcedurereturn(SqlCommand cmdSQL, string strConnection)
        {
            SqlConnection m_Connection = new SqlConnection(strConnection);



            try
            {
                //cmdSQL.Parameters.AddWithValue("SeqName", "SeqNameValue");
                var returnParameter = cmdSQL.Parameters.Add("@ReturnVal", SqlDbType.NVarChar);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                m_Connection.Open();
                cmdSQL.Connection = m_Connection;
                cmdSQL.ExecuteNonQuery();
                var result = returnParameter.Value;
                //cmdSQL.Parameters[0].Value
                return result.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDS(string sqlstr, string ls_table, string  ConString)
        {
            //Get or Retrive the dataset from a particular table in MSSQL and fill it in dataset 
            SqlConnection conn=null ;
          

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet DBSds = new DataSet();
            string[] Tab_Select = new string[10];
            // string[] Tables = new string[10];
            try
            {
                conn = new SqlConnection(ConString);
                conn.Open();
                adapter.SelectCommand = new SqlCommand(sqlstr, conn);
                adapter.Fill(DBSds, ls_table);


                Tab_Select[DBSds.Tables.Count - 1] = sqlstr;
                conn.Close(); 
                return DBSds;
            }
            catch (Exception ex)
            {
                DBSds = null;
                adapter = null;
                conn.Close(); 
                return DBSds;
            }
        }
      //  public bool UpdDs(SqlConnection conn, string Tablename, DataSet DBSds)
        public bool UpdDs(string  ConString, string Tablename, DataSet DBSds)
      
        {
            // Save or Update  one table  in MSSQL database 

            SqlCommandBuilder SqlCB;
            SqlConnection conn=null;
            SqlTransaction trans = null;
            SqlDataAdapter da;
            try
            {
                conn = new SqlConnection(ConString);
                conn.Open();  
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("select * from " + Tablename, conn, trans);
                SqlCB = new SqlCommandBuilder(da);
                da.Update(DBSds, Tablename);

               trans.Commit();
               conn.Close();  
                return true;
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                trans.Rollback();
                conn.Close();
                return false;
            }
        }
        public DataSet GetDataset(string Table_Name, string PROC_NAME, string connectionString)
        {
            SqlCommand  cmd = null;
            SqlDataAdapter dbAdapter = null;
            SqlConnection _conn = null;

            DataSet ds = new DataSet();
            try
            {
                _conn = new SqlConnection();

                _conn = DALSQL.getConnection(connectionString);



                SqlCommand command = new SqlCommand(PROC_NAME, _conn);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = 1;
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(ds, Table_Name);
//this.dataGrid1.DataSource = ds;
//this.dataGrid1.DataMember = "Products";
                //SqlConnection conn = new SqlConnection(_conn);
  //              SqlDataAdapter adapter = new SqlDataAdapter();
    //            adapter.SelectCommand = new SqlCommand("exec MASTER_MENUS_ALL_SP_READ", _conn);
      //          adapter.Fill(ds);

            }
            catch (Exception e)
            {
                ds = null;
                throw e;
            }
            finally
            {
                //dbAdapter.Dispose();
               // cmd.Dispose();
              //  dbAdapter = null;
               // cmd = null;
              //  _conn.Dispose();
                _conn.Close();

            }
            return ds;
        }
        public DataSet GetDataset(string DataSetName, string TableName, string SPName, String[,] _params, string connectionString)
        {

            SqlCommand oCmd = new SqlCommand(SPName);
            SqlConnection _conn = new SqlConnection();
            SqlDataAdapter oAdp = null;

            SqlParameter[] _Sqlparam = new SqlParameter[_params.GetLength(0)];
            for (int i = 0; i < _params.GetLength(0); i++)
            {
                _Sqlparam[i] = new SqlParameter(_params[i, 0], _params[i, 1]);

            }
            int _paramsLength = _Sqlparam.Length;
            DataSet Ds;
            try
            {
                //_conn.ConnectionTimeout = 0;
                _conn = DALSQL.getConnection(connectionString);
               
                oCmd.Connection = _conn;
                oCmd.CommandTimeout = 0;
                for (int i = 0; i < _paramsLength; i++)
                {
                    //Add values as parameters
                    oCmd.Parameters.Add(_Sqlparam[i]);
                    oCmd.Parameters[oCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
                }

                oCmd.CommandType = CommandType.StoredProcedure;
              //  oCmd.Parameters.Add(new SqlParameter(TableName, SqlDbType.VarChar)).Direction = ParameterDirection.Output;
                Ds = new DataSet(DataSetName);
                oAdp = new SqlDataAdapter(oCmd);
                oAdp.Fill(Ds, TableName);
                oAdp.Dispose();
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
               
            }
            return Ds;
        }
        public static SqlConnection getConnection(string _connectionString)
        {

         
            SqlConnection _connection = null;
           

            try
            {
              _connection = new SqlConnection(_connectionString);
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
               // tr.Dispose();
               // tr.Close();
                //tr = null;
            }
            return _connection;
        }

        public static int[] executeSP(String _spName, String[,] _params, String conn)
        //accepts stored procedure name a string & values as string array
        {
            int[] valuesReturned;
            try
            {

              //  String conn = "Data Source=DEVELOPMENT\\OFFICESERVERS;Initial Catalog=BadgeMaker;User ID=BadgeMaker;Password=BadgeMaker;Persist Security Info=False;";
                //SqlConnection _conn = Progr; 
                SqlConnection FileConn;
                FileConn = DALSQL.getConnection(conn);
                SqlCommand _command = null;
                // declare valuesRetuned as an int array to store 

                _command = new SqlCommand();
                _command.Connection = FileConn;
                //_command.Transaction = _trans;
                _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
                _command.CommandText = _spName; //sets command text as procedurename to be executed


                SqlParameter returnParam = _command.Parameters.Add("v_Newid", SqlDbType.Int, 8);
                //int _paramsLength = _sqlparam.Length;

                SqlParameter[] _sqlparam = new SqlParameter[_params.GetLength(0)];
                for (int i = 0; i < _params.GetLength(0); i++)
                {

                    _sqlparam[i] = new SqlParameter(_params[i, 0], _params[i, 1]);
                    _command.Parameters.Add(_sqlparam[i]);
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

        }
        
        
        public int executePicBMSP(String filename, String file_type, String Remarks, DateTime created_date, Byte[] DOC_FILE,String ProjectID,String MemberID,int isEdit, String conn)
        {
            SqlConnection FileConn;
            SqlCommand cmdUploadDoc;

            // SqlConnection connection = new SqlConnection("Data Source=SHERMIN\\SQLEXPRESS;Initial Catalog=BadgeMaker;User ID=shermin;Password=shermin;Persist Security Info=False;");


         //   String conn = "Data Source=DEVELOPMENT\\OFFICESERVERS;Initial Catalog=BadgeMaker;User ID=BadgeMaker;Password=BadgeMaker;Persist Security Info=False;";
            try
            {
                FileConn = DALSQL.getConnection(conn);


                cmdUploadDoc = new SqlCommand("UploadFile", FileConn);
                cmdUploadDoc.CommandType = CommandType.StoredProcedure;

                cmdUploadDoc.Parameters.Add("@FileName ", SqlDbType.NVarChar, 200);
                cmdUploadDoc.Parameters.Add("@Doc ", SqlDbType.Image);
                cmdUploadDoc.Parameters.Add("@FileType ", SqlDbType.NVarChar, 50);
                cmdUploadDoc.Parameters.Add("@remarks ", SqlDbType.NVarChar, 200);
                cmdUploadDoc.Parameters.Add("@createddate ", SqlDbType.DateTime, 200);
                cmdUploadDoc.Parameters.Add("@Proj_ID ", SqlDbType.NVarChar, 200);
                cmdUploadDoc.Parameters.Add("@Member_ID ", SqlDbType.NVarChar, 200);
                cmdUploadDoc.Parameters.Add("@isEdit ", SqlDbType.Int);
                
                cmdUploadDoc.Parameters[0].Value = filename;
                cmdUploadDoc.Parameters[1].Value = DOC_FILE;
                cmdUploadDoc.Parameters[2].Value = file_type;
                cmdUploadDoc.Parameters[3].Value = Remarks;
                cmdUploadDoc.Parameters[4].Value = created_date;
                cmdUploadDoc.Parameters[5].Value = ProjectID;
                cmdUploadDoc.Parameters[6].Value = MemberID;
                cmdUploadDoc.Parameters[7].Value = isEdit;

                // FileConn.Open();
                cmdUploadDoc.ExecuteNonQuery();


                /*
                connection.Open();

                cmdUploadDoc = new SqlCommand("insert into Data_Archive "
                + "(FileName, Doc,FileType,remarks,createddate) values (@FileName, @Doc,@FileType,@remarks,@createddate)", connection);
                cmdUploadDoc.Parameters.Add("@FileName", filename);
                cmdUploadDoc.Parameters.Add("@Doc", DOC_FILE);
                cmdUploadDoc.Parameters.Add("@FileType", file_type);
                cmdUploadDoc.Parameters.Add("@remarks", Remarks);
                cmdUploadDoc.Parameters.Add("@createddate", created_date);
                cmdUploadDoc.ExecuteNonQuery();




                connection.Close();
                */






            }
            catch (Exception ex)
            {
                throw (ex);


            }
            finally
            {
               // cmdUploadDoc.Dispose();

               // FileConn.Close();
              //  FileConn.Dispose();
            }

            return 1;
        }
        public int Image_Upload(int proj_id, Byte[] DOC_FILE, long length, int type, int ID,string connection)
        {
            SqlConnection FileConn;
            SqlCommand cmdUploadDoc;


            try
            {
                FileConn = DALSQL.getConnection(connection);


                cmdUploadDoc = new SqlCommand("Image_Upload", FileConn);
                cmdUploadDoc.CommandType = CommandType.StoredProcedure;

                cmdUploadDoc.Parameters.Add("@Proj_ID ", SqlDbType.Int);
                cmdUploadDoc.Parameters.Add("@Doc ", SqlDbType.Image);
                cmdUploadDoc.Parameters.Add("@FileType ", SqlDbType.NVarChar, 50);
                cmdUploadDoc.Parameters.Add("@length ", SqlDbType.Int);
                cmdUploadDoc.Parameters.Add("@ID ", SqlDbType.Int);

                cmdUploadDoc.Parameters[0].Value = proj_id;
                cmdUploadDoc.Parameters[1].Value = DOC_FILE;
                cmdUploadDoc.Parameters[2].Value = type;
                cmdUploadDoc.Parameters[3].Value = length;

                cmdUploadDoc.Parameters[4].Value = ID;

                // FileConn.Open();
                cmdUploadDoc.ExecuteNonQuery();




            }
            catch (Exception ex)
            {
                throw (ex);


            }
            finally
            {

            }

            return 1;
        }
        public int Logo_Save(string[] _param, byte[] logo, string connection, string spname,int isEdit)
        {
            
                SqlConnection FileConn;
                SqlCommand cmdUploadDoc;


                try
                {
                    FileConn = DALSQL.getConnection(connection);


                    cmdUploadDoc = new SqlCommand(spname, FileConn);
                    cmdUploadDoc.CommandType = CommandType.StoredProcedure;

                    cmdUploadDoc.Parameters.Add("@companyName ", SqlDbType.NVarChar, 50);
                    cmdUploadDoc.Parameters.Add("@Doc ", SqlDbType.Image);
                    cmdUploadDoc.Parameters.Add("@Address1 ", SqlDbType.NVarChar, 50);
                    cmdUploadDoc.Parameters.Add("@Address2 ", SqlDbType.NVarChar, 50);
                    cmdUploadDoc.Parameters.Add("@isEdit ", SqlDbType.Int);

                    cmdUploadDoc.Parameters[0].Value = _param[0];
                    cmdUploadDoc.Parameters[1].Value = logo;
                    cmdUploadDoc.Parameters[2].Value = _param[1];
                    cmdUploadDoc.Parameters[3].Value = _param[2];
                    cmdUploadDoc.Parameters[4].Value = isEdit;



                    // FileConn.Open();
                    cmdUploadDoc.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            return 0;
            }
            
                
        
        


        
    }
}
       
