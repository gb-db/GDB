using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Users {

    public enum Cities {
        None, London, Paris, Chicago
    }

    public enum QualificationLevels {
        None, Basic, Advanced
    }

    public class AppUser : IdentityUser {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        public Cities City { get; set; }
        public QualificationLevels Qualifications { get; set; }
        [StringLength(8)]
        public string UserCode { get; set; }
    }

    public class Code
    {
        public int Id { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8)]
        public string UserCode { get; set; }

        public bool status { get; set; }

        public int country { get; set; }

        public int province { get; set; }

        public int church { get; set; }

        [StringLength(50)]
        public string person { get; set; }
        [StringLength(50)]
        public string fName { get; set; }
        [StringLength(50)]
        public string lName { get; set; }
        [StringLength(500)]
        public string description { get; set; }
    }
}
