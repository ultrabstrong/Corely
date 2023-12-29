using Corely.Domain.Services.Users;
using Corely.Domain.Validators.FluentValidators.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.Domain
{
    public static class DomainServiceRegistration
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainServiceRegistration).Assembly);
            services.AddValidatorsFromAssemblyContaining<UserValidator>(includeInternalTypes: true);

            services.AddTransient<IUserService, UserService>();
        }
    }
}
