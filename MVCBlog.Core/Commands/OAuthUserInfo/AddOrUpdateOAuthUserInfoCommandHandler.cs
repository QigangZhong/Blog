using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class AddOrUpdateOAuthUserInfoCommandHandler : 
        ICommandHandler<AddOAuthUserInfoCommand>,
        ICommandHandler<UpdateOAuthUserInfoCommand>
    {
        private readonly IRepository repository;

        public AddOrUpdateOAuthUserInfoCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(AddOAuthUserInfoCommand command)
        {
            this.repository.OAuthUserInfos.Add(command.Entity);
            await this.repository.SaveChangesAsync();
        }

        public async Task HandleAsync(UpdateOAuthUserInfoCommand command)
        {
            var entry = this.repository.Entry(command.Entity);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.repository.OAuthUserInfos.Attach(command.Entity);
            }

            entry.State = System.Data.Entity.EntityState.Modified;

            await this.repository.SaveChangesAsync();
        }
    }
}
