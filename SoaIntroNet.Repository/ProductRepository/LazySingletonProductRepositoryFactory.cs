using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Repository.ProductRepository
{
	public class LazySingletonProductRepositoryFactory : IProductRepositoryFactory
	{
		public InMemoryProductRepository Create()
		{
			return InMemoryProductRepository.Instance;
		}
	}
}
