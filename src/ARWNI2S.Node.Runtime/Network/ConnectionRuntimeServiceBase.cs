using ARWNI2S.Node.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ARWNI2S.Runtime.Network
{
    public abstract class ConnectionRuntimeServiceBase : NodeRuntimeService
    {
        private readonly NodeConnectionManager _connectionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _nodePort; // Puerto dedicado para conexiones inter-nodo
        private TcpListener _nodeListener;
        protected ConnectionRuntimeServiceBase(NI2SSettings ni2sSettings, NodeConnectionManager connectionManager, IServiceProvider serviceProvider)
        {
            _nodePort = ni2sSettings.Get<NodeConfig>().Port;
            _connectionManager = connectionManager;
            _serviceProvider = serviceProvider;
            // Inter-node listener
            _nodeListener = new TcpListener(IPAddress.Any, _nodePort);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Inicialización de listener para conexiones inter-nodo
            InitializeNodeListener();

            // Escuchar conexiones de usuario mediante el método abstracto
            while (!stoppingToken.IsCancellationRequested)
            {
                await ListenForConnectionsAsync(stoppingToken);
            }
        }

        protected virtual void InitializeNodeListener()
        {
            _nodeListener = new TcpListener(IPAddress.Any, _nodePort);
            _nodeListener.Start();
            Task.Run(() => AcceptNodeConnectionsAsync(), CancellationToken.None);
        }

        protected virtual async Task AcceptNodeConnectionsAsync()
        {
            while (true)
            {
                var nodeSocket = await _nodeListener!.AcceptTcpClientAsync();
                _ = HandleNodeConnectionAsync(nodeSocket);
            }
        }

        protected virtual async Task HandleNodeConnectionAsync(TcpClient nodeSocket)
        {
            using (nodeSocket)
            {
                var stream = nodeSocket.GetStream();
                var connectionId = Guid.NewGuid().ToString();

                // Paso 1: Handshake de Validación del Nodo
                if (!await PerformNodeHandshakeAsync(stream))
                {
                    Console.WriteLine("Handshake failed: Node connection refused.");
                    nodeSocket.Close();
                    return;
                }

                // Paso 2: Asignación de Scope y Establecimiento de Conexión
                var scope = _connectionManager.CreateConnectionScope(connectionId);
                await EstablishNodeConnectionAsync(nodeSocket, scope);
            }
        }

        private async Task<bool> PerformNodeHandshakeAsync(NetworkStream stream)
        {
            // Enviar y recibir mensaje de handshake para validación
            byte[] handshakeMessage = Encoding.UTF8.GetBytes("NODE_HANDSHAKE");
            await stream.WriteAsync(handshakeMessage, 0, handshakeMessage.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Validación en base de datos de cluster
            return ValidateNodeInCluster(response);
        }

        protected virtual bool ValidateNodeInCluster(string nodeInfo)
        {
            // Realizar la validación del nodo usando servicios de base de datos (registrado en el cluster)
            // Devuelve true si el nodo es válido
            return true; // Simulación de validación
        }

        private async Task EstablishNodeConnectionAsync(TcpClient nodeSocket, IServiceScope scope)
        {
            var stream = nodeSocket.GetStream();
            byte[] buffer = new byte[1024];

            // Loop de manejo de comunicación del nodo
            while (nodeSocket.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                // Procesar los datos recibidos
                ProcessNodeData(buffer[..bytesRead]);
            }

            // Cleanup después de la desconexión
            _connectionManager.RemoveConnectionScope(scope.ServiceProvider.GetHashCode().ToString());
        }


        protected virtual void ProcessNodeData(ReadOnlySpan<byte> data)
        {
            Console.WriteLine("Data received from node: " + BitConverter.ToString(data.ToArray()));
        }

        //private async Task ListenForNodeConnectionsAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            // Aceptar una conexión entrante de otro nodo
        //            HandleNodeConnection(await _nodeListener.AcceptTcpClientAsync(stoppingToken));
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejar excepciones relacionadas con la conexión inter-nodo
        //            Console.WriteLine($"Error en conexión inter-nodo: {ex.Message}");
        //        }
        //    }
        //}

        //private void HandleNodeConnection(TcpClient nodeSocket, CancellationToken stoppingToken)
        //{
        //    // Aquí implementamos el manejo específico de la conexión inter-nodo
        //    // Este método se ejecutará para cada conexión inter-nodo aceptada
        //    Task.Run(async () =>
        //    {
        //        using (nodeSocket)
        //        using (scope)
        //        {
        //            var stream = nodeSocket.GetStream();
        //            byte[] buffer = new byte[1024];

        //            while (!stoppingToken.IsCancellationRequested)
        //            {
        //                // Leer datos del nodo remoto (este es un ejemplo simple de manejo de socket TCP)
        //                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);
        //                if (bytesRead == 0) break; // La conexión se cerró

        //                // Procesar los datos recibidos del nodo (implementación específica)
        //                ProcessNodeData(buffer[..bytesRead]);
        //            }

        //            _connectionManager.RemoveConnectionScope(scope.ServiceProvider.GetHashCode().ToString());
        //        }
        //    }, stoppingToken);
        //}


        protected abstract Task ListenForConnectionsAsync(CancellationToken stoppingToken);

        //protected IServiceScope AssignScopeToConnection(string connectionId)
        //{
        //    return _connectionManager.CreateConnectionScope(connectionId);
        //}

        //protected void ReleaseConnectionScope(string connectionId)
        //{
        //    _connectionManager.RemoveConnectionScope(connectionId);
        //}
    }
}
