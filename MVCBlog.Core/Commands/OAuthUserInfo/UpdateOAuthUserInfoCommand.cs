using System.Collections.Generic;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class UpdateOAuthUserInfoCommand
    {
        public OAuthUserInfo Entity { get; set; }
    }
}
