using ProjetFilRouge.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ProjetFilRouge.Chat;
using ProjetFilRouge.JeuDeLaVie;

namespace ProjetFilRouge.JeuDeLaVie
{
    /// <summary>
    /// Logique d'interaction pour GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        private SocketClient clientSocket;
        private Game game;
        private DispatcherTimer timer;
        private int currentStep;

        public GameUserControl()
        {
            /*
             * Initialise une partie et lance le serveur Socket (pour cette instance)
             */

            InitializeComponent();

            this.game = new Game(x: 14, y: 35, ruleAlive: 3);

            // Partie serveur Socket
            Thread serverThread = new Thread(new ThreadStart(() =>
            {
                Game game = new Game(x: 10, y: 20, ruleAlive: 3);
                SocketServeur serveur = new SocketServeur(port: 9999);
                serveur.Start(game: game);
            }));
            serverThread.Start();

            // Partie client Socket
            clientSocket = new SocketClient("127.0.0.1", 9999);
            clientSocket.StartListening(AfficherMessage);
        }

        /*
         * ######################
         * PARTIE CHAT VIA SOCKET
         * ######################
         */

        private void AfficherMessage(string message)
        {
            /*
             * Affiche le message dans le chat (réçu par le serveur via socket)
             */
            if (richTextBoxOutput.Dispatcher.CheckAccess())
            {
                // Nous sommes déjà sur le thread de l'interface utilisateur, donc nous pouvons mettre à jour directement le contrôle
                richTextBoxOutput.AppendText(message + "\n");
            }
            else
            {
                // Nous ne sommes pas sur le thread de l'interface utilisateur, donc nous devons utiliser Invoke pour exécuter le code sur ce thread
                richTextBoxOutput.Dispatcher.Invoke((Action)(() => AfficherMessage(message)));
            }
        }

        public void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Envoie le message au servieur (via socket)
             */
            TextRange textRange = new TextRange(richTextBoxInput.Document.ContentStart, richTextBoxInput.Document.ContentEnd);
            string messageToSend = textRange.Text.TrimEnd();

            string username = Environment.GetEnvironmentVariable("user")!;

            clientSocket.Send($"{username} :\n{messageToSend}");
            richTextBoxInput.Document.Blocks.Clear();
        }

        /*
         * ##########
         * PARTIE JEU
         * ##########
         */
        private void Timer_Tick(object sender, EventArgs e)
        {
            /*
             * Cette fonction est déclenchée à chaque tick (1 seconde) :
             * Calcul l'état du jeu suivant 
             * Affiche le nouvel état
             */

            // Incrémenter le compteur d'étapes
            currentStep++;

            // Afficher le jeu
            if (currentStep == 1)
            {
                AfficherJeuDeLaVie(game.GetGrid()); // On affiche l'état initial
            }
            else if (currentStep >= 10)
            {
                int[,] newState = game.NewState(); // On calcul l'état suivant
                AfficherJeuDeLaVie(newState, "-------END-------");
                // Arrêter le timer après 10 étapes
                timer.Stop();
            }
            else
            {
                int[,] newState = game.NewState();
                AfficherJeuDeLaVie(newState);
            }
        }

        private void AfficherJeuDeLaVie(int[,] games, string option = "")
        {
            /*
             * Parcourt notre grille de jeu et l'affiche
             */

            richTextBoxGameOfLife.Document.Blocks.Clear(); // Efface le contenu du RichTextBox

            StringBuilder jeuBuilder = new StringBuilder(); // Utilisation d'un StringBuilder pour améliorer les performances

            for (int i = 0; i < games.GetLength(0); i++)
            {
                for (int j = 0; j < games.GetLength(1); j++)
                {
                    jeuBuilder.Append(games[i, j]);
                }
                jeuBuilder.AppendLine(); // Ajoute une ligne après chaque ligne de jeu
            }
            jeuBuilder.Append(option); // Ajoute l'option à la fin du jeu

            richTextBoxGameOfLife.AppendText(jeuBuilder.ToString()); // Ajoute le jeu complet au RichTextBox
        }


        public void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Démarre une simulation du jeu de la vie
             */

            game.Reset(); // Reset de la grille

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Interval en millisecondes entre chaque étape
            timer.Tick += Timer_Tick;

            currentStep = 0; // Réinitialiser le compteur d'étapes
            richTextBoxGameOfLife.Document.Blocks.Clear(); // Effacer la zone de texte

            timer.Start(); // Démarrer le timer
        }
    }
}
