using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() { }
        public ConflictException(string message) : base(message) { }
        public ConflictException(string message, Exception inner) : base(message, inner) { }
    }
}
