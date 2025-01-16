using ARWNI2S.Caching;
using ARWNI2S.Cluster.Entities;
using ARWNI2S.Engine.Data;
using ARWNI2S.Engine.Data.Extensions;

namespace ARWNI2S.Cluster.Nodes
{
    /// <summary>
    /// NodeInfo service
    /// </summary>
    public partial class NodeService : INodeService
    {
        #region Fields

        protected readonly IRepository<Node> _nodeRepository;
        private readonly IShortTermCacheManager _shortTermCacheManager;
        //private static readonly char[] _separator = [','];

        #endregion

        #region Ctor

        public NodeService(IRepository<Node> nodeRepository,
            IShortTermCacheManager shortTermCacheManager)
        {
            _nodeRepository = nodeRepository;
            _shortTermCacheManager = shortTermCacheManager;
        }

        #endregion

        #region Utilities

        ///// <summary>
        ///// Parse comma-separated Hosts
        ///// </summary>
        ///// <param name="node">NodeInfo</param>
        ///// <returns>Comma-separated hosts</returns>
        //protected virtual string[] ParseHostValues(Node node)
        //{
        //    ArgumentNullException.ThrowIfNull(node);

        //    var parsedValues = new List<string>();
        //    if (string.IsNullOrEmpty(node.Addresses))
        //        return parsedValues.ToArray();

        //    var hosts = node.Addresses.Split(_separator, StringSplitOptions.RemoveEmptyEntries);

        //    foreach (var host in hosts)
        //    {
        //        var tmp = host.Trim();
        //        if (!string.IsNullOrEmpty(tmp))
        //            parsedValues.Add(tmp);
        //    }

        //    return [.. parsedValues];
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteNodeAsync(Node node)
        {
            ArgumentNullException.ThrowIfNull(node);

            var allNodes = await GetAllNodesAsync();
            if (allNodes.Count == 1)
                throw new Exception("You cannot delete the only configured node");

            await _nodeRepository.DeleteAsync(node);
        }

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the nodes
        /// </returns>
        public virtual async Task<IList<Node>> GetAllNodesAsync()
        {
            return await _nodeRepository.GetAllAsync(query =>
            {
                return from s in query orderby s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <returns>
        /// The nodes
        /// </returns>
        public virtual IList<Node> GetAllNodes()
        {
            return _nodeRepository.GetAll(query =>
            {
                return from s in query orderby s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="id">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        public virtual async Task<Node> GetNodeByIdAsync(int id)
        {
            return await _nodeRepository.GetByIdAsync(id, cache => default, false);
        }

        /// <summary>
        /// Gets a node 
        /// </summary>
        /// <param name="nodeId">NodeInfo identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the node
        /// </returns>
        public async Task<Node> GetNodeByNodeIdAsync(Guid nodeId)
        {
            if (nodeId == Guid.Empty)
                return null;

            var query = from n in _nodeRepository.Table
                        where n.NodeId == nodeId
                        orderby n.Id
                        select n;

            return await _shortTermCacheManager.GetAsync(async () => await query.FirstOrDefaultAsync(), NodeServicesDefaults.NodeByGuidCacheKey, nodeId);
        }

        /// <summary>
        /// Inserts a node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertNodeAsync(Node node)
        {
            await _nodeRepository.InsertAsync(node);
        }

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateNodeAsync(Node node)
        {
            await _nodeRepository.UpdateAsync(node);
        }

        /// <summary>
        /// Updates the node
        /// </summary>
        /// <param name="node">NodeInfo</param>
        public virtual void UpdateNode(Node node)
        {
            _nodeRepository.Update(node);
        }

        ///// <summary>
        ///// Indicates whether a node contains a specified host
        ///// </summary>
        ///// <param name="node">NodeInfo</param>
        ///// <param name="host">Host</param>
        ///// <returns>true - contains, false - no</returns>
        //public virtual bool ContainsHostValue(Node node, string host)
        //{
        //    ArgumentNullException.ThrowIfNull(node);

        //    if (string.IsNullOrEmpty(host))
        //        return false;

        //    var contains = ParseHostValues(node).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

        //    return contains;
        //}

        /// <summary>
        /// Returns a list of names of not existing nodes
        /// </summary>
        /// <param name="nodeIdsNames">The names and/or IDs of the node to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing nodes
        /// </returns>
        public async Task<string[]> GetNotExistingNodesAsync(string[] nodeIdsNames)
        {
            ArgumentNullException.ThrowIfNull(nodeIdsNames);

            var query = _nodeRepository.Table;
            var queryFilter = nodeIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = await query.Select(node => node.Name)
                .Where(node => queryFilter.Contains(node))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (queryFilter.Length == 0)
                return [.. queryFilter];

            //filtering by IDs
            filter = await query.Select(node => node.Id.ToString())
                .Where(node => queryFilter.Contains(node))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            return [.. queryFilter];
        }

        #endregion
    }
}