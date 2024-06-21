using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

public class CustomerController : IController<Customer> {

    private SqlConnection? _sqlConnection { get; set; } = null;
    private static string SqlGetAll = "SELECT * from Customers;";

    public List<Customer> GetAll() {
        var sql = SqlGetAll;
        var cmd = new SqlCommand(sql, _sqlConnection);
        var reader = cmd.ExecuteReader();
        List<Customer> customers = new List<Customer>();
        while(reader.Read()) {
            var customer = new Customer();
            ConvertToCustomer(customer, reader);
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
        ConvertToCustomer(customer, reader);
        reader.Close();
        return customer;
    }

    public bool Create(Customer customer) {
        //var sql = $" INSERT Customers (Name, City, State, Sales, Active) " +
        //            $" VALUES ('{customer.Name}', '{customer.City}', '{customer.State}', {customer.Sales}, {(customer.Active ? 1 : 0)}) ";
        var sql = $" INSERT Customers (Name, City, State, Sales, Active) VALUES " +
                                      " (@Name, @City, @State, @Sales, @Active);";
        var cmd = new SqlCommand(sql, _sqlConnection);
        //cmd.Parameters.AddWithValue("@Id", customer.Id);
        CustomerSqlParameters(cmd, customer);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false; 

    }

    public bool Change(Customer customer) {
        var sql = " UPDATE Customers Set " +
                  " Name = @Name, " +
                  " City = @City, " +
                  " State = @State, " +
                  " Sales = @Sales, " +
                  " Active = @Active " +
                  " Where Id = @Id;";
        var cmd = new SqlCommand(sql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", customer.Id);
        CustomerSqlParameters(cmd, customer);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false;
    }

    private void CustomerSqlParameters(SqlCommand cmd, Customer customer) {
        cmd.Parameters.AddWithValue("@Name", customer.Name);
        cmd.Parameters.AddWithValue("@City", customer.City);
        cmd.Parameters.AddWithValue("@State", customer.State);
        cmd.Parameters.AddWithValue("@Sales", customer.Sales);
        cmd.Parameters.AddWithValue("@Active", customer.Active);
    }

    public bool Remove(int Id) {
        var sql = " DELETE Customers " +
                  " Where Id = @Id;";
        var cmd = new SqlCommand(sql, _sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1 ? true : false;
    }

    public List<Customer> Search(string searchString) {
        var sql = $"SELECT * from Customers " +
                    $" Where Name like '%{searchString}%';";
        Console.WriteLine(sql);
        var cmd = new SqlCommand(sql, _sqlConnection);
        var reader = cmd.ExecuteReader();
        List<Customer> customers = new List<Customer>();
        while(reader.Read()) {
            var customer = new Customer();
            ConvertToCustomer(customer, reader);
            customers.Add(customer);
        }
        reader.Close();
        return customers;
    }

    private void ConvertToCustomer(Customer customer, SqlDataReader reader) {
        customer.Id = Convert.ToInt32(reader["Id"]);
        customer.Name = Convert.ToString(reader["Name"])!;
        customer.City = Convert.ToString(reader["City"])!;
        customer.State = Convert.ToString(reader["State"])!;
        customer.Sales = Convert.ToDecimal(reader["Sales"]);
        customer.Active = Convert.ToBoolean(reader["Active"]);
    }

    public CustomerController(Connection connection) {
        if(connection.GetSqlConnection() != null) {
            _sqlConnection = connection.GetSqlConnection()!;
        }
    } // end of constructor

} // end of class
