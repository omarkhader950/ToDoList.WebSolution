﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Enums;

namespace ToDoList.Core.DTO
{
    public class PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;


        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        //create range class date range "DateTime" 

        public DateRange? CreatedDateRange { get; set; }

        public DateRange? DueDateRange { get; set; }

        public List<TodoStatus>? Statuses { get; set; }

        public string? Title { get; set; } 

        public Guid? UserId { get; set; }

        public string? SortBy { get; set; } = "Id"; 

        
        public bool SortAscending { get; set; } = true;



    }
}
