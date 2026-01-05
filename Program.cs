using System.Diagnostics;

bool debug = args.Contains("--debug");
bool now = args.Contains("--now");


static void SendNotification(string title, string message)
{
    Process.Start("notify-send", $"\"{title}\"  \"{message}\"");
}

if (debug)
{
    SendNotification("Debug", "Debugging message");
}