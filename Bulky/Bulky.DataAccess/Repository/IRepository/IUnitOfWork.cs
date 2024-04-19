using System;
namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		// have all repos
		ICategoryRepository Category { get; }

		// global method
		void Save();
	 }
}

