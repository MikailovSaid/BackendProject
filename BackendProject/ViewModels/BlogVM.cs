﻿using BackendProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        public List<Category> Categories { get; set; }
    }
}
