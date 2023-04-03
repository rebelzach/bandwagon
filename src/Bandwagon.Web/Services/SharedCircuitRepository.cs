using System.Collections.Concurrent;

namespace Bandwagon.Web.Services;

public class SharedCircuitRepository
{
    private readonly ConcurrentDictionary<(Type SharedCircuitType, string Id), object> _sharedCircuts = new();
    private readonly IServiceProvider _provider;

    public SharedCircuitRepository(
        IServiceProvider provider)
    {
        _provider = provider;
    }

    public TCircuit GetOrCreate<TCircuit>(string id) where TCircuit : notnull, ISharedCircuit
    {
        var key = (typeof(TCircuit), id);
        if (_sharedCircuts.TryGetValue(key, out var circuit))
        {
            return (TCircuit)circuit;
        }

        circuit = _provider.GetRequiredService<TCircuit>();

        if (_sharedCircuts.TryAdd(key, circuit))
        {
            return (TCircuit)circuit;
        }

        // Thread battle lost, get actual circuit
        if (_sharedCircuts.TryGetValue(key, out circuit))
        {
            return (TCircuit)circuit;
        }

        throw new InvalidOperationException("Couldnt't get session");
    }
}
