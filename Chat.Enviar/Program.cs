using DataC.MessageBus;
using System;

namespace Chat.Enviar
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

            PreparaAmbiente();

            EnviaMensagem();

            Console.WriteLine("Hello World!");
        }

        private static void EnviaMensagem()
        {
            while (true)
            {
                Console.WriteLine("Digita a mensagem a ser enviada no padrão: destinatario|mensagem");
                var linha = Console.ReadLine();

                var dados = linha.Split("|");

                if (dados.Length != 2)
                {
                    Console.WriteLine("ERRO: Mensagem fora do padrão");
                    continue;
                }

                bus.PublishExchange("chat_ex", dados[0], $"De {usuario}: {dados[1]}");
                Console.WriteLine($"INFO: Mensagem enviada para '{dados[0]}'");
            }
        }

        public static void PreparaAmbiente()
        {
            bus = new MessageBus("amqps://pgtryzgh:QaRZzgqSXy_h6ra--oEqJNgNu9av6CgU@turkey.rmq.cloudamqp.com/pgtryzgh");

            bus.QueueDeclare(usuario.ToLower());

            bus.QueueBindingEntry(usuario.ToLower(), "chat_ex", "todos");
        }
    }
}
