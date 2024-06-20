using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

public class CustomerController {

    private SqlConnection? _sqlConnection { get; set; } = null;

    public List<Customer> GetAll() {
        var sql = "SELECT * from Customers;";
        var cmd = new SqlCommand(sql, _sqlConnection);
        var reader = cmd.ExecuteReader();
        List<Customer> customers = new List<Customer>();
        while(reader.Read()) {
            var customer = new Customer();
            customer.Id = Convert.ToInt32(reader["Id"]);
            customer.Name = Convert.ToString(reader["Name"])!;
            customer.City = Convert.ToString(reader["City"])!;
            customer.State = Convert.ToString(reader["State"])!;
            customer.Sales = Convert.ToDecimal(reader["Sales"]);
            customer.Active = Convert.ToBoolean(reader["Active"]);
            customers.Add(customer);
        }
        reader.Close();
        return customers;
    }

    public Customer? GetByPK(int Id) {
        var sql = $"SELECT * from Customers Where Id = {Id};";
        var cmd = new SqlCommand(sql, _sqlConnection);
        var reader = cmd.ExecuteReader();
        if(!reader.HasRows) {
            reader.Close();
            return null;
        }
        reader.Read();
        var customer = new Customer();
        customer.Id = Convert.ToInt32(reader["Id"]);
        customer.Name = Convert.ToString(reader["Name"])!;
        customer.City = Convert.ToString(reader["City"])!;
        customer.State = Convert.ToString(reader["State"])!;
        customer.Sales = Convert.ToDecimal(reader["Sales"]);
        customer.Active = Convert.ToBoolean(reader["Active"]);
        reader.Close();
        return customer;
    }

    public bool Create(Customer customer) {
        var sql = $" INSERT Customers (Name, City, State, Sales, Active) " +
                    $" VALUES ('{customer.Name}', '{customer.City}', '{customer.State}', {customer.Sales}, {(customer.Active ? 1 : 0)}) ";
        var cmd = new SqlCommand(sql, _sqlConnection);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false; 

    }

    public CustomerController(Connection connection) {
        if(connection.GetSqlConnection() != null) {
            _sqlConnection = connection.GetSqlConnection()!;
        }
    } // end of constructor

} // end of class
