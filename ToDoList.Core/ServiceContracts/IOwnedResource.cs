﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.ServiceContracts
{
    public interface IOwnedResource
    {
        Guid OwnerId { get; }

    }
}
