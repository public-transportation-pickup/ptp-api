using System.Data;
using Microsoft.Data.SqlClient;
using PTP.Application;


namespace PTP.Infrastructure.Data.Configuration;
public class ConnectionConfiguration : IConnectionConfiguration
{
    private readonly AppSettings _appSettings;
    public ConnectionConfiguration(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public void DbConnectionClose(IDbConnection dbConnection)
    {
        if (dbConnection.State == ConnectionState.Open || dbConnection.State == ConnectionState.Broken)
        {
            dbConnection.Close();
        }
    }

    public string GetConnectionString() => _appSettings.ConnectionStrings.DefaultConnection;




    public IDbConnection GetDbConnection()
    {
        return new SqlConnection(_appSettings.ConnectionStrings.DefaultConnection);
    }
}