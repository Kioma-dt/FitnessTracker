using FitnessTracker.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FitnessTracker.DataAccess.Interceptors;

public class AuditingSaveChangesInterceptor
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        UpdateDocuments(eventData.Context);
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateDocuments(eventData.Context);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateDocuments(DbContext? dbContext)
    {
        if (dbContext is not null
            && dbContext.ChangeTracker.HasChanges())
        {
            foreach (var entity in dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                if (entity.Entity is Document document)
                {
                    document.UpdatedAt =  DateTime.UtcNow;
                }
            }
        }
    }
}