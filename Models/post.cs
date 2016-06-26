using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class post
    {
        public int id { get; set; }
        public String PosterName { get; set; }

        public String PosterMail { get; set; }

        public String PosterImagePath { get; set; }
       
        public bool IsPublic { get; set; }

        public DateTime PostingDate { get; set; }
        
        public String Caption { get; set; }

        public String ImagePath { get; set; }

        public int PostId { get; set; }


    }
}