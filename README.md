# Projet Fil Rouge

## Prérequis
Avant de lancer le projet, assurez-vous d'avoir les éléments suivants installés sur votre machine :
- **C# :** Le projet est développé en utilisant le langage de programmation C#. Assurez-vous d'avoir un environnement de développement compatible avec C# installé sur votre machine.
- **Docker :** Nous utilisons Docker pour gérer la base de données MySQL de l'application. Vous pouvez télécharger Docker Desktop sur [le site officiel de Docker](https://www.docker.com/products/docker-desktop).

## Lancement du Projet
Voici les étapes pour lancer le projet sur votre machine :
1. **Clonez le Dépôt Git :** Utilisez la commande `git clone https://github.com/MrTanguy/ProjetFilRouge.git` pour cloner le dépôt Git sur votre machine locale.
2. **Démarrage de Docker Compose :** Naviguez jusqu'au répertoire du projet et exécutez la commande `docker-compose up` pour démarrer le conteneur Docker contenant la base de données MySQL.
3. **Lancement de l'Application :** Ouvrez le projet dans votre environnement de développement C# et lancez l'application.

## Fonctionnalités
L'application offre les fonctionnalités suivantes :

### 1. Connexion/Register
- Permet aux utilisateurs de se connecter à leur compte existant ou de s'enregistrer pour créer un nouveau compte.
- Les informations d'identification des utilisateurs sont stockées en toute sécurité dans la base de données MySQL.

### 2. Chat en Ligne
- Utilise un socket web pour permettre une communication en temps réel entre les utilisateurs connectés.
- Les utilisateurs peuvent discuter entre eux, échanger des messages et participer à des discussions de groupe.

### 3. Simulation du Jeu de la Vie
- Implémente le célèbre jeu de la vie de Conway avec une interface graphique conviviale.
- Les utilisateurs peuvent observer l'évolution des cellules en temps réel et interagir avec la simulation.

### 4. Personnalisation des Règles du Jeu
- Offre la possibilité de modifier les règles du jeu de la vie en utilisant une commande spéciale `/ALIVE=X` (X est un chiffre entre 0 et 8).
- Les utilisateurs peuvent expérimenter différentes configurations pour créer des scénarios uniques et intéressants.

## Développement
L'application offre 3 environnements de développements prod/quali/dev afin de pouvoir réaliser de multiples tests avant de déployer en production.
Elle offre aussi des tests unitaires dans l'onglet ProjetFilRougeTests.

Concernant la pipeline sur Jenkins, je n'ai pas réussi à la faire fonctionner avec un projet en C#.
