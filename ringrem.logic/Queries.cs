using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

static class Queries
{
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>( this Dictionary<TKey, TValue> target, Dictionary<TKey, TValue> source)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(target);

        foreach (var elt in source)
            result[elt.Key] = elt.Value;

        return result;
    }

    public static Dictionary<int, Dictionary<string, object>> JoinAtGroupID(Dictionary<int, Dictionary<string, object>> people, Dictionary<int, Dictionary<string, object>> groups)
    {
        Dictionary<int, Dictionary<string, object>> result = new Dictionary<int, Dictionary<string, object>>();
        foreach(var elt in people)
        {
            var eltGroup = groups.GetValueOrDefault((int)elt.Value["group_id"]) ?? new Dictionary<string, object>();
            result.Add(elt.Key, elt.Value.Merge(eltGroup));
        }
        return result;
    }

    public static void AddPerson(Dictionary<int, Dictionary<string, object>> dict, 
    string name, string description, DateTime lastSpoke, int groupId)
    {   
        int newId = dict.Count > 0 ? dict.Keys.Max() + 1 : 0;
        string lastSpokeStr = Parser.DateToStr(lastSpoke);

        dict[newId] = new Dictionary<string, object>
            {
                ["name"] = name,
                ["ppl_description"] = description,
                ["lastSpoke"] = lastSpokeStr,
                ["group_id"] = groupId
            };
    }

    public static void AddGroup( Dictionary<int, Dictionary<string, object>> dict,
    string description, double intervalDays, double notifyHour)
    {
        int newId = dict.Count > 0 ? dict.Keys.Max() + 1 : 0;

        dict[newId] = new Dictionary<string, object>
        {
            ["grp_description"] = description,
            ["intervalDays"] = intervalDays,
            ["notifyHour"] = notifyHour
        };
    }

    public static bool DeleteElt( Dictionary<int, Dictionary<string, object>> dict, int id)
    {
        return dict.Remove(id);
    }

}