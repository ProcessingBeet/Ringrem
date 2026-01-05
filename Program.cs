using System.Data.Common;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.VisualBasic;

bool debug = args.Contains("--debug");
bool now = args.Contains("--now");

DateTime time = DateTime.Now;

string dataPath = Path.Combine(AppContext.BaseDirectory, "people.json");


static void SendNotification(string title, string message)
{
    Process.Start("notify-send", $"\"{title}\"  \"{message}\"");
}

Dictionary<int, JsonElement> LoadData(string path)
{
    if (!File.Exists(path))
        return new Dictionary<int, JsonElement>();

    string json = File.ReadAllText(path);

    return JsonSerializer.Deserialize<Dictionary<int, JsonElement>>(json)
           ?? new Dictionary<int, JsonElement>();
}


if (debug)
{
    SendNotification("Debug", "Debugging message");
    Console.WriteLine($"Debug log:\nLocal time is {time.ToString()}");
    Console.WriteLine(dataPath);
    string data = File.ReadAllText(dataPath);
    Console.WriteLine($"\nData stored:\n{data}\n\nData read (first 5):");
    var dataDict = LoadData(dataPath);
    foreach (var elt in dataDict.Take(5))
        Console.WriteLine($"{elt.Key} : {elt.Value}");
}