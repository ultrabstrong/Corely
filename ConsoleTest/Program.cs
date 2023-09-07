#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0090 // Use 'new(...)'

using Corely.Shared.Providers.Data;
using Corely.Shared.Providers.Data.Models;
using Serilog;
using System.Text;

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
                string s = File.ReadAllText(desktop + "\\csvData.csv");
                var provider = new DelimitedTextProvider(logger);
                List<ReadRecordResult> results;
                using (MemoryStream stream = new(Encoding.UTF8.GetBytes(s)))
                {
                    results = provider.ReadAllRecords(stream);
                }

                string d;
                using (MemoryStream ms = new MemoryStream())
                {
                    provider.WriteAllRecords(results.Select(m => m.Tokens), ms);
                    d = Encoding.UTF8.GetString(ms.ToArray());
                }
                ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}


#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0090 // Use 'new(...)'