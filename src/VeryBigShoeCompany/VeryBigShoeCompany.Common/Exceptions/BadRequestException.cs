using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryBigShoeCompany.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException()
        {
            UiMessage = "An error occured when trying to upload the file.";
        }

        public BadRequestException(string message) : base(message)
        {
        }
    }
}
