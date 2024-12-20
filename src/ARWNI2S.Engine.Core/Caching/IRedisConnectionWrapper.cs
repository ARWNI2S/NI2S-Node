﻿using StackExchange.Redis;
using System.Net;

namespace ARWNI2S.Engine.Caching
{
    /// <summary>
    /// Represents Redis connection wrapper
    /// </summary>
    public interface IRedisConnectionWrapper
    {
        /// <summary>
        /// Obtain an interactive connection to a database inside Redis
        /// </summary>
        /// <returns>Redis cache database</returns>
        Task<IDatabase> GetDatabaseAsync();

        /// <summary>
        /// Obtain an interactive connection to a database inside Redis
        /// </summary>
        /// <returns>Redis cache database</returns>
        IDatabase GetDatabase();

        /// <summary>
        /// Obtain a configuration API for an individual node
        /// </summary>
        /// <param name="endPoint">The network endpoint</param>
        /// <returns>Redis server</returns>
        Task<IServer> GetServerAsync(EndPoint endPoint);

        /// <summary>
        /// Gets all endpoints defined on the node
        /// </summary>
        /// <returns>Array of endpoints</returns>
        Task<EndPoint[]> GetEndPointsAsync();

        /// <summary>
        /// Gets a subscriber for the node
        /// </summary>
        /// <returns>Array of endpoints</returns>
        Task<ISubscriber> GetSubscriberAsync();

        /// <summary>
        /// Gets a subscriber for the node
        /// </summary>
        /// <returns>Array of endpoints</returns>
        ISubscriber GetSubscriber();

        /// <summary>
        /// Delete all the keys of the database
        /// </summary>
        Task FlushDatabaseAsync();

        /// <summary>
        /// The Redis instance name
        /// </summary>
        string Instance { get; }
    }
}