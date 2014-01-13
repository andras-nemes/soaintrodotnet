using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Repository.MessagingHistory
{
	public class MessageRepository : IMessageRepository
	{
		private Dictionary<string, object> _responseHistory;

		public MessageRepository()
		{
			_responseHistory = new Dictionary<string, object>();
		}

		public bool IsUniqueRequest(string correlationId)
		{
			return !_responseHistory.ContainsKey(correlationId);
		}

		public void SaveResponse<T>(string correlationId, T response)
		{
			_responseHistory[correlationId] = response;
		}

		public T RetrieveResponseFor<T>(string correlationId)
		{
			if (_responseHistory.ContainsKey(correlationId))
			{
				return (T)_responseHistory[correlationId];
			};
			return default(T);
		}

		public static MessageRepository Instance
		{
			get
			{
				return Nested.instance;
			}
		}

		private class Nested
		{
			static Nested()
			{
			}
			internal static readonly MessageRepository instance = new MessageRepository();
		}
	}
}
