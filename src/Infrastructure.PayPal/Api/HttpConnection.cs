//using SharedKernel.Infrastructure.PayPal.Exceptions;
//using SharedKernel.Infrastructure.PayPal.Log;
//using System.Globalization;
//using System.Net;

//namespace SharedKernel.Infrastructure.PayPal.Api;

///// <summary>Helper class for sending HTTP requests.</summary>
//internal class HttpConnection
//{
//    private static readonly Logger Logger = Logger.GetLogger(typeof(HttpConnection));
//    //private readonly Dictionary<string, string> _config;

//    /// <summary>Gets the HTTP request details.</summary>
//    public RequestDetails RequestDetails { get; private set; }

//    /// <summary>Gets the HTTP response details.</summary>
//    public ResponseDetails ResponseDetails { get; private set; }

//    /// <summary>
//    /// Initializes a new instance of <seealso cref="T:SharedKernel.Infrastructure.PayPal.Api.HttpConnection" /> using the given config.
//    /// </summary>
//    /// <param name="config">The config to use when making HTTP requests.</param>
//    public HttpConnection()//Dictionary<string, string> config)
//    {
//        //this._config = config;
//        RequestDetails = new RequestDetails();
//        ResponseDetails = new ResponseDetails();
//    }

//    /// <summary>
//    /// Copying existing HttpWebRequest parameters to newly created HttpWebRequest, can't reuse the same HttpWebRequest for retries.
//    /// </summary>
//    /// <param name="httpRequest"></param>
//    /// <param name="config"></param>
//    /// <param name="url"></param>
//    /// <returns>HttpWebRequest</returns>
//    private HttpWebRequest CopyRequest(
//        HttpWebRequest httpRequest,
//        Dictionary<string, string> config,
//        string url)
//    {
//        var connection = ConnectionManager.Instance.GetConnection(config, url);
//        connection.Method = httpRequest.Method;
//        connection.Accept = httpRequest.Accept;
//        connection.ContentType = httpRequest.ContentType;
//        if (httpRequest.ContentLength > 0L)
//            connection.ContentLength = httpRequest.ContentLength;
//        connection.UserAgent = httpRequest.UserAgent;
//        connection.ClientCertificates = httpRequest.ClientCertificates;
//        return CopyHttpWebRequestHeaders(httpRequest, connection);
//    }

//    /// <summary>
//    /// Copying existing HttpWebRequest headers into newly created HttpWebRequest
//    /// </summary>
//    /// <param name="httpRequest"></param>
//    /// <param name="newHttpRequest"></param>
//    /// <returns>HttpWebRequest</returns>
//    private HttpWebRequest CopyHttpWebRequestHeaders(
//        HttpWebRequest httpRequest,
//        HttpWebRequest newHttpRequest)
//    {
//        foreach (var allKey in httpRequest.Headers.AllKeys)
//            switch (allKey.ToLower(CultureInfo.InvariantCulture))
//            {
//                case "accept":
//                case "connection":
//                case "content-length":
//                case "content-type":
//                case "date":
//                case "expect":
//                case "host":
//                case "if-modified-since":
//                case "proxy-connection":
//                case "range":
//                case "referer":
//                case "transfer-encoding":
//                case "user-agent":
//                    continue;
//                default:
//                    newHttpRequest.Headers[allKey] = httpRequest.Headers[allKey];
//                    continue;
//            }

//        return newHttpRequest;
//    }

