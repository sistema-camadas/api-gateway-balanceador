namespace ApiGatewayBalanceador.Services
{
    public class LoadBalancer
    {
        private readonly List<string> _servers = new()
        {
            "https://server-main-4yoc.onrender.com",  // Main Server
            "https://flask-server-replica.onrender.com"   // Replica Server
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