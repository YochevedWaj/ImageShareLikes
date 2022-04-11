using ImageShareLikes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageShareLikes.Web.Models
{
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
        public bool DisableLike { get; set; }
    }
}
