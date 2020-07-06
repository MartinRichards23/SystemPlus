using Microsoft.SqlServer.Server;
using System.Collections.Generic;

namespace SystemPlus.Data
{
    /// <summary>
    /// Holds SQL database row data
    /// </summary>
    public class SqlRecordCollection : List<SqlDataRecord>
    {
        readonly SqlMetaData[] columns;

        public SqlRecordCollection(params SqlMetaData[] columns)
        {
            this.columns = columns;
        }

        public IList<SqlMetaData> Columns
        {
            get { return columns; }
        }

        public void AddRow(params object[] items)
        {
            SqlDataRecord row = new SqlDataRecord(columns);

            for (int i = 0; i < items.Length; i++)
            {
                object data = items[i];

                if (data == null)
                    row.SetDBNull(i);
                else
                    row.SetValue(i, data);
            }

            Add(row);
        }
    }
}