using Microsoft.Data.SqlClient;

namespace SqlServerLib;

public class Connection {

    private string _connectionString { get; set; } = string.Empty;
    private SqlConnection? _sqlConnection { get; set; } = null;

    public SqlConnection? GetSqlConnection() {
        return _sqlConnection;
    }

    public void Open() {
        _sqlConnection = new SqlConnection(_connectionString);
        _sqlConnection.Open();
        if(_sqlConnection.State != System.Data.ConnectionState.Open) {
            _sqlConnection = null;
            throw new Exception("Connection failed to open.");
        }
    }

    public void Close() {
        _sqlConnection?.Close();
    }


    public Connection(string connectionString) {
        _connectionString = connectionString;
    }

} // end of class
