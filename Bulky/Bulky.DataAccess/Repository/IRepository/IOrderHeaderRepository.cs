using System;
using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
	public interface IOrderHeaderRepository : IRepository<OrderHeader>
	{
		void Update(OrderHeader obj);
		void UpdateStatue(int id, string orderStatus, string? paymentStatus = null);
		void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
	}
}

