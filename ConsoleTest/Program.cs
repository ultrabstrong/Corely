#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0090 // Use 'new(...)'

using Corely.Domain.Models.Users;
using Serilog;

namespace ConsoleTest
{
    internal class Program
    {
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static readonly ILogger logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();


        static void Main(string[] args)
        {
            try
            {
                User user = new User
                {
                    Username = "test",
                    Email = ""
                };
                ;
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


#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0090 // Use 'new(...)'