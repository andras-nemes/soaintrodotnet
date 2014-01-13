using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Domain.ProductDomain
{
	public interface IProductRepository
	{
		Product FindBy(Guid productId);
		void Save(Product product);
	}
}
