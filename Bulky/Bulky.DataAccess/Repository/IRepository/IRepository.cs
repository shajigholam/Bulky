using System;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		//T - can be a generic model that we want to do CRUD operation on it
		IEnumerable<T> GetAll();
		T Get(Expression<Func<T, bool>> filter);
		void Add(T entity);
        void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
    }
}

