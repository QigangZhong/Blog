using System.Collections.Generic;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class AddOAuthUserInfoCommand
    {
        public OAuthUserInfo Entity { get; set; }
    }
}
