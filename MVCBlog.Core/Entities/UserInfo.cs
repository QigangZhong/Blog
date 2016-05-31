using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Core.Entities
{
    public class UserInfo:EntityBase
    {
        [StringLength(100)]
        [Required]
        public string UserName { get; set; }

        [StringLength(100)]
        public string NickName { get; set; }

        public int Gender { get; set; }

        public DateTime Birthday { get; set; }

        [StringLength(100)]
        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        public string CellPhone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int Status { get; set; }
    }
}
