﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villa_project.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }

        public string Description { get; set; }

        [Range(10,1000)]
        public double Price { get; set; }
        public int Sqft {  get; set; }
        [Range (1,10)]
        public int Occupancy { get; set; }
        [Display(Name="Image")]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get;  set; }

    }
}
