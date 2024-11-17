using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker
{
    interface MethodRepository
    {
        Object runWhenSuccess();
        Object runWhenFail();
    }
}
