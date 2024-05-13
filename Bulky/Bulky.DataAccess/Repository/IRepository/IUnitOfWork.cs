using System;
namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		// have all repos
		ICategoryRepository Category { get; }
		IProductRepository Product { get; }
        ICompanyRepository Company { get; }

        // global method
        void Save();
	 }
}

