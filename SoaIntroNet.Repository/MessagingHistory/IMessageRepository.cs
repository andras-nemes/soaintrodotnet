using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Repository.MessagingHistory
{
	public interface IMessageRepository
	{
		bool IsUniqueRequest(string correlationId);
		void SaveResponse<T>(string correlationId, T response);
		T RetrieveResponseFor<T>(string correlationId);
	}
}
