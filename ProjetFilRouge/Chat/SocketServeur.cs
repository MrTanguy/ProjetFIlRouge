using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using ProjetFilRouge.JeuDeLaVie;

namespace ProjetFilRouge.Chat
{
    internal class SocketServeur
    {
        private TcpListener listener;
        private List<TcpClient> clients;
        private Game game;

        public SocketServeur(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            clients = new List<TcpClient>();
        }

        public void Start(Game game)
        {
            /*
             * Lance le Socket : partie serveur
             */
            this.game = game;

            listener.Start();
            Console.WriteLine("Serveur démarré. En attente de connexion...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connecté.");

                // Ajouter le client à la liste des clients
                lock (clients)
                {
                    clients.Add(client);
                }

                // Démarrer un thread pour gérer les communications avec ce client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private void HandleClient(object obj)
        {
            /*
             * Ecoute les messages des clients
             */
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Données reçues du client : {dataReceived}");

                    string message = dataReceived.Split("\n")[1].ToString()!;

                    if (message.StartsWith("/ALIVE="))
                    {
                        string nbAlive = message.Remove(startIndex: 0, count: 7);
                        int nbAliveInt;

                        if (int.TryParse(nbAlive, out nbAliveInt))
                        {
                            string caption = "Nouvelle regle";
                            MessageBoxResult result = MessageBox.Show($"Acceptez-vous de passer le nombre de cellule necessaire pour etre vivant soit de {nbAlive}", caption, MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                SendMessageToAllClients($"[Nouvelle regle] : le nombre de cellules necessaires pour etre vivant est de {nbAlive}");
                                game.SetRuleAlive(nbAliveInt);
                            }
                        }
                        else
                        {
                            // La conversion a échoué
                            SendMessageToAllClients("Pour utiliser la commande /ALIVE=X : Merci de choisir un chiffre entre 0 et 8.");
                        }
                    }
                    else
                    {
                        // Envoyer le message à tous les clients
                        SendMessageToAllClients(dataReceived);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la communication avec le client : {ex.Message}");
            }
            finally
            {
                // Supprimer le client de la liste des clients lorsqu'il se déconnecte
                lock (clients)
                {
                    clients.Remove(client);
                }
                stream.Close();
                client.Close();
                Console.WriteLine("Client déconnecté.");
            }
        }

        private void SendMessageToAllClients(string message)
        {
            /*
             * Envoie le message à tous les clients
             */
            byte[] data = Encoding.ASCII.GetBytes(message);
            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }
}
