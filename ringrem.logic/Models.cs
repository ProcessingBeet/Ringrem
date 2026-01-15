public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LastSpoke { get; set; } = string.Empty;
    public int GroupId { get; set; }
}

public class Group
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public double IntervalDays { get; set; }
    public double NotifyHour { get; set; }

}