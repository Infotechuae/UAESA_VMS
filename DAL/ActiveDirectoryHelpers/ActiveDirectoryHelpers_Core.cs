
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
        public static string GetFullExceptionDetails(Exception Ex)
        {
            var Details = Ex.Message + Environment.NewLine;

            if (Ex.InnerException != null)
            {
                Details = Details + Ex.InnerException.ToString() + Environment.NewLine;
            }
            if (Ex.StackTrace != null)
            {
                Details = Details + Ex.StackTrace.ToString() + Environment.NewLine;
            }
            if (Ex.Source != null)
            {
                Details = Details + Ex.Source.ToString() + Environment.NewLine;
            }
            var JSON_TOKENS_ERROR = @"THE INPUT DOES NOT CONTAIN ANY JSON TOKENS. EXPECTED THE INPUT TO START WITH A VALID JSON TOKEN, WHEN ISFINALBLOCK IS TRUE";

            if (Details.ToUpper().Contains(JSON_TOKENS_ERROR.ToUpper()))
            {
                Details = "AN ERROR OCCURRED  , PLEASE CONTACT YOUR ADMIN";
            }
            return Details;
        }
        public static ActiveDirectoryRecord GetActiveDirectoryRecordFromPrincipal(Principal _Principal,bool IncludePhoto)
        {
            try
            {
                DirectoryEntry _DirectoryEntry = _Principal.GetUnderlyingObject() as DirectoryEntry;

                var Record = new ActiveDirectoryRecord();
                try
                {
                    var _UserName = _DirectoryEntry.Properties["userPrincipalName"].Value.ToString();
                    Record.UserName = _UserName.Trim();
                }
                catch
                {
                }
                try
                {
                    var Email = _DirectoryEntry.Properties["mail"].Value.ToString();
                    Record.Email = Email.Trim();
                }
                catch
                {
                }




                try
                {
                    var FullName = _DirectoryEntry.Properties["name"].Value.ToString();
                    Record.FullName = FullName.Trim();
                }
                catch
                {
                }

              
                   
                try
                {
                    var Unit = _DirectoryEntry.Properties["extensionAttribute4"].Value.ToString();
                    Record.Unit = Unit.Trim();
                }
                catch
                {
                }
                try
                {
                    var Department = _DirectoryEntry.Properties["department"].Value.ToString();
                    Record.Department = Department.Trim();
                }
                catch
                {
                }

                try
                {
                    var Designation = _DirectoryEntry.Properties["title"].Value.ToString();
                    Record.Designation = Designation.Trim();
                }
                catch
                {
                }
                try
                {
                    var SectionParts = Record.Designation.Split(',').ToList();
                    var Section = SectionParts[1].Trim();
                    Record.Section = Section;
                }
                catch
                {
                }

                try
                {
                    var Phone = _DirectoryEntry.Properties["ipPhone"].Value.ToString();
                    Record.Phone = Phone.Trim();
                }
                catch
                {
                }
                try
                {
                    var Manager = _DirectoryEntry.Properties["manager"].Value.ToString().Split(',')[0].Split('=')[1];
                    Record.ManagerName = Manager.Trim();
                }
                catch
                {
                }

                try
                {
                    var EmployeeCode = _DirectoryEntry.Properties["EmployeeID"].Value.ToString();
                    Record.EmployeeCode = EmployeeCode.Trim();
                }
                catch
                {
                }

                try
                {
                    var Extension = _DirectoryEntry.Properties["pager"].Value.ToString();
                    Record.Extension = Extension.Trim();
                }
                catch
                {
                }
                try
                {
                    var Company = _DirectoryEntry.Properties["company"].Value.ToString();
                    Record.Company = Company.Trim();
                }
                catch
                {
                }
                if(IncludePhoto)
                {
                    try
                    {
                        var PhotoObject = _DirectoryEntry.Properties["jpegPhoto"].Value;
                        var Photo = PhotoObject != null ? PhotoObject as byte[] : null;


                        var ThumbnailObject = _DirectoryEntry.Properties["thumbnailPhoto"].Value;
                        var Thumbnail = ThumbnailObject != null ? ThumbnailObject as byte[] : null;

                        if (Photo != null && Thumbnail == null)
                        {
                            Thumbnail = Photo;
                        }
                        if (Photo == null && Thumbnail != null)
                        {
                            Photo = Thumbnail;
                        }

                        if (Photo != null)
                        {
                          
                        }
                    }
                    catch
                    {

                    }

      


                }
                Record = BindServiceAccount(_Principal, Record);

                return Record;  
            }
            catch(System.Exception ex)
            {
                var Error=GetFullExceptionDetails(ex);
                return null;
            }
           
        }
        public static ActiveDirectoryRecord BindServiceAccount(Principal _Principal,ActiveDirectoryRecord Record)
        {
            try
            {
                var IsServiceAccount = false;
                DirectoryEntry _DirectoryEntry = _Principal.GetUnderlyingObject() as DirectoryEntry;
             
                var SERVICE_ACOUNT_OU = "OU=ADMIN ACCOUNTS,OU=GENERAL SERVICES";
                var SERVICE_ACCOUNT = "SERVICE ACCOUNT";

                var DistinguishedName = string.Empty;
                try
                {
                    DistinguishedName = _DirectoryEntry.Properties["distinguishedName"].Value.ToString();
                }
                catch
                {

                }

                var Description = string.Empty;
                try
                {
                   Description = _DirectoryEntry.Properties["description"].Value.ToString();
                }
                catch
                {

                }
              
                if (DistinguishedName.ToUpper().Contains(SERVICE_ACOUNT_OU)&& Description.ToUpper().Contains(SERVICE_ACCOUNT))
                {
                    IsServiceAccount = true;

                }
                try
                {
                  
                    if (IsServiceAccount)
                    {
                        Record.FullName = Description.ToUpper().Replace(SERVICE_ACCOUNT, string.Empty);
                        Record.FullName = Record.FullName + " # "+SERVICE_ACCOUNT;
                        Record.Company = SERVICE_ACCOUNT;
                        Record.Unit = SERVICE_ACCOUNT;
                        Record.Department = SERVICE_ACCOUNT;
                        Record.Section = SERVICE_ACCOUNT;
                        Record.EmployeeCode = SERVICE_ACCOUNT;
                        Record.Designation = SERVICE_ACCOUNT;   
                        Record.ManagerName = SERVICE_ACCOUNT;
                        Record.Phone = SERVICE_ACCOUNT;
                        Record.Extension = SERVICE_ACCOUNT;
                        Record.Email = Record.UserName;

                    }
                }
                catch
                {
                }



            }
            catch (System.Exception ex)
            {
                var Error = GetFullExceptionDetails(ex);



                Console.WriteLine(Error);
            }
            return Record;
        }
        public static ActiveDirectoryRecord GetActiveDirectoryRecordFromPrincipal(Principal _Principal)
        {
         
            var Result= GetActiveDirectoryRecordFromPrincipal(_Principal, false);
            return Result;

        }
    }
}