//    /// <summary>Executing API calls</summary>
//    /// <param name="payLoad"></param>
//    /// <param name="httpRequest"></param>
//    /// <returns>A string containing the response from the remote host.</returns>
//    public string Execute(string payLoad, HttpWebRequest httpRequest)
//    {
//        var int32 = _config.ContainsKey("requestRetries") ? Convert.ToInt32(_config["requestRetries"]) : 0;
//        var num = 0;
//        RequestDetails.Reset();
//        ResponseDetails.Reset();
//        RequestDetails.Body = payLoad;
//        RequestDetails.Headers = httpRequest.Headers;
//        RequestDetails.Url = httpRequest.RequestUri.AbsoluteUri;
//        RequestDetails.Method = httpRequest.Method;
//        try
//        {
//            do
//            {
//                if (num > 0)
//                {
//                    Logger.Info("Retrying....");
//                    httpRequest = CopyRequest(httpRequest, _config, httpRequest.RequestUri.ToString());
//                    ++RequestDetails.RetryAttempts;
//                }
//                try
//                {
//                    var method = httpRequest.Method;
//                    if (method == "POST" || method == "PUT" || method == "PATCH")
//                        using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
//                        {
//                            streamWriter.Write(payLoad);
//                            streamWriter.Flush();
//                            streamWriter.Close();
//                            if (ConfigManager.IsLiveModeEnabled(_config))
//                                Logger.Debug("Request details are hidden in live mode.");
//                            else
//                                Logger.Debug(payLoad);
//                        }

//                    using (var response = httpRequest.GetResponse())
//                    {
//                        ResponseDetails.Headers = response.Headers;
//                        if (response is HttpWebResponse)
//                            ResponseDetails.StatusCode = new HttpStatusCode?(((HttpWebResponse)response).StatusCode);
//                        using (var streamReader = new StreamReader(response.GetResponseStream()))
//                        {
//                            ResponseDetails.Body = streamReader.ReadToEnd().Trim();
//                            if (ConfigManager.IsLiveModeEnabled(_config))
//                            {
//                                Logger.Debug("Response details are hidden in live mode.");
//                            }
//                            else
//                            {
//                                Logger.Debug("Service response: ");
//                                Logger.Debug(ResponseDetails.Body);
//                            }
//                            return ResponseDetails.Body;
//                        }
//                    }
//                }
//                catch (WebException ex)
//                {
//                    var str = string.Empty;
//                    if (ex.Response != null)
//                        using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
//                        {
//                            str = streamReader.ReadToEnd().Trim();
//                            Logger.Error("Error response:");
//                            Logger.Error(str);
//                        }

//                    Logger.Error(ex.Message);
//                    ConnectionException connectionException;
//                    if (ex.Status == WebExceptionStatus.ProtocolError)
//                    {
//                        var response = (HttpWebResponse)ex.Response;
//                        if (response.StatusCode != HttpStatusCode.GatewayTimeout)
//                        {
//                            if (response.StatusCode != HttpStatusCode.RequestTimeout)
//                            {
//                                if (response.StatusCode != HttpStatusCode.BadGateway)
//                                    connectionException = new HttpException(ex.Message, str, response.StatusCode, ex.Status, response.Headers, httpRequest);
//                                else
//                                    goto label_43;
//                            }
//                            else
//                            {
//                                goto label_43;
//                            }
//                        }
//                        else
//                        {
//                            goto label_43;
//                        }
//                    }
//                    else if (ex.Status == WebExceptionStatus.ReceiveFailure || ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.KeepAliveFailure)
//                    {
//                        Logger.Debug("There was a problem connecting to the server: " + ex.Status.ToString());
//                        goto label_43;
//                    }
//                    else
//                    {
//                        connectionException = ex.Status != WebExceptionStatus.Timeout ? new ConnectionException("Invalid HTTP response: " + ex.Message, str, ex.Status, httpRequest) : new ConnectionException(
//                            $"{ex.Message} (HTTP request timeout was set to {httpRequest.Timeout}ms)", str, ex.Status, httpRequest);
//                    }

//                    if (ex.Response != null && ex.Response is HttpWebResponse)
//                    {
//                        var response = ex.Response as HttpWebResponse;
//                        ResponseDetails.StatusCode = new HttpStatusCode?(response.StatusCode);
//                        ResponseDetails.Headers = response.Headers;
//                    }
//                    ResponseDetails.Exception = connectionException;
//                    throw connectionException;
//                }
//                label_43:;
//            }
//            while (num++ < int32);
//        }
//        catch (PayPalException ex)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            throw new PayPalException("Exception in PayPal.HttpConnection.Execute(): " + ex.Message, ex);
//        }
//        throw new PayPalException("Retried " + int32 + " times.... Exception in PayPal.HttpConnection.Execute(). Check log for more details.");
//    }
//}