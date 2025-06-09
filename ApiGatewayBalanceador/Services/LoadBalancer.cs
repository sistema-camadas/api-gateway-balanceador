namespace ApiGatewayBalanceador.Services
{
    public class LoadBalancer
    {
        private readonly List<string> _servers = new()
        {
            "http://localhost:8000",  // Server 1
            "http://localhost:8001"   // Server 2
        };

        private int _current = 0;
        private readonly object _lock = new();

        public string GetNextServer()
        {
            lock (_lock)
            {
                var server = _servers[_current]; // Obtém o servidor atual
                _current = (_current + 1) % _servers.Count; // Avança para o próximo servidor
                return server;
            }
        }

    }
}