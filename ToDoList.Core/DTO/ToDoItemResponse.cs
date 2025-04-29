using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DTO
{
    public class ToDoItemResponse : ToDoDtoBase
    {

       
        public Guid Id { get; set; }


        public bool IsCompleted { get; set; }

        public string UserName { get; set; }




    }



    
    }

