class Program
{
    static bool debug = false;

    static void Main(string[] args)
    {
        debug = args.Contains("--debug");
        DateTime currentTime = DateTime.Now;

        if (debug)
        {
            RunDebug();
            return;
        }

        RunCheck(currentTime);

    }

    private static void RunCheck(DateTime currentTime)
    {
        string peoplePath = Path.Combine(AppContext.BaseDirectory, "people.json");
        string groupsPath = Path.Combine(AppContext.BaseDirectory, "groups.json");

        Console.WriteLine("RunCheck!");
        var people = DataIO.LoadData(peoplePath); var groups = DataIO.LoadData(groupsPath);
        
    }

    private static void RunDebug()
    {
        DateTime time = DateTime.Now;
        string peoplePath = Path.Combine(AppContext.BaseDirectory, "people.json");
        string groupsPath = Path.Combine(AppContext.BaseDirectory, "groups.json");

        Notifier.SendNotification("Debug", "Debugging message");
        Console.WriteLine($"Debug log:\nLocal time is {time}");
        Console.WriteLine(peoplePath);

        string people = File.ReadAllText(peoplePath);
        Console.WriteLine($"\nData stored:\n{people}\n\nData read (first 5):");

        var peopleDict = DataIO.LoadData(peoplePath);
        Parser.Print(peopleDict);

        Console.WriteLine("Checking groups:");
        var groups = DataIO.LoadData(groupsPath);
        Parser.Print(groups);

        var newGroupsData = new Dictionary<int, Dictionary<string, object>>
        {
            { 2, new Dictionary<string, object>
                {
                    { "grp_description", "Projekt X" },
                    { "breakLenght", 23 }
                }
            }
        };

        if (!DataIO.SaveData(groupsPath, newGroupsData))
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
        Console.WriteLine("Restored old groups successfully.\nChecking merging function. \nBelow result of people and groups by group id:");

        var mergedData = Queries.JoinAtGroupID(peopleDict, groups);
        Parser.Print(mergedData);

    }
}
