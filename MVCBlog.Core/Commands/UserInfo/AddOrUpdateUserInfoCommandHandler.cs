using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Commands
{
    public class AddOrUpdateUserInfoCommandHandler : 
        ICommandHandler<AddUserInfoCommand>,
        ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IRepository repository;

        public AddOrUpdateUserInfoCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(AddUserInfoCommand command)
        {
            this.repository.UserInfos.Add(command.Entity);
            await this.repository.SaveChangesAsync();
        }

        public async Task HandleAsync(UpdateUserInfoCommand command)
        {
            var entry = this.repository.Entry(command.Entity);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.repository.UserInfos.Attach(command.Entity);
            }

            entry.State = System.Data.Entity.EntityState.Modified;

            await this.repository.SaveChangesAsync();
        }
    }
}
