using Microsoft.Data.SqlClient;
using System.Threading;

namespace SystemPlus.Data
{
    /// <summary>
    /// Base class for Sql Server database api
    /// </summary>
    public abstract class SqlServerBase
    {
        protected SqlServerBase(string conString)
        {
            ConString = conString;
        }

        protected string ConString
        {
            get;
        }

        /// <summary>
        /// Gets an open SqlConnection
        /// </summary>
        public virtual async Task<SqlConnection> GetConnectionAsync(CancellationToken token)
        {
            SqlConnection cn = new SqlConnection(ConString);
            await cn.OpenAsync(token);

            return cn;
        }

        public async Task TestConnection(CancellationToken token)
        {
            SqlConnection? con = null;

            try
            {
                con = await GetConnectionAsync(token);
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }
        }

        public static string MakeConString(string address, string dbName, bool windowsAuth, string userName, string password, int timeout = 60)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = address,
                IntegratedSecurity = windowsAuth,
                PersistSecurityInfo = true,
                Pooling = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = timeout
            };

            if (!string.IsNullOrEmpty(userName))
                sqlBuilder.UserID = userName;
            if (!string.IsNullOrEmpty(password))
                sqlBuilder.Password = password;

            if (!string.IsNullOrWhiteSpace(dbName))
                sqlBuilder.InitialCatalog = dbName;

            return sqlBuilder.ConnectionString;
        }
    }
}