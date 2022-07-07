using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.Note
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
        public bool IsPin { get; set; }
        public bool IsReminder { get; set; }
        public bool IsTrash { get; set; }
  
       
    }
}
