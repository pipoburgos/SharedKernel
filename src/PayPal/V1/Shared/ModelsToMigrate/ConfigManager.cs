//using SharedKernel.Infrastructure.PayPal.Log;

//namespace SharedKernel.Infrastructure.PayPal.Api;

///// <summary>
///// ConfigManager loads the configuration file and hands out appropriate parameters to application
///// </summary>
//public sealed class ConfigManager
//{
//    /// <summary>Logger</summary>
//    private static readonly Logger Logger = Logger.GetLogger(typeof(ConfigManager));
//    /// <summary>
//    /// The configValue is readonly as it should not be changed outside constructor (but the content can)
//    /// </summary>
//    private readonly Dictionary<string, string> _configValues;
//    private static readonly Dictionary<string, string> DefaultConfig;
//    /// <summary>Singleton instance of the ConfigManager</summary>
//    private static volatile ConfigManager _singletonInstance;
//    private static readonly object SyncRoot = new object();

//    /// <summary>
//    /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
//    /// </summary>
//    static ConfigManager()
//    {
//        DefaultConfig = new Dictionary<string, string>
//        {
//            ["connectionTimeout"] = "30000",
//            ["requestRetries"] = "3",
//            ["mode"] = "sandbox"
//        };
//    }

//    /// <summary>Gets the Singleton instance of the ConfigManager</summary>
//    public static ConfigManager Instance
//    {
//        get
//        {
//            if (_singletonInstance == null)
//                lock (SyncRoot)
//                {
//                    if (_singletonInstance == null)
//                        _singletonInstance = new ConfigManager();
//                }

//            return _singletonInstance;
//        }
//    }

//    /// <summary>Private constructor</summary>
//    private ConfigManager()
//    {
//        var obj = (object)null;
//        try
//        {
//            obj = ConfigurationManager.GetSection("paypal");
//        }
//        catch (Exception ex)
//        {
//            Logger.Warn("Unable to load 'paypal' section from *.config: " + ex.Message);
//        }
//        _configValues = new Dictionary<string, string>();
//        if (obj == null)
//        {
//            Logger.Warn("Cannot parse *.Config file. Ensure you have configured the 'paypal' section correctly.");
//        }
//        else
//        {
//            foreach (NameValueConfigurationElement configurationElement in (ConfigurationElementCollection)obj.GetType().GetProperty("Settings").GetValue(obj, null))
//                _configValues.Add(configurationElement.Name, configurationElement.Value);
//            var num = 0;
//            foreach (Account account in (ConfigurationElementCollection)obj.GetType().GetProperty("Accounts").GetValue(obj, null))
//            {
//                if (!string.IsNullOrEmpty(account.APIUserName))
//                    _configValues.Add("account" + num + ".apiUsername", account.APIUserName);
//                if (!string.IsNullOrEmpty(account.APIPassword))
//                    _configValues.Add("account" + num + ".apiPassword", account.APIPassword);
//                if (!string.IsNullOrEmpty(account.APISignature))
//                    _configValues.Add("account" + num + ".apiSignature", account.APISignature);
//                if (!string.IsNullOrEmpty(account.APICertificate))
//                    _configValues.Add("account" + num + ".apiCertificate", account.APICertificate);
//                if (!string.IsNullOrEmpty(account.PrivateKeyPassword))
//                    _configValues.Add("account" + num + ".privateKeyPassword", account.PrivateKeyPassword);
//                if (!string.IsNullOrEmpty(account.CertificateSubject))
//                    _configValues.Add("account" + num + ".subject", account.CertificateSubject);
//                if (!string.IsNullOrEmpty(account.ApplicationId))
//                    _configValues.Add("account" + num + ".applicationId", account.ApplicationId);
//                ++num;
//            }
//        }
//    }

//    /// <summary>Returns all properties from the config file</summary>
//    /// <returns></returns>
//    public Dictionary<string, string> GetProperties()
//    {
//        return new Dictionary<string, string>(_configValues);
//    }

//    /// <summary>
//    /// Creates new configuration that combines incoming configuration dictionary
//    /// and defaults
//    /// </summary>
//    /// <returns>Default configuration dictionary</returns>
//    public static Dictionary<string, string> GetConfigWithDefaults(Dictionary<string, string> config)
//    {
//        var configWithDefaults = config != null ? new Dictionary<string, string>(config) : new Dictionary<string, string>();
//        foreach (var key in DefaultConfig.Keys)
//            if (!configWithDefaults.ContainsKey(key))
//                configWithDefaults.Add(key, DefaultConfig[key]);
//        return configWithDefaults;
//    }

//    /// <summary>
//    /// Gets the default configuration value for the specified key.
//    /// </summary>
//    /// <param name="configKey">The key to lookup in the default configuration.</param>
//    /// <returns>A string containing the default configuration value for the specified key. If the key is not found, returns null.</returns>
//    public static string GetDefault(string configKey)
//    {
//        return DefaultConfig.ContainsKey(configKey) ? DefaultConfig[configKey] : null;
//    }

//    /// <summary>
//    /// Returns whether or not live mode is enabled in the given configuration.
//    /// </summary>
//    /// <param name="config">Configuration to use</param>
//    /// <returns>True if live mode is enabled; false otherwise.</returns>
//    public static bool IsLiveModeEnabled(Dictionary<string, string> config)
//    {
//        return config != null && config.ContainsKey("mode") && config["mode"] == "live";
//    }
//}