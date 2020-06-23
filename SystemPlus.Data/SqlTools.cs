using System;
using System.Data;
using System.Globalization;

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
            return sqlType switch
            {
                SqlDbType.NVarChar => typeof(string),
                SqlDbType.VarChar => typeof(string),
                SqlDbType.NChar => typeof(string),
                SqlDbType.Char => typeof(string),
                SqlDbType.Text => typeof(string),
                SqlDbType.NText => typeof(string),
                SqlDbType.TinyInt => typeof(byte),
                SqlDbType.SmallInt => typeof(short),
                SqlDbType.Int => typeof(int),
                SqlDbType.BigInt => typeof(long),
                SqlDbType.Money => typeof(decimal),
                SqlDbType.Decimal => typeof(decimal),
                SqlDbType.Real => typeof(float),
                SqlDbType.Float => typeof(double),
                SqlDbType.Bit => typeof(bool),
                SqlDbType.UniqueIdentifier => typeof(Guid),
                SqlDbType.Binary => typeof(byte[]),
                SqlDbType.VarBinary => typeof(byte[]),
                SqlDbType.Image => typeof(byte[]),
                SqlDbType.DateTime => typeof(DateTime),
                SqlDbType.DateTime2 => typeof(DateTime),
                SqlDbType.DateTimeOffset => typeof(DateTimeOffset),
                SqlDbType.Time => typeof(TimeSpan),
                _ => throw new Exception("Unknown data type " + sqlType.ToString()),
            };
        }

        public static string SqlDbTypeToTypeName(SqlDbType sqlType)
        {
            return sqlType switch
            {
                SqlDbType.NVarChar => "string",
                SqlDbType.NChar => "string",
                SqlDbType.VarChar => "string",
                SqlDbType.Char => "string",
                SqlDbType.Text => "string",
                SqlDbType.NText => "string",
                SqlDbType.TinyInt => "byte",
                SqlDbType.SmallInt => "short",
                SqlDbType.Int => "int",
                SqlDbType.BigInt => "long",
                SqlDbType.Money => "decimal",
                SqlDbType.Decimal => "decimal",
                SqlDbType.Real => "float",
                SqlDbType.Float => "double",
                SqlDbType.Bit => "bool",
                SqlDbType.UniqueIdentifier => "Guid",
                SqlDbType.Binary => "byte[]",
                SqlDbType.VarBinary => "byte[]",
                SqlDbType.Image => "byte[]",
                SqlDbType.DateTime => "DateTime",
                SqlDbType.DateTime2 => "DateTime",
                SqlDbType.DateTimeOffset => "DateTimeOffset",
                SqlDbType.Time => "TimeSpan",
                _ => throw new Exception("Unknown data type " + sqlType.ToString()),
            };
        }

        public static string GetSqlDataTypeName(SqlDbType dataType, int length)
        {
            string sqlDataType = dataType.ToString().ToUpperInvariant();

            if (dataType == SqlDbType.NChar || dataType == SqlDbType.NVarChar)
            {
                string l;
                if (length > 0)
                    l = (length / 2).ToString(CultureInfo.InvariantCulture);
                else
                    l = "max";

                sqlDataType += $"({l})";
            }
            else if (dataType == SqlDbType.Char || dataType == SqlDbType.VarChar)
            {
                string l;
                if (length > 0)
                    l = length.ToString(CultureInfo.InvariantCulture);
                else
                    l = "max";

                sqlDataType += $"({l})";
            }

            return sqlDataType;
        }

    }

}