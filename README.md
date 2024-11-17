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

```C#
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
```

3. Create an Instance and Set Config and run
```C#
    class RunMain
    {
        const string TEST_URL = "";
        const int HTTP_REQUEST_INTERVAL = 5;
        public static async Task Main(string[] args)
        {
            HttpStatusChecker statusChecker = new HttpStatusChecker(TEST_URL, HTTP_REQUEST_INTERVAL);
            statusChecker.setMethodRepository(new CallbackMethod());
            statusChecker.keepTryWhenFail(false);
            statusChecker.setSuccessResponse("{\"status\":\"200\"}"); // success
            // statusChecker.setSuccessResponse("{\"status\":\"400\"}"); // fail

            await statusChecker.run();
        }
    }
```

4. Check It Work<br/>
   -successful
![image](https://github.com/user-attachments/assets/43c3651c-735f-4767-8ff9-3e04d339cd76)

   -failed
![image](https://github.com/user-attachments/assets/47c54dfe-e9c3-46a6-ac87-bafd07bd81f0)
