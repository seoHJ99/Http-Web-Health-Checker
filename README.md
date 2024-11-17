# Http-Web-Health-Checker

Language : C#

Describe:
You Can Check your server status. It is continuously send Http Request and check the response.
You must create a URL in advance and set the URL to Http request.
To run certain Function when Http Resonse is successful or failed, you must create a Class that inherit MethodRepository Interface and set to HttpChecker's Instance.

1. Create a URL for health check on the server <br/>

```C#
[HttpGet]
public JsonResult healthChecker()
{
    return Json(new { status = "200" }, JsonRequestBehavior.AllowGet);
}
```

2.  Class that inherit MethodRepository Interface and Override Function

```
    class CallbackMethod : MethodRepository
    {
        public Object runWhenFail()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Server status: OK");
            return null;
        }

        public Object runWhenSuccess()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Server status: ERROR - Unexpected response");
            return null;
        }
    }
```

3. Create an Instance and Set Config and run
```
    class RunMain
    {
        const string TEST_URL = "";
        const int HTTP_REQUEST_INTERVAL = 5;
        public static async Task Main(string[] args)
        {
            HttpStatusChecker statusChecker = new HttpStatusChecker(TEST_URL, HTTP_REQUEST_INTERVAL);
            statusChecker.setMethodRepository(new CallbackMethod());
            statusChecker.keepTryWhenFail(false);
            statusChecker.setSuccessResponse("{\"status\":\"200\"}"); //fail

            await statusChecker.run();
        }
    }
```

4. Check It Work
![image](https://github.com/user-attachments/assets/193f710d-6104-4fa7-981f-4c68979db1e5)

