//using SharedKernel.Infrastructure.PayPal.Exceptions;
//using SharedKernel.Infrastructure.PayPal.Log;
//using System.Net;

//namespace SharedKernel.Infrastructure.PayPal.Api;

///// <summary>
/////  ConnectionManager retrieves HttpConnection objects used by API service
///// </summary>
//public sealed class ConnectionManager
//{
//    /// <summary>Logger</summary>
//    private static readonly Logger Logger = Logger.GetLogger(typeof(ConnectionManager));
//    /// <summary>
//    /// System.Lazy type guarantees thread-safe lazy-construction
//    /// static holder for instance, need to use lambda to construct since constructor private
//    /// </summary>
//    private static readonly Lazy<ConnectionManager> LazyConnectionManager = new Lazy<ConnectionManager>((Func<ConnectionManager>)(() => new ConnectionManager()));
//    private bool _logTlsWarning;

//    /// <summary>
//    /// Accessor for the Singleton instance of ConnectionManager
//    /// </summary>
//    public static ConnectionManager Instance => LazyConnectionManager.Value;

//    /// <summary>
//    /// Private constructor, private to prevent direct instantiation
//    /// </summary>
//    private ConnectionManager()
//    {
//        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
//    }

//    /// <summary>Create and Config a HttpWebRequest</summary>
//    /// <param name="config">Config properties</param>
//    /// <param name="url">Url to connect to</param>
//    /// <returns></returns>
//    public HttpWebRequest GetConnection(Dictionary<string, string> config, string url)
//    {
//        HttpWebRequest connection;
//        try
//        {
//            connection = (HttpWebRequest)WebRequest.Create(url);
//        }
//        catch (UriFormatException ex)
//        {
//            Logger.Error(ex.Message, ex);
//            throw new ConfigException("Invalid URI: " + url);
//        }
//        int result;
//        if (!config.ContainsKey("connectionTimeout") || !int.TryParse(config["connectionTimeout"], out result))
//            int.TryParse(ConfigManager.GetDefault("connectionTimeout"), out result);
//        connection.Timeout = result;
//        if (config.TryGetValue("proxyAddress", out var value))
//        {
//            var webProxy = new WebProxy
//            {
//                Address = new Uri(value)
//            };
//            if (config.TryGetValue("proxyCredentials", out var value1))
//            {
//                var strArray = value1.Split(':');
//                if (strArray.Length == 2)
//                    webProxy.Credentials = new NetworkCredential(strArray[0], strArray[1]);
//            }
//            connection.Proxy = webProxy;
//        }
//        connection.ServicePoint.Expect100Continue = false;
//        if (_logTlsWarning)
//            Logger.Warn("SECURITY WARNING: TLSv1.2 is not supported on this system. Please update your .NET framework to a version that supports TLSv1.2.");
//        return connection;
//    }
//}