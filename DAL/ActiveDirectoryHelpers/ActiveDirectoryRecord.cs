using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.ADConnectors
{
    public class ActiveDirectoryRecord
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }

        public string Designation { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }

        public string Phone { get; set; }

        public string Extension { get; set; }
        public string EmployeeCode { get; set; }

        public string Category { get; set; }

        public string Photo { get; set; }

        public byte[] PhotoBytes { get; set; }

        public byte[] Thumbnail { get; set; }
    }
}