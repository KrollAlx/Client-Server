using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 1500);
            string sName = Dns.GetHostName();
            bool bTerminate = false;
            Console.WriteLine($"Server host: {sName}");

            Socket S = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                S.Bind(ipEndPoint);
                S.Listen(10);
                
                while (!bTerminate)
                {
                    Console.WriteLine($"Waiting for connection through port {ipEndPoint}");

                    Socket NS = S.Accept();
                    string data = null;

                    NS.Send(Encoding.UTF8.GetBytes("***Welcome to simple UPCASE TCP-server***\r\n"));
                    byte[] sReceiveBuffer=new byte[1024];
                    while(true)
                    {
                        int nReaded = NS.Receive(sReceiveBuffer);
                        if (nReaded <= 0)
                            break;
                        data = Encoding.UTF8.GetString(sReceiveBuffer, 0, nReaded);
                        Console.WriteLine($"Received data: {data}");
                        if (data == "info")
                        {
                            NS.Send(Encoding.UTF8.GetBytes("Test TCP-server\r\nDeveloper: Rakitin Aleksey Evgenevich\r\n"));
                        }
                        else if (data == "exit")
                        {
                            NS.Send(Encoding.UTF8.GetBytes("Bye\n"));
                            Console.WriteLine("Client initialize discinnection.\r\n");
                            break;
                        }
                        else if (data == "shutdown")
                        {
                            NS.Send(Encoding.UTF8.GetBytes("Server go to shutdown\r\n"));
                            Thread.Sleep(200);
                            bTerminate = true;
                            break;
                        }
                        else if (data == "time") 
                        {
                            NS.Send(Encoding.UTF8.GetBytes(DateTime.Now.ToString() + "\r\n"));
                        }
                        else if (data == "task")
                        {
                            NS.Send(Encoding.UTF8.GetBytes("Variant: 18\r\n" +
                                "Add support to the service for an additional team that implements the calculation" +
                                " of the sum of a number of natural numbers\r\n"));
                        }
                        else if(data.Contains("sum "))
                        {
                            int input_parameter = Convert.ToInt32(data.Substring(4));
                            int result = 0;
                            for (int i = 1; i <= input_parameter; i++) 
                            {
                                result += i;
                            }
                            NS.Send(Encoding.UTF8.GetBytes("Input parameter: " + input_parameter.ToString() + "\r\n" +
                                "Result: " + result.ToString() + "\r\n"));
                        }
                        else
                        {
                            NS.Send(Encoding.UTF8.GetBytes(data.ToUpper()));
                        }
                    }

                    NS.Shutdown(SocketShutdown.Both);
                    NS.Close();
                    Console.WriteLine("Client disconnected");
                }

                S.Shutdown(SocketShutdown.Both);
                S.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}