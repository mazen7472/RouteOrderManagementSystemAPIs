﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync (string email , string subject , string body );

    }
}
