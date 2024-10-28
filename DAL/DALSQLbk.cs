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
                _conn = DALSQL.getConnection(connectionString);
                oCmd.Connection = _conn;
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
        public static SqlConnection getConnection(string _connectionString)
        {

          // string _connectionString;
           // string FName = CONFIG_FILE;
            SqlConnection _connection = null;
           // TextReader tr = null;

            try
            {
              //  tr = new StreamReader(Application.LocalUserAppDataPath + FName);
               // _connectionString = tr.ReadLine();
                //SqlConnection conn = new SqlConnection("Data Source=SHERMIN\\SQLEXPRESS;Initial Catalog=BadgeMaker;User ID=shermin;Password=shermin;Persist Security Info=False;");
                //_connectionString = "Data Source=SC-9D2149FB3AFC\\SQLEXPRESS;Initial Catalog=BadgeMaker;User ID=sa;Password=sa123;Persist Security Info=False";
               // _connectionString =  Program.gConnString;
                
                //Password=sa123;Persist Security Info=False;User ID=sa;Initial Catalog=BadgeMaker;Data Source=SC-9D2149FB3AFC/SQLEXPRESS;Workstation ID=SC-9D2149FB3AFC/SQLEXPRESS;";
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
    }
}
       
