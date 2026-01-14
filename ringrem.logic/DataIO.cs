using System.Text.Json;

static class DataIO
{
    public static Dictionary<int, Dictionary<string, object>> LoadData(string path){
        if (!File.Exists(path))
            return new Dictionary<int, Dictionary<string, object>>();

        string json = File.ReadAllText(path);

        var result = JsonSerializer.Deserialize<Dictionary<int, JsonElement>>(json)
                ?? new Dictionary<int, JsonElement>();

        return Parser.FromJsonElts(result);
    }

    static bool CheckData(string path, Dictionary<int, Dictionary<string, object>> newData)
    {
        Dictionary<int, Dictionary<string, object>> oldData = LoadData(path);
        Dictionary<string, object> firstElement = oldData.First().Value;
        HashSet<string> oldFieldNames = new HashSet<string>();
        oldFieldNames = new HashSet<string>(firstElement.Keys);
        foreach(Dictionary<string, object> elt in newData.Values)
        {
            HashSet<string> newFieldNames = new HashSet<string>(elt.Keys);
            if (!newFieldNames.SetEquals(oldFieldNames))
                return false;
        }
        return true;
    }

    public static bool SaveData(string path, Dictionary<int, Dictionary<string, object>> newData)
    {
        if (!CheckData(path, newData))
        {
            Console.WriteLine("Error: Saved data is incompatibile with data stored.");
            return false;
        }
        try
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            writer.WriteStartObject();
            foreach (var elt in Parser.ToJsonElts(newData))
            {
                writer.WritePropertyName(elt.Key.ToString()); // JSON wymaga string jako klucz
                elt.Value.WriteTo(writer);
            }
            writer.WriteEndObject();

            return true; // zapis zakończony sukcesem
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy zapisie JSON: {ex.Message}");
            return false;
        }
    }

}