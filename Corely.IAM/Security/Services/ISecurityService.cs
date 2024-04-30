using Corely.IAM.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corely.IAM.Security.Services
{
    public interface ISecurityService
    {
        SymmetricKey GetSymmetricKeyEncryptedWithSystemKey();
    }
}
