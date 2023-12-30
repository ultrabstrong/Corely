using Serilog;
using Serilog.Core;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
#pragma warning restore IDE0052 // Remove unread private members


        static void Main()
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred");
            }
            logger.Information("Program finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}