using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;



namespace HealthChecker
{
    class HttpStatusChecker
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string url;
        private static int interval;
        private static string successResponse;
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static MethodRepository methodRepository;
        private static bool keepWhenFail = false;
        private static int failLimit = 0;

        /// <summary>
        /// Set Http Reqeust URL and Request interval as second
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_interval"></param>
        public HttpStatusChecker(String _url, int _interval = 5) 
        {
            url = _url;
            interval = _interval;
        }

        /// <summary>
        /// Set string to receive on success
        /// </summary>
        /// <param name="successStr"></param>
        public void setSuccessResponse(string successStr)
        {
            successResponse = successStr;
        }

        /// <summary>
        /// Set a Instance that containes methods to work on success or failure
        /// </summary>
        /// <param name="anObject"></param>
        public void setMethodRepository(MethodRepository anObject)
        {
            methodRepository = anObject;
        }

        /// <summary>
        /// Set whether or not to continue if the HTTP response fails. Default is false.
        /// You can set Number of attempts.
        /// </summary>
        /// <param name="keepTrying"></param>
        /// <param name="tryLimit"></param>
        public void keepTryWhenFail(bool keepTrying, int tryLimit = 0)
        {
            keepWhenFail = keepTrying;
            failLimit = tryLimit;
        }


        private static Object successMethod()
        {
            MethodInfo methodInfo = methodRepository.GetType().GetMethod("runWhenSuccess");
            object result = methodInfo.Invoke(methodRepository, null);
            return result;
        }

        private static Object failMethod()
        {
            MethodInfo methodInfo = methodRepository.GetType().GetMethod("runWhenFail");
            object result = methodInfo.Invoke(methodRepository, null);
            return result;
        }

        public async Task run()
        {
            try
            {
                await continuousSendingRequest(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                _httpClient.Dispose();
            }
        }

        private static async Task continuousSendingRequest(CancellationToken cancellationToken)
        {
            int gcCount = 0;
            int failCount = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(interval), cancellationToken);
                try
                {
                    gcCount++;

                    if (gcCount == 100)  // 100번 요청을 보낼때마다 gc 강제 실행. 메모리 관리를 위함
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers(); 
                        gcCount = 0;
                    }

                    var response = await _httpClient.GetAsync(url, cancellationToken);
                    var content = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(content);

                    if (content.Equals(successResponse))
                    {
                        successMethod();
                    }
                    else
                    {
                        failCount++;
                        failMethod();
                        if (keepWhenFail && ((failLimit != 0 && failLimit > failCount) || failLimit == 0))
                        {
                            continue;
                        }
                        break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Connection error: {ex.Message}");
                    // todo: 연결 오류 처리
                    break;
                }
            }
        }
    }
}
