using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFilRouge.JeuDeLaVie
{
    public class Game
    {
        int x;
        int y;
        int[,] grid;
        int RuleAlive { get; set; }

        /*
         * Constructeur
         */
        public Game(int x, int y, int ruleAlive)
        {
            // Initialisation
            this.x = x;
            this.y = y;
            this.RuleAlive = ruleAlive;
            grid = new int[x, y];

            Random random = new Random();

            // Remplissage de notre grille avec des 1 et des 0
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (random.Next(100) < 30) // Utilisez le même générateur de nombres aléatoires
                    {
                        grid[i, j] = 1;
                    }
                    else
                    {
                        grid[i, j] = 0;
                    }
                }
            }
        }
        public int[,] GetGrid()
        {
            return grid;
        }

        public int GetRuleAlive()
        {
            return this.RuleAlive;
        }

        public void SetRuleAlive(int newRuleAlive)
        {
            this.RuleAlive = newRuleAlive;
        }

        /*
         * Permet de réinitialiser la grid (notamment au lancement du jeu)
         */
        public void Reset()
        {

            Random random = new Random(); // Initialisez le générateur de nombres aléatoires une seule fois

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (random.Next(100) < 30) // Utilisez le même générateur de nombres aléatoires
                    {
                        grid[i, j] = 1;
                    }
                    else
                    {
                        grid[i, j] = 0;
                    }
                }
            }
        }

        /*
         * Calcul un nouvel état de la grid
         */
        public int[,] NewState()
        {

            // On ititialise la prochaine grille
            int[,] nextGrid = new int[x, y];

            // On parcourt toute notre grille
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    // On compte le nombre de case à 1 (alive) autour
                    int alive = 0;

                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (!(k == 0 && l == 0) &&
                                i + k >= 0 && i + k < x &&
                                j + l >= 0 && j + l < y)
                            {
                                if (grid[i + k, j + l] == 1)
                                {
                                    alive++;
                                }
                            }
                        }
                    }
                    // On assigne les 1 dans la nouvelle grille
                    if (alive >= RuleAlive)
                    {
                        nextGrid[i, j] = 1;
                    }
                }
            }
            grid = nextGrid;
            return nextGrid;
        }
    }
}
