﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public interface IRUserRepository
    {
        User GetUser (string id);
        User LoginUser (string login, string password);
        Task<bool> UserExistAsync (string phone);
    }
}