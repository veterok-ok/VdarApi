using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Contracts;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private VdarDbContext _repoContext;
        private IUserRepository _user;
        private ITokenRepository _token;
        private IConfirmationRepository _confirmationKey;

        public IUserRepository User
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public ITokenRepository Token
        {
            get
            {
                if (_token == null)
                {
                    _token = new TokenRepository(_repoContext);
                }
                return _token;
            }
        }

        public IConfirmationRepository ConfirmationKey
        {
            get
            {
                if (_confirmationKey == null)
                {
                    _confirmationKey = new ConfirmationRepository(_repoContext);
                }
                return _confirmationKey;
            }
        }

        public RepositoryWrapper(VdarDbContext repositoryContext)
        {
            this._repoContext = repositoryContext;
        }

    }
}
