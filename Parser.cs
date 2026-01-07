using System.Text.Json;

static class Parser
{
    public static Dictionary<int, JsonElement> ToJsonElts(Dictionary<int, Dictionary<string, object>> input)
    {
        var result = new Dictionary<int, JsonElement>();
        foreach(var elt in input)
        {
            string json = JsonSerializer.Serialize(elt.Value);
            using var doc = JsonDocument.Parse(json);
            result[elt.Key] = doc.RootElement.Clone();
        }
        return result;
    }
    public static void Print(Dictionary<int, JsonElement> data, int howMuch = 5)
    {
        foreach (var elt in data.Take(howMuch))
            Console.WriteLine($"{elt.Key} : {elt.Value}");
    }
}