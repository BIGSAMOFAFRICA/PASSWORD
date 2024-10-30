using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Password Generator made by Olabisi Samuel!");

        int length;
        while (true)
        {
            Console.Write("Enter password length (min 6): ");
            if (int.TryParse(Console.ReadLine(), out length) && length >= 6) break;
            Console.WriteLine("Invalid. Enter a number >= 6.");
        }

        bool includeUpper = GetUserChoice("Uppercase letters? (y/n): ");
        bool includeLower = GetUserChoice("Lowercase letters? (y/n): ");
        bool includeDigits = GetUserChoice("Digits? (y/n): ");
        bool includeSpecial = GetUserChoice("Special chars? (y/n): ");

        string password = GeneratePassword(length, includeUpper, includeLower, includeDigits, includeSpecial);
        Console.WriteLine($"Generated Password: {password}");

        CheckPasswordStrength(password);

        if (GetUserChoice("Save this password? (y/n): "))
        {
            SavePasswordToFile(password);
            Console.WriteLine("Password saved to 'Passwords.txt'.");
        }
    }

    static bool GetUserChoice(string question)
    {
        Console.Write(question);
        char choice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return choice == 'y' || choice == 'Y';
    }

    static string GeneratePassword(int length, bool includeUpper, bool includeLower, bool includeDigits, bool includeSpecial)
    {
        StringBuilder password = new StringBuilder();
        string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        string digitChars = "0123456789";
        string specialChars = "!@#$%^&*()_-+=<>?";

        string availableChars = "";
        if (includeUpper) availableChars += upperChars;
        if (includeLower) availableChars += lowerChars;
        if (includeDigits) availableChars += digitChars;
        if (includeSpecial) availableChars += specialChars;

        if (availableChars.Length == 0) throw new ArgumentException("Select at least one character type.");

        Random random = new Random();

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(availableChars.Length);
            password.Append(availableChars[index]);
        }

        return password.ToString();
    }

    static void CheckPasswordStrength(string password)
    {
        int score = 0;

        if (password.Length >= 8) score++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) score++;
        if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[\W]")) score++;

        Console.WriteLine("Password Strength: " + (score switch
        {
            0 => "Very Weak",
            1 => "Weak",
            2 => "Moderate",
            3 => "Strong",
            4 => "Very Strong",
            _ => "Unknown"
        }));
    }

    static void SavePasswordToFile(string password)
    {
        using (StreamWriter writer = new StreamWriter("Passwords.txt", true))
        {
            writer.WriteLine(password);
        }
    }
}
