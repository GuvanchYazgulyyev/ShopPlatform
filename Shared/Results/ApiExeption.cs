using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPlatform.Shared.Results
{
    public class ApiExeption : Exception
    {
        public ApiExeption(string message, Exception exception) : base(message, exception)
        {

        }
        public ApiExeption(string message) : base(message)
        {

        }
    }
}
