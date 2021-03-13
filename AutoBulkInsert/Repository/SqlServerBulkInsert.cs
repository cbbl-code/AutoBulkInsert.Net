using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Repository
{
    public class Class1
    {

        public async Task BulkInsertAsync<T>(IEnumerable<T> insertCollection, SqlConnection sqlConnection)
        {
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection);
            await sqlConnection.OpenAsync();
            await ExecuteInsertAsync(sqlBulkCopy, sqlConnection,(IDataReader)null);
        }

        private async Task ExecuteInsertAsync(SqlBulkCopy sqlBulkCopy, 
            SqlConnection sqlConnection,
            IDataReader dataReader)
        {
            await sqlConnection.OpenAsync();
            await sqlBulkCopy.WriteToServerAsync(dataReader);
        }

    }
}
