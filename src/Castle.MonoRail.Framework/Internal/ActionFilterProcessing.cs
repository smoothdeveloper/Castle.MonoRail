using System.Collections.Generic;
using System.Linq;
using Castle.Core.Logging;
using Castle.MonoRail.Framework.Descriptors;
using Castle.MonoRail.Framework.Filters;

namespace Castle.MonoRail.Framework.Internal
{
	internal static class ActionFilterProcessing
	{
		static IEnumerable<FilterDescriptor> getFilterDescriptorsToExecute(this IExecutableAction action, ExecuteWhen when)
		{
			var filters = action.CollectFilters()
				.Where(f => (when & f.When) == when)
				.OrderBy(f => f.ExecutionOrder);
			return filters;
		}

		internal static void ExecuteFilters(this IExecutableAction action, IExecutionContext executionContext, ExecuteWhen when, ILogger filterOwnerLogger) {
			var filterDescriptors = action.getFilterDescriptorsToExecute(when);
			var filterFactory = executionContext.EngineContext.Services.FilterFactory;
			FilterProcessor.ProcessFilters(filterOwnerLogger, filterFactory, executionContext, action, when, filterDescriptors);
		}
	}
}