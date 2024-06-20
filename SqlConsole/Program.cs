using Microsoft.Data.SqlClient;

using SqlServerLib;

namespace SqlConsole;

internal class Program {

    static void Main(string[] args) {

        var connStr = "server=localhost\\sqlexpress;" +
                "database=SalesDb;" +
                "trusted_connection=true;" +
                "trustServerCertificate=true;";
        Connection connection = new Connection(connStr);
        connection.Open();

        CustomerController custCtrl = new CustomerController(connection);
        //var customers = custCtrl.GetAll();
        //foreach(var c in customers) {
        //    Console.WriteLine(c.Name);
        //}
        //var id = 15;
        //var customer = custCtrl.GetByPK(id);
        //Console.WriteLine($"Id {id} is {customer!.Name}");

        SqlServerLib.Customer newCustomer = new SqlServerLib.Customer {
            Id = 0, Name = "ACME MFG", 
            City = "Cincinnati", State = "OH", 
            Sales = 1000, Active = true
        };
        var added = custCtrl.Create(newCustomer);
        Console.WriteLine($"Did the create succeed? {added}");

        connection.Close();
    }

    static void LearningCode() { 

        var connStr = "server=localhost\\sqlexpress;" +
                        "database=SalesDb;" +
                        "trusted_connection=true;" +
                        "trustServerCertificate=true;";
        var conn = new SqlConnection(connStr);
        conn.Open();
        if(conn.State != System.Data.ConnectionState.Open) {
            throw new Exception("The connection didn't open!");
        }
        Console.WriteLine("Connection opened...");

        var sql = "SELECT * from Customers;";
        var sqlcmd = new SqlCommand(sql, conn);
        var reader = sqlcmd.ExecuteReader();
        if(!reader.HasRows) {
            Console.WriteLine("The Customer returned no rows...");
        }
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        while(reader.Read()) {
            Customer customer = new Customer();
            customer.Id = Convert.ToInt32(reader["Id"]);
            customer.Name = Convert.ToString(reader["Name"])!;
            customer.City = Convert.ToString(reader["City"])!;
            customer.State = Convert.ToString(reader["State"])!;
            customer.Sales = Convert.ToDecimal(reader["Sales"]);
            customer.Active = Convert.ToBoolean(reader["Active"]);
            customers.Add(customer.Id, customer);
            Console.WriteLine($"Id: {customer.Id} | Name: {customer.Name} | Sales: {customer.Sales:C}");
        }

        reader.Close(); // don't forget this!!!!
        conn.Close();

    }
    /* Customer Controller
    static void CustomerController() { 
        var connStr = "server=localhost\\sqlexpress;" +
                "database=SalesDb;" +
                "trusted_connection=true;" +
                "trustServerCertificate=true;";
        Connection conn = new Connection(connStr);
        conn.Open();
        CustomerController custCtrl = new CustomerController(conn);
        conn.Close();
    }
    */
}

