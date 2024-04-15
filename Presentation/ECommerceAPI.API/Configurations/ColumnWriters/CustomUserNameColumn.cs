using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using Serilog.Core;

namespace ECommerceAPI.API.Configurations.ColumnWriters
{
	public class CustomUserNameColumn : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			var (username, value) = logEvent.Properties.FirstOrDefault(x => x.Key == "Username");
			if (value != null)
			{
				var getValue = propertyFactory.CreateProperty(username, value);
				logEvent.AddPropertyIfAbsent(getValue);
			}
		}
	}
}
