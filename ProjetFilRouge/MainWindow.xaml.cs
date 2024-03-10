using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjetFilRouge.User;

namespace ProjetFilRouge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectUserControl connectUserControl = new ConnectUserControl();
        RegisterUserControl registerUserControl = new RegisterUserControl();
        ContentControl Content;

        public MainWindow()
        {
            InitializeComponent();

            Content = ContenuPrincipal;
        }

        public void Connect_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = connectUserControl;
        }

        public void Register_Click(object sender, RoutedEventArgs e)
        {
            Content.Content = registerUserControl;
        }
    }
}