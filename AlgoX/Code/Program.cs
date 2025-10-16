namespace AlgoX.Code;

public class Program
{
    public static void Main()
    {
        const string txtPath = "../../../Associations/Associations.txt";
        const string selectOption = "\n Selected option: ";
        const string menu = "\n ALGO X MENU" +
            "\n -------------" +
            "\n [1] - Encode message" +
            "\n [2] - Decode message" +
            "\n [3] - New associations" +
            "\n [0] - Exit";

        bool exit = false;
        string menuOption = string.Empty;

        int numLetters = 26;
        int numNumbers = 10;

        char[] letters = new char[numLetters];
        int[] numbers = new int[numNumbers];

        for (int i = 65; i < numLetters + 65; i++)
        {
            letters[i-65] = (char)i;
        }
        for (int i = 0; i < numNumbers; i++)
        {
            numbers[i] = i;
        }

        AlgoXHelper algox = new AlgoXHelper(letters, numbers, txtPath);
        algox.GeneralSetup();

        while (!exit)
        {
            Console.WriteLine(menu);
            Console.Write(selectOption);
            menuOption = Console.ReadLine();

            switch (menuOption)
            {
                case "0":
                    exit = true;
                    break;
                case "1":
                    Console.Write("\n Write your message: ");
                    string msgEncode = Console.ReadLine();
                    AlgoXHelper.PrintWithColor($"\n Encoded message: {algox.EncodeMessage(msgEncode)}", ConsoleColor.Yellow);
                    break;
                case "2":
                    Console.Write("\n Write your message: ");
                    string msgDecode = Console.ReadLine();
                    AlgoXHelper.PrintWithColor($"\n Decoded message: {algox.DecodeMessage(msgDecode)}", ConsoleColor.Cyan);
                    break;
                case "3":
                    algox.RegenerateAssociations();
                    AlgoXHelper.PrintWithColor($"\n New associations created!", ConsoleColor.Green);
                    break;
                default:
                    Console.WriteLine("\n This option is invalid!");
                    break;
            }
        }
    }
}