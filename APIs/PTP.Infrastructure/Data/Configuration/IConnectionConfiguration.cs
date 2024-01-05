using System.Data;

namespace PTP.Infrastructure.Data.Configuration;
public interface IConnectionConfiguration
{
    IDbConnection GetDbConnection();
    string GetConnectionString();
    void DbConnectionClose(IDbConnection dbConnection);
}