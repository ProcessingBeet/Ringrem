using System.Text.Json;

static class DataIO
{
    public static Dictionary<int, JsonElement> LoadData(string path)
    {
        if (!File.Exists(path))
            return new Dictionary<int, JsonElement>();

        string json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<Dictionary<int, JsonElement>>(json)
                ?? new Dictionary<int, JsonElement>();
    }

    static bool CheckData(string path, Dictionary<int, JsonElement> newData)
    {
        Dictionary<int, JsonElement> oldData = LoadData(path);
        JsonElement firstElement = oldData.First().Value;
        HashSet<string> oldFieldNames = new HashSet<string>();
        if (firstElement.ValueKind == JsonValueKind.Object)
        {
            oldFieldNames = new HashSet<string>(
            firstElement.EnumerateObject().Select(p => p.Name)
            );
        }
        foreach (JsonElement elt in newData.Values)
        {
            if (elt.ValueKind != JsonValueKind.Object)
                return true;
            HashSet<string> newFieldNames = new HashSet<string>(
            firstElement.EnumerateObject().Select(p => p.Name)
            );
            if (!newFieldNames.SetEquals(oldFieldNames))
                return true;
        }
        return false;
    }

    public static bool SaveData(string path, Dictionary<int, JsonElement> newData)
    {
        if (CheckData(path, newData))
        {
            Console.WriteLine("Error: Saved data is incompatibile with data stored.");
            return true;
        }
        try
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            writer.WriteStartObject();
            foreach (var elt in newData)
            {
                writer.WritePropertyName(elt.Key.ToString()); // JSON wymaga string jako klucz
                elt.Value.WriteTo(writer);
            }
            writer.WriteEndObject();

            return false; // zapis zakończony sukcesem
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd przy zapisie JSON: {ex.Message}");
            return true;
        }
    }

}