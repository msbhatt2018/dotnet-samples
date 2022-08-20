﻿namespace Dapper.CleanArchitecture.Domain.Common.Interfaces;

public interface IDbContext
{
    public IDbTransaction Transaction { get; }

    /// <summary>
    /// Add events to a temporary collection to be fired once all changes have been successfully saved
    /// </summary>
    /// <param name="domainEvent"></param>
    void AddEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Commit in progress Database transaction and fire any pending events
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken token = default);
}