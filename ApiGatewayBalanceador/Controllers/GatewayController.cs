using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGatewayBalanceador.Services;



namespace ApiGatewayBalanceador.Controllers
{
    [ApiController]
    [Route("{**path}")] // Rota que captura todas as requisições
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly LoadBalancer _loadBalancer;

        public GatewayController(IHttpClientFactory httpClientFactory, LoadBalancer loadBalancer)
        {
            _httpClient = httpClientFactory.CreateClient("Insecure");
            _loadBalancer = loadBalancer;
        }

        [HttpGet, HttpPost, HttpPut, HttpPatch, HttpDelete]
        public async Task<IActionResult> Proxy(string path) // Método que recebe todas as requisições
        {
            var targetServer = _loadBalancer.GetNextServer(); // Obtém o próximo servidor do Load Balancer
            var targetUrl = $"{targetServer}/{path}";
            var requestMessage = new HttpRequestMessage(new HttpMethod(Request.Method), targetUrl);

            foreach (var header in Request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            // Se houver conteúdo na requisição, adiciona à nova requisição como JSON
            if (Request.ContentLength > 0)
            {
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();
                requestMessage.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            }
            try
            {
                var response = await _httpClient.SendAsync(requestMessage); // Envia a requisição para o servidor de destino
                var content = await response.Content.ReadAsStringAsync(); // Lê o conteúdo da resposta
                return new ContentResult // Devolve o resultado da requisição
                {
                    Content = content,
                    ContentType = "application/json",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                // Erro de conexão
                return StatusCode(502, new { message = "Servidor indisponível", error = ex.Message });
            }
        }
    }
    


}


