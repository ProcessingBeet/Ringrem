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

    static bool CheckData(Dictionary<int, JsonElement> data)
    {
        return true;
    }

    public static void SaveData(string path, Dictionary<int, JsonElement> newData)
    {

        if (!File.Exists(path))
            File.Delete(path);

    }

}