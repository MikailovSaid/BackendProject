using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Models
{
    public class SubscribeArea
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Desc { get; set; }
    }
}
