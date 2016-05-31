using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Core.Entities
{
    public class OAuthUserInfo : EntityBase
    {
        [StringLength(100)]
        public string NickName { get; set; }

        [StringLength(50)]
        [Required]
        public string LoginType { get; set; }

        [StringLength(200)]
        [Required]
        public string OpenId { get; set; }

        [StringLength(200)]
        public string Avatar { get; set; }

        public Guid UserInfoId { get; set; }

        public string UserName { get; set; }

        public int Status { get; set; }

    }
}
