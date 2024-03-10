using ProjetFilRouge.BDD;
using ProjetFilRouge.JeuDeLaVie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetFilRouge.User
{
    /// <summary>
    /// Logique d'interaction pour ConnectUserControl.xaml
    /// </summary>
    public partial class ConnectUserControl : UserControl
    {
        BddMySql bdd = new BddMySql();
        public string username;
        public string password;

        public ConnectUserControl()
        {
            InitializeComponent();
        }

        [STAThread]
        public void Valid_Click(object sender, RoutedEventArgs e)
        {

            username = Username.Text;
            password = Password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(messageBoxText: "Merci de remplir les champs.");
            }
            else
            {
                bool result = bdd.Login(username: username, password: password);
                if (result)
                {
                    Environment.SetEnvironmentVariable("user", username);
                    GameUserControl gameUserControl = new GameUserControl();

                    // On charge la mainWindow
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.ContenuPrincipal.Content = gameUserControl;
                }
                else
                {
                    MessageBox.Show("Échec de la connexion. Veuillez réessayer.");
                }
            }
        }
    }
}
