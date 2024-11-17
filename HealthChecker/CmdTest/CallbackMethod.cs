using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker
{
    class CallbackMethod : MethodRepository
    {
        public Object runWhenFail()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Server status: ERROR - Unexpected response");
            return null;
        }

        public Object runWhenSuccess()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Server status: OK");
            return null;
        }
    }
}
