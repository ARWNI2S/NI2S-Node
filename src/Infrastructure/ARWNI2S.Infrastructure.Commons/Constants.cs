namespace ARWNI2S.Infrastructure
{
    internal class Constants
    {
        // This needs to be first, as EntityId static initializers reference it. Otherwise, EntityId actually see a uninitialized (ie Zero) value for that "constant"!
        public static readonly TimeSpan INFINITE_TIMESPAN = TimeSpan.FromMilliseconds(-1);

        //// We assume that clock skew between nodes and between nodeClients and nodes is always less than 1 second
        //public static readonly TimeSpan MAXIMUM_CLOCK_SKEW = TimeSpan.FromSeconds(1);

        //public const string DEFAULT_STORAGE_PROVIDER_NAME = "Default";
        //public const string MEMORY_STORAGE_PROVIDER_NAME = "MemoryStore";
        //public const string DATA_CONNECTION_STRING_NAME = "DataConnectionString";
        //public const string ADO_INVARIANT_NAME = "AdoInvariant";
        //public const string DATA_CONNECTION_FOR_REMINDERS_STRING_NAME = "DataConnectionStringForReminders";
        //public const string ADO_INVARIANT_FOR_REMINDERS_NAME = "AdoInvariantForReminders";

        //public const string NI2S_AZURE_UTILS_DLL = "Softwar.NI2S.Azure";

        //public const string NI2S_SQL_UTILS_DLL = "Softwar.NI2S.SQLTools";

        //public const string INVARIANT_NAME_SQL_SERVER = "System.Data.SqlClient";

        ///// <summary>
        ///// The default timeout before a request is assumed to have failed.
        ///// </summary>
        //public static readonly TimeSpan DEFAULT_RESPONSE_TIMEOUT = Debugger.IsAttached ? TimeSpan.FromMinutes(30) : TimeSpan.FromSeconds(30);

        ///// <summary>
        ///// The delay after a cancellation token holder entity will be marked for collection.
        ///// </summary>
        //public static readonly TimeSpan DEFAULT_CANCELLATION_TOKEN_HOLDER_DEACTIVATION_DELAY = Debugger.IsAttached ? TimeSpan.FromMinutes(30) : TimeSpan.FromSeconds(30);

        ///// <summary>
        ///// Minimum period for registering a reminder ... we want to enforce a lower bound
        ///// </summary>
        //public static readonly TimeSpan MinReminderPeriod = TimeSpan.FromMinutes(1); // increase this period, reminders are supposed to be less frequent ... we use 2 seconds just to reduce the running time of the unit tests
        ///// <summary>
        ///// Refresh local reminder list to reflect the global reminder table every 'REFRESH_REMINDER_LIST' period
        ///// </summary>
        //public static readonly TimeSpan RefreshReminderList = TimeSpan.FromMinutes(5);

        //public const int LARGE_OBJECT_HEAP_THRESHOLD = 85000;

        //public const bool DEFAULT_PROPAGATE_E2E_ACTIVITY_ID = false;

        public const int DEFAULT_LOGGER_BULK_MESSAGE_LIMIT = 5;

        //public static readonly TimeSpan DEFAULT_CLIENT_DROP_TIMEOUT = TimeSpan.FromMinutes(1);
    }
}