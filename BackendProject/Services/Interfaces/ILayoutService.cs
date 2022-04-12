using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProject.Services.Interfaces
{
    public interface ILayoutService
    {
        Dictionary<string, string> GetSettings();
    }
}
