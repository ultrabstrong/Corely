namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var s = GetBaseInterleavedTests(",");
            ;
        }

        static private List<string> GetBaseInterleavedTests(string interleaveWith)
        {

            string[] baseInterleaves = new[] {
                interleaveWith,
                "a",
                $"{interleaveWith}a",
                $"a{interleaveWith}",
                $"{interleaveWith}a{interleaveWith}"
            };

            HashSet<string> results = new(baseInterleaves);

            for (int i = 0; i < 3; i++)
            {
                string[] currentInterleaves = results.ToArray();
                foreach (var result in currentInterleaves)
                {
                    foreach (var baseInterleave in baseInterleaves)
                    {
                        results.Add(result + baseInterleave);
                    }
                }
            }

            return results.ToList();
        }

        static private List<string> Interleave(List<string> toInterleave, string interleaveWith)
        {
            HashSet<string> results = new(toInterleave);

            string[] currentInterleaves = results.ToArray();
            foreach (var result in currentInterleaves)
            {
                foreach (var baseInterleave in toInterleave)
                {
                    results.Add(result + baseInterleave);
                }
            }

            return results.ToList();
        }
    }
}