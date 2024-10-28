

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
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