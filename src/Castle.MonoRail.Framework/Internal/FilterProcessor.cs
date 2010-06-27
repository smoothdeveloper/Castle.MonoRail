using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework.Descriptors;

namespace Castle.MonoRail.Framework.Internal {
	static internal class FilterProcessor {
		public static bool ProcessFilters(IEnumerable<FilterDescriptor> filters, IExecutableAction action, ExecuteWhen when, IFilterFactory filterFactory, ILogger filterOwnerLogger, IEngineContext engineContext, IController controller, IControllerContext controllerContext)
		{
			foreach (var desc in filters) {
				if (action.ShouldSkipFilter(desc.FilterType)) {
					continue;
				}

				if ((desc.When & when) != 0) {
					if (!ProcessFilter(when, desc, filterFactory, filterOwnerLogger, engineContext, controller, controllerContext)) {
						return false;
					}
				}
			}

			return true;
		}

		private static bool ProcessFilter(ExecuteWhen when, FilterDescriptor desc, IFilterFactory filterFactory, ILogger logger, IEngineContext engineContext, IController controller, IControllerContext controllerContext)
		{
			if (desc.FilterInstance == null) {
				desc.FilterInstance = filterFactory.Create(desc.FilterType);

				var filterAttAware = desc.FilterInstance as IFilterAttributeAware;

				if (filterAttAware != null) {
					filterAttAware.Filter = desc.Attribute;
				}
			}

			try {
				if (logger.IsDebugEnabled) {
					logger.DebugFormat("Running filter {0}/{1}", when, desc.FilterType.FullName);
				}

				return desc.FilterInstance.Perform(when, engineContext, controller, controllerContext);
			}
			catch (Exception ex) {
				if (logger.IsErrorEnabled) {
					logger.Error("Error processing filter " + desc.FilterType.FullName, ex);
				}

				throw;
			}
		}

	}
}
