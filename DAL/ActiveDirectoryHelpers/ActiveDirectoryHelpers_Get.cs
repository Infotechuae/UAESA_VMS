

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.ADConnectors
{
    public partial class ActiveDirectoryHelpers
    {

         public static  List<ActiveDirectoryRecord> GetRecords(string ServiceAccountName,string ServiceAccountPassword, string DomainPath ,List<string> OUS)
        {

            try
            {
                var Employees = new List<ActiveDirectoryRecord>();

             

                foreach (var OU in OUS)
                {
                    using (var _PrincipalContext = new PrincipalContext(ContextType.Domain, DomainPath, OU, ServiceAccountName, ServiceAccountPassword))
                    {

                        using (var _PrincipalSearcher = new PrincipalSearcher(new UserPrincipal(_PrincipalContext)))
                        {
                            var Results = _PrincipalSearcher.FindAll();

                            foreach (Principal Result in Results)
                            {

                                var Employee = GetActiveDirectoryRecordFromPrincipal(Result);
                                CreateLog("Employee Code :" + Employee.EmployeeCode + " ,Employee Name :" + Employee.FullName);

                                if (!string.IsNullOrWhiteSpace(Employee.EmployeeCode))
                                {
                                    Employee.Unit = !string.IsNullOrWhiteSpace(Employee.Unit) ? Employee.Unit : "N-A";
                                    Employee.Department = !string.IsNullOrWhiteSpace(Employee.Department) ? Employee.Department : "N-A";
                                    Employee.FullName = !string.IsNullOrWhiteSpace(Employee.FullName) ? Employee.FullName : "N-A";
                                    Employees.Add(Employee);
                                }
                            }
                        }
                    }
                }

                return Employees;
            }
            catch (System.Exception ex)
            {
                var Error = GetFullExceptionDetails(ex);

             
            
                Console.WriteLine(Error);
                return new List<ActiveDirectoryRecord>();
            }
        }
        public static void CreateLog(string sMessage)
        {

            //System.Data.DataRow drUser = ((System.Data.DataRow)System.Web.HttpContext.Current.Session["UserInfoRow"]);
            string sUserID = "System";
            string sError = Environment.NewLine + "Date and Time : " + DateTime.Now + " ; Message : " + sMessage;
            string InitialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "Temp", "Log");
            if (!System.IO.Directory.Exists(InitialPath)) //Log Folder Checking 
            {
                System.IO.Directory.CreateDirectory(InitialPath);
            }
            string SubFolderRelativePath = Convert.ToString(DateTime.Today.ToString("dd-MM-yyyy"));//First SubFolder Name

            string subfolderPath = System.IO.Path.Combine(InitialPath, SubFolderRelativePath);//First Sub Folder Path 
            string subOfsubFolder = System.IO.Path.Combine(subfolderPath, Convert.ToString(sUserID));//Second SubFolder Path 
            string tempFilePath = System.IO.Path.Combine(subOfsubFolder, Convert.ToString(DateTime.Today.ToString("dd-MM-yyyy")));// Text File Path

            System.IO.DirectoryInfo tempFolder = new System.IO.DirectoryInfo(InitialPath); //Initial Path
            System.IO.DirectoryInfo newTempPath = new System.IO.DirectoryInfo(subfolderPath); //First Sub Folder Path in DirectoryInfo

            string[] sErr = { sError };

            if (!System.IO.Directory.Exists(subfolderPath)) // First Sub Folder Check
            {
                System.IO.DirectoryInfo subFolder = tempFolder.CreateSubdirectory(SubFolderRelativePath); // First SubFolder Create Using Date(Folder Name)


                #region CreateSubFolderOfSubFolder
                System.IO.DirectoryInfo subfolderOfSub = subFolder.CreateSubdirectory(Convert.ToString(sUserID));// Second SubFolder Create Using UserID(Folder Name)
                #endregion

                System.IO.File.WriteAllLines(tempFilePath, sErr); // File Creation
            }
            else
            {
                #region CreateSubFolder
                if (!System.IO.Directory.Exists(subOfsubFolder))  // Second Sub Folder Check
                {
                    #region CreateSubFolderOfSubFolder
                    System.IO.DirectoryInfo subfolderOfSub = newTempPath.CreateSubdirectory(Convert.ToString(sUserID));// Second SubFolder Create Using UserID(Folder Name)
                    System.IO.File.WriteAllLines(tempFilePath, sErr);
                    #endregion
                }
                else
                {
                    System.IO.File.AppendAllText(tempFilePath, sError);
                }

                #endregion
            }

        }
        //public static async Task<ActiveDirectoryRecord> GetRecordFromActiveDirectoryByUserName(string UserName, string ServiceAccountName, string ServiceAccountPassword, string DomainPath, List<string> OUS)
        //{

        //    try
        //    {
        //        var Employee = new ActiveDirectoryRecord();



        //        foreach (var OU in OUS)
        //        {
        //            using (var _PrincipalContext = new PrincipalContext(ContextType.Domain, DomainPath, OU, ServiceAccountName, ServiceAccountPassword))
        //            {
        //                using (var _PrincipalSearcher = new PrincipalSearcher(new UserPrincipal(_PrincipalContext)))
        //                {
        //                    var _UserPrincipal = new UserPrincipal(_PrincipalContext);
        //                    _UserPrincipal.UserPrincipalName = UserName;
        //                    _PrincipalSearcher.QueryFilter = _UserPrincipal;
        //                    var Result = _PrincipalSearcher.FindOne();
        //                    if (Result != null)
        //                    {
        //                        Employee = GetActiveDirectoryRecordFromPrincipal(Result);
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        return Employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        var Error = GetFullExceptionDetails(ex);



        //        Console.WriteLine(Error);
        //        return new ActiveDirectoryRecord();
        //    }
        //}















    }
}