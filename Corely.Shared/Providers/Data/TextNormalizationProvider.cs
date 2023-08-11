using System.Text.RegularExpressions;

namespace Corely.Shared.Providers.Data
{
    public class TextNormalizationProvider : ITextNormalizationProvider
    {
        public string BasicNormalize(string s)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));
            return Regex.Replace(s.ToUpper(), "[^\\w]", string.Empty);
        }

        public string NormalizeAddress(string street, params string[] additional)
        {
            ArgumentNullException.ThrowIfNull(street, nameof(street));
            return NormalizeAddress(false, street, additional);
        }

        public string NormalizeAddressAndState(string street, params string[] additional)
        {
            ArgumentNullException.ThrowIfNull(street, nameof(street));
            return NormalizeAddress(true, street, additional);
        }

        private string NormalizeAddress(bool includestate, string street, params string[] additional)
        {
            string normalizedaddress = street.ToUpper() + " ";
            normalizedaddress = Regex.Replace(normalizedaddress, "[^\\w\\s]", string.Empty);

            foreach (Tuple<string, string, bool> abbreviation in _commonStreetAbbreviations)
            {
                normalizedaddress = normalizedaddress.Replace(abbreviation.Item1.ToUpper(), $" {abbreviation.Item2.ToUpper()} ");

                if (abbreviation.Item3)
                {
                    normalizedaddress = normalizedaddress.Replace($" {abbreviation.Item2.ToUpper()} ", " ");
                }
            }

            normalizedaddress = $"{normalizedaddress}{string.Join(" ", additional)}".ToUpper();

            if (includestate)
            {
                foreach (Tuple<string, string> abbreviation in _commonStateAbbreviations)
                {
                    normalizedaddress = normalizedaddress.Replace(abbreviation.Item1.ToUpper(), abbreviation.Item2.ToUpper());
                }
            }
            normalizedaddress = Regex.Replace(normalizedaddress, "[^\\w]", string.Empty);
            return normalizedaddress;
        }

        private readonly List<Tuple<string, string, bool>> _commonStreetAbbreviations = new()
        {
            // Ordinal 
            new Tuple<string, string, bool>("Northeast","NE", false),
            new Tuple<string, string, bool>("Northwest","NW", false),
            new Tuple<string, string, bool>("Southeast","SE", false),
            new Tuple<string, string, bool>("Southwest","SW", false),
            new Tuple<string, string, bool>("North","N", false),
            new Tuple<string, string, bool>("East","E", false),
            new Tuple<string, string, bool>("South","S", false),
            new Tuple<string, string, bool>("West","W", false),
            // Address
            new Tuple<string, string, bool>("Avenue","AVE", true),
            new Tuple<string, string, bool>("Boulevard","BLVD", true),
            new Tuple<string, string, bool>("Center","CTR", true),
            new Tuple<string, string, bool>("Circle","CIR", true),
            new Tuple<string, string, bool>("Court","CT", true),
            new Tuple<string, string, bool>("Drive","DR", true),
            new Tuple<string, string, bool>("Expressway","EXPY", true),
            new Tuple<string, string, bool>("Heights","HTS", true),
            new Tuple<string, string, bool>("Highway","HWY", true),
            new Tuple<string, string, bool>("Island","IS", true),
            new Tuple<string, string, bool>("Junction","JCT", true),
            new Tuple<string, string, bool>("Lake","LK", true),
            new Tuple<string, string, bool>("Lane","LN", true),
            new Tuple<string, string, bool>("Mountain","MTN", true),
            new Tuple<string, string, bool>("Parkway","PKWY", true),
            new Tuple<string, string, bool>("Place","PL", true),
            new Tuple<string, string, bool>("Plaza","PLZ", true),
            new Tuple<string, string, bool>("Ridge","RDG", true),
            new Tuple<string, string, bool>("Road","RD", true),
            new Tuple<string, string, bool>("Square","SQ", true),
            new Tuple<string, string, bool>("Street","ST", true),
            new Tuple<string, string, bool>("Station","STA", true),
            new Tuple<string, string, bool>("Terrace","TER", true),
            new Tuple<string, string, bool>("Trail","TRL", true),
            new Tuple<string, string, bool>("Turnpike","TPKE", true),
            new Tuple<string, string, bool>("Valley","VLY", true),
            new Tuple<string, string, bool>("Way","WAY", true),
            // Building
            new Tuple<string, string, bool>("Apartment","APT", true),
            new Tuple<string, string, bool>("Room","RM", true),
            new Tuple<string, string, bool>("Suite","STE", true),
            new Tuple<string, string, bool>("Unit","UNIT", true)
        };

        private readonly List<Tuple<string, string>> _commonStateAbbreviations = new()
        { 
            // States
            new Tuple<string, string> ("Alabama","AL"),
            new Tuple<string, string> ("Alaska","AK"),
            new Tuple<string, string> ("Arizona","AZ"),
            new Tuple<string, string> ("Arkansas","AR"),
            new Tuple<string, string> ("California","CA"),
            new Tuple<string, string> ("Colorado","CO"),
            new Tuple<string, string> ("Connecticut","CT"),
            new Tuple<string, string> ("Delaware","DE"),
            new Tuple<string, string> ("Florida","FL"),
            new Tuple<string, string> ("Georgia","GA"),
            new Tuple<string, string> ("Hawaii","HI"),
            new Tuple<string, string> ("Idaho","ID"),
            new Tuple<string, string> ("Illinois","IL"),
            new Tuple<string, string> ("Indiana","IN"),
            new Tuple<string, string> ("Iowa","IA"),
            new Tuple<string, string> ("Kansas","KS"),
            new Tuple<string, string> ("Kentucky","KY"),
            new Tuple<string, string> ("Louisiana","LA"),
            new Tuple<string, string> ("Maine","ME"),
            new Tuple<string, string> ("Maryland","MD"),
            new Tuple<string, string> ("Massachusetts","MA"),
            new Tuple<string, string> ("Michigan","MI"),
            new Tuple<string, string> ("Minnesota","MN"),
            new Tuple<string, string> ("Mississippi","MS"),
            new Tuple<string, string> ("Missouri","MO"),
            new Tuple<string, string> ("Montana","MT"),
            new Tuple<string, string> ("Nebraska","NE"),
            new Tuple<string, string> ("Nevada","NV"),
            new Tuple<string, string> ("New Hampshire","NH"),
            new Tuple<string, string> ("New Jersey","NJ"),
            new Tuple<string, string> ("New Mexico","NM"),
            new Tuple<string, string> ("New York","NY"),
            new Tuple<string, string> ("North Carolina","NC"),
            new Tuple<string, string> ("North Dakota","ND"),
            new Tuple<string, string> ("Ohio","OH"),
            new Tuple<string, string> ("Oklahoma","OK"),
            new Tuple<string, string> ("Oregon","OR"),
            new Tuple<string, string> ("Pennsylvania","PA"),
            new Tuple<string, string> ("Rhode Island","RI"),
            new Tuple<string, string> ("South Carolina","SC"),
            new Tuple<string, string> ("South Dakota","SD"),
            new Tuple<string, string> ("Tennessee","TN"),
            new Tuple<string, string> ("Texas","TX"),
            new Tuple<string, string> ("Utah","UT"),
            new Tuple<string, string> ("Vermont","VT"),
            new Tuple<string, string> ("Virginia","VA"),
            new Tuple<string, string> ("Washington","WA"),
            new Tuple<string, string> ("West Virginia","WV"),
            new Tuple<string, string> ("Wisconsin","WI"),
            new Tuple<string, string> ("Wyoming","WY"),
            // Extras
            new Tuple<string, string> ("American Samoa","AS"),
            new Tuple<string, string> ("District of Columbia","DC"),
            new Tuple<string, string> ("Federated States of Micronesia","FM"),
            new Tuple<string, string> ("Guam","GU"),
            new Tuple<string, string> ("Marshall Islands","MH"),
            new Tuple<string, string> ("Northern Mariana Islands","MP"),
            new Tuple<string, string> ("Puerto Rico","PR"),
            new Tuple<string, string> ("Virgin Islands, U.S.","VI"),
            new Tuple<string, string> ("Armed Forces the Americas","AA"),
            new Tuple<string, string> ("Armed Forces Europe","AE"),
            new Tuple<string, string> ("Armed Forces Pacific","AP")
        };
    }
}
