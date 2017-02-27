namespace Sophie.Cache
{
    // Summary: Defines the supported expiration modes for cache items. Value None will indicate that
    // no expiration should be set.
    public enum Expiration
    {
        // Summary: Default value for the expircation mode enum. CacheManager will default to None.
        // The Default entry in the enum is used as separation from the other values and to make it
        // possible to explicitly set the expiration to None.
        Default = 0,

        // Summary: Defines no expiration.
        None = 1,

        // Summary: Defines sliding expiration. The expiration timeout will be refreshed on every access.
        Sliding = 2,

        // Summary: Defines absolute expiration. The item will expire after the expiration timeout.
        Absolute = 3
    }
}