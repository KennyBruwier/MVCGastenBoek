using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCGastenBoek.Models
{
    public class CommentViewModel : Comment
    {
        [DisplayName("Foto")]
        public IFormFile NewImage { get; set; }
        public IEnumerable<SelectListItem> MoodStatus { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("none","none"),
            new SelectListItem("blij","blij"),
            new SelectListItem("matig","matig"),
            new SelectListItem("droevig","droevig"),
            new SelectListItem("boos","boos")
        };
    }
}
