using System;
namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		// have all repos
		ICategoryRepository Category { get; }
		IProductRepository Product { get; }

		// global method
		void Save();
	 }
}

