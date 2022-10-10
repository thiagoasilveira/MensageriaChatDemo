using DataC.MessageBus;
using RabbitMQ.Client.Events;
using System;

namespace Chat.Receber
{
    class Program
    {
        private static MessageBus bus;
        private static string usuario = "todos";

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine($"Aguardando mensagens");

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
                Console.WriteLine($"{DateTime.Now:T} | {mensagem}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{mensagem}");
            }

            Console.ForegroundColor = ConsoleColor.White;

            bus.BasicAck(e.DeliveryTag, false);

        }
    }
}
