using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Entities
{
    // Sample entity kept in memory on purpose — this module is a wiring example,
    // not a real feature. Swap this for a real EF Core / Dapper-backed entity
    // in your own modules.
    public class Greeting : BaseEntity
    {
        public required string Message { get; set; }
    }
}
