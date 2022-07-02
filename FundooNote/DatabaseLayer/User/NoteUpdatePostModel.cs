using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.User
{
   public class NoteUpdatePostModel
    {
        [Required]
        public int noteID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Colour { get; set; }
        [Required]
        public bool IsArchive { get; set; }
        [Required]

        public bool IsPin { get; set; }
        [Required]
        public bool IsReminder { get; set; }
        [Required]
        public bool IsTrash { get; set; }
        [Required]
        public DateTime Reminder { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]

        public DateTime ModifiedDate { get; set; }
    }
}
