using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SecuLobbyVMS.App_Code
{
  public class DBSQL
  {

    public DataSet DBSds = new DataSet();
    public string[] Tab_Select = new string[10];
    public string[] Tables = new string[10];
    public bool datavalid = false;
    string DBcon_string;
    private SqlTransaction m_Trans;
    public DBSQL()
    {
      //
      // TODO: Add constructor logic here
      //
    }
    public SqlConnection DBConnect(string sConn)
    {
      string strConn = sConn;// MyConnection.ReadConStr(sConn);//  MyConnection.ReadConStr("Local");
                             //string strConn = ConfigurationManager.ConnectionStrings["connectionstring"];
      SqlConnection DBcon = new SqlConnection(strConn);
      if (DBcon.State == ConnectionState.Open)
      {
        DBcon.Close();
      }
      DBcon.Open();
      return DBcon;
    }

    public void DisconnectDB(SqlConnection Cconn)
    {
      //********* DisConnecting from the Sql Server
      if (Cconn.State == ConnectionState.Open)
      {
        Cconn.Close();

      }

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

    private static DataSet ComputateDiff(DataSet LocalDS, DataSet ServerDs)
    {
      DataSet diff = null;
      LocalDS.Merge(ServerDs);
      bool foundChanges = LocalDS.HasChanges();
      if (foundChanges)
      {
        diff = LocalDS.GetChanges();
      }
      return diff;
    }
    public static bool SyncDataset(DataSet SourceDataSet, string Tablename, string LocID, string sConn)
    {
      bool rbCal = true;
      SqlCommandBuilder SqlCB;
      SqlConnection _Centralconn = null;
      SqlTransaction trans = null;
      SqlDataAdapter da;
      try
      {
        string strConn = MyConnection.ReadConStr(sConn);
        _Centralconn = new SqlConnection(strConn);

        if (_Centralconn.State == ConnectionState.Open)
        {
          _Centralconn.Close();
        }
        _Centralconn.Open();

        foreach (DataRow dr in SourceDataSet.Tables[0].Rows)
        {

          SqlCommand cmdOrl = new SqlCommand(Tablename + "_Sync");
          cmdOrl.CommandType = CommandType.StoredProcedure;

          foreach (DataColumn c in dr.Table.Columns)  //loop through the columns. 
          {
            if (c.DataType.ToString() == "System.Byte[]")
            {
              cmdOrl.Parameters.Add(c.ColumnName, SqlDbType.Image);
              cmdOrl.Parameters[c.ColumnName].Value = dr[c.ColumnName];
              cmdOrl.Parameters[c.ColumnName].Direction = System.Data.ParameterDirection.Input;
            }
            else if (c.DataType.ToString() == "System.DateTime")
            {
              cmdOrl.Parameters.Add(c.ColumnName, SqlDbType.DateTime);
              cmdOrl.Parameters[c.ColumnName].Value = dr[c.ColumnName];
              cmdOrl.Parameters[c.ColumnName].Direction = System.Data.ParameterDirection.Input;
            }
            else if (c.DataType.ToString() == "System.Int")
            {
              cmdOrl.Parameters.Add(c.ColumnName, SqlDbType.Int);
              cmdOrl.Parameters[c.ColumnName].Value = dr[c.ColumnName].ToString();
              cmdOrl.Parameters[c.ColumnName].Direction = System.Data.ParameterDirection.Input;
            }
            else
            {
              cmdOrl.Parameters.Add(c.ColumnName, SqlDbType.NVarChar);
              cmdOrl.Parameters[c.ColumnName].Value = dr[c.ColumnName].ToString();
              cmdOrl.Parameters[c.ColumnName].Direction = System.Data.ParameterDirection.Input;
            }
          }


          string PKval = "";
          PKval = ExecuteSProcedure(cmdOrl, _Centralconn);
          cmdOrl = null;
        }



      }

      catch (SqlException ex)
      {
        Console.WriteLine(ex.Message);
        trans.Rollback();
        _Centralconn.Close();
        rbCal = false;
      }

      return rbCal;
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
    //private Int64 EY_MasterInterfaceDS( string procedure, string Operation, List<SqlDbParam> list, DataSet ds, out string errcode, out string message)
    //{
    //    SqlParameter param = null;
    //    DataTable table = null;
    //    object value = null;
    //    SqlConnection _Centralconn = null;

    //    try
    //    {

    //        string strConn = MyConnection.ReadConStr("Central");
    //        _Centralconn = new SqlConnection(strConn);

    //        if (_Centralconn.State == ConnectionState.Open)
    //        {
    //            _Centralconn.Close();
    //        }
    //        _Centralconn.Open();

    //        using (SqlCommand cmdSQL = new SqlCommand(procedure))
    //        {
    //            cmdSQL.CommandType = CommandType.StoredProcedure;

    //            foreach (SqlDbParam o in list)
    //            {
    //                if (o.Value == DBNull.Value)
    //                {
    //                    value = null;
    //                }
    //                else
    //                {
    //                    value = o.Value;
    //                }

    //                param = new SqlParameter(o.Name, value);
    //                if (Enum.IsDefined(typeof(ParameterDirection), o.Direction))
    //                {
    //                    param.Direction = (ParameterDirection)param.Direction;
    //                }

    //                if (Enum.IsDefined(typeof(SqlDbType), o.ParamType))
    //                {
    //                    param.SqlDbType = (SqlDbType)o.ParamType;
    //                }
    //                if (o.Size > 0) { param.Size = o.Size; }

    //                switch (param.SqlDbType)
    //                {
    //                    case SqlDbType.DateTime:
    //                        if (param.Value != null)
    //                        {
    //                            if (Convert.ToDateTime(param.Value) <= DateTime.MinValue)
    //                            {
    //                                param.Value = null;
    //                            }
    //                        }
    //                        break;

    //                    case SqlDbType.Structured:
    //                        if (ds != null && ds.Tables.Count > 0)
    //                        {
    //                            table = ds.Tables[o.TableName];
    //                            if (table.Rows.Count > 0)
    //                            {
    //                                //oDAL.OpenConnection(strConnection);
    //                                DataTable tblConverted = ConvertDataTableToTabType(table, o.TableTypeName, "dbo.TABTPTO_TABLE");
    //                                //table = StaticDef.ConvertToTableType(strConnection, table, o.TableTypeName);
    //                                param.Value = tblConverted;
    //                            }
    //                            else
    //                            {
    //                                param.Value = null;
    //                            }
    //                        }
    //                        break;
    //                }

    //                cmdSQL.Parameters.Add(param);
    //            }
    //            param = new SqlParameter("@Operation", Operation);
    //            cmdSQL.Parameters.Add(param);
    //            //oDAL.OpenConnection(strConnection);


    //            return 0;//ExecuteProcedureWithReturn(cmdSQL, out  errcode, out  message);
    //        }
    //    }
    //    catch { throw; }
    //    finally {   }
    //}
    //public long ExecuteProcedureWithReturn(SqlCommand cmd, out string errCode, out string message)
    //{
    //    long num=0;
    //    try
    //    {
    //        cmd.Connection = this.oConnection;
    //        cmd.CommandTimeout = this.Timeout;
    //        if (this.oTransaction != null)
    //        {
    //            cmd.Transaction = this.oTransaction;
    //        }
    //        this.AddDefaultOutParameterswithMsg(cmd);
    //        cmd.ExecuteNonQuery();
    //        num = this.GetDefaultOutParameterValue(cmd, out errCode, out message);
    //    }
    //    catch (Exception ex)
    //    {
    //        num = 5;
    //        errCode = "Error";
    //        message = ex.Message.ToString();
    //        throw ex;
    //    }
    //    return num;
    //}

    public DataTable ConvertDataTableToTabType(DataTable tblSource, string strTabTypeName, string Spname)
    {
      //dbo.TABTPTO_TABLE
      SqlParameter sqlparam = new SqlParameter("@tabtype_name", strTabTypeName);
      List<SqlParameter> lstparams = new List<SqlParameter>();
      lstparams.Add(sqlparam);
      DataTable tbltabtype = null;
      try
      {
        //  tbltabtype = GetDataTable(tblSource.TableName, Spname, CommandType.StoredProcedure, lstparams);
        if (tbltabtype != null)
        {
          foreach (DataRow srRow in tblSource.Rows)
          {
            DataRow nr = tbltabtype.NewRow();
            foreach (DataColumn dcol in tbltabtype.Columns)
            {

              if (tblSource.Columns.Contains(dcol.ColumnName))
              {
                nr[dcol.ColumnName] = srRow[dcol.ColumnName];
              }

            }
            tbltabtype.Rows.Add(nr);
          }
        }
      }
      catch
      {
        throw;
      }
      return tbltabtype;
    }
    public DataSet GetDataset(string SPName, String[,] _params, string sConn)
    {

      SqlCommand sCmd = new SqlCommand(SPName);
      SqlConnection _conn = new SqlConnection();
      _conn = DBConnect(sConn);
      sCmd.Connection = _conn;
      //System.Data.DataSet ds = new DataSet(DsName);
      SqlDataAdapter dbAdapter;

      SqlParameter[] _Sqlparam = new SqlParameter[_params.GetLength(0)];
      for (int i = 0; i < _params.GetLength(0); i++)
      {
        _Sqlparam[i] = new SqlParameter(_params[i, 0], _params[i, 1]);

      }
      int _paramsLength = _Sqlparam.Length;
      try
      {

        for (int i = 0; i < _paramsLength; i++)
        {
          //Add values as parameters
          sCmd.Parameters.Add(_Sqlparam[i]);
          sCmd.Parameters[sCmd.Parameters.Count - 1].Direction = ParameterDirection.Input;
        }


        sCmd.CommandType = CommandType.StoredProcedure;
        // sCmd.Parameters.Add(new SqlParameter(Tbname, SqlDbType.NVarChar)).Direction = ParameterDirection.Output;
        DataSet Ds = new DataSet();
        SqlDataAdapter sAdp = new SqlDataAdapter(sCmd);
        sAdp.Fill(Ds);
        return Ds;





      }
      catch (Exception ex)
      {
        throw ex;
      }






    }
    public int AttachImageSave(string ID, byte[] blob, string Itype, string sConn)
    {
      try
      {
        SqlConnection m_Connection = new SqlConnection(MyConnection.ReadConStr(sConn));
        m_Connection.Open();
        if (Itype == "1")
        {
          String dqry = "Delete from Visiter_IMAGE where Type= '" + Itype + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, "S2");
          dt.Dispose();
        }
        else
        {
          //String dqry = "update Visiter_IMAGE set  where Type= '" + Itype + "' and  ID=" + ID + ";";
          //DataTable dt = null;
          //dt = fetchRecords(dqry);
          //dt.Dispose();
        }

        if (Itype == "1")
        {
          String dqry = "Update dbo.Visiter_Registration set PPhoto=1 where   ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, "S2");
          dt.Dispose();
        }

        if (Itype == "3")
        {
          String dqry = "Update dbo.Visiter_Registration set PATTACH=1 where   ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, "S2");
          dt.Dispose();
        }

        if (Itype == "4")
        {
          String dqry = "Update dbo.Visiter_Registration set PATTACH=1 where  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, "S2");
          dt.Dispose();
        }

        string qry = "insert into Visiter_IMAGE (ID,LENGTH,PICTURE,TYPE) values(@id,@length,@imageData,@type)";
        SqlCommand SqlCom = new SqlCommand(qry, m_Connection);
        SqlCom.Parameters.Add(new SqlParameter("@id", ID));
        SqlCom.Parameters.Add(new SqlParameter("@length", blob.Length));
        SqlCom.Parameters.Add(new SqlParameter("@imageData", (object)blob));
        SqlCom.Parameters.Add(new SqlParameter("@type", Itype));

        // m_Connection.Open();
        SqlCom.ExecuteNonQuery();
        m_Connection.Close();
        SqlCom.Dispose();
        return 1;
      }
      catch (Exception e)
      {
        return 0;
      }
    }
    public static DataTable fetchRecords(String _query, string sConn)
    //this is a newly added methods which takes only a query as param
    //this is mainly used for lists
    {

      SqlConnection _conn = new SqlConnection(MyConnection.ReadConStr(sConn));

      _conn.Open();
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
        _conn.Close();
        _command.Dispose();
        if (_conn != null && _conn.State == ConnectionState.Open)
        {
          _conn.Close();
          _conn.Dispose();

        }
      }
      return _dataTable;
    }
    public DataTable fetchQueryRecords(string strQury)
    {
      DataSet ds = null;
      try
      {
        string strtab = "StrTable";

        ds = GetDS(strQury, strtab, "");
      }
      catch (Exception ex)
      {
        ds = null;
        throw (ex);
      }
      finally
      {

      }
      return ds.Tables[0];
    }

    public DataSet GetDataset(string DataSetName, string TableName,
                           string strSQL, CommandType cmdType, SqlParameter[] Parameters, string sConn)
    {
      SqlCommand cmd;
      SqlDataAdapter dbAdapter;
      SqlConnection m_Connection = new SqlConnection(MyConnection.ReadConStr(sConn));

      try
      {
        m_Connection.Open();
        System.Data.DataSet ds = new DataSet(DataSetName);
        cmd = new SqlCommand(strSQL, m_Connection);
        cmd.CommandType = cmdType;
        foreach (SqlParameter objParam in Parameters)
        {
          cmd.Parameters.Add(objParam);
        }
        dbAdapter = new SqlDataAdapter(cmd);
        dbAdapter.Fill(ds, TableName);
        return ds;
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        dbAdapter = null;
        cmd = null;
        if (m_Connection.State == ConnectionState.Open)
        {
          m_Connection.Close();
        }
      }
    }
    public int ImageSave(string ID, byte[] blob, string Itype, string iPath, string Remarks, String sConn)
    {
      string qry = "";
      try
      {
        SqlConnection m_Connection = new SqlConnection(MyConnection.ReadConStr(sConn));
        m_Connection.Open();
        if (Itype == "1")
        {
          String dqry = "Delete from Attach_Image where Type= '" + Itype + "' and iPath= '" + iPath + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, sConn);
          dt.Dispose();
          qry = "insert into Attach_Image (ID,IPath,TYPE,Remarks) values(@id,@iPath,@type,@Remarks)";
        }

        else if (Itype == "2")
        {
          String dqry = "Delete from Attach_Image where Type= '" + Itype + "' and iPath= '" + iPath + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, sConn);
          dt.Dispose();
          qry = "insert into Attach_Image (ID,IPath,TYPE,Remarks) values(@id,@iPath,@type,@Remarks)";
        }
        else if (Itype == "3")
        {
          String dqry = "Delete from Attach_Image where Type= '" + Itype + "' and iPath= '" + iPath + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, sConn);
          dt.Dispose();
          qry = "insert into Attach_Image (ID,IPath,TYPE,Remarks) values(@id,@iPath,@type,@Remarks)";
        }
        else if (Itype == "4")
        {
          String dqry = "Delete from Attach_Image where Type= '" + Itype + "' and iPath= '" + iPath + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, sConn);
          dt.Dispose();
          qry = "insert into Attach_Image (ID,IPath,TYPE,Remarks) values(@id,@iPath,@type,@Remarks)";
        }
        else if (Itype == "J")
        {
          String dqry = "Delete from JobAttach_Image where Type= '" + Itype + "' and iPath= '" + iPath + "' and  ID=" + ID + ";";
          DataTable dt = null;
          dt = fetchRecords(dqry, sConn);
          dt.Dispose();
          qry = "insert into JobAttach_Image (ID,IPath,TYPE,Remarks) values(@id,@iPath,@type,@Remarks)";
        }
        else
        {
          //String dqry = "update Visiter_IMAGE set  where Type= '" + Itype + "' and  ID=" + ID + ";";
          //DataTable dt = null;
          //dt = fetchRecords(dqry);
          //dt.Dispose();
        }

        //if (Itype == "1")
        //{
        //    String dqry = "Update IMAGE_Data set PPhoto=1 where   ID=" + ID + ";";
        //    DataTable dt = null;
        //    dt = fetchRecords(dqry, sConn);
        //    dt.Dispose();
        //}

        //if (Itype == "3")
        //{
        //    String dqry = "Update IMAGE_Data set PATTACH=1 where   ID=" + ID + ";";
        //    DataTable dt = null;
        //    dt = fetchRecords(dqry, sConn);
        //    dt.Dispose();
        //}

        //if (Itype == "4")
        //{
        //    String dqry = "Update IMAGE_Data set PATTACH=1 where  ID=" + ID + ";";
        //    DataTable dt = null;
        //    dt = fetchRecords(dqry, sConn);
        //    dt.Dispose();
        //}


        SqlCommand SqlCom = new SqlCommand(qry, m_Connection);
        SqlCom.Parameters.Add(new SqlParameter("@id", ID));
        // SqlCom.Parameters.Add(new SqlParameter("@length", 0));
        // SqlCom.Parameters.Add(new SqlParameter("@imageData", (object)blob));
        SqlCom.Parameters.Add(new SqlParameter("@iPath", iPath));
        SqlCom.Parameters.Add(new SqlParameter("@type", Itype));
        SqlCom.Parameters.Add(new SqlParameter("@Remarks", Remarks));

        // m_Connection.Open();
        SqlCom.ExecuteNonQuery();
        m_Connection.Close();
        SqlCom.Dispose();
        return 1;
      }
      catch (Exception e)
      {
        return 0;
      }
    }
    //public static DataTable AttfetchRecords(String _query,string sConn)
    ////this is a newly added methods which takes only a query as param
    ////this is mainly used for lists
    //{

    //    SqlConnection _conn = new SqlConnection(MyConnection.ReadConStr(sConn));

    //    _conn.Open();
    //    SqlCommand _command = new SqlCommand();
    //    _command.Connection = _conn;
    //    _command.CommandText = _query;
    //    SqlDataAdapter _adapter = new SqlDataAdapter(_command);
    //    DataTable _dataTable = null;
    //    _dataTable = new DataTable("_Tab1");

    //    try
    //    {
    //        _adapter.Fill(_dataTable);
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }
    //    finally
    //    {
    //        _conn.Close();
    //        _command.Dispose();
    //        if (_conn != null && _conn.State == ConnectionState.Open)
    //        {
    //            _conn.Close();
    //            _conn.Dispose();

    //        }
    //    }
    //    return _dataTable;
    //}

    public void ExecutenonQueryExt(String _query, String sConn)
    {

      SqlConnection _conn = new SqlConnection(MyConnection.ReadConStr(sConn));

      _conn.Open();
      SqlCommand _command = new SqlCommand();
      _command.Connection = _conn;
      _command.CommandText = _query;


      try
      {
        _command.ExecuteNonQuery();
      }
      catch (Exception e)
      {
        throw e;
      }
      finally
      {
        _conn.Close();
        _command.Dispose();
        if (_conn != null && _conn.State == ConnectionState.Open)
        {
          _conn.Close();
          _conn.Dispose();

        }
      }

    }
    //public void ExecutenonQueryatch(dynamic _query)
    //{

    //    SqlConnection _conn = new SqlConnection(MyConnection.ReadConStr("Local"));

    //    _conn.Open();
    //    SqlCommand _command = new SqlCommand();
    //    _command.Connection = _conn;


    //    try
    //    {
    //        _command.CommandText = _query;

    //        _command.ExecuteNonQuery();
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }
    //    finally
    //    {
    //        _conn.Close();
    //        _command.Dispose();
    //        if (_conn != null && _conn.State == ConnectionState.Open)
    //        {
    //            _conn.Close();
    //            _conn.Dispose();

    //        }
    //    }

    //}


    //accepts stored procedure name a string & values as Sqlparameters     1
    public string[] executeSP(String _spName, SqlParameter[] _params, string sConn)
    {
      SqlConnection _conn = null;
      SqlTransaction _trans = null;
      string[] returnValue;
      try
      {
        _conn = new SqlConnection(MyConnection.ReadConStr(sConn));
        _conn.Open();
        _trans = _conn.BeginTransaction();
        // begins a transaction and 
        //calls executeSP with procedurename,values & transaction

        returnValue = executeSP(_spName, _params, _trans, sConn);
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
    //  2
    public static string[] executeSP(String _spName, SqlParameter[] _params, SqlTransaction _trans, string sConn)
    {

      SqlCommand _command = null;
      string[] valuesReturned; // declare valuesRetuned as an int array to store 
      try
      {
        _command = new SqlCommand();
        _command.Connection = _trans.Connection;
        //_command.Transaction = _trans;
        _command.CommandType = CommandType.StoredProcedure; // sets the type of command to stored procedure
        _command.CommandText = _spName; //sets command text as procedurename to be executed

        //An additional parameter is added
        //which fetches the value returned by the procedure
        //which is usally the new id when a new record is inserted
        //this id is used incase if we have transaction/detail tables
        //as a foreign key
        SqlParameter returnParam = _command.Parameters.Add("v_Newid", SqlDbType.NVarChar, 8);
        int _paramsLength = _params.Length;
        for (int i = 0; i < _paramsLength; i++)
        {
          //Add values as parameters
          _command.Parameters.Add(_params[i]);
          _command.Parameters[_command.Parameters.Count - 1].Direction = ParameterDirection.Input;
        }
        returnParam.Value = 0;
        returnParam.Direction = ParameterDirection.InputOutput;

        valuesReturned = new string[2]; //records effected & New ID returned during insertion


        //stores recordsAffected variable recordsAffected
        int recordsAffected = _command.ExecuteNonQuery();

        //stores new ID to variable newIDReturned incase of insertion
        string newIDReturned = Convert.ToString(returnParam.Value);

        valuesReturned[0] = newIDReturned;
        valuesReturned[1] = Convert.ToString(recordsAffected);

      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        _command.Dispose();
      }
      return valuesReturned;
    }


    public DataSet GetDS(string sqlstr, string ls_table, string sConn)
    {

      SqlConnection _conn = new SqlConnection();
      _conn = DBConnect(sConn);



      //Get or Retrive the dataset from a particular table in MSSQL and fill it in dataset 
      SqlDataAdapter adapter = new SqlDataAdapter();
      adapter.SelectCommand = new SqlCommand(sqlstr, _conn);
      adapter.Fill(DBSds, ls_table);

      Tab_Select[DBSds.Tables.Count - 1] = sqlstr;

      return DBSds;
    }



    public bool UpdDs(SqlConnection conn, string Tablename)
    {
      // Save or Update  one table  in MSSQL database 

      SqlCommandBuilder SqlCB;

      SqlTransaction trans = null;
      SqlDataAdapter da;
      try
      {
        trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
        //for (int i=0;i<=Tab_Select.GetUpperBound(0);i++)
        //{

        //if(Tab_Select[i] != null)
        //{
        //da = new SqlDataAdapter();
        //da.SelectCommand = new SqlCommand(Tab_Select[i],conn,trans);
        //custCB = new SqlCommandBuilder(da);
        //da.Update(DBSds, DBSds.Tables[i].TableName);
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand("select * from " + Tablename, conn, trans);
        SqlCB = new SqlCommandBuilder(da);
        // Console.WriteLine(SqlCB.GetInsertCommand().CommandText);
        da.Update(DBSds, Tablename);


        //}

        //	}
        trans.Commit();
        return true;
      }

      catch (SqlException ex)
      {
        Console.WriteLine(ex.Message);
        trans.Rollback();

        return false;
      }
    }


    public string fetchValues(string _query, string sConn)
    {
      string Val;
      SqlConnection _conn = new SqlConnection();
      _conn = DBConnect(sConn);
      SqlCommand sCmd = new SqlCommand(_query, _conn);

      sCmd.Connection = _conn;


      try
      {
        Val = Convert.ToString(sCmd.ExecuteScalar());
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        sCmd.Dispose();
        if (_conn != null && _conn.State == ConnectionState.Open)
        {
          _conn.Close();
          _conn.Dispose();
        }

      }
      return Val;
    }


    public int ExecuteStoredProcedure(string strConnection, SqlCommand cmdSQL)
    {
      SqlConnection m_Connection = new SqlConnection(strConnection);



      try
      {
        m_Connection.Open();
        cmdSQL.Connection = m_Connection;
        cmdSQL.ExecuteNonQuery();
        return Convert.ToInt32(cmdSQL.Parameters[0].Value);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    public string ExecuteStoredProcedureOutParam(SqlCommand cmdOrl, string sConn)
    {
      SqlConnection m_Connection = new SqlConnection();
      //
      m_Connection = DBConnect(sConn);
      try
      {
        cmdOrl.Connection = m_Connection;


        string recordsAffected = "";
        try
        {
          recordsAffected = cmdOrl.ExecuteScalar().ToString();
        }
        catch
        {
          recordsAffected = "";
        }
        //  string newIDReturned = Convert.ToString(returnParam.Value);

        return recordsAffected;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public DataTable ExecuteStoredProcedureDataTable(SqlCommand cmdOrl, string sConn)
    {
      SqlConnection m_Connection = new SqlConnection();
      //
      DataTable dt = new DataTable();
      m_Connection = DBConnect(sConn);
      try
      {

        cmdOrl.Connection = m_Connection;
        cmdOrl.CommandTimeout = 0;
        //SqlParameter returnParam = cmdOrl.Parameters.Add("v_Newid", SqlDbType.NVarChar, 8);
        //returnParam.Value = 0;
        //returnParam.Direction = ParameterDirection.InputOutput;
        SqlDataAdapter da = new SqlDataAdapter(cmdOrl);

        da.Fill(dt);
        //  string newIDReturned = Convert.ToString(returnParam.Value);

        return dt;
      }
      catch (Exception ex)
      {

      }
      return dt;
    }
    public string ExecuteStoredProcedure(SqlCommand cmdOrl, string sConn)
    {
      SqlConnection m_Connection = new SqlConnection();
      //
      m_Connection = DBConnect(sConn);
      try
      {
        cmdOrl.Connection = m_Connection;
        cmdOrl.CommandTimeout = 0;
        //SqlParameter returnParam = cmdOrl.Parameters.Add("v_Newid", SqlDbType.NVarChar, 8);
        //returnParam.Value = 0;
        //returnParam.Direction = ParameterDirection.InputOutput;


        string recordsAffected = Convert.ToString(cmdOrl.ExecuteNonQuery());
        //  string newIDReturned = Convert.ToString(returnParam.Value);
        //if (m_Connection.State == ConnectionState.Open)
        //{
        m_Connection.Close();
        //}
        return recordsAffected;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    //public DataSet GetDataset(string strConnection, string DataSetName, string TableName,
    //                         string strSQL, CommandType cmdType, SqlParameter[] Parameters)
    //{
    //    SqlCommand cmd;
    //    SqlDataAdapter dbAdapter;
    //    SqlConnection m_Connection = new SqlConnection(strConnection);

    //    try
    //    {
    //        m_Connection.Open();
    //        System.Data.DataSet ds = new DataSet(DataSetName);
    //        cmd = new SqlCommand(strSQL, m_Connection);
    //        cmd.CommandType = cmdType;
    //        foreach (SqlParameter objParam in Parameters)
    //        {
    //            cmd.Parameters.Add(objParam);
    //        }
    //        dbAdapter = new SqlDataAdapter(cmd);
    //        dbAdapter.Fill(ds, TableName);
    //        return ds;
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;
    //    }
    //    finally
    //    {
    //        dbAdapter = null;
    //        cmd = null;
    //        if (m_Connection.State == ConnectionState.Open)
    //        {
    //            m_Connection.Close();
    //        }
    //    }
    //}



  }
  public class SqlDbParam
  {
    public string Name { get; set; }
    public object Value { get; set; }
    public int ParamType { get; set; }
    public int Size { get; set; }
    public ParameterDirection Direction { get; set; }
    public string TableTypeName { get; set; }
    public string TableName { get; set; }
  }
}
