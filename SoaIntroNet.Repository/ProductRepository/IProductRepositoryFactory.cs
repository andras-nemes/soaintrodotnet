using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Repository.ProductRepository
{
	public interface IProductRepositoryFactory
	{
		InMemoryProductRepository Create();
	}
}
