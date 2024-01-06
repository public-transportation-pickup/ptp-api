using System.Data;

namespace PTP.Application.Data.Configuration;
public interface IConnectionConfiguration
{
    IDbConnection GetDbConnection();
    string GetConnectionString();
    void DbConnectionClose(IDbConnection dbConnection);
}