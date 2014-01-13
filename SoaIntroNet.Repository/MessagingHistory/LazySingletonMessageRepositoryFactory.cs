using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Repository.MessagingHistory
{
	public class LazySingletonMessageRepositoryFactory : IMessageRepositoryFactory
	{
		public MessageRepository Create()
		{
			return MessageRepository.Instance;
		}
	}
}
