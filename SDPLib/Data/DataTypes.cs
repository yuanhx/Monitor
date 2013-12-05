using System;
using System.Collections.Generic;
using System.Text;

namespace SDP.Data
{
    //public enum DataTypes
    //{
    //      dtDefault  = 0,
    //      dtString   = 1,
    //      dtInteger  = 2,
    //      dtNumber   = 3,
    //      dtMoney    = 4,
    //      dtBoolean  = 5,
    //      dtDateTime = 10,
    //      dtDate     = 11,
    //      dtTime     = 12,
    //      dtBLOB     = 101,
    //      dtCLOB     = 102,
    //      dtXMLType  = 103,
    //      dtLONG     = 111
    //}

    public class DataTypes
    {
        public const int dtDefault = 0;
        public const int dtString = 1;
        public const int dtInteger = 2;
        public const int dtNumber = 3;
        public const int dtMoney = 4;
        public const int dtBoolean = 5;
        public const int dtDateTime = 10;
        public const int dtDate = 11;
        public const int dtTime = 12;
        public const int dtBLOB = 101;
        public const int dtCLOB = 102;
        public const int dtXMLType = 103;
        public const int dtLONG = 111;

        public static int ToDataType(Type type)
        {
            string st = type!=null?type.ToString():"";

            if (st.Equals("System.String"))
                return dtString;
            else if (st.Equals("System.Int32"))
                return dtInteger;
            else if (st.Equals("System.Double"))
                return dtNumber;
            else if (st.Equals("System.DateTime"))
                return dtDateTime;
            else if (st.Equals("System.Boolean"))
                return dtBoolean;
            else if (st.Equals("System.Byte[]"))
                return dtBLOB;
            else 
                return dtString;
        }

        public static int ToDataType(string type)
        {
            type = type.ToUpper();

            if (type.Equals("STRING") || type.Equals("VARCHAR") || type.Equals("VARCHAR2")) return dtString;
            if (type.Equals("INTEGER") || type.Equals("INT")) return dtInteger;
            if (type.Equals("NUMBER") || type.Equals("FLOAT") || type.Equals("DOUBLE")) return dtNumber;
            if (type.Equals("BOOLEAN") || type.Equals("BOOL")) return dtBoolean;
            if (type.Equals("DATETIME")) return dtDateTime;
            if (type.Equals("DATE")) return dtDate;
            if (type.Equals("TIME")) return dtTime;
            if (type.Equals("MONEY") || type.Equals("NUMERIC")) return dtMoney;
            if (type.Equals("BLOB")) return dtBLOB;
            if (type.Equals("CLOB")) return dtCLOB;
            if (type.Equals("XMLTYPE")) return dtXMLType;
            if (type.Equals("LONG")) return dtLONG;

            throw new Exception("无效的数据类型：" + type);
        }

        public static Type ToType(string type)
        {
            if (type == null || type.Equals(""))
                return Type.GetType("System.String");

            string ts = type.ToUpper();

            if (ts.Equals("STRING") || ts.Equals("VARCHAR") || ts.Equals("VARCHAR2"))
                return Type.GetType("System.String");
            else if (ts.Equals("INT") || ts.Equals("INTEGER"))
                return Type.GetType("System.Int32");
            else if (ts.Equals("NUMBER") || ts.Equals("FLOAT") || ts.Equals("DOUBLE"))
                return Type.GetType("System.Double");
            else if (ts.Equals("BOOL") || ts.Equals("BOOLEAN"))
                return Type.GetType("System.Boolean");
            else if (ts.Equals("DATETIME") || ts.Equals("DATE") || ts.Equals("TIME"))
                return Type.GetType("System.DateTime");
            else if (ts.Equals("BLOB") || ts.Equals("LONG"))
                return Type.GetType("System.Byte[]");
            else
                return ToType(Convert.ToInt32(type.StartsWith("System.") ? type : "System." + type));
        }

        public static Type ToType(int type)
        {
            switch (type)
            {
                case dtInteger:
                    return Type.GetType("System.Int32");
                case dtNumber:
                    return Type.GetType("System.Double");
                case dtMoney:
                    return Type.GetType("System.Double");
                case dtBoolean:
                    return Type.GetType("System.Boolean");
                case dtDateTime:
                    return Type.GetType("System.DateTime");
                case dtDate:
                    return Type.GetType("System.DateTime");
                case dtTime:
                    return Type.GetType("System.DateTime");
                case dtBLOB:
                    return Type.GetType("System.Byte[]");
                case dtLONG:
                    return Type.GetType("System.Byte[]");
                //    break;
                //case dtCLOB:
                //    break;
                //case dtXMLType:
                //    break;
                default:
                    return Type.GetType("System.String");
            }
        }
    }
}
