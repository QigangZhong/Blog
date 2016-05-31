using System.Collections.Generic;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class UpdateUserInfoCommand
    {
        public UserInfo Entity { get; set; }
    }
}
