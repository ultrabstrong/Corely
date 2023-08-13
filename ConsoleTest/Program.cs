using Corely.Shared.Extensions;
using Corely.Shared.Providers.Data;
using Corely.Shared.Providers.Data.Models;
using System.Text;

namespace ConsoleTest
{
    internal class Program
    {
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            try
            {
                DelimitedTextProvider provider = new DelimitedTextProvider();

                string input = "dGVzdDEsdGVzdDIsdGVzdDMNCiIiInRlc3QxIiwiIiJ0ZXN0MiIsIiIidGVzdDMiDQoidGVzdDEiIiIsInRlc3QyIiIiLCJ0ZXN0MyIiIg0KInRlIiJzdDEiLCJ0ZSIic3QyIiwidGUiInN0MyINCiIsdGVzdDEiLCIsdGVzdDIiLCIsdGVzdDMiDQoidGVzdDEsIiwidGVzdDIsIiwidGVzdDMsIg0KInRlLHN0MSIsInRlLHN0MiIsInRlLHN0MyINCiIKdGVzdDEiLHRlc3QyLHRlc3QzDQoidGVzdDEKIix0ZXN0Mix0ZXN0Mw0KdGVzdDEsIgp0ZXN0MiIsdGVzdDMNCnRlc3QxLCJ0ZXN0MgoiLHRlc3QzDQp0ZXN0MSx0ZXN0MiwiCnRlc3QzIg0KdGVzdDEsdGVzdDIsInRlc3QzCiINCiIiIix0ZXN0MSwKdGVzdDEuMSwiIiIsdGVzdDIsdGVzdDMNCnRlc3QxLCIiIix0ZXN0MiwKdGVzdDIuMiwiIiIsdGVzdDMNCnRlc3QxLHRlc3QyLCIiIix0ZXN0MywKdGVzdDMuMywiIiI=".Base64Decode();

                List<ReadRecordResult> results = new List<ReadRecordResult>();
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
                {
                    results = provider.ReadAllRecords(stream);
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