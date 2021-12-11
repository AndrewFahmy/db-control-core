using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using DbControlCore.Helpers;
using DbControlCore.Models;
using Microsoft.Data.SqlClient;

namespace DbControlCore.Services
{
    public static class ExecutionService
    {
        private static Dictionary<string, IDbTransaction> _transactions = new Dictionary<string, IDbTransaction>();


        public static void Execute(string[] args)
        {
            if (!FileSystemHelper.CheckIfFileExists(Constants.Configurations.JsonFileName))
            {
                throw new FileNotFoundException("Couldn't find 'compiled.json' file to execute.");
            }

            ConsoleHelper.WriteInfo("Starting script execution ...");

            var json = FileSystemHelper.GetFileContents(Constants.Configurations.JsonFileName);
            var data = JsonHelper.DerializeObject<List<DatabaseModel>>(json);

            try
            {
                foreach (var database in data)
                {
                    ExecuteDatabaseQueries(database);
                }

                UpdateTransactions(true);
            }
            catch
            {
                UpdateTransactions(false);

                throw;
            }
        }


        private static void ExecuteDatabaseQueries(DatabaseModel model)
        {
            var connection = new SqlConnection(model.Connection);
            var transaction = connection.BeginTransaction();

            _transactions.Add(model.Name, transaction);

            foreach (var query in model.Queries)
            {
                ConsoleHelper.WriteInfo($"Executing query: {query.Name} for database: {model.Name} ...");

                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query.Text;
                    cmd.Transaction = transaction;

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private static void UpdateTransactions(bool shouldCommit)
        {
            foreach (var transaction in _transactions.Values)
            {
                if (shouldCommit) transaction.Commit();

                else transaction.Rollback();

                transaction.Connection.Dispose();
            }
        }
    }
}
