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

    public static Dictionary<int, Dictionary<string, object>> FromJsonElts(Dictionary<int, JsonElement> input)
    {
        var result = new Dictionary<int, Dictionary<string, object>>();

        foreach (var elt in input)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(elt.Value.GetRawText());
            result[elt.Key] = dict ?? new Dictionary<string, object>();
        }

        return result;
    }


    public static void Print<T>(Dictionary<int, T> data, int howMuch = 5)
    {
        foreach (var elt in data.Take(howMuch))
            Console.WriteLine($"{elt.Key} : {FormatValue(elt.Value)}");
    }

    static string FormatValue(object? value)
    {
        if (value == null)
            return "null";

        if (value is JsonElement json)
            return json.GetRawText();

        if (value is Dictionary<string, object> dict)
            return JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });

        return value.ToString() ?? "<unprintable>";
    }

}