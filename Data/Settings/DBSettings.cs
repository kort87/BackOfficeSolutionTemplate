using System;
using System.Collections.Generic;
using System.Diagnostics;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Data.Settings
{
    public static class DBSettings
    {
        public static void SetConnectionString(string configurationName = "DefaultConnection", string connectionString = null, string providerName = "SqlServer")
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException();
            }
            DataConnection.DefaultSettings = new LinqToDBSettings(
                new ConnectionStringSettings
                {
                    Name = configurationName,
                    ConnectionString = connectionString,
                    ProviderName = providerName
                    
                });
            Trace.TraceInformation("Linq2DB : {0} database parameters setted", configurationName);
        }
    }

    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class LinqToDBSettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public LinqToDBSettings(ConnectionStringSettings connectionStringSettings)
        {
            this.ConnectionStringSettings = connectionStringSettings;
            this.DefaultConfiguration = connectionStringSettings.Name;
            this.DefaultDataProvider = connectionStringSettings.ProviderName;
        }

        public string DefaultConfiguration { get; }
        public string DefaultDataProvider { get; }

        private ConnectionStringSettings ConnectionStringSettings { get; }

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get { yield return this.ConnectionStringSettings; }
        }
    }
}
