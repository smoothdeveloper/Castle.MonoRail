using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework.Descriptors;

namespace Castle.MonoRail.Framework.Internal {
	/// <summary>
	/// encapsulate current filter execution behaviour
	/// <remarks>was extracted from <see cref="Controller"/> implementation</remarks>
	/// </summary>
	static internal class FilterProcessor {

		/// <summary>
		/// executes all filters 
		/// <list>
		/// <item>that are not meant to be skipped</item>
		/// <item>matching the When criteria</item>
		/// </list> 
		/// </summary>
		/// <param name="filters">non filtered filters that are meant to be executed</param>
		/// <param name="action">the executable action (used to skip filters)</param>
		/// <param name="when">restrict filters execution to those meant to execute at this time</param>
		/// <param name="filterFactory">filter factory to instanciate filters</param>
		/// <param name="filterOwnerLogger">logger of the component declaring the filter (probably a controller or a dynamic action)</param>
		/// <param name="engineContext">engine context instance</param>
		/// <param name="controller">controller instance</param>
		/// <param name="controllerContext">controller context instance</param>
		/// <returns>true if all filters were executed, false if a filter returned false</returns>
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
