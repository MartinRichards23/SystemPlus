using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
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
        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Stored procedure")]
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
        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Stored procedure")]
        public static SqlCommand StoredProcedure(this SqlConnection connection, string storedProcedure, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(storedProcedure, connection, transaction)
            {
                CommandType = CommandType.StoredProcedure,
            };

            return command;
        }

        /// <summary>
        /// Gets the value or the given default value is dbnull
        /// </summary>
        public static T GetValue<T>(this IDataReader reader, string name, T defaultVal)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

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

        public static T? TryGetJsonValue<T>(this IDataReader reader, string name) where T : class, new()
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
        public static IList<T> ReadAll<T>(this IDataReader rdr, Func<IDataReader, T> func)
        {
            if (rdr == null)
                throw new ArgumentNullException(nameof(rdr));

            IList<T> items = new List<T>();

            if (func != null)
            {
                while (rdr.Read())
                {
                    T value = func(rdr);
                    items.Add(value);
                }
            }

            return items;
        }

        /// <summary>
        /// Read a row from the IDataReader, returns default if no result
        /// </summary>
        [return: MaybeNull]
        public static T TryReadItem<T>(this IDataReader rdr, Func<IDataReader, T> t)
        {
            if (rdr == null)
                throw new ArgumentNullException(nameof(rdr));
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            if (!rdr.Read())
                return default;

            return t(rdr);
        }

        /// <summary>
        /// Read a row from the IDataReader
        /// </summary>
        public static T ReadItem<T>(this IDataReader rdr, Func<IDataReader, T> t)
        {
            if (rdr == null)
                throw new ArgumentNullException(nameof(rdr));
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            if (!rdr.Read())
                throw new Exception("Data not found");

            return t(rdr);
        }

        /// <summary>
        /// Gets value as compressed byte[] and returns it deserialized
        /// </summary>
        public static T? GetCompressedJsonValue<T>(this IDataReader reader, string name) where T : class, new()
        {
            byte[] compressedData = reader.GetValue<byte[]>(name);

            if (compressedData == null)
                return default;

            byte[] data = CompressionTools.Decompress(compressedData);
            string json = Encoding.UTF8.GetString(data);

            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Json serializes, compresses and adds the parameter
        /// </summary>
        public static SqlParameter AddCompressedJsonValue<T>(this SqlParameterCollection target, string parameterName, T value) where T : class, new()
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

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
                string json = JsonSerializer.Serialize(value);

                byte[] data = Encoding.UTF8.GetBytes(json);
                byte[] compressedData = CompressionTools.Compress(data);

                return target.AddWithValue(parameterName, compressedData);
            }
        }

        /// <summary>
        /// Gets value as string and returns it deserialized
        /// </summary>
        public static T? GetJsonValue<T>(this IDataReader reader, string name) where T : class, new()
        {
            string json = reader.GetValue<string>(name);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Json serializes and adds the parameter
        /// </summary>
        public static SqlParameter AddJsonValue<T>(this SqlParameterCollection target, string parameterName, T value) where T : class, new()
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (value == null)
            {
                return target.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                string json = JsonSerializer.Serialize(value);
                return target.AddWithValue(parameterName, json);
            }
        }

        public static SqlParameter AddWithValue(this SqlParameterCollection target, string parameterName, object value, object nullValue)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (value == null)
                return target.AddWithValue(parameterName, nullValue ?? DBNull.Value);

            return target.AddWithValue(parameterName, value);
        }

        public static SqlParameter AddWithValue(this SqlParameterCollection target, string parameterName, object value, SqlDbType sqltype)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            SqlParameter param = new SqlParameter(parameterName, sqltype)
            {
                Value = value
            };

            return target.Add(param);
        }

        public static SqlParameter? AddRecords(this SqlParameterCollection target, string parameterName, SqlRecordCollection value)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

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
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            ISet<string> columns = new HashSet<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                columns.Add(name);
            }

            return columns;
        }

        public static bool HasColumn(this IDataRecord record, string columnName)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            for (int i = 0; i < record.FieldCount; i++)
            {
                if (record.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Write the contents of the datareader to the streamWriter in csv format
        /// </summary>
        public static void ToCsv(this IDataReader reader, TextWriter sw, bool includeHeader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (sw == null)
                throw new ArgumentNullException(nameof(sw));

            while (reader.NextResult())
            {
                // get all the column names first
                object[] columns = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns[i] = reader.GetName(i);
                }

                // write header
                if (includeHeader)
                {
                    sw.WriteCsvVals(",", columns);
                }

                // write all the data
                while (reader.Read())
                {
                    object[] vals = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        vals[i] = reader[i];
                    }

                    sw.WriteCsvVals(",", vals);
                }

                sw.WriteLine();
            }
        }
    }
}