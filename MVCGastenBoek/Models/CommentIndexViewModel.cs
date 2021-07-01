using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCGastenBoek.Models
{
    public class CommentIndexViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
        public CommentViewModel Comment { get; set; } = new CommentViewModel();
    }
}
