using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryBigShoeCompany.Common.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string UiMessage { get; protected set; }

        protected BaseException()
        {
        }

        protected BaseException(string message) : base(message)
        {
            UiMessage = message;
        }
    }
}
