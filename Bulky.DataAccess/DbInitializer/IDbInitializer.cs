using System;
namespace Bulky.DataAccess.DbInitializer
{
	public interface IDbInitializer
    {
		// responsible to create admin and the roles of the website
		void Initialize();
	}
}

