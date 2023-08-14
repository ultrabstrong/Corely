namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
#pragma warning restore IDE0052 // Remove unread private members

#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            try
            {

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