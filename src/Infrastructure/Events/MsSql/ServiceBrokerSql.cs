using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    /// <summary>  </summary>
    public class ServiceBrokerSql
    {
        private readonly string _connectionString;
        private readonly string _query;
        private readonly SqlConnection _conexion;
        private readonly CancellationTokenSource _cancellationToken;

        /// <summary>  </summary>
        public delegate Task MessageReceivedAsync(object sender, SqlNotificationEventArgs e, CancellationToken cancellationToken);

        /// <summary>  </summary>
        public event MessageReceivedAsync Received;

        /// <summary> Initializer. </summary>
        public ServiceBrokerSql(string connectionString, string query)
        {
            if (query.Contains("*"))
                throw new ArgumentException("Not allowed.");

            _connectionString = connectionString;
            _query = query;

            _cancellationToken = new CancellationTokenSource();

            // Start listening. The user must have SUBSCRIBE QUERY NOTIFICATIONS permissions.
            // The database must have SSB enabled.
            // ALTER DATABASE NombreBaseDatos SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE.
            SqlDependency.Start(_connectionString);

            // Crear la conexión a base de datos.
            _conexion = new SqlConnection(_connectionString);
        }

        /// <summary> Destroyer. </summary>
        ~ServiceBrokerSql()
        {
            // Stop listening before leaving.
            SqlDependency.Stop(_connectionString);
        }

        /// <summary> Obtener mensajes desde la base de datos. </summary>
        /// <returns></returns>
        public void Start()
        {
            // Create the listener command. You must include the schema and not use *. Example: SELECT field FROM dbo.Table
            using var cmd = new SqlCommand(_query, _conexion);
            cmd.CommandType = CommandType.Text;

            // Clear any existing notifications
            cmd.Notification = null;

            // Create a dependency for the command
            var dependency = new SqlDependency(cmd);

            // Add an event handler
            dependency.OnChange += OnChange;

            // Open the connection if necessary
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            // Get the messages and then close the connection
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            reader.Close();
        }

        /// <summary> Stop listening. </summary>
        public void Stop()
        {
            _cancellationToken.Cancel();
            SqlDependency.Stop(_connectionString);
        }

        /// <summary> Controller for the event OnChange of SqlDependency. </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
                Received?.Invoke(sender, e, _cancellationToken.Token);

            Start();
        }
    }
}
