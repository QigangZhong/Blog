using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class DeleteOAuthUserInfoCommandHandler : ICommandHandler<DeleteOAuthUserInfoCommand>
    {
        private readonly IRepository repository;

        public DeleteOAuthUserInfoCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(DeleteOAuthUserInfoCommand command)
        {
            var entity = await this.repository.OAuthUserInfos
                .SingleOrDefaultAsync(e => e.Id == command.Id);

            if (entity != null)
            {
                this.repository.OAuthUserInfos.Remove(entity);

                await this.repository.SaveChangesAsync();
            }
        }
    }
}
