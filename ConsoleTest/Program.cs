using Serilog;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static readonly ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
#pragma warning restore IDE0052 // Remove unread private members


        static void Main()
        {
            try
            {
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}