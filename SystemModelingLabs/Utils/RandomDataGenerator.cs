namespace SystemModelingLabs.Utils
{
    public static class RandomDataGenerator
    {
        private static string[] LicencePlatePrefixes = { "AA", "AI", "AB", "AC", "AE", "AH", "AP", "BC", "BH", "AX" };
        private static string[] LicencePlatePostfixes = { "EC", "KO", "MI", "TX", "PM", "BB", "HK", "BM", "XX", "OO" };

        public static string GetRandomLicensePlate()
        {
            var rng = new Random();
            var prefix = LicencePlatePrefixes[rng.Next(LicencePlatePrefixes.Length)];
            var postfix = LicencePlatePostfixes[rng.Next(LicencePlatePostfixes.Length)];
            var number = rng.Next(10000);
            return prefix + number + postfix;
        }

        private static string[] FirstNames = { "James", "Robert", "John", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Christopher" ,
                                                "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen" };

        private static string[] LastNames = { "Smith", "Jones", "Taylor", "Williams", "Brown", "White", "Harris"," Martin", "Davies", "Wilson" };

        public static string GetRandomPersonName()
        {
            var rng = new Random();
            var firstName = FirstNames[rng.Next(FirstNames.Length)];
            var lastName = LastNames[rng.Next(LastNames.Length)];
            return $"{firstName} {lastName}";
        }
    }
}
