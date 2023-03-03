using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    /// <summary> </summary>
    public class EscucharCambiosTablaEventos : IDisposable
    {
        private readonly ServiceBrokerSql _serviceBrokerSql;

        /// <summary>  </summary>
        public EscucharCambiosTablaEventos(string connectionString, string query)
        {
            _serviceBrokerSql = new ServiceBrokerSql(connectionString, query);

            _serviceBrokerSql.Received += Consume;

            _serviceBrokerSql.Start();
        }

        private Task Consume(object sender, SqlNotificationEventArgs e, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Leer la tabla y volcarla a otra para el ack y reintentos");

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _serviceBrokerSql?.Stop();
        }
    }
}