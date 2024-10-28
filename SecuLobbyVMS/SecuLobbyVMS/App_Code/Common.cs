using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SecuLobbyVMS.smartWS;
namespace SecuLobby.App_Code
{
    //public class Common
    //{
    //}

    public class wsSqlDbParam
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int ParamType { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }
        public string TableTypeName { get; set; }
        public string TableName { get; set; }
    }
    public static class StaticFuncs
    {

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam()
        {
            return DbParam(string.Empty, null, null, -1);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name)
        {
            return DbParam(name, null, null, -1);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name, object value)
        {
            return DbParam(name, value, null, -1);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name, System.Data.SqlDbType type)
        {
            return DbParam(name, null, type, -1);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name, System.Data.SqlDbType type, int size)
        {
            return DbParam(name, null, type, size);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name, object value, System.Data.SqlDbType type)
        {
            return DbParam(name, value, type, -1);
        }

        public static SecuLobbyVMS.smartWS.SqlDbParam DbParam(string name, object value, System.Data.SqlDbType? type, int size)
        {
            SecuLobbyVMS.smartWS.SqlDbParam o = new SecuLobbyVMS.smartWS.SqlDbParam();
            o.Name = name;
            o.Value = value;
            if (type.HasValue)
            {
                o.ParamType = (int)type.Value;
            }
            else
            {
                o.ParamType = -1;
            }
            o.Size = size;
            //o.Direction =   ParameterDirection.Input;
            o.Direction = SecuLobbyVMS.smartWS.ParameterDirection.Input; //AppService.ParameterDirection.Input;
            o.TableTypeName = string.Empty;
            return o;
        }
    }
}