using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFilRouge.Chat
{
    internal class SocketClient
    {

        private TcpClient client;

        public SocketClient(string serverIp, int port)
        {
            this.client = new TcpClient();
            client.Connect(serverIp, port);
        }

        public void StartListening(Action<string> messageReceivedCallback)
        {
            /*
             * Ecoute le serveur pour recevoir les messages
             */

            Thread listenerThread = new Thread(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Données reçues du serveur : {dataReceived}");

                        // Appeler la méthode messageReceivedCallback pour afficher les données dans l'interface utilisateur
                        messageReceivedCallback?.Invoke(dataReceived);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la réception des données du serveur : {ex.Message}");
                }
            });

            listenerThread.Start();
        }


        public void Send(string message)
        {
            /*
             * Envoi le message au serveur 
             */

            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

    }
}
