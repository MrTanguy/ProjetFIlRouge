using MySql.Data.MySqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using dotenv.net;
using System.Windows;
using System.Reflection;

namespace ProjetFilRouge.BDD
{
    public class BddMySql
    {
        private readonly MySqlConnection connection;
        private readonly string salt;

        public BddMySql()
        {
            /*
             * Initialisation de la connexion string pour la BDD
             * \/!\ Je n'ai pas réussi à récupérer mon fichier .env
             */

            // Construction de la chaîne de connexion
            string server = "localhost";
            int port = 3306;
            string database = "socket";
            string username = "mysql";
            string password = "Password123";

            String connectionString = $"Server={server};Port={port};Database={database};User={username};Pwd={password};";
            connection = new MySqlConnection(connectionString);

            // Définition du salt pour le mdp (doit être dans le fichier .Env)
            salt = "salt";
        }

        public bool Login(string username, string password)
        {
            /*
             * Permet à l'utilisateur de se login
             */

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");
                return false;
            }

            try
            {
                // Chargement de la requête SQL à partir du fichier
                string query = File.ReadAllText("../../../../ProjetFilRouge/BDD/request/login.sql");

                // Envoie la requête avec les binds variables
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", HashAndSaltPassword(password));
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du requêtage : {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool Register(string username, string password)
        {
            /*
             * Permet à l'utilisateur de se register
             */

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");
                return false;
            }
            try
            {
                // Chargement de la requête SQL à partir du fichier
                string query = File.ReadAllText("../../../../ProjetFilRouge/BDD/request/register.sql");

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", HashAndSaltPassword(password));
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du requêtage : {ex.Message}");
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        private string HashAndSaltPassword(string password)
        {
            /*
             * Permet de hasher et saler le mot de passe
             */

            // Convertir le mot de passe et le sel en tableaux de bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Concaténer le mot de passe avec le sel
            byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Array.Copy(passwordBytes, passwordWithSaltBytes, passwordBytes.Length);
            Array.Copy(saltBytes, 0, passwordWithSaltBytes, passwordBytes.Length, saltBytes.Length);

            // Calculer le haché SHA-256 du mot de passe avec le sel
            byte[] hashedBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashedBytes = sha256.ComputeHash(passwordWithSaltBytes);
            }

            // Convertir le haché en une chaîne hexadécimale
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            string hashedPassword = builder.ToString();

            // Retourner le haché du mot de passe concaténé avec le sel (pour pouvoir le vérifier plus tard)
            return hashedPassword;
        }
    }
}
