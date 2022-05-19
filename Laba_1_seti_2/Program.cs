using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace Laba_1_seti_2
{
    class Program
    { // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static string address = "192.168.0.105"; // адрес сервера
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                 Console.Write("Введите сообщение(до 252 символов):");
                 string message = Console.ReadLine();
                byte[] messageByte = new byte[252];
                int count = 0;
               foreach(char i in message)
                {
                    messageByte[count] = (byte)i;
                    count++;
                }
                //messageByte = Encoding.Unicode.GetBytes(message);
                byte[] adres = new byte[256] ;
                adres[0] = (byte)11;
                adres[1] = (byte)21;
                for (int j=2;j < 254;j++)
                {
                    
                    adres[j] = messageByte[j - 2];
                }
                int p = 69665;
                adres[254] = (byte)11;
                adres[255] = (byte)22;
                /*for (int i= 0; i < messageByte.Length; i++)
                {
                    
                    Console.WriteLine(messageByte[i]);
                    Console.WriteLine("i = " + i);
                }*/
                
              
                socket.Send(adres);

                // получаем ответ
                byte[] data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
