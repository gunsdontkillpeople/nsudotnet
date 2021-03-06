﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {


            Game game = new Game();
            DateTime startTime = DateTime.Now;
            game.StartGame();
            TimeSpan spentTime = DateTime.Now - startTime;
            Console.WriteLine("It took {0} minutes", spentTime.TotalMinutes);
            Console.ReadLine();
        }
    }

    class Game
    {
        private string _name = null;
        private int _number = 0;    //nubmer [0..100]

        public void StartGame()
        {
            Console.WriteLine("Please, enter your name:");
            while(String.IsNullOrEmpty(_name = Console.ReadLine()));
            Random rand = new Random();
            _number = rand.Next(0, 100);
            Console.WriteLine("Game started!!!");
            GameAlg();
        }

        public void GameAlg()
        {
            string[] insults = new string[]
            {
                "{0}, you are doing great (no)",
                "{0}, are you stupid?",
                "You are stupid, {0}.",
                "{0}, they say, a person who couldn't guess within 4 attempts is a complete loser. How many trials do you think you had?",
                "{0}, there is a pokemon called \"Slowpoke\", is it your brother?",
                "{0}, if my parrot knew 10 words less, he would  be smarter then you anyway. My parrot knows 9 words...",
                "{0}, today is just not your day. As always."
            };
            string number = "";
            int[] arrayTries = new int[1000];
            int iterations = 0;
            while (!number.Equals("q"))
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("Enter your number:");
                    number = Console.ReadLine();
                    int number2;
                    if(Int32.TryParse(number, out number2))
                    { 
                        arrayTries[iterations] = number2;
                        iterations++;
                        if (_number > number2)
                        {
                            Console.WriteLine("Your number is less");
                        }
                        else
                        {
                            if (_number < number2)
                            {
                                Console.WriteLine("Your number is bigger");
                            }
                            else
                            {
                                Console.WriteLine("Congratulations, you won!!!");
                                Console.WriteLine("You made {0} tries:", iterations);
                                for (int j = 0; j < iterations - 1; j++)
                                {
                                    if (arrayTries[j] < _number)
                                    {
                                        Console.WriteLine("{0}) Number = {1} <", j, arrayTries[j]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0}) Number = {1} >", j, arrayTries[j]);
                                    }
                                }
                                Console.WriteLine("{0}) Number = {1} =", iterations, arrayTries[iterations - 1]);
                                return;
                            }
                        }

                    }
                    else
                    {
                        if (number.Equals("q"))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input data!");
                        }
                    }
                }
                if (!number.Equals("q"))
                {
                    Random random = new Random();
                    Console.WriteLine(insults[random.Next(0, 6)], _name);
                }
            }
            Console.WriteLine("Sorry game is over, see you later)");
        }

    }
}
