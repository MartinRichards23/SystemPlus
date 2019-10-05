using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using SystemPlus.IO;
using SystemPlus.IO.Csv;

namespace SystemPlus.Data
{
    /// <summary>
    /// Extension methods for accessing sql
    /// </summary>
    public static class SqlExtenions
    {
        /// <summary>
        /// Makes a command for a stored procedure
        /// </summary>
        public static SqlCommand StoredProcedure(this SqlConnection connection, string storedProcedure)
        {
            SqlCommand command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            return command;
        }

        /// <summary>
        /// Makes a command for a stored procedure
        /// </summary>
        public static SqlCommand StoredProcedure(this SqlConnection connection, string storedProcedure, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(storedProcedure, connection, transaction)
            {
                CommandType = CommandType.StoredProcedure,
            };

            return command;
        }

        /// <summary>
        /// Makes a command for sql 
        /// </summary>
        public static SqlCommand GetTextCommand(this SqlConnection connection, string sql)
        {
            SqlCommand command = new SqlCommand(sql, connection)
            {

            };

            return command;
        }

        /// <summary>
        /// Gets the value or the given default value is dbnull
        /// </summary>
        public static T GetValue<T>(this IDataReader reader, string name, T defaultVal)
        {
            object o = reader[name];

            if (o == DBNull.Value)
                return defaultVal;
            return (T)o;
        }

        /// <summary>
        /// Gets the value or the type's default value is dbnull
        /// </summary>
        public static T GetValue<T>(this IDataReader reader, string name)
        {
            return GetValue(reader, name, default(T));
        }

        /// <summary>
        /// Gets the value or the default value if it doesn't exist.
        /// Returns default rather than throwing exception
        /// </summary>
        public static T TryGetValue<T>(this IDataReader reader, string name, T defaultVal)
        {
            try
            {
                return GetValue(reader, name, defaultVal);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static T TryGetValue<T>(this IDataReader reader, string name)
        {
            return TryGetValue(reader, name, default(T));
        }

        public static T TryGetJsonValue<T>(this IDataReader reader, string name) where T : class, new()
        {
            try
            {
                return GetJsonValue<T>(reader, name);
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// Reads all the rows from the IDataReader
        /// </summary>
        public static IList<T> ReadAll<T>(this IDataReader rdr, Func<IDataReader, T> t)
        {
            IList<T> items = new List<T>();
            while (rdr.Read())
            {
                items.Add(t(rdr));
            }
            return items;
        }

        /// <summary>
        /// Read a row from the IDataReader, returns default if no result
        /// </summary>
        public static T TryReadItem<T>(this IDataReader rdr, Func<IDataReader, T> t)
        {
            if (!rdr.Read())
                return default;

            return t(rdr);
        }

        /// <summary>
        /// Read a row from the IDataReader
        /// </summary>
        public static T ReadItem<T>(this IDataReader rdr, Func<IDataReader, T> t)
        {
            if (!rdr.Read())
                throw new Exception("Data not found");

            return t(rdr);
        }

        /// <summary>
        /// Gets value as compressed byte[] and returns it deserialized
        /// </summary>
        public static T GetCompressedJsonValue<T>(this IDataReader reader, string name) where T : class, new()
        {
            byte[] compressedData = reader.GetValue<byte[]>(name);

            if (compressedData == null)
                return default;

            byte[] data = Compression.Decompress(compressedData);
            string json = Encoding.UTF8.GetString(data);

            return Serialization.JsonDeserialize<T>(json);
        }

        /// <summary>
        /// Json serializes, compresses and adds the parameter
        /// </summary>
        public static SqlParameter AddCompressedJsonValue<T>(this SqlParameterCollection target, string parameterName, T value) where T : class, new()
        {
            if (value == null)
            {
                SqlParameter param = new SqlParameter(parameterName, SqlDbType.VarBinary, -1)
                {
                    Value = DBNull.Value,
                };

                target.Add(param);

                return param;
            }
            else
            {
                string json = Serialization.JsonSerialize(value);

                byte[] data = Encoding.UTF8.GetBytes(json);
                byte[] compressedData = Compression.Compress(data);

                return target.AddWithValue(parameterName, compressedData);
            }
        }

        /// <summary>
        /// Gets value as string and returns it deserialized
        /// </summary>
        public static T GetJsonValue<T>(this IDataReader reader, string name) where T : class, new()
        {
            string json = reader.GetValue<string>(name);

            if (string.IsNullOrEmpty(json))
                return default;

            return Serialization.JsonDeserialize<T>(json);
        }

        /// <summary>
        /// Json serializes and adds the parameter
        /// </summary>
        public static SqlParameter AddJsonValue<T>(this SqlParameterCollection target, string parameterName, T value) where T : class, new()
        {
            if (value == null)
            {
                return target.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                string json = Serialization.JsonSerialize(value);
                return target.AddWithValue(parameterName, json);
            }
        }

        public static SqlParameter AddWithValue(this SqlParameterCollection target, string parameterName, object value, object nullValue)
        {
            if (value == null)
                return target.AddWithValue(parameterName, nullValue ?? DBNull.Value);

            return target.AddWithValue(parameterName, value);
        }

        public static SqlParameter AddWithValue(this SqlParameterCollection target, string parameterName, object value, SqlDbType sqltype)
        {
            SqlParameter param = new SqlParameter(parameterName, sqltype)
            {
                Value = value
            };

            return target.Add(param);
        }

        public static SqlParameter? AddRecords(this SqlParameterCollection target, string parameterName, SqlRecordCollection value)
        {
            if (value != null && value.Count > 0)
            {
                SqlParameter param = new SqlParameter(parameterName, SqlDbType.Structured)
                {
                    Value = value
                };
                return target.Add(param);
            }

            return null;
        }

        /// <summary>
        /// Gets a hashset of the unique column names
        /// </summary>
        public static ISet<string> GetColumns(this IDataRecord reader)
        {
            ISet<string> columns = new HashSet<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                columns.Add(name);
            }

            return columns;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Write the contents of the datareader to the streamWriter in csv format
        /// </summary>
        public static void ToCsv(this IDataReader rdr, TextWriter sw, bool includeHeader)
        {
            while (rdr.NextResult())
            {
                // get all the column names first
                object[] columns = new object[rdr.FieldCount];
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    columns[i] = rdr.GetName(i);
                }

                // write header
                if (includeHeader)
                {
                    sw.WriteCsvVals(",", columns);
                }

                // write all the data
                while (rdr.Read())
                {
                    object[] vals = new object[rdr.FieldCount];

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        vals[i] = rdr[i];
                    }

                    sw.WriteCsvVals(",", vals);
                }

                sw.WriteLine();
            }
        }
    }
}