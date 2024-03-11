using Corely.Common.Providers.Redaction;
using Serilog.Core;
using Serilog.Events;

namespace Corely.DevTools
{
    public class RedactionEnricher(List<IRedactionProvider> redactors)
        : ILogEventEnricher
    {
        private readonly List<IRedactionProvider> _redactors = redactors;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var property in logEvent.Properties.ToList())
            {
                foreach (var redactor in _redactors)
                {
                    var redactedValue = redactor.Redact(property.Value.ToString());
                    logEvent.AddOrUpdateProperty(new LogEventProperty(property.Key, new ScalarValue(redactedValue)));
                }
            }
        }
    }
}
