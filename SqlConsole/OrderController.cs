using Microsoft.Data.SqlClient;

using SqlServerLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConsole;

public class OrderController {

    private SqlConnection? _sqlConnection { get; set; } = null;
    private static string OrderGetAllSql = "SELECT * from Orders;";
    private static string OrderGetByPkSql = "SELECT * from Orders Where Id = @Id;";
    private static string OrderCreateSql = "INSERT Orders (CustomerId, Date, Description) VALUES (@CustomerId, @Date, @Description)";
    private static string OrderChangeSql = "UPDATE Orders SET CustomerId=@CustomerId, Date=@Date, Description=@Description Where Id = @Id;";
    private static string OrderRemoveSql = "DELETE Orders Where Id = @Id;";

    public List<Order> GetAll() {
        var cmd = new SqlCommand(OrderGetAllSql, _sqlConnection);
        var reader = cmd.ExecuteReader();
        var orders = new List<Order>();
        while(reader.Read()) {
            var order = new Order();
            order.Id = Convert.ToInt32(reader["Id"]);
            order.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId"))
                ? null
                : Convert.ToInt32(reader["CustomerId"]);
            order.Date = Convert.ToDateTime(reader["Date"]);
            order.Description = Convert.ToString(reader["Description"])!;
            orders.Add(order);
        }
        reader.Close();
        return orders;
    }

    public OrderController(Connection connection) {
        if(connection.GetSqlConnection() != null) {
            _sqlConnection = connection.GetSqlConnection();
        }
    }
}
