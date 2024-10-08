using Orleans;

namespace ARWNI2S.GDESK.Orleans.Infrastructure
{
    internal class MasterClockGrain : Grain, IMasterClockGrain
    {
        private const int _electionTimeout = 5000; // 5 segundos para timeout

        private enum Role { Follower, Candidate, Leader }

        private Role _currentRole = Role.Follower;
        private long _currentTime = 0;
        private long _lastHeartbeat = 0;
        private Timer _electionTimer;
        private Timer _heartbeatTimer;
        //private CancellationTokenSource cancellationTokenSource;

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            // Inicializamos el Grain como seguidor y empezamos a escuchar por latidos
            _currentRole = Role.Follower;
            _lastHeartbeat = DateTime.UtcNow.Ticks;
            _electionTimer = new Timer(CheckForElection, null, _electionTimeout, _electionTimeout);
            return Task.CompletedTask;
        }

        // Método que inicia el proceso de elección (si el Grain es candidato)
        public async Task StartElectionAsync()
        {
            if (_currentRole == Role.Follower)
            {
                // Convertir el grain en candidato y comenzar la elección
                _currentRole = Role.Candidate;
                Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} se ha convertido en candidato");

                // Solicitar votos de otros Grains (no implementado aquí, se puede usar una lista de grains)
                bool wonElection = await SolicitVotesAsync();
                if (wonElection)
                {
                    BecomeLeader();
                }
                else
                {
                    _currentRole = Role.Follower;
                }
            }
        }

        private Task<bool> SolicitVotesAsync()
        {
            // Lógica simplificada para pedir votos a otros silos (Grains)
            // Aquí puedes implementar un sistema de consenso más avanzado
            Console.WriteLine("Solicitando votos...");
            return Task.FromResult(true); // Simplificación: asumimos que siempre ganamos la elección
        }

        private void BecomeLeader()
        {
            Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} ha sido elegido líder");
            _currentRole = Role.Leader;
            // Inicia un timer para avanzar el tiempo
            _heartbeatTimer = new Timer(LeaderTick, null, 0, 1000); // Avanzamos cada 1 segundo
        }

        private void LeaderTick(object state)
        {
            _currentTime++;
            Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} (Líder) avanza el tiempo a {_currentTime}");

            // Aquí puedes enviar el latido a los otros nodos (Grains)
            // Enviaremos un latido a los seguidores para que sepan que estamos activos
            SendHeartbeatToFollowersAsync().Ignore();
        }

        private async Task SendHeartbeatToFollowersAsync()
        {
            // Lógica para enviar latidos a otros grains (posiblemente un grupo o lista de grains)
            Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} envía latido a seguidores");
            // Aquí enviarías un mensaje a los demás Grains para que reciban el latido
            await Task.CompletedTask;
        }

        public Task ReceiveHeartbeatAsync()
        {
            _lastHeartbeat = DateTime.UtcNow.Ticks;
            Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} ha recibido un latido");
            return Task.CompletedTask;
        }

        private void CheckForElection(object state)
        {
            var now = DateTime.UtcNow.Ticks;
            if (now - _lastHeartbeat > TimeSpan.FromMilliseconds(_electionTimeout).Ticks && _currentRole != Role.Leader)
            {
                Console.WriteLine($"Grain {this.GetPrimaryKeyLong()} no ha recibido latidos. Iniciando elección...");
                StartElectionAsync().Ignore();
            }
        }

        public Task<long> GetCurrentTimeAsync()
        {
            return Task.FromResult(_currentTime);
        }

        public Task LeaderTickAsync()
        {
            if (_currentRole == Role.Leader)
            {
                LeaderTick(null);
            }
            return Task.CompletedTask;
        }
    }

}
