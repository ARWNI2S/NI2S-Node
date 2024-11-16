using System.Diagnostics;

namespace ARWNI2S.Infrastructure
{
    internal class Constants
    {
        #region CONVENTIONS

        public static readonly string NAME_None = "NAME_None";

        #endregion

        #region SIMULATION CONSTANTS

        public static readonly int MINIMUM_DESIRED_FRAMERATE = 15;

        #endregion

        #region TIME CONSTANTS

        /// <summary>
        /// This needs to be first, as EntityId static initializers reference it. Otherwise, EntityId actually see a uninitialized (ie Zero) value for that "constant"!
        /// </summary>
        public static readonly TimeSpan INFINITE_TIMESPAN = TimeSpan.FromMilliseconds(-1);

        /// <summary>
        /// We assume that clock skew between nodes and between nodeClients and nodes is always less than 1 second
        /// </summary>
        public static readonly TimeSpan MAXIMUM_CLOCK_SKEW = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The default timeout before a request is assumed to have failed.
        /// </summary>
        public static readonly TimeSpan DEFAULT_RESPONSE_TIMEOUT = Debugger.IsAttached ? TimeSpan.FromMinutes(3) : TimeSpan.FromSeconds(10);

        /// <summary>
        /// The delay after a cancellation token holder entity will be marked for collection.
        /// </summary>
        public static readonly TimeSpan DEFAULT_CANCELLATION_TOKEN_HOLDER_DEACTIVATION_DELAY = Debugger.IsAttached ? TimeSpan.FromMinutes(3) : TimeSpan.FromSeconds(10);

        public static readonly TimeSpan DEFAULT_CLIENT_DROP_TIMEOUT = TimeSpan.FromMinutes(1);

        #endregion

        #region ASSEMBLY NAMES

        public const string NI2S_DYNAMIC_CODE_ASSEMBLY = "ARWNI2S.Engine.Dynamic";

        #endregion

        #region SERVICE NAMES

        public const string DEFAULT_STORAGE_PROVIDER_NAME = "Default";
        public const string MEMORY_STORAGE_PROVIDER_NAME = "MemoryStore";
        public const string DATA_CONNECTION_STRING_NAME = "DataConnectionString";
        public const string ADO_INVARIANT_NAME = "AdoInvariant";
        public const string DATA_CONNECTION_FOR_REMINDERS_STRING_NAME = "DataConnectionStringForReminders";
        public const string ADO_INVARIANT_FOR_REMINDERS_NAME = "AdoInvariantForReminders";

        #endregion

        #region NAMESPACES


        public const string NI2S_AZURE_UTILS_DLL = "ARWNI2S.Azure";

        public const string NI2S_SQL_UTILS_DLL = "ARWNI2S.SQLTools";

        public const string INVARIANT_NAME_SQL_SERVER = "System.Data.SqlClient";

        #endregion

        #region LIMITS

        public const int LARGE_OBJECT_HEAP_THRESHOLD = 85000;

        public const bool DEFAULT_PROPAGATE_E2E_ACTIVITY_ID = false;

        public const int DEFAULT_LOGGER_BULK_MESSAGE_LIMIT = 5;

        #endregion

        #region RESERVED UUIDS

        public static readonly Guid _UUID_WORLD_ACTOR = new("00000001-0001-0001-0001-000000000001");

        #endregion

        #region EVENT TYPES

        public static readonly int EVENTTYPE_ERRORCRITICAL = -9999;

        #endregion
    }
}