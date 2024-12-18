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
    public enum CategoryEnum
    {
        Job,
        Personal,
        Learning
    }

    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public PriorityLevel PriorityLevel { get; set; }
        public CategoryEnum CategoryEnum { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public bool Deadline { get; set; }
    }
}
