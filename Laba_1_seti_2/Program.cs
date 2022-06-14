using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Laba_1_seti_2
{
    class Program
    { // адрес и порт сервера, к которому будем подключаться
        static int port = 1337; // порт сервера
        static string address = "vps2.iskiserver.ru"; // адрес сервера

        

        static void Main(string[] args)
        {
            UInt16 ModRTU_CRC(byte[] buf, int len)
            {
                UInt16 crc = 0xFFFF;

                for (int pos = 0; pos < len; pos++)
                {
                    crc ^= (UInt16)buf[pos];

                    for (int i = 8; i != 0; i--)
                    {
                        if ((crc & 0x0001) != 0)
                        {
                            crc >>= 1;
                            crc ^= 0xA001;
                        }
                        else
                            crc >>= 1;
                    }
                }

                return crc;
            }
            
               // IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(address, port);
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
               
           


                /*for (int i= 0; i < messageByte.Length; i++)
                {
                    
                    Console.WriteLine(messageByte[i]);
                    Console.WriteLine("i = " + i);
                }*/

                int n = 11;
                for (int i=0; i < 14000; i++)
                {
                
                n++;
                    adres[0] = (byte)n;
                    adres[1] = (byte)21;
                    for (int j = 2; j < 254; j++)
                    {

                        adres[j] = messageByte[j - 2];
                    }
                adres[254] = 0;
                adres[255] = 0;
                byte HighByte = (byte)(ModRTU_CRC(adres, 256) >> 8);
                     byte LowByte = (byte)(ModRTU_CRC(adres, 256) & 0xFF);
                    adres[254] = HighByte;
                    adres[255] = LowByte;
                // Console.WriteLine(HighByte);
                // Console.WriteLine(LowByte);
                Thread.Sleep(3);
                    socket.Send(adres);
                Console.WriteLine(i);
                //Console.WriteLine(HighByte);
                //Console.WriteLine(LowByte);

            }



            // закрываем сокет
            Environment.Exit(0);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            
            
            Console.Read();
        }
    }
}
