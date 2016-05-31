using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteUserInfoCommandHandler : ICommandHandler<DeleteUserInfoCommand>
    {
        private readonly IRepository repository;

        public DeleteUserInfoCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(DeleteUserInfoCommand command)
        {
            var entity = await this.repository.UserInfos
                .SingleOrDefaultAsync(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.UserInfos.Remove(entity);

                await this.repository.SaveChangesAsync();
            }
        }
    }
}
