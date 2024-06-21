using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerLib;

public interface IController<T> where T : class {

    public List<T> GetAll();
    public T? GetByPK(int Id);
    public bool Create(T t);
    public bool Change(T t);
    public bool Remove(int Id);

}
