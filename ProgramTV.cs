using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace WebScrapper;

public class ProgramTV
{
    static List<CanalTV> ListChannels = new List<CanalTV>();
    static string filePathListChannels = "ListChannels.json";

    private const string ChannelsListLink = "https://www.telemagazyn.pl/stacje";
    private const string BaseURL = "https://www.telemagazyn.pl";

    /// <summary>
    /// A method that checks at startup whether a list of TV channels is created.
    /// </summary>
    public static void ChannelsListCheck()
    {
        if (ListChannels.Count == 0)
        {
            ChannelsListNew();
            Menu.MenuChoice();
        }
        else
            Menu.MenuChoice();
    }

    /// <summary>
    /// A method that displays a list of TV channels (if it exists).
    /// </summary>
    public static void ChannelsList()
    {
        if (ListChannels.Count == 0)
        {
            ChannelsListNew();
        }
        else
        {
            var count = 0;

            Console.WriteLine("\nLista kanałów telewizyjnych:\n");
            foreach (var ListChannel in ListChannels)
            {
                Console.WriteLine($"{ListChannel.lp}. {ListChannel.nameCanal} | {ListChannel.href}");
                count++;
            }

            Console.WriteLine($"\nLiczba kanałów: {count}\n");
        }
    }

    /// <summary>
    /// A method that displays a message to the user about the absence of a list of TV channels and asks whether to create such a list.
    /// </summary>
    static string ChannelsListNewInfo()
    {
        string info = "Nie masz żadnej listy kanałów telewizyjnych, czy chcesz ją pobrać?\n" +
            "1. Tak\n" +
            "2. Nie\n";

        return info;
    }

