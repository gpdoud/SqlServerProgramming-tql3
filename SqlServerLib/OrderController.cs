using Microsoft.Data.SqlClient;

using SqlServerLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

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
            var order = SqlConvertToClass(reader);
            orders.Add(order);
        }
        reader.Close();
        return orders;
    }

    public Order? GetByPK(int Id) {
        var cmd = new SqlCommand(OrderGetByPkSql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        var reader = cmd.ExecuteReader();
        if(!reader.HasRows) {
            reader.Close();
            return null;
        }
        // if I get here, there is one order
        reader.Read();
        var order = SqlConvertToClass(reader);
        reader.Close();
        return order;
    }

    private Order SqlConvertToClass(SqlDataReader reader) {
        var order = new Order();
        order.Id = Convert.ToInt32(reader["Id"]);
        order.CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId"))
        ? null
            : Convert.ToInt32(reader["CustomerId"]);
        order.Date = Convert.ToDateTime(reader["Date"]);
        order.Description = Convert.ToString(reader["Description"])!;
        return order;
    }

    public bool Create(Order order) {
        var cmd = new SqlCommand(OrderCreateSql, _sqlConnection);
        cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
        cmd.Parameters.AddWithValue("@Date", order.Date);
        cmd.Parameters.AddWithValue("@Description", order.Description);
        var rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected == 1 ? true : false;
    }

    public bool Change(Order order) {
        var cmd = new SqlCommand(OrderChangeSql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", order.Id);
        cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
        cmd.Parameters.AddWithValue("@Date", order.Date);
        cmd.Parameters.AddWithValue("@Description", order.Description);
        var rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected == 1 ? true : false;
    }

    public bool Remove(int Id) {
        var cmd = new SqlCommand(OrderRemoveSql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        var rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected == 1 ? true : false;
    }

    public OrderController(Connection connection) {
        if(connection.GetSqlConnection() != null) {
            _sqlConnection = connection.GetSqlConnection();
        }
    }
}
