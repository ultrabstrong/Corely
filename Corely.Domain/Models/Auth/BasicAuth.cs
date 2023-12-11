using AutoMapper;
using Corely.Domain.Entities.Auth;
using Corely.Shared.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth : Profile
    {
        public string Username { get; set; }
        public IEncryptedValue Password { get; set; }
        public DateTime ModifiedUtc { get; set; }

        public BasicAuth()
        {
            CreateMap<BasicAuth, BasicAuthEntity>();
        }
    }
}
