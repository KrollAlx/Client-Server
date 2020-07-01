using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 1500);

            Socket S = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            string msg;
            string answer;          

            try
            {
                S.Connect(ipEndPoint);
                byte[] sReceiveBuffer = new byte[1024];
                while(true)
                {
                    int nReaded = S.Receive(sReceiveBuffer);
                    answer = Encoding.UTF8.GetString(sReceiveBuffer, 0, nReaded);
                    Console.WriteLine(answer);

                    if (answer == "Bye\n")
                        break;

                    msg = Console.ReadLine();
                    S.Send(Encoding.UTF8.GetBytes(msg));                                                      
                }
                S.Shutdown(SocketShutdown.Both);
                S.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
