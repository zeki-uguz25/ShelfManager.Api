using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetAllByUserIdAsync(Guid userId);
    Task SoftDeleteAsync(Notification notification);
}
