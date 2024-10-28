using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace DAL
{
    public class DBConnection
    {


        #region Class Level Variables

        public System.Data.SqlClient.SqlConnection oConnection;
        [NonSerialized()]
        public System.Data.SqlClient.SqlTransaction oTransaction;
        private System.Data.SqlClient.SqlDataAdapter oAdapter = new System.Data.SqlClient.SqlDataAdapter();
        private string sConnectionString;
        private bool IsConnectionOpen = false;
        public string sError = string.Empty;
        public string sErrorSQL = string.Empty;
        public string sExceptionMessage = string.Empty;
        private bool bIsTransactionOn = false;
        #endregion

        #region Class Level Properties
        public bool IsConnected
        {
            get { return (oConnection.State == ConnectionState.Open); }
        }
        private bool IsTransactionOn
        {
            get { return bIsTransactionOn; }
            set { bIsTransactionOn = value; }
        }
        #endregion


        public DBConnection(String sCoonectionString)
        {
            sConnectionString = sCoonectionString;
            try
            {

                oConnection = new System.Data.SqlClient.SqlConnection(sConnectionString);
                //oConnection.ConnectionString = sConnectionString;
                oConnection.Open();
                IsConnectionOpen = true;
            }
            catch (Exception ex)
            {
                sExceptionMessage = ex.Message;
            }
            oConnection.Close();
            oAdapter = new System.Data.SqlClient.SqlDataAdapter();
        }



        public bool IsConnectionSuccessFull
        {
            get
            {
                return IsConnectionOpen;
            }
        }
        public System.Data.SqlClient.SqlCommand GetCommand(string sSQL)
        {
            ////Connection object
            //string sConnectionString = "Server=IBMSERVER; Database=MyDB; UID=sa; PWD=citytech;";
            //oConnection = new System.Data.SqlClient.SqlConnection(sConnectionString);
            //oConnection.Open();


            System.Data.SqlClient.SqlCommand oCommand = default(System.Data.SqlClient.SqlCommand);
            oCommand = new System.Data.SqlClient.SqlCommand();
            oCommand.CommandTimeout = 0;
            oCommand.CommandText = sSQL;
            oCommand.Connection = oConnection;
            oCommand.CommandType = CommandType.Text;
            oCommand.Transaction = oTransaction;


            return oCommand;
        }

        public void SetError(System.Data.SqlClient.SqlException oErr, string sSQL)
        {
            sError = "Query Execution failed./r/n" + oErr.Message + "/r/n/r/n";
            sErrorSQL = sSQL;
        }

        public bool Execute(string sSQL)
        {
            System.Data.SqlClient.SqlCommand oCMD;
            int iRowsChanged = 0;
            bool IsConnectionOpened = false;
            bool IsExecute = true;

            if (sSQL != string.Empty)
            {
                IsConnectionOpened = CheckAndOpenConnection();
                oCMD = GetCommand(sSQL);
                try
                {
                    iRowsChanged = oCMD.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException xErr)
                {
                    SetError(xErr, sSQL);
                }
                finally
                {
                    if (IsConnectionOpened)
                    {
                        CheckAndCloseConnection();
                    }
                }
            }
            return IsExecute = (iRowsChanged > 0);

        }
        private bool CheckAndOpenConnection()
        {
            bool IsConnectionOpen = false;


            try
            {
                IsConnectionOpen = oConnection.State.Equals(ConnectionState.Open);
                if (!IsConnectionOpen)
                {
                    oConnection.Open();
                    IsConnectionOpen = oConnection.State.Equals(ConnectionState.Open);
                }
            }
            catch (Exception oException)
            {
                if (!IsConnectionOpen)
                {
                    oConnection.Open();
                    IsConnectionOpen = oConnection.State.Equals(ConnectionState.Open);
                }
                sExceptionMessage = oException.Message;
            }
            return IsConnectionOpen;
        }

        private void CheckAndCloseConnection()
        {
            if (IsConnected && (!IsTransactionOn))
            {
                oConnection.Close();
            }
        }

        public bool StartTransaction()
        {
            bool bIsStartTransaction = true;
            IsTransactionOn = false;
            CheckAndOpenConnection();
            try
            {
                oTransaction = oConnection.BeginTransaction();
                IsTransactionOn = true;
            }
            catch (System.Data.SqlClient.SqlException oEx)
            {
                SetError(oEx, "Initiate Transaction");
                bIsStartTransaction = false;
            }
            return bIsStartTransaction;
        }

        public System.Data.SqlClient.SqlTransaction StartTransaction(System.Data.IsolationLevel iTransactionLevel)
        {
            //bool bIsStartTransaction = true;
            oTransaction = null;
            IsTransactionOn = false;
            CheckAndOpenConnection();
            try
            {
                oTransaction = oConnection.BeginTransaction(iTransactionLevel);
                IsTransactionOn = true;
            }
            catch (System.Data.SqlClient.SqlException oEx)
            {
                SetError(oEx, "Initiate Transaction");
                IsTransactionOn = false;
                //bIsStartTransaction = false;
            }
            return oTransaction;
        }

        public bool Commit()
        {
            bool bIsCommit = true;
            try
            {
                oTransaction.Commit();
                IsTransactionOn = false;
                CheckAndCloseConnection();
            }
            catch (System.Data.SqlClient.SqlException xErr)
            {
                SetError(xErr, "Commit");
                bIsCommit = false;
            }
            return bIsCommit;
        }

        public bool RollBack()
        {
            bool bIsRollBack = true;
            try
            {
                oTransaction.Rollback();
                IsTransactionOn = false;
                CheckAndCloseConnection();
            }
            catch (System.Data.SqlClient.SqlException xErr)
            {
                SetError(xErr, "RollBack");
                bIsRollBack = false;
            }
            return bIsRollBack;
        }

        public object GetValue(string sSQL)
        {
            object oReturnValue = null;
            bool bIsConnectionedOpened = false;
            bIsConnectionedOpened = CheckAndOpenConnection();
            System.Data.SqlClient.SqlCommand oCMD;
            oCMD = GetCommand(sSQL);
            try
            {
                oReturnValue = oCMD.ExecuteScalar();
            }
            catch (System.Data.SqlClient.SqlException xErr)
            {
                SetError(xErr, sSQL);
            }
            finally
            {
                if (bIsConnectionedOpened)
                {
                    CheckAndCloseConnection();
                }
            }
            return oReturnValue;
        }

        public void Disconnect()
        {
            if (!IsTransactionOn)
            {
                oConnection.Close();
            }
        }

        public DataTable GetTable(string sSQL, DataSet oDataSet)
        {

            DataTable oTable = null;
            bool bIsConnectionOpened = false;
            bIsConnectionOpened = CheckAndOpenConnection();
            System.Data.SqlClient.SqlCommand oCMD = GetCommand(sSQL);

            oAdapter.SelectCommand = oCMD;

            try
            {
                oAdapter.Fill(oDataSet, "mytable");
                oTable = oDataSet.Tables[0];
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                SetError(ex, sSQL);
            }
            finally
            {
                if (bIsConnectionOpened)
                {
                    oConnection.Close();
                }

            }
            return oTable;
        }
        public DataSet GetTableSet(string sSQL)
        {
            DataSet oDataSet = new DataSet();
            bool bIsConnectionOpened = false;
            bIsConnectionOpened = CheckAndOpenConnection();
            System.Data.SqlClient.SqlCommand oCMD = GetCommand(sSQL);

            oAdapter.SelectCommand = oCMD;

            try
            {
                oAdapter.Fill(oDataSet, "mytable");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                SetError(ex, sSQL);
            }
            finally
            {
                if (bIsConnectionOpened)
                {
                    oConnection.Close();
                }

            }
            return oDataSet;
        }
        public DataTable GetTable(string sSQL, DataSet oDataSet, Dictionary<string, string> dList)
        {

            DataTable oTable = null;
            bool bIsConnectionOpened = false;
            bIsConnectionOpened = CheckAndOpenConnection();
            System.Data.SqlClient.SqlCommand oCMD = GetCommand(sSQL);

            if (dList != null && dList.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in dList)
                {
                    oCMD.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }

            }

            oAdapter.SelectCommand = oCMD;

            try
            {
                oAdapter.Fill(oDataSet, "mytable");
                oTable = oDataSet.Tables[0];
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                SetError(ex, sSQL);
            }
            finally
            {
                if (bIsConnectionOpened)
                {
                    oConnection.Close();
                }

            }
            return oTable;
        }


        public bool IsTableExist(string sTableName)
        {
            string sSQL = "select count(1) from information_schema.tables where table_name='" + sTableName + "'";
            return ((int)GetValue(sSQL) > 0);
        }
        public bool IsTriggerExists(string sTriggerName)
        {
            string sSQL = "select count(1) from sysobjects where xtype = 'tr' and name ='" + sTriggerName + "'";
            return ((int)GetValue(sSQL) > 0);
        }
        public bool IsIndexExists(string sIndexName)
        {
            string sSQL = "select count(1) from sysindexes where name ='" + sIndexName + "'";
            return ((int)GetValue(sSQL) > 0);
        }
        public void ExecuteWithoutTry(string sSQL)
        {
            System.Data.SqlClient.SqlCommand oCMD;
            int iRowChanged = 0;
            if (sSQL != string.Empty)
            {
                CheckAndOpenConnection();
                oCMD = GetCommand(sSQL);
                try
                {
                    iRowChanged = oCMD.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException xErr)
                {
                    SetError(xErr, sSQL);
                }
            }
        }

        public System.Data.SqlClient.SqlDataReader ExecuteReader(string sSQL)
        {
            System.Data.SqlClient.SqlDataReader rdr;
            rdr = null;
            if (sSQL != string.Empty)
            {
                CheckAndOpenConnection();
                System.Data.SqlClient.SqlCommand oCMD = new System.Data.SqlClient.SqlCommand(sSQL, oConnection);
                try
                {
                    rdr = oCMD.ExecuteReader();
                }
                catch (System.Data.SqlClient.SqlException xErr)
                {
                    SetError(xErr, sSQL);
                }
            }
            return rdr;
        }
    }
}
