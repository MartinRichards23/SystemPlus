﻿using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using SystemPlus.Collections.Generic;
using SystemPlus.Text;

namespace SystemPlus.Data
{
    /// <summary>
    /// Generates stored procedures and ADO.NET code for basic CRUD operations
    /// </summary>
    public class SqlGenerator
    {
        #region Fields

        string? databaseName;
        readonly List<SqlTable> tables = new List<SqlTable>();

        #endregion

        public SqlGenerator()
        {

        }

        #region Public methods

        public void GetSchema(SqlConnection connection, IEnumerable<string>? tableNames = null)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            databaseName = connection.Database;
            List<string> names = GetTableNames(connection);

            foreach (string name in names)
            {
                if (tableNames != null && !tableNames.Any(t => string.Equals(t, name, StringComparison.OrdinalIgnoreCase)))
                    continue;

                SqlTable table = GetTableSchema(connection, name);
                tables.Add(table);
            }
        }

        public void WriteDatabaseClass(TextWriter tw)
        {
            if (tw == null)
                return;

            tw.WriteLine("using System;");
            tw.WriteLine("using System.Data;");
            tw.WriteLine("using System.Data.SqlClient;");
            tw.WriteLine();

            tw.WriteLine("namespace SqlGenerator");
            tw.WriteLine("{");

            tw.WriteLine("public class {0}", databaseName);
            tw.WriteLine("{");

            foreach (SqlTable table in tables)
            {
                tw.WriteLine("#region {0}", table.Name);
                tw.WriteLine();

                WriteAdd(tw, table);
                tw.WriteLine();

                WriteGet(tw, table);
                tw.WriteLine();

                WriteGetAll(tw, table);
                tw.WriteLine();

                WriteUpdate(tw, table);
                tw.WriteLine();

                WriteDelete(tw, table);
                tw.WriteLine();

                WriteReader(tw, table);
                tw.WriteLine();

                tw.WriteLine("#endregion");
                tw.WriteLine();
            }

            tw.WriteLine("}");
            tw.WriteLine("}");
        }

        public void WriteModelClasses(TextWriter tw)
        {
            if (tw == null)
                return;

            tw.WriteLine("using System;");
            tw.WriteLine();

            tw.WriteLine("namespace SqlGenerator");
            tw.WriteLine("{");
            foreach (SqlTable table in tables)
            {
                WriteModelClass(tw, table);
                tw.WriteLine();
            }
            tw.WriteLine("}");
        }

        public void WriteSqlScript(TextWriter tw)
        {
            if (tw == null)
                return;

            foreach (SqlTable table in tables)
            {
                WriteAddSql(tw, table);
                tw.WriteLine("GO");
                tw.WriteLine();

                WriteGetSql(tw, table);
                tw.WriteLine("GO");
                tw.WriteLine();

                WriteGetAllSql(tw, table);
                tw.WriteLine("GO");
                tw.WriteLine();

                WriteUpdateSql(tw, table);
                tw.WriteLine("GO");
                tw.WriteLine();

                WriteDeleteSql(tw, table);
                tw.WriteLine("GO");
                tw.WriteLine();
            }
        }

        #endregion

        #region Private methods

