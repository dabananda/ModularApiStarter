using System.Collections.Concurrent;

namespace ModularApiStarter.Modules.Greeting
{
    // Stand-in for a real repository/DbContext so this module has zero external
    // dependencies (no DB connection string needed) and works immediately after
    // you scaffold from the template. Registered as a singleton — replace with
    // your actual data access when you build a real module.
    public class GreetingStore
    {
        private readonly ConcurrentDictionary<Guid, Entities.Greeting> _greetings = new();

        public Entities.Greeting Add(Entities.Greeting greeting)
        {
            _greetings[greeting.Id] = greeting;
            return greeting;
        }

        public List<Entities.Greeting> GetAll() =>
            _greetings.Values.OrderByDescending(g => g.CreatedAt).ToList();
    }
}
