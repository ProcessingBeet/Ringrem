// using System.Diagnostics;

bool debug = args.Contains("--debug");
bool now = args.Contains("--now");

DateTime time = DateTime.Now;

string dataPath = Path.Combine(AppContext.BaseDirectory, "people.json");


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
    foreach (var elt in dataDict.Take(5))
        Console.WriteLine($"{elt.Key} : {elt.Value}");
}