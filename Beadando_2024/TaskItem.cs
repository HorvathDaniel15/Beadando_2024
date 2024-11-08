﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando_2024
{
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CaterogryId { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public Category Category { get; set; }
    }
}