    /// <summary>
    /// A method that tells the user to select whether the program should create a list of TV channels.
    /// </summary>
    public static void ChannelsListNew()
    {
        Console.WriteLine($"{ChannelsListNewInfo()}");
        var userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine(ChannelsListNewInfoChoice());
                ChannelsListNewChoice();
                break;
            case "2":
                break;
            default:
                Console.WriteLine("\nWpisałeś/aś niewłaściwą cyfrę!\n");
                break;
        }
    }

    /// <summary>
    /// A method that displays a message to the user about which specific TV channels to download.
    /// </summary>
    static string ChannelsListNewInfoChoice()
    {
        string info = "\nWybierz, jakie konkretne kanały telewizyjne mają zostać pobrane?\n" +
            "1. Polskie\n" +
            "2. Zagraniczne\n" +
            "3. Wszystkie\n";

        return info;
    }

    /// <summary>
    /// A method that tells the user to choose which list of TV channels the program should download.
    /// </summary>
    public static void ChannelsListNewChoice()
    {
        var userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                ChannelsListPL();
                break;
            case "2":
                ChannelsListAbroad();
                break;
            case "3":
                ChannelsListAll();
                break;
            default:
                Console.WriteLine("\nWpisałeś/aś niewłaściwą cyfrę!\n");
                break;
        }
    }

    /// <summary>
    /// A method that creates a Polish list of TV channels.
    /// </summary>
    public static void ChannelsListPL()
    {
        var web = new HtmlWeb();
        var document = web.Load(ChannelsListLink);

        var sectionLIsPL = document.QuerySelectorAll("section li.polska");

        var count = 0;
        var lp = 1;

        foreach (var sectionLI in sectionLIsPL)
        {
            var LIs = sectionLI.QuerySelectorAll("li");

            var nameCanals = LIs[0].InnerText;
            var hrefLink = LIs[0].QuerySelector("a").Attributes["href"].Value;

            var link = $"{BaseURL}{hrefLink}";
            Console.WriteLine($"{lp}. {nameCanals} | {link}");

            CanalTV canalTV = new CanalTV(lp, nameCanals, link);
            ListChannels.Add(canalTV);

            count++;
            lp++;
        }

        Console.WriteLine($"\nLiczba kanałów: {count}\n");

        ChannelsListSave();
    }

    /// <summary>
    /// A method that creates a foreign TV channel list.
    /// </summary>
    public static void ChannelsListAbroad()
    {
        var web = new HtmlWeb();
        var document = web.Load(ChannelsListLink);

        var sectionLIsAbroad = document.QuerySelectorAll("section li.zagraniczna");

        var count = 0;
        var lp = 1;

        foreach (var sectionLI in sectionLIsAbroad)
        {
            var LIs = sectionLI.QuerySelectorAll("li");

            var nameCanals = LIs[0].InnerText;
            var hrefLink = LIs[0].QuerySelector("a").Attributes["href"].Value;

            var link = $"{BaseURL}{hrefLink}";
            Console.WriteLine($"{lp}. {nameCanals} | {link}");

            CanalTV canalTV = new CanalTV(lp, nameCanals, link);
            ListChannels.Add(canalTV);

            count++;
            lp++;
        }

        Console.WriteLine($"\nLiczba kanałów: {count}\n");

        ChannelsListSave();
    }

    /// <summary>
    /// A method that creates a list of all TV channels.
    /// </summary>
    public static void ChannelsListAll()
    {
        var web = new HtmlWeb();
        var document = web.Load(ChannelsListLink);

        var sectionLIsAll = document.QuerySelectorAll("section li").SkipLast(1);

        var count = 0;
        var lp = 1;

        foreach (var sectionLI in sectionLIsAll)
        {
            var LIs = sectionLI.QuerySelectorAll("li");

            var nameCanals = LIs[0].InnerText;
            var hrefLink = LIs[0].QuerySelector("a").Attributes["href"].Value;

            var link = $"{BaseURL}{hrefLink}";
            Console.WriteLine($"{lp}. {nameCanals} | {link}");

            CanalTV canalTV = new CanalTV(lp, nameCanals, link);
            ListChannels.Add(canalTV);

            count++;
            lp++;
        }

        Console.WriteLine($"\nLiczba kanałów: {count}\n");

        ChannelsListSave();
    }

    /// <summary>
    /// A method that saves a list of TV channels.
    /// </summary>
    public static void ChannelsListSave()
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(ListChannels);
            File.WriteAllText(filePathListChannels, jsonData);
        }
        catch (Exception)
        {
            Console.WriteLine("\nWystąpił błąd podczas zapisywania listy kanałów telewizyjnych!\n");
        }
    }

    /// <summary>
    /// A method that loads a list of TV channels.
    /// </summary>
    public static void ChannelsListLoad()
    {
        if (File.Exists(filePathListChannels))
        {
            try
            {
                string jsonData = File.ReadAllText(filePathListChannels);
                ListChannels = JsonConvert.DeserializeObject<List<CanalTV>>(jsonData)!;
            }
            catch (Exception)
            {
                Console.WriteLine("\nWystąpił błąd podczas wczytywania listy kanałów telewizyjnych!\n");
            }
        }
    }

    /// <summary>
    /// A method that displays a message to the user which list of TV channels to update.
    /// </summary>
    static string ChannelsListUpdateInfo()
    {
        string info = "\nWybierz, jakie konkretne kanały telewizyjne mają zostać zaktualizowane?\n" +
            "UWAGA! W tym momencie obecna lista kanałów telewizyjnych została usunięta, ale zostanie zastąpiona nową!\n" +
            "1. Polskie\n" +
            "2. Zagraniczne\n" +
            "3. Wszystkie\n";

        return info;
    }

    /// <summary>
    /// A method that updates a list of TV channels.
    /// </summary>
    public static void ChannelsListUpdate()
    {
        if (ListChannels.Count == 0)
        {
            Console.WriteLine("\nBrak listy kanałów telewizyjnych do zaktualizowania!\n");
        }
        else
        {
            ChannelsListDelete();
            Console.WriteLine(ChannelsListUpdateInfo());
            ChannelsListNewChoice();
        }
    }

    /// <summary>
    /// A method that deletes a list of TV channels.
    /// </summary>
    public static void ChannelsListDelete()
    {
        if (ListChannels.Count == 0)
        {
            Console.WriteLine("\nBrak listy kanałów telewizyjnych do usunięcia!\n");
        }
        else
        {
            File.Delete(filePathListChannels);
            ListChannels.Clear();
        }
    }

    /// <summary>
    /// A method that searches for the name of a TV channel.
    /// </summary>
    public static void CanalTVSearch()
    {
        if (ListChannels.Count == 0)
        {
            Console.WriteLine("\nBrak kanałów do wyszukiwania!\n");
            Menu.MenuChoice();
        }
        else
        {
            Console.WriteLine("\nWpisz nazwę kanału telewizyjnego:");
            var searchCanalTV = Console.ReadLine();

            List<CanalTV> searchResults = ListChannels.Where(s => s.nameCanal.Contains(searchCanalTV!, StringComparison.OrdinalIgnoreCase)).ToList();

            if (searchResults.Count == 0)
            {
                Console.WriteLine("\nBrak wyników wyszukiwania!\n");
                Menu.MenuChoice();
            }
            else
            {
                var count = 0;
                var lp = 1;

                Console.WriteLine("\nWyniki wyszukiwania:\n");
                foreach (var canalTV in searchResults)
                {
                    Console.WriteLine($"{lp}. {canalTV.nameCanal}");
                    lp++;
                    count++;
                }

                Console.WriteLine($"\nLiczba kanałów wyszukanych: {count}\n");
            }
        }
    }
}