using System;
using System.Collections.Generic;
using System.Linq;

namespace WoD20th_DiceRoller
{
    class Program
    {
        static readonly Random rng = new Random();
        // ---------- Dice Randomization ---------- \\

        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\
        // | ---------- |                                                                                                                                 | ---------- | \\
        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\

        static void Main(string[] args) 
        // ---------- Main ---------- \\
        {
            Console.WriteLine("\n|| - - - - - World of Darkness: 20th Anniversary - Dice Roller - - - - - ||");

            bool rollAgain = true;
            while (rollAgain)
            {
                Console.WriteLine(" ");
                roll();
            }

            Console.WriteLine("End.");
        }

        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\
        // | ---------- |                                                                                                                                 | ---------- | \\
        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\

        static void roll()
        // ---------- Rolling ---------- \\
        {
            int diceCount = ReadInt("Dice Amount: ", min: 1);
            int difficulty = ReadInt("Difficulty: ", min: 2, max: 10);

            Console.Write("Willpower (y/n): ");
            bool willpower = YesNo();

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

            // ----- Roll ----- \\
            List<int> results = new List<int>();
            for (int i = 0; i < diceCount; i++)
            {
                results.Add(rng.Next(1, 11));
            }

            // ----- Successes ----- \\
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

            // ----- Botch ----- \\
            int rawSuccesses = successes;
            if (!noBotch)
            {
                successes -= ones;
            }

            // ----- Modifier ----- \\
            int totalSuccesses = successes + modifier;

            // ----- Willpower ----- \\
            if (willpower)
            {
                totalSuccesses += 1;
            }

            // ----- Output ----- \\
            Console.WriteLine();
            Console.Write($"Dice Pool ({diceCount}): {string.Join(", ", results)} | Difficulty: {difficulty}");
            if (willpower) Console.WriteLine(" | Willpower: On ");
            if (specialty) Console.Write("Specialty: On ");
            if (modifierToggle) Console.Write($"| Modifier: {modifier:+0;-0;0} ");
            if (noBotch) Console.Write("| No Botch: On ");

            Console.WriteLine();
            if (totalSuccesses <= 0)
            {
                if (!noBotch && ones > 0 && rawSuccesses == 0 && !willpower)
                {
                    Console.WriteLine("---> BOTCHED <---");
                }
                else
                {
                    Console.WriteLine("---> FAILED <---");
                }
            }
            else
            {
                Console.WriteLine($"---> {totalSuccesses} Success{(totalSuccesses == 1 ? "" : "es")} <---");
            }
        }

        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\
        // | ---------- |                                                                                                                                 | ---------- | \\
        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\

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

                Console.WriteLine($"Number{(min != int.MinValue || max != int.MaxValue ? $" between {min} and {max}" : "")}.");
            }
        }

        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\
        // | ---------- |                                                                                                                                 | ---------- | \\
        // | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | ---------- | \\

        static bool YesNo()
        // ---------- Loop ---------- \\
        {
            while (true)
            {
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "Y") return true;
                if (input == "n" || input == "N") return false;
                Console.WriteLine("(y/n)");
            }
        }
    }
}
