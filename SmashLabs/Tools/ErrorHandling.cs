using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashLabs.Tools
{
    public class ErrorHandling
    {
        public static void ThrowError(object error)
        {
            throw new Exception(error.ToString());
        }
    }
}
