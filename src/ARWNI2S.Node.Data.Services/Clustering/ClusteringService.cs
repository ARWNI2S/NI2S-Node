using ARWNI2S.Node.Core;
using ARWNI2S.Node.Data.Entities.Clustering;
using ARWNI2S.Node.Data.Extensions;

namespace ARWNI2S.Node.Data.Services.Clustering
{
    /// <summary>
    /// Server service
    /// </summary>
    public partial class ClusteringService : IClusteringService
    {
        #region Fields

        private readonly IRepository<BladeServer> _serverRepository;
        private static readonly char[] separator = new[] { ',' };

        #endregion

        #region Ctor

        public ClusteringService(IRepository<BladeServer> serverRepository)
        {
            _serverRepository = serverRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>Comma-separated hosts</returns>
        protected virtual string[] ParseHostValues(BladeServer server)
        {
            ArgumentNullException.ThrowIfNull(server);

            var parsedValues = new List<string>();
            if (string.IsNullOrEmpty(server.Hosts))
                return parsedValues.ToArray();

            var hosts = server.Hosts.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var host in hosts)
            {
                var tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a server
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteServerAsync(BladeServer server)
        {
            ArgumentNullException.ThrowIfNull(server);

            var allNodes = await GetAllServersAsync();
            if (allNodes.Count == 1)
                throw new ServerException("You cannot delete the only configured server");

            await _serverRepository.DeleteAsync(server);
        }

        /// <summary>
        /// Gets all servers
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the servers
        /// </returns>
        public virtual async Task<IList<BladeServer>> GetAllServersAsync()
        {
            return await _serverRepository.GetAllAsync(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets all servers
        /// </summary>
        /// <returns>
        /// The servers
        /// </returns>
        public virtual IList<BladeServer> GetAllNodes()
        {
            return _serverRepository.GetAll(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets a server 
        /// </summary>
        /// <param name="serverId">Server identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the server
        /// </returns>
        public virtual async Task<BladeServer> GetNodeByIdAsync(int serverId)
        {
            return await _serverRepository.GetByIdAsync(serverId, cache => default, false);
        }

        /// <summary>
        /// Inserts a server
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertServerAsync(BladeServer server)
        {
            await _serverRepository.InsertAsync(server);
        }

        /// <summary>
        /// Updates the server
        /// </summary>
        /// <param name="server">Server</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateServerAsync(BladeServer server)
        {
            await _serverRepository.UpdateAsync(server);
        }

        /// <summary>
        /// Updates the server
        /// </summary>
        /// <param name="server">Server</param>
        public virtual void UpdateNode(BladeServer server)
        {
            _serverRepository.Update(server);
        }

        /// <summary>
        /// Indicates whether a server contains a specified host
        /// </summary>
        /// <param name="server">Server</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(BladeServer server, string host)
        {
            ArgumentNullException.ThrowIfNull(server);

            if (string.IsNullOrEmpty(host))
                return false;

            var contains = ParseHostValues(server).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        /// <summary>
        /// Returns a list of names of not existing servers
        /// </summary>
        /// <param name="serverIdsNames">The names and/or IDs of the server to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing servers
        /// </returns>
        public async Task<string[]> GetNotExistingServersAsync(string[] serverIdsNames)
        {
            ArgumentNullException.ThrowIfNull(serverIdsNames);

            var query = _serverRepository.Table;
            var queryFilter = serverIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = await query.Select(server => server.Name)
                .Where(server => queryFilter.Contains(server))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (queryFilter.Length == 0)
                return queryFilter.ToArray();

            //filtering by IDs
            filter = await query.Select(server => server.Id.ToString())
                .Where(server => queryFilter.Contains(server))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        #endregion
    }
}