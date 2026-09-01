using System;
using System.Collections.Generic;
using System.Linq;

namespace WoD20th_DiceRoller
{
    class Program
    {
        static readonly Random rng = new Random();
        // ---------- Dice Randomization ---------- \\

        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //
        // ---------- |            |            |            |            |            |            |            |            |            |            | ---------- | //
        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //

        static void Main(string[] args) 
        // ---------- Main ---------- \\
        {
            Console.WriteLine("|| - - - - - World of Darkness 20th Anniversary Dice Roller - - - - - ||\n");

            bool rollAgain = true;
            while (rollAgain)
            {
                roll();
                Console.WriteLine("\nRoll again (y/n):");
                rollAgain = YesNo();
                Console.WriteLine();
            }

            Console.WriteLine("End.");
        }

        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //
        // ---------- |            |            |            |            |            |            |            |            |            |            | ---------- | //
        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //

        static void roll()
        // ---------- Rolling ---------- \\
        {
            int diceCount = ReadInt("Dice Amount: ", min: 1);
            int difficulty = ReadInt("Difficulty: ", min: 2, max: 10);

            Console.Write("Specialty (y/n): ");
            bool specialty = YesNo();

            Console.Write("Modifier (y/n): ");
            bool modifierToggle = YesNo();
            int modifier = 0;
            if (modifierToggle)
            {
                modifier = ReadInt("Modifier Value: ", allowNegative: true);
            }

            Console.Write("No Botch (y/n): ");
            bool noBotch = YesNo();

            // --- Roll --- \\
            List<int> results = new List<int>();
            for (int i = 0; i < diceCount; i++)
            {
                results.Add(rng.Next(1, 11));
            }

            // --- Successes --- \\
            int successes = 0;
            int ones = 0;

            foreach (int die in results)
            {
                if (die == 1)
                {
                    ones++;
                }

                if (die >= difficulty)
                {
                    successes += (specialty && die == 10) ? 2 : 1;
                }
            }

            // --- Botch --- \\
            int rawSuccesses = successes;
            if (!noBotch)
            {
                successes -= ones;
            }

            // --- Modifier --- \\
            int totalSuccesses = successes + modifier;

            // --- Output --- \\
            Console.WriteLine();
            Console.WriteLine($"Dice rolled: {string.Join(", ", results)}");
            Console.WriteLine($"Difficulty: {difficulty}");
            if (specialty) Console.WriteLine("Specialty: Active");
            if (modifierToggle) Console.WriteLine($"Modifier: {modifier:+0;-0;0}");
            Console.WriteLine(noBotch ? "No Botch: Yes" : "No Botch: No");

            Console.WriteLine();
            if (totalSuccesses <= 0)
            {
                if (!noBotch && ones > 0 && rawSuccesses == 0)
                {
                    Console.WriteLine("-> BOTCHED <-");
                }
                else
                {
                    Console.WriteLine("-> FAILED <-");
                }
            }
            else
            {
                Console.WriteLine($"-> {totalSuccesses} Success{(totalSuccesses == 1 ? "" : "es")} <-");
            }
        }

        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //
        // ---------- |            |            |            |            |            |            |            |            |            |            | ---------- | //
        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //

        static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue, bool allowNegative = false)
        // ---------- Input Validity Check ---------- \\
        {
            int value;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out value))
                {
                    if (allowNegative || value >= 0)
                    {
                        if (value >= min && value <= max)
                        {
                            return value;
                        }
                    }
                }

                Console.WriteLine($"Please enter a valid number{(min != int.MinValue || max != int.MaxValue ? $" between {min} and {max}" : "")}.");
            }
        }

        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //
        // ---------- |            |            |            |            |            |            |            |            |            |            | ---------- | //
        // ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | //

        static bool YesNo()
        // ---------- Loop ---------- \\
        {
            while (true)
            {
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;
                Console.WriteLine("Invalid.");
            }
        }
    }
}