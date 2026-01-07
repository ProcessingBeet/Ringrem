// using System.Diagnostics;

bool debug = args.Contains("--debug");
bool now = args.Contains("--now");

DateTime time = DateTime.Now;

string dataPath = Path.Combine(AppContext.BaseDirectory, "people.json");
string groupsPath = Path.Combine(AppContext.BaseDirectory, "groups.json");


static void SendNotification(string title, string message)
{
    System.Diagnostics.Process.Start("notify-send", $"\"{title}\"  \"{message}\"");
}

if (debug)
{
    SendNotification("Debug", "Debugging message");
    Console.WriteLine($"Debug log:\nLocal time is {time.ToString()}");
    Console.WriteLine(dataPath);
    string data = File.ReadAllText(dataPath);
    Console.WriteLine($"\nData stored:\n{data}\n\nData read (first 5):");
    var dataDict = DataIO.LoadData(dataPath);
    Parser.Print(dataDict);
    Console.WriteLine("Checking data menagment:\nThere are such groups in groups:");
    var groups = DataIO.LoadData(groupsPath);
    Parser.Print(groups);
    
    var newGroupsdata = new Dictionary<int, Dictionary<string, object>>
    {
        { 2, new Dictionary<string, object>
            {
                { "description", "Projekt Y" },
                { "breakLenght", 14 }
            }
        }
    };
    if(DataIO.SaveData(groupsPath, Parser.ToJsonElts(newGroupsdata)))
    {
        Console.WriteLine("Saving failed.");
        Environment.Exit(1);
    }
    Console.WriteLine("Saving succeded, new groups are:");
    var newGroups = DataIO.LoadData(groupsPath);
    Parser.Print(newGroups);
    if(DataIO.SaveData(groupsPath, groups))
    {
        Console.WriteLine("but saving old groups failed.");
        Environment.Exit(1);
    }
    Console.WriteLine("and saving old groups succeded.");
    
}