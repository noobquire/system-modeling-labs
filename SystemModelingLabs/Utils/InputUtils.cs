namespace SystemModelingLabs.Utils
{
    public static class InputUtils
    {
        public static int GetInt(string name, int min = 0, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write($"Please enter {name} from {min} to {max}: ");
                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out int number))
                {
                    Console.WriteLine("Incorrect input, please try again");
                    continue;
                }

                if (number < min || number > max)
                {
                    Console.WriteLine($"{name} must be between {min} and {max}");
                    continue;
                }

                return number;
            }
        }

        public static double GetDouble(string name, double min = 0, double max = double.MaxValue)
        {
            while (true)
            {
                Console.Write($"Please enter {name} from {min:D1} to {max:D1}: : ");
                var input = Console.ReadLine()?.Trim();
                if (!double.TryParse(input, out double number))
                {
                    Console.WriteLine("Incorrect input, please try again");
                    continue;
                }

                if (number < min || number > max)
                {
                    Console.WriteLine($"{name} must be between {min:D1} and {max:D1}");
                    continue;
                }

                return number;
            }
        }

        private static string GetString(string name, bool notEmpty = true)
        {
            while (true)
            {
                Console.Write($"Please enter {name}: ");
                var input = Console.ReadLine();

                if (notEmpty && string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{name} must not be empty");
                    continue;
                }

                return input ?? "";
            }
        }
    }
}
