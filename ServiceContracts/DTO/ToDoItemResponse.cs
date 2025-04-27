using Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ToDoItemResponse : ToDoDtoBase
    {

       
        public Guid Id { get; set; }


        public bool IsCompleted { get; set; }

        public string UserName { get; set; }




    }



    
    }

