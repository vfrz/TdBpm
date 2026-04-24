namespace TdBpm;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var application = new BpmApplication();
        application.Run();
    }
}