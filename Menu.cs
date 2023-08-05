using System;

namespace WebScrapper;

public class Menu
{
    /// <summary>
    /// The program's main method.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        ProgramTV.ChannelsListLoad();
        ProgramTV.ChannelsListCheck();
    }

    /// <summary>
    /// A method that displays a program menu.
    /// </summary>
    static string MenuInfo()
    {
        var infoM = "MENU: \n\n" +
            "1. Wyświetl listę kanałów \n" +
            "2. Aktualizuj listę kanałów \n" +
            "3. Usuń listę kanałów \n" +
            "4. Wyszukaj kanał TV \n" +
            "5. Informacje \n" +
            "6. Zamknij program \n";

        return infoM;
    }

    /// <summary>
    /// A method that makes the user choose what I want him to do in this program.
    /// </summary>
    public static void MenuChoice()
    {
        Console.WriteLine(MenuInfo());

        var userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                ProgramTV.ChannelsList();
                MenuChoice();
                break;
            case "2":
                ProgramTV.ChannelsListUpdate();
                MenuChoice();
                break;
            case "3":
                ProgramTV.ChannelsListDelete();
                MenuChoice();
                break;
            case "4":
                ProgramTV.CanalTVSearch();
                MenuChoice();
                break;
            case "5":
                Console.WriteLine(Info());
                MenuChoice();
                break;
            case "6":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("\nWpisałeś/aś niewłaściwą cyfrę!\n");
                MenuChoice();
                break;
        }
    }

    /// <summary>
    /// A method that displays program information.
    /// </summary>
    static string Info()
    {
        var info = "\nWebScraper jest aplikacją, która służy do sprawdzania programów TV. \n" +
            "Aplikacja umożliwia wyświetlanie listy kanałów, jej aktualizowanie, usuwanie, czy nawet wyszukanie danego kanału telewizyjnego.\n" +
            "Można również zapisywać dane informacje na komputerze, plik będzie się znajdował wewnątrz plików programu. \n" +
            "© Marcin Koperski | 2023\n";

        return info;
    }
}