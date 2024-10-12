﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Book.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [DisplayName("Category Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(0, 100, ErrorMessage = "the field must contain order between 1 to 100")]
        public int DisplayOrder { get; set; }
    }
}