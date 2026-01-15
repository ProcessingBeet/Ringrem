using System.IO.Pipelines;
using System.Reflection.Emit;
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
            var dict = new Dictionary<string, object>();

            foreach (var prop in elt.Value.EnumerateObject())
            {
                object value = prop.Value.ValueKind switch
                {
                    JsonValueKind.String => prop.Value.GetString()!,
                    JsonValueKind.Number =>  prop.Value.TryGetInt32(out int i) ? (object)i : prop.Value.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => prop.Value
                };

                dict[prop.Name] = value;
            }

            result[elt.Key] = dict;
        }

        return result;
    }

    public static Dictionary<int, Person> MapToPerson(Dictionary<int, Dictionary<string, object>> input)
    {
        var result = new Dictionary<int, Person>();
        foreach(var elt in input)
        {
          var data = elt.Value;
          var person = new Person
          {
              Name = data.TryGetValue("name", out var name) ? name as string : "",
              Description = data.TryGetValue("ppl_description", out var description) ? description as string : "",
              LastSpoke = data.TryGetValue("last_spoke", out var lastSpoke) ? lastSpoke.ToString() : DateTime.Now.ToString(),
              GroupId = data.TryGetValue("group_id", out var group_id) ? Convert.ToInt32(group_id) : 0,
              Id = elt.Key
          };
          result[elt.Key] = person;
        }
        return result;
    }

    public static Dictionary<int, Group> MapToGroup(Dictionary<int, Dictionary<string, object>> input)
    {
        var result = new Dictionary<int, Group>();
        foreach(var elt in input)
        {
          var data = elt.Value;
          var person = new Group
          {
              Description = data.TryGetValue("grp_description", out var description) ? description as string : "",
              IntervalDays = data.TryGetValue("interval_days", out var intervalDays) ? (double)intervalDays : 0.0,
              NotifyHour = data.TryGetValue("notify_hour", out var notifyHour) ? (double)notifyHour : 0.0,
              Id = elt.Key
          };
          result[elt.Key] = person;
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
    public static string DateToStr(DateTime date)
    {
        return date.ToString("yyyy-MM-ddTHH:mm:ss");
    }

}