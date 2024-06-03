using System.Net.Sockets;

namespace TcpSnifferOWN
{
    public class ClientSession
    {
        int id;
        bool run;
        TcpClient clientSession, tcpServer;
        Thread threadCS, threadSC;
        NetworkStream clientStream, serverStream;

        public ClientSession(TcpClient _session, int _id)
        {
            run = true;
            clientSession = _session;

            tcpServer = new TcpClient("192.168.1.231", 20000);

            serverStream = clientSession.GetStream();
            clientStream = tcpServer.GetStream();

            threadCS = new Thread(new ThreadStart(ClientToServer));
            threadSC = new Thread(new ThreadStart(ServerToClient));
            
            id = _id;

            threadCS.Start();
            threadSC.Start();

        }

        private void ClientToServer()
        {
            byte[] buffer = new byte[256];

            while (run)
            {
                try
                {
                    int size = clientStream.Read(buffer, 0, buffer.Length);
                    if (size > 0)
                    {
                        lock(Program.MUTEX_CONSOLE)
                        {
                            Program.ReciveData(id, Side.SERVER_TO_CLIENT, buffer, size);
                        }

                        serverStream.Write(buffer, 0, size);
                    }
                }
                catch (Exception e)
                {
                    run = false;
                    break;
                }
            }

            clientSession.Close();
            tcpServer.Close();
        }
        
        private void ServerToClient()
        {
            byte[] buffer = new byte[256];

            while (run)
            {
                try
                {
                    int size = serverStream.Read(buffer, 0, buffer.Length);
                    if (size > 0)
                    {
                        lock (Program.MUTEX_CONSOLE)
                        {
                            Program.ReciveData(id, Side.CLIENT_TO_SERVER, buffer, size);
                        }

                        clientStream.Write(buffer, 0, size);
                    }
                }
                catch (Exception e)
                {
                    run = false;
                    break;
                }
            }

            clientSession.Close();
            tcpServer.Close();
        }
    }
}
