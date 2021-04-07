using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Repository
{
    public class SqlBulkInsert
    {
        private IConfiguration _configuration;

        public SqlBulkInsert(IConfiguration configuration)
        {
            _configuration = configuration;
        }

   
        public async Task BulkInsertAsync<T>(IEnumerable<T> insertCollection, string targetDbTable)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection);
                sqlBulkCopy.DestinationTableName = "SampleData";
                var dataReader = new ObjectReader<T>(insertCollection);
                await ExecuteInsertAsync(sqlBulkCopy, sqlConnection, dataReader);
            }
        
        }

        private async Task ExecuteInsertAsync(SqlBulkCopy sqlBulkCopy, 
            SqlConnection sqlConnection,
            IDataReader dataReader)
        {
            await sqlConnection.OpenAsync();
            await sqlBulkCopy.WriteToServerAsync(dataReader);
        }

        private string GetConnectionString()
        {
           return _configuration.GetConnectionString("TargetDb");
        }

    }
}
