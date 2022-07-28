using System;

namespace Projet_VeloMax
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Veuillez choisir la version de l'application: 1. Version Présentation\t 2. version complete");
            int rep = int.Parse(Console.ReadLine());
            
            if (rep == 1)
            {
                Console.WriteLine("Bienvenue sur la demo de l'application : ");
                string user = "root";
                VeloMax_Manager manager = new VeloMax_Manager(user);
                manager.Chargement_bdd();
                Console.Clear();
                manager.Demo();
                

            }
            else
            {
                Console.WriteLine("Veuillez entrer votre identifiant: \t");
                string user = Console.ReadLine();
                bool user_ok = false;
                if (user == "bozo" || user == "root") user_ok = true;
                while (user_ok == false)
                {
                    Console.WriteLine("Nom de user inexistant: Veuillez réessayer: \t");
                    user = Console.ReadLine();
                    if (user == "bozo" || user == "root") user_ok = true;
                }
                Console.WriteLine("Bienvenue sur l'application VeloMax user " + user + " !");
                VeloMax_Manager manager = new VeloMax_Manager(user);//chargement table central de l'appli : veloMax_Manager
                if (user == "bozo") // cas utilisateur = "bozo" càd il n'a que le droit de lecture de la bdd et donc les fonctions qui sont base sur celui-ci
                {
                    bool quit = false;
                    manager.Chargement_bdd();
                    Console.WriteLine();// fonction pour afficher les tables à la suite 
                    do
                    {
                        Console.WriteLine("Veuillez choisir un type d'action parmis les suivantes: ");
                        Console.WriteLine("1. Menu gestion des stocks" + "\t2. Menu statistique" + "\t3. Quitter l'application");
                        Console.Write("Numéro action choisie: ");
                        int choix = int.Parse(Console.ReadLine());
                        switch (choix)
                        {
                            case 1: manager.Gestion_Stock(); break;
                            case 2: manager.Statistiques(); break;
                            case 3: quit = true; break;
                        }
                    } while (quit != true);
                    Console.WriteLine();
                }

                else // cas utilisateur = "root" càd il a tous les droits
                {

                    bool quit = false;
                    manager.Chargement_bdd();
                    Console.WriteLine();// fonction pour afficher les tables à la suite 
                    do
                    {
                        Console.WriteLine("Veuillez choisir un type d'action parmis les suivantes: ");
                        Console.WriteLine("1. Menu Insertion" + "\t2. Menu Mise à jour" + "\t3. Menu Suppression" + "\t4. Menu gestion des stocks" + "\t5. Menu statistique" + "\t6. Quitter l'application");
                        Console.Write("Numéro action choisie: ");
                        int choix = int.Parse(Console.ReadLine());
                        switch (choix)
                        {
                            case 1: manager.Insertion_element(); break;
                            case 2: manager.Mise_A_Jour(); break;
                            case 3: manager.Supprimer_element(); break;
                            case 4: manager.Gestion_Stock(); break;
                            case 5: manager.Statistiques(); break;
                            case 6: quit = true; break;
                        }
                    } while (quit != true);
                }
            }            
        }
    }
}
