using ProjetFilRouge.BDD;
using ProjetFilRouge.JeuDeLaVie;
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

namespace ProjetFilRouge.User
{
    /// <summary>
    /// Logique d'interaction pour RegisterUserControl.xaml
    /// </summary>
    public partial class RegisterUserControl : UserControl
    {
        BddMySql bdd = new BddMySql();
        public string username;
        public string password;


        public RegisterUserControl()
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
                bool result = bdd.Register(username: username, password: password);
                if (result)
                {
                    MessageBox.Show("Inscription réussie !");

                    Environment.SetEnvironmentVariable("user", username);

                    GameUserControl gameUserControl = new GameUserControl();

                    // On charge la mainWindow
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.ContenuPrincipal.Content = gameUserControl;
                }
                else
                {
                    MessageBox.Show("Échec de l'inscription. Veuillez réessayer.");
                }
            }
        }
    }
}
