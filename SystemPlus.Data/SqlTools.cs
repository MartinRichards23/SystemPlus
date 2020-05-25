using System;
using System.Data;

namespace SystemPlus.Data
{
    /// <summary>
    /// Utilities for accessing sql
    /// </summary>
    public static class SqlTools
    {
        public static DateTime MinTime
        {
            get { return new DateTime(1753, 1, 1); }
        }

        /// <summary>
        /// Splits a sql script on GO statements
        /// </summary>
        public static string[] SplitScript(string sql)
        {
            string[] splitter = { "\r\nGO" };
            string[] statements = sql.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            return statements;
        }

        public static Type SqlDbTypeToType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.NVarChar: return typeof(string);
                case SqlDbType.VarChar: return typeof(string);
                case SqlDbType.NChar: return typeof(string);
                case SqlDbType.Char: return typeof(string);
                case SqlDbType.Text: return typeof(string);
                case SqlDbType.NText: return typeof(string);

                case SqlDbType.TinyInt: return typeof(byte);
                case SqlDbType.SmallInt: return typeof(short);
                case SqlDbType.Int: return typeof(int);
                case SqlDbType.BigInt: return typeof(long);
                case SqlDbType.Money: return typeof(decimal);
                case SqlDbType.Decimal: return typeof(decimal);
                case SqlDbType.Real: return typeof(float);
                case SqlDbType.Float: return typeof(double);

                case SqlDbType.Bit: return typeof(bool);
                case SqlDbType.UniqueIdentifier: return typeof(Guid);

                case SqlDbType.Binary: return typeof(byte[]);
                case SqlDbType.VarBinary: return typeof(byte[]);
                case SqlDbType.Image: return typeof(byte[]);

                case SqlDbType.DateTime: return typeof(DateTime);
                case SqlDbType.DateTime2: return typeof(DateTime);
                case SqlDbType.DateTimeOffset: return typeof(DateTimeOffset);
                case SqlDbType.Time: return typeof(TimeSpan);
                default: throw new Exception("Unknown data type " + sqlType.ToString());
            }
        }

        public static string SqlDbTypeToTypeName(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.NVarChar: return "string";
                case SqlDbType.NChar: return "string";
                case SqlDbType.VarChar: return "string";
                case SqlDbType.Char: return "string";
                case SqlDbType.Text: return "string";
                case SqlDbType.NText: return "string";

                case SqlDbType.TinyInt: return "byte";
                case SqlDbType.SmallInt: return "short";
                case SqlDbType.Int: return "int";
                case SqlDbType.BigInt: return "long";
                case SqlDbType.Money: return "decimal";
                case SqlDbType.Decimal: return "decimal";
                case SqlDbType.Real: return "float";
                case SqlDbType.Float: return "double";

                case SqlDbType.Bit: return "bool";
                case SqlDbType.UniqueIdentifier: return "Guid";

                case SqlDbType.Binary: return "byte[]";
                case SqlDbType.VarBinary: return "byte[]";
                case SqlDbType.Image: return "byte[]";

                case SqlDbType.DateTime: return "DateTime";
                case SqlDbType.DateTime2: return "DateTime";
                case SqlDbType.DateTimeOffset: return "DateTimeOffset";
                case SqlDbType.Time: return "TimeSpan";

                default: throw new Exception("Unknown data type " + sqlType.ToString());
            }
        }

        public static string GetSqlDataTypeName(SqlDbType dataType, int length)
        {
            string sqlDataType = dataType.ToString().ToUpperInvariant();

            if (dataType == SqlDbType.NChar || dataType == SqlDbType.NVarChar)
            {
                string l;
                if (length > 0)
                    l = (length / 2).ToString();
                else
                    l = "max";

                sqlDataType += string.Format("({0})", l);
            }
            else if (dataType == SqlDbType.Char || dataType == SqlDbType.VarChar)
            {
                string l;
                if (length > 0)
                    l = length.ToString();
                else
                    l = "max";

                sqlDataType += string.Format("({0})", l);
            }

            return sqlDataType;
        }

    }

}