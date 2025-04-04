﻿using System.Data;
using SystemPlus.Text;

namespace SystemPlus.Data
{
    public class SqlTable
    {
        public SqlTable(string name)
        {
            Name = name;
            ClassName = Name.ToUpperFirst().RemovePlural();
            InstanceName = ClassName.ToLowerFirst();
        }

        public string Name { get; }
        public ICollection<SqlColumn> Columns { get; } = new List<SqlColumn>();

        public string ClassName { get; }
        public string InstanceName { get; }

        public IEnumerable<SqlColumn> PrimaryKeyColumns
        {
            get { return Columns.Where(c => c.IsPrimaryKey); }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SqlColumn
    {
        public SqlColumn(string colName, SqlDbType dataType, bool computed, int length, bool nullable)
        {
            Name = colName;
            DataType = dataType;
            Computed = computed;
            Length = length;
            Nullable = nullable;

            PropertyName = Name.ToUpperFirst();
            InstanceName = Name.ToLowerFirst();
            PropertyType = SqlTools.SqlDbTypeToType(DataType);
            string propertyTypeName = SqlTools.SqlDbTypeToTypeName(DataType);

            if (Nullable && !PropertyType.IsNullable())
                propertyTypeName += "?";

            PropertyTypeName = propertyTypeName;

            SqlParamName = "@" + Name.ToLowerFirst();
            SqlDataType = SqlTools.GetSqlDataTypeName(DataType, Length);
        }

        public string Name { get; set; }
        public SqlDbType DataType { get; set; }
        public bool Computed { get; set; }
        public int Length { get; set; }
        public bool Nullable { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool IsIdentity { get; set; }

        public string PropertyName { get; set; }
        public string InstanceName { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyTypeName { get; set; }
        public string SqlParamName { get; set; }
        public string SqlDataType { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
