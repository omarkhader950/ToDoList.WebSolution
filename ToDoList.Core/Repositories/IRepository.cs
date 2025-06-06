﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Entities;

namespace ToDoList.Core.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {

       
        

        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();

    }
}
