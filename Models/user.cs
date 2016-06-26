using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Model
{
    public class user
    {

        public int ProfileOwnerId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public String FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public String LastName { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public String UserPassWord { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public String PhoneNumber { get; set; }
        [Required]
        public String Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public String PicturePath { get; set; }
        public bool LoggedIn { get; set; }
        [DisplayName("Home Town")]
        public String HomeTown { get; set; }
        [DisplayName("Marital Status")]
        public String MaritalStatus { get; set; }
        [DisplayName("About Me")]
        public String Aboutme { get; set; }
        public LinkedList<post> posts { get; set; }
        public LinkedList<int> likes { get; set; }



    }
}