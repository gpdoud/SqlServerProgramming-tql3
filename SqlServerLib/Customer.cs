using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

public class Customer {

    public int Id { get; set; } = 0;
    public string Name { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string State { get; set; } = String.Empty;
    public decimal Sales { get; set; } = 0;
    public bool Active { get; set; } = true;

}
