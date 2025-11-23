using Ninject.Modules;
using BookManagementSystem.Domain.Repositories;
using BookManagementSystem.DataAccessLayer.Repositories;
using BookManagementSystem.DataAccessLayer.Contexts;
using BookManagementSystem.DataAccessLayer.Configuration;

namespace BookManagementSystem.BusinessLogicLayer
{
    public class SimpleConfigModule : NinjectModule
    {
        private readonly bool _useDapper;

        public SimpleConfigModule(bool useDapper = false)
        {
            _useDapper = useDapper;
        }

        public override void Load()
        {
            var connectionString = SqliteConnectionProvider.GetDefaultConnectionString();

            if (_useDapper)
            {
                Bind<IBookRepository>().To<DapperBookRepository>()
                    .InSingletonScope()
                    .WithConstructorArgument("connectionString", connectionString);
            }
            else
            {
                Bind<IBookRepository>().To<EfBookRepository>().InSingletonScope();
                
                Bind<BookDbContext>().ToMethod(ctx => 
                {
                    return BookDbContextFactory.Create(connectionString);
                }).InSingletonScope();
            }
        }
    }
}
