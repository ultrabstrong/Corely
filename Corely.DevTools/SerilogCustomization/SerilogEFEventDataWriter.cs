using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using System.Data.Common;

namespace Corely.DevTools.SerilogCustomization
{
    internal class SerilogEFEventDataWriter
    {
        public void Write(EventData eventData)
        {
            var logEvent = new LogEvent(
                timestamp: DateTimeOffset.Now,
                level: GetSerilogLogEventLevel(eventData.LogLevel),
                exception: null,
                messageTemplate: new MessageTemplate(
                    "Entity Framework {EventDataType}",
                    [new PropertyToken("EventDataType", "")]),
                properties: [
                    new LogEventProperty("EventDataType", new ScalarValue(eventData.GetType().Name))
                ]);

            if (eventData is CommandExecutedEventData commandExecutedEventData)
            {
                Update(commandExecutedEventData, logEvent);
            }

            Log.Logger.Write(logEvent);
        }

        private static LogEventLevel GetSerilogLogEventLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Critical => LogEventLevel.Fatal,
                _ => LogEventLevel.Information
            };
        }

        private void Update(CommandExecutedEventData eventData, LogEvent logEvent)
        {
            UpsertScalarProperty(logEvent, nameof(eventData.Connection.DataSource), eventData.Connection.DataSource);
            UpsertScalarProperty(logEvent, nameof(eventData.Connection.Database), eventData.Connection.Database);
            UpsertScalarProperty(logEvent, nameof(eventData.Connection.ServerVersion), eventData.Connection.ServerVersion);
            UpsertScalarProperty(logEvent, nameof(eventData.Context), eventData.Context?.GetType());
            UpsertScalarProperty(logEvent, nameof(eventData.Duration) + " (ms)", eventData.Duration.TotalMilliseconds);
            UpsertScalarProperty(logEvent, nameof(eventData.CommandSource), eventData.CommandSource);
            UpsertScalarProperty(logEvent, nameof(eventData.ExecuteMethod), eventData.ExecuteMethod);
            UpsertScalarProperty(logEvent, nameof(eventData.IsAsync), eventData.IsAsync);
            UpsertScalarProperty(logEvent, nameof(eventData.Command), eventData.Command);
            UpsertScalarProperty(logEvent, nameof(eventData.CommandId), eventData.CommandId);
            UpsertScalarProperty(logEvent, nameof(eventData.Command.CommandType), eventData.Command.CommandType);
            UpsertScalarProperty(logEvent, nameof(eventData.Command.CommandText), eventData.Command.CommandText);
            UpsertParameterProperties(logEvent, eventData.Command.Parameters, eventData.LogParameterValues);
        }

        private void UpsertParameterProperties(LogEvent logEvent, DbParameterCollection parameters, bool logValues)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var parameterName = $"p{i}";
                object? parameterValue = "?";

                if (parameters[i] is MySqlParameter mySqlParameter)
                {
                    parameterName = mySqlParameter.ParameterName;
                    if (logValues) { parameterValue = mySqlParameter.Value; }
                }
                UpsertScalarProperty(logEvent, parameterName, parameterValue);
            }
        }

        private void UpsertScalarProperty(LogEvent logEvent, string propName, object? propValue)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty(propName, new ScalarValue(propValue)));
        }
    }
}
