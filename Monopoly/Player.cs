﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
    public class Player : IEquatable<Player>
    {
        static Board board;
        private string name;
        public ushort position;
        private int money;
        private ushort inJailCount = 0;
        public List<BuyableCase> possessions;

        public string Name { get => name; set => name = value; }
        public int Money { get => money; set => money = value; }

        public Player(string _name, Board board)
        {
           // board = Board.Instance;
            name = _name;
            position = 0;
            money = 150000;
            possessions = new List<BuyableCase>();
            Player.board = board;
        }

        /// <summary>
        /// Roll the dices, move the player and trigger effetcs
        /// If it's a double, play again
        /// Player will go to jail after 3 consecutive double
        /// </summary>
        /// TO REDO
        public void Play() 
        {
            ushort[] dices;
            ushort combo;

            combo = 3;
            do
            {
                dices = board.dices.Roll();
                combo--;
                Console.WriteLine("Vous avez fait {0}", dices[0] + dices[1]);
                if(dices[0] == dices[1])
                {
                    Console.WriteLine("C'est un double !");
                    if(inJailCount > 0)
                    {
                        Console.WriteLine("Vous sortez de prison !");
                        inJailCount = 0;
                    }
                    if(combo >= 0)
                    {
                        this.Forward((ushort)(dices[0]+dices[1]));
                        Console.WriteLine("\nVous allez rejouer.");
                    }
                    else
                    {
                        Console.WriteLine("Vous allez directement en prison.");
                    }
                }
                else if(inJailCount == 0)
                    this.Forward((ushort)(dices[0] + dices[1]));
                else
                {
                    inJailCount--;
                    Console.WriteLine("Dommage...");
                }

            }
            while (dices[0] == dices[1] && combo >= 1);
            if (combo == 0)
            {
                this.teleport(30);
            }
        }

        public int[] GetHousesAndHotels()
        {
            int[] ret = new int[2];
            foreach(BuyableCase poss in possessions)
            {
                ret[0] += poss.houses;
                ret[1] += poss.hotel;
            }
            return ret;
        }

        public void Backward(ushort value) 
        {
            this.teleport((ushort)((position - value)%40));
        }

        public void SendToJail()
        {
            position = 10;
            inJailCount = 3;
        }

        public void Forward(ushort value) 
        {
            if (position + value > 39)
                money += 20000;
            this.teleport((ushort)((position + value)%40));
        }

        public void teleport(ushort position)
        {
            this.position = (ushort)(position % 40);
            Board.PositionUpdate(this);
        }

        public void PositionDisp()
        {
            foreach(Player p in Board.players)
                Console.WriteLine("Le joueur " + p.name + " est actuellement à la case " + Board.cases[p.position].name + " (" + ((p.position/5)+1) + "eme ligne)");
        }

        public void JailDisp()
        {
            bool verif;
            string answer;

            verif = false;
            this.Summary();
            Console.WriteLine("Vous êtes en prison");
            Console.WriteLine("Que voulez-vous faire ?");
            Console.WriteLine("1. Payer 5 000 euros pour sortir de prison");
            Console.WriteLine("2. Tenter de faire un double pour vous échapper");
            while(!verif)
            {
                Console.WriteLine("\nEntrez votre choix : ");
                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        Console.WriteLine("Vous payer 5 000 euros et sortez de prison");
                        Console.WriteLine("Appuyez sur une touche pour continuer");
                        Console.ReadKey();
                        this.Taxe(5000);
                        inJailCount = 0;
                        this.Turn();
                        verif = true;
                        break;

                    case "2":
                        this.Play();
                        verif = true;
                        break;

                    default:
                        break;
                }
            }
        }

        public void Turn()
        {
            bool verif1 = false;
            bool verif2 = false;
            Console.Clear();
            Board.Display();
            PositionDisp();
            Console.WriteLine("C'est au tour de {0}", this.Name);
            if(inJailCount > 0)
                JailDisp();
            else
            {
            while (!verif1)
            {
                this.Summary();
                Console.WriteLine("Que voulez-vous faire ?");
                Console.WriteLine("1. Lancer les des");
                Console.WriteLine("2. Consulter vos proprietes");
                Console.WriteLine("\nEntrez votre choix : ");
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        this.Play();
                        verif1 = true;
                        break;

                    case "2":
                        this.Consult();
                        break;

                    default:
                        break;
                }
            }
            
            Console.Clear();
            Board.Display();
            PositionDisp();
            while (!verif2)
            {
                this.Summary();
                Console.WriteLine("Que voulez-vous faire ?");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("1. Lancer les des");
                Console.ResetColor();
                Console.WriteLine("2. Consulter vos proprietes");
                Console.WriteLine("0. Passer au joueur suivant");
                Console.WriteLine("\nEntrez votre choix : ");
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
                        Console.WriteLine("Vous avez deja joue votre tour.");
                        Console.ReadKey();
                        break;

                    case "2":
                        this.Consult();
                        break;

                    case "0":
                        verif2 = true;
                        break;

                    default:
                        break;
                }
            }
            }
            Console.WriteLine("Votre tour est termine. Veuillez appuyer sur une touche pour passer au joueur suivant.");
            Console.ReadKey();

        }

        public void Summary()
        {
            int propDisp = 0;
            int propMort = 0;
            foreach(BuyableCase bc in possessions)
            {
                if (bc.IsMort)
                {
                    propMort++;
                }
                else
                {
                    propDisp++;
                }
            }
            Console.WriteLine("Joueur {0} \n\nArgent disponible {1}\nProporietes disponibles : {2}\nProprietes hypothequees : {3}\n\n",this.Name, this.Money, propDisp, propMort);
        }

        public void Consult()
        {
            Console.Clear();
            Console.WriteLine("Liste des proprietes de {0} :", this.Name);
            Console.WriteLine("Les proprietes en rouge sont hypothequees \n");
            possessions.Sort();
            int i = 1;
            foreach(BuyableCase bc in this.possessions)
            {
                Console.Write("{0}. ", i);
                i++;
                bc.Display();
            }
            Console.WriteLine("\nEntrez le numero de la propriete que vous voulez gerer.. (0 pour retourner au menu de jeu)");
            bool verif2 = false;
            do
            {
                string answer = Console.ReadLine();
                if (answer == "0")
                {
                    return;
                }
                i = 1;
                bool verif = false;
                foreach (BuyableCase bc in this.possessions)
                {
                    if (i.ToString() == answer)
                    {
                        bc.Manage();
                        verif = true;
                        verif2 = true;
                    }
                    i++;
                }
                if (!verif)
                {
                    Console.WriteLine("Je n'ai pas compris..Veuillez reessayer");
                }
            } while (!verif2);
        }

        public void Taxe(int amount)
        {
            this.money = this.money - amount;
            Board.Failure(this);
        }

        public bool Equals(Player other)
        {
            if(other != null)
            {
                return this.name.Equals(other.name);
            }
            else
            {
                return false;
            }
        }
    }
}
