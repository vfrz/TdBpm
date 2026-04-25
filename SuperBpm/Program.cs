namespace SuperBpm;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using var application = new SuperBpmApp();
        application.Run();
    }
}