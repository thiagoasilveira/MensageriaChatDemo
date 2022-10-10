using DataC.MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnviarMensagemController : ControllerBase
    {
        private readonly IMessageBus _bus;

        public EnviarMensagemController(IMessageBus bus)
        {
            _bus = bus;
            _bus.QueueDeclare("todos");

        }

        [HttpGet("{nome}/{mensagem}")]
        public ActionResult Get(string nome, string mensagem)
        {
            try
            {
                _bus.PublishExchange("chat_ex", "todos", $"{nome}: {mensagem}");

                return Ok($"Mensagem enviada!\n\nMenagem: {nome}: {mensagem}");
            }
            catch (Exception E)
            {
                return StatusCode(500, "Erro ocorrido: " + E.Message);
            }
        }
    }
}