using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Repositories
{
    public interface IRepository<T> where T : class
    {

       
        Task<IEnumerable<T>> GetAllAsync();
      
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();



       








    }
}
