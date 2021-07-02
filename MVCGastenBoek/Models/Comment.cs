using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCGastenBoek.Models
{
    public enum Mood
    {
        None,
        Blij,
        Matig,
        Droevig,
        Boos
    }
    public class Comment
    {
        public int CommentId { get; set; }
        [DisplayName("Titel")]
        [MaxLength(40)]
        public string Title { get; set; }
        [DisplayName("Your comment")]
        public string MyComment { get; set; }
        [MaxLength(20)]
        public string Naam { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = @"{0:ddd dd, MMM yyyy HH:MM}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [DisplayName("Foto")]
        public string ImgPath { get; set; }
        [DisplayName("Huidig gevoel")]
        [MaxLength(20)]
        public string Mood { get; set; }
        public string Author { get; set; }
    }
}
