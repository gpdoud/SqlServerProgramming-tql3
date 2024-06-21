using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

public class Order {

    public int Id { get; set; } = 0;
    public int? CustomerId { get; set; } = null;
    public DateTime Date { get; set; } = default(DateTime);
    public string Description { get; set; } = string.Empty;

    public override string ToString() {
        return $"{Id,3} | {CustomerId,3} | {Date,-25} | {Description} ";
    }
}
