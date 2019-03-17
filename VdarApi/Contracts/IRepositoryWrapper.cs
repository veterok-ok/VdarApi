using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IConfirmationRepository ConfirmationKey { get; }
        ITokenRepository Token { get; }
    }
}
