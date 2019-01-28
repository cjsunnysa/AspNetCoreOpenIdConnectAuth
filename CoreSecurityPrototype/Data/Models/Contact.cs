using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreSecurityPrototype.Data.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(2), MaxLength(30)]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(30)]
        public string LastName { get; set; }
    }
}
