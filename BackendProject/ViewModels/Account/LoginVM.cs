﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
