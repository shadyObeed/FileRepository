using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Repositories;

public class CosmosDbSettings
{
    public string Account { get; set; }

    public string Key { get; set; }

    public string DatabaseName { get; set; }
}

