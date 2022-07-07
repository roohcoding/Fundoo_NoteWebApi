using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Services.Entities
{
    [Keyless]
    public class Label
    {
        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Notes")]
        public int NoteId { get; set; }

        [Required]

        public string LabelName { get; set; }
    }
}
