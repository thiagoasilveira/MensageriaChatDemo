using DataC.MessageBus;
using RabbitMQ.Client.Events;
using System;

namespace Chat.Receber
{
    class Program
    {
        private static MessageBus bus;
        private static string usuario = "";

        static void Main(string[] args)
        {
            while (string.IsNullOrEmpty(usuario))
            {
                Console.WriteLine("Digite o usuário: ");
                usuario = Console.ReadLine();
            }

            Console.Clear();
            Console.WriteLine($"Aguardando mensagens para {usuario}");

            PreparaAmbiente();
            Console.ReadKey();
        }

        public static void PreparaAmbiente()
        {
            bus = new MessageBus("amqps://lcevdixb:4mudmhFXAyRvfPphwx9myckUSpkBGe54@toad.rmq.cloudamqp.com/lcevdixb");

            bus.SubscribeQueue(usuario?.ToLower(), ReceberMensagem);
        }

        private static void ReceberMensagem(object sender, BasicDeliverEventArgs e)
        {
            var mensagem = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

            var t = e.BasicProperties.Headers["entry"] as byte[];
            var remetente = System.Text.Encoding.UTF8.GetString(t);

            if (remetente?.ToLower() == "todos")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Pública: {mensagem}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Privada: {mensagem}");
            }

            Console.ForegroundColor = ConsoleColor.White;

            bus.BasicAck(e.DeliveryTag, false);

        }
    }
}
