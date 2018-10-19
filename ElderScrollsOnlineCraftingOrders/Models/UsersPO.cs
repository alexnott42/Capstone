using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElderScrollsOnlineCraftingOrders.Models
{
    public class UsersPO
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string ESOname { get; set; }

        public byte RoleID { get; set; }

        [Required]
        [StringLength(2)]
        public string Server { get; set; }
    }
}