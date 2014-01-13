using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoaIntroNet.Service.Exceptions
{
	public class LimitedAvailabilityException : Exception
	{
		public LimitedAvailabilityException(string message)
			: base(message)
		{}

		public LimitedAvailabilityException()
			: base("There are not enough products left to fulfil your request.")
		{}
	}
}
