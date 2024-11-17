using System.Threading.Tasks;

namespace HealthChecker
{
    class RunMain
    {
        const string url = "";
        public static async Task Main(string[] args)
        {
            HttpStatusChecker statusChecker = new HttpStatusChecker("https://run.mocky.io/v3/943f6462-f2d5-4669-a29b-269f25c65a83", 5);
            statusChecker.setMethodRepository(new CallbackMethod());
            statusChecker.keepTryWhenFail(false);
            statusChecker.setSuccessResponse("{\"status\":\"200\"}");
            await statusChecker.run();
        }
    }
}
 