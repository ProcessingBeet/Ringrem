class Program
{
    static bool debug = false;
    static bool now = false;

    static void Main(string[] args)
    {
        debug = args.Contains("--debug");
        now = args.Contains("--now");

        if (debug)
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "people.json");
            string groupsPath = Path.Combine(AppContext.BaseDirectory, "groups.json");
            RunDebug(dataPath, groupsPath);
        }

        if (now)
        {
            SendNotification("Reminder", "It's time to do something!");
        }

    }

    private static void RunDebug(string dataPath, string groupsPath)
    {
        DateTime time = DateTime.Now;

        SendNotification("Debug", "Debugging message");
        Console.WriteLine($"Debug log:\nLocal time is {time}");
        Console.WriteLine(dataPath);

        string data = File.ReadAllText(dataPath);
        Console.WriteLine($"\nData stored:\n{data}\n\nData read (first 5):");

        var dataDict = DataIO.LoadData(dataPath);
        Parser.Print(dataDict);

        Console.WriteLine("Checking groups:");
        var groups = DataIO.LoadData(groupsPath);
        Parser.Print(groups);

        var newGroupsData = new Dictionary<int, Dictionary<string, object>>
        {
            { 2, new Dictionary<string, object>
                {
                    { "description", "Projekt X" },
                    { "breakLenght", 23 }
                }
            }
        };

        if (!DataIO.SaveData(groupsPath, Parser.ToJsonElts(newGroupsData)))
        {
            Console.WriteLine("Saving failed.");
            Environment.Exit(1);
        }

        Console.WriteLine("Saving succeeded, new groups:");
        var newGroups = DataIO.LoadData(groupsPath);
        Parser.Print(newGroups);

        if (!DataIO.SaveData(groupsPath, groups))
        {
            Console.WriteLine("Restoring old groups failed.");
            Environment.Exit(1);
        }
        Console.WriteLine("Restored old groups successfully.");
    }

    private static void SendNotification(string title, string message)
    {
        System.Diagnostics.Process.Start("notify-send", $"\"{title}\" \"{message}\"");
    }
}
