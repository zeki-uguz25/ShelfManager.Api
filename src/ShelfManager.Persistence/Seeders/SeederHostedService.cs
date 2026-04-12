using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Seeders
{
    public class SeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeederHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShelfManagerDbContext>();
            var hashingService = scope.ServiceProvider.GetRequiredService<IHashingService>();
            await RoleSeeder.SeedAsync(context, hashingService);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
