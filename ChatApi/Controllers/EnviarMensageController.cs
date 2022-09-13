using DataC.MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnviarMensagemController : ControllerBase
    {
        private static MessageBus bus;
        private readonly ILogger<EnviarMensagemController> _logger;

        public EnviarMensagemController(ILogger<EnviarMensagemController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{nome}/{mensagem}")]
        public ActionResult Get(string nome, string mensagem)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new ArgumentException($"'{nameof(nome)}' cannot be null or empty.", nameof(nome));
            }

            if (string.IsNullOrEmpty(mensagem))
            {
                throw new ArgumentException($"'{nameof(mensagem)}' cannot be null or empty.", nameof(mensagem));
            }

            try
            {
                PreparaAmbiente(nome);
                bus.PublishExchange("chat_ex", "todos", $"De {nome}: {mensagem}");

                return Ok($"Mensagem enviada!\n\nMenagem: {nome}: {mensagem}");
            }
            catch (Exception E)
            {
                return StatusCode(500, "Erro ocorrido: " + E.Message);
            }
        }

        public static void PreparaAmbiente(string usuario)
        {
            bus = new MessageBus("amqps://lcevdixb:4mudmhFXAyRvfPphwx9myckUSpkBGe54@toad.rmq.cloudamqp.com/lcevdixb");

            bus.QueueDeclare("todos");

            //bus.QueueBindingEntry(usuario.ToLower(), "chat_ex", usuario.ToLower());
            //bus.QueueBindingEntry(usuario.ToLower(), "chat_ex", "todos");
        }
    }
}