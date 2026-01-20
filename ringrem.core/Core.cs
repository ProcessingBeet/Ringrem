using System.Data;

namespace ringrem.core;

public class Core
{
    public static void Run(Models.ILog? log = null)
    {
        
    }

    public static void RunCheck(Models.ILog? log)
    {
        var now = DateTime.Now;
        log?.Log("RunCheck start.\nSearching data under paths:");
        string peoplePath = Path.Combine(AppContext.BaseDirectory, "people.json");
        string groupsPath = Path.Combine(AppContext.BaseDirectory, "groups.json");

        var people = DataIO.LoadData<Models.Person>(peoplePath, log);
        var groups = DataIO.LoadData<Models.Group>(groupsPath, log);

        var toNotify = WhoNotify(now, people, groups, log);
        Notify(toNotify, log);

        if(!dryRun)
            DataIO.SaveData(peoplePath, people, log);
    }
    public static List<Models.Person> WhoNotify(DateTime currentTime, 
                                               List<Models.Person> people,
                                               List<Models.Group> groups,
                                               Models.ILog? log)
    {
        log?.Log("Merging data...");
        var mergedData = from p in people
                         join g in groups on p.GroupId equals g.Id
                         select new { Person = p, Group = g };
        log?.Log("...done.");
        var toNotify = new List<Models.Person>();
        foreach (var elt in mergedData)
        {
            DateTime lastSpoke = elt.Person.LastSpoke;
            DateTime notifyAt = lastSpoke.Date.AddDays(elt.Group.IntervalDays).AddHours(elt.Group.NotifyHour);
            if (currentTime >= notifyAt)
                toNotify.Add(elt.Person);
        }
        return toNotify;
    }

    public static bool Notify(List<Models.Person> toNotify, Models.ILog? log)
    {
        try
        {
            foreach(var elt in toNotify)
            {
                System.Diagnostics.Process.Start("notify-send", $"\"{elt.Name}\" \"{elt.Description}\"");
                elt.LastSpoke = DateTime.Now;
            }
            return true;
        }
        catch (Exception ex)
        {
            log?.Log(ex.Message);
            return false;
        }
        
    }
}
