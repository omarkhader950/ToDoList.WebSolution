using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class TodoItemAddRequest : ToDoDtoBase
    {

        // Optional — only used by Admins
        public Guid? UserId { get; set; }

      
    }
    }