        static List<string> GetTableNames(SqlConnection conn)
        {
            string sql = GetSqlForTableNames();
            List<string> names = new List<string>();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    string? name = rdr.GetValue<string>("TABLE_NAME");

                    if (name != null)
                        names.Add(name);
                }
            }

            return names;
        }

        static SqlTable GetTableSchema(SqlConnection conn, string tableName)
        {
            string sql = $"sp_help '{tableName}'";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            using SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();

            string? name = rdr.GetValue<string>("Name");

            if (name == null)
                throw new Exception("Table name was null");

            SqlTable table = new SqlTable(name);

            rdr.NextResult();
            while (rdr.Read())
            {
                string? colName = rdr.GetValue<string>("Column_name");
                SqlDbType dataType = ParseTools.Enum(rdr.GetValue<string>("Type"), SqlDbType.NVarChar);
                bool computed = rdr.GetValue<string>("Computed") == "yes";
                int length = rdr.GetValue<int>("Length");
                bool nullable = rdr.GetValue<string>("Nullable") == "yes";

                SqlColumn col = new SqlColumn(colName ?? string.Empty, dataType, computed, length, nullable);

                table.Columns.Add(col);
            }

            rdr.NextResult();
            while (rdr.Read())
            {
                string? identityCol = rdr.GetValue<string>("Identity");

                if (!string.IsNullOrWhiteSpace(identityCol))
                {
                    SqlColumn? col = table.Columns.FirstOrDefault(c => c.Name == identityCol);

                    if (col != null)
                        col.IsIdentity = true;
                }
            }

            rdr.NextResult();
            // rowguidcol table

            rdr.NextResult();

            rdr.NextResult();
            // indexes table
            while (rdr.Read())
            {
                if (rdr.HasColumn("index_name"))
                {
                    string? indexName = rdr.GetValue<string>("index_name");
                    string? indexDescription = rdr.GetValue<string>("index_description");
                    string? indexKeys = rdr.GetValue<string>("index_keys");

                    if (indexDescription != null && indexKeys != null && indexDescription.Contains("primary key", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string[] colNames = indexKeys.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                        table.Columns.Where(c => colNames.Contains(c.Name)).ForEach(c => c.IsPrimaryKey = true);
                    }
                }
            }

            return table;
        }

        static string GetSqlForTableNames()
        {
            return "SELECT TABLE_NAME FROM .INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
        }

        static void WriteModelClass(TextWriter tw, SqlTable table)
        {
            tw.WriteLine("public class {0}", table.ClassName);
            tw.WriteLine("{");

            foreach (SqlColumn col in table.Columns)
            {
                tw.WriteLine("public {0} {1} {{ get; set; }}", col.PropertyTypeName, col.PropertyName);
            }

            tw.WriteLine("}");
        }

        static void WriteReader(TextWriter tw, SqlTable table)
        {
            string methodName = $"Read{table.ClassName}";

            tw.WriteLine("public {0} {1}(IDataReader rdr)", table.ClassName, methodName);
            tw.WriteLine("{");

            tw.WriteLine("{0} {1} = new {0}", table.ClassName, table.InstanceName);
            tw.WriteLine("{");
            foreach (SqlColumn col in table.Columns)
            {
                tw.WriteLine("{0} = rdr.GetValue<{1}>(\"{2}\"),", col.PropertyName, col.PropertyTypeName, col.Name);
            }
            tw.WriteLine("};");

            tw.WriteLine("return {0};", table.InstanceName);

            tw.WriteLine("}");
        }

        static void WriteAdd(TextWriter tw, SqlTable table)
        {
            string methodName = $"Add{table.ClassName}";

            tw.WriteLine("public void {0}({1} {2})", methodName, table.ClassName, table.InstanceName);
            tw.WriteLine("{");

            tw.WriteLine("using (SqlConnection con = GetConnection())");
            tw.WriteLine("using (SqlCommand cmd = new SqlCommand(\"{0}\", con))", methodName);
            tw.WriteLine("{");
            tw.WriteLine("cmd.CommandType = CommandType.StoredProcedure;");

            foreach (SqlColumn col in table.Columns)
            {
                if (col.Computed)
                    continue;
                if (col.IsIdentity)
                    continue;

                if (col.Nullable)
                    tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1}.{2}, null);", col.SqlParamName, table.InstanceName, col.PropertyName);
                else
                    tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1}.{2});", col.SqlParamName, table.InstanceName, col.PropertyName);
            }

            tw.WriteLine();
            tw.WriteLine("cmd.ExecuteNonQueryWithRetry(RetryPolicy);");
            tw.WriteLine("}");

            tw.WriteLine("}");
        }

        static void WriteAddSql(TextWriter tw, SqlTable table)
        {
            List<string> paramaters = new List<string>();
            List<string> columnNames = new List<string>();
            List<string> paramNames = new List<string>();

            foreach (SqlColumn col in table.Columns)
            {
                if (col.Computed)
                    continue;
                if (col.IsIdentity)
                    continue;

                paramaters.Add($"{col.SqlParamName} {col.SqlDataType}");
                columnNames.Add(col.Name);
                paramNames.Add(col.SqlParamName);
            }

            string allParams = string.Join(",\r\n", paramaters);
            string allColNames = string.Join(", ", columnNames);
            string allParamNames = string.Join(", ", paramNames);

            string methodName = $"Add{table.ClassName}";

            tw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", methodName);
            tw.WriteLine(allParams);

            tw.WriteLine("AS");

            tw.WriteLine("BEGIN");

            tw.WriteLine("INSERT INTO {0} ({1})", table.Name, allColNames);
            tw.WriteLine("VALUES ({0});", allParamNames);

            tw.WriteLine("END");
        }

        static void WriteGet(TextWriter tw, SqlTable table)
        {
            string methodName = $"Get{table.ClassName}";
            string parameters = string.Empty;

            foreach (SqlColumn col in table.PrimaryKeyColumns)
            {
                parameters += $"{col.PropertyTypeName} {col.InstanceName},";
            }

            parameters = parameters.Trim(',');

            tw.WriteLine("public {0} {1}({2})", table.ClassName, methodName, parameters);
            tw.WriteLine("{");

            tw.WriteLine("using (SqlConnection con = GetConnection())");
            tw.WriteLine("using (SqlCommand cmd = new SqlCommand(\"{0}\", con))", methodName);
            tw.WriteLine("{");
            tw.WriteLine("cmd.CommandType = CommandType.StoredProcedure;");

            foreach (SqlColumn col in table.PrimaryKeyColumns)
            {
                tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1});", col.SqlParamName, col.InstanceName);
            }

            tw.WriteLine();
            tw.WriteLine("using (SqlDataReader rdr = cmd.ExecuteReaderWithRetry(RetryPolicy))");
            tw.WriteLine("{");
            tw.WriteLine("if (!rdr.Read())");
            tw.WriteLine("throw new Exception(\"Not found\");");
            tw.WriteLine("return Read{0}(rdr);", table.ClassName);
            tw.WriteLine("}");

            tw.WriteLine("}");
            tw.WriteLine("}");
        }

        static void WriteGetSql(TextWriter tw, SqlTable table)
        {
            List<string> paramaters = new List<string>();
            List<string> valEquals = new List<string>();

            foreach (SqlColumn col in table.PrimaryKeyColumns)
            {
                paramaters.Add($"{col.SqlParamName} {col.SqlDataType}");
                valEquals.Add($"{col.Name}={col.SqlParamName}");
            }

            string allParams = string.Join(",\r\n", paramaters);
            string allValEquals = string.Join(" AND ", valEquals);

            string methodName = $"Get{table.ClassName}";

            tw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", methodName);
            tw.WriteLine(allParams);

            tw.WriteLine("AS");

            tw.WriteLine("BEGIN");

            tw.WriteLine("SELECT * FROM {0} WHERE {1}", table.Name, allValEquals);

            tw.WriteLine("END");
        }

        static void WriteGetAll(TextWriter tw, SqlTable table)
        {
            string methodName = $"GetAll{table.Name}";

            tw.WriteLine("public IList<{0}> {1}()", table.ClassName, methodName);
            tw.WriteLine("{");

            tw.WriteLine("IList<{0}> items = new List<{0}>();", table.ClassName);

            tw.WriteLine("using (SqlConnection con = GetConnection())");
            tw.WriteLine("using (SqlCommand cmd = new SqlCommand(\"{0}\", con))", methodName);
            tw.WriteLine("{");
            tw.WriteLine("cmd.CommandType = CommandType.StoredProcedure;");

            tw.WriteLine();
            tw.WriteLine("using (SqlDataReader rdr = cmd.ExecuteReaderWithRetry(RetryPolicy))");
            tw.WriteLine("{");
            tw.WriteLine("while (rdr.Read())");
            tw.WriteLine("{");
            tw.WriteLine("items.Add(Read{0}(rdr));", table.ClassName);
            tw.WriteLine("}");
            tw.WriteLine("}");

            tw.WriteLine("}");
            tw.WriteLine("return items;");
            tw.WriteLine("}");
        }

        static void WriteGetAllSql(TextWriter tw, SqlTable table)
        {
            string methodName = $"GetAll{table.Name}";

            tw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", methodName);

            tw.WriteLine("AS");

            tw.WriteLine("BEGIN");

            tw.WriteLine("SELECT * FROM {0}", table.Name);

            tw.WriteLine("END");
        }

        static void WriteUpdate(TextWriter tw, SqlTable table)
        {
            string methodName = $"Update{table.ClassName}";

            tw.WriteLine("public void {0}({1} {2})", methodName, table.ClassName, table.InstanceName);
            tw.WriteLine("{");

            tw.WriteLine("using (SqlConnection con = GetConnection())");
            tw.WriteLine("using (SqlCommand cmd = new SqlCommand(\"{0}\", con))", methodName);
            tw.WriteLine("{");
            tw.WriteLine("cmd.CommandType = CommandType.StoredProcedure;");

            foreach (SqlColumn col in table.Columns)
            {
                if (col.Computed)
                    continue;

                if (col.Nullable)
                    tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1}.{2}, null);", col.SqlParamName, table.InstanceName, col.PropertyName);
                else
                    tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1}.{2});", col.SqlParamName, table.InstanceName, col.PropertyName);
            }

            tw.WriteLine();
            tw.WriteLine("cmd.ExecuteNonQueryWithRetry(RetryPolicy);");
            tw.WriteLine("}");

            tw.WriteLine("}");
        }

        static void WriteUpdateSql(TextWriter tw, SqlTable table)
        {
            List<string> paramaters = new List<string>();
            List<string> seters = new List<string>();
            List<string> valEquals = new List<string>();

            foreach (SqlColumn col in table.Columns)
            {
                if (col.Computed)
                    continue;

                paramaters.Add($"{col.SqlParamName} {col.SqlDataType}");

                if (col.IsPrimaryKey)
                    valEquals.Add($"{col.Name}={col.SqlParamName}");
                else
                    seters.Add($"{col.Name}={col.SqlParamName}");
            }

            string allParams = string.Join(",\r\n", paramaters);
            string allseters = string.Join(",\r\n", seters);
            string allvalEquals = string.Join(" AND ", valEquals);

            string methodName = $"Update{table.ClassName}";

            tw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", methodName);
            tw.WriteLine(allParams);

            tw.WriteLine("AS");

            tw.WriteLine("BEGIN");

            tw.WriteLine("UPDATE {0} SET ", table.Name);
            tw.WriteLine("{0}", allseters);
            tw.WriteLine("WHERE {0};", allvalEquals);

            tw.WriteLine("END");
        }

        static void WriteDelete(TextWriter tw, SqlTable table)
        {
            string methodName = $"Delete{table.ClassName}";
            string parameters = string.Empty;

            foreach (SqlColumn col in table.PrimaryKeyColumns)
            {
                parameters += $"{col.PropertyTypeName} {col.InstanceName},";
            }

            parameters = parameters.Trim(',');

            tw.WriteLine("public void {0}({1})", methodName, parameters);
            tw.WriteLine("{");

            tw.WriteLine("using (SqlConnection con = GetConnection())");
            tw.WriteLine("using (SqlCommand cmd = new SqlCommand(\"{0}\", con))", methodName);
            tw.WriteLine("{");
            tw.WriteLine("cmd.CommandType = CommandType.StoredProcedure;");

            foreach (SqlColumn col in table.PrimaryKeyColumns)
            {
                tw.WriteLine("cmd.Parameters.AddWithValue(\"{0}\", {1});", col.SqlParamName, col.InstanceName);
            }

            tw.WriteLine();
            tw.WriteLine("cmd.ExecuteNonQueryWithRetry(RetryPolicy);");
            tw.WriteLine("}");

            tw.WriteLine("}");
        }

        static void WriteDeleteSql(TextWriter tw, SqlTable table)
        {
            List<string> paramaters = new List<string>();
            List<string> valEquals = new List<string>();

            foreach (SqlColumn col in table.Columns)
            {
                if (col.IsPrimaryKey)
                {
                    paramaters.Add($"{col.SqlParamName} {col.SqlDataType}");
                    valEquals.Add($"{col.Name}={col.SqlParamName}");
                }
            }

            string allParams = string.Join(",\r\n", paramaters);
            string allvalEquals = string.Join(" AND ", valEquals);

            string methodName = $"Delete{table.ClassName}";

            tw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", methodName);
            tw.WriteLine(allParams);

            tw.WriteLine("AS");

            tw.WriteLine("BEGIN");

            tw.WriteLine("DELETE FROM {0} WHERE {1};", table.Name, allvalEquals);

            tw.WriteLine("END");
        }

        #endregion
    }
}
