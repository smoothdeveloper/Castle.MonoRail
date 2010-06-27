// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.Framework.Filters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Castle.Core.Logging;
	using Castle.MonoRail.Framework.Descriptors;

	/// <summary>
	/// encapsulate current filter execution behaviour
	/// <remarks>was extracted from <see cref="Controller"/> implementation</remarks>
	/// </summary>
	internal static class FilterProcessor
	{
		/// <summary>
		/// executes all filters 
		/// <list>
		/// <item>that are not meant to be skipped</item>
		/// <item>matching the When criteria</item>
		/// </list> 
		/// </summary>
		/// <param name="logger">logger of the component declaring the filter (probably a controller or a dynamic action)</param>
		/// <param name="filterFactory">filter factory to instanciate filters</param>
		/// <param name="executionContext">current execution context</param>
		/// <param name="action">the executable action (used to skip filters)</param>
		/// <param name="when">restrict filters execution to those meant to execute at this time</param>
		/// <param name="filters">non filtered filters that are meant to be executed</param>
		/// <returns>true if all filters were executed, false if a filter return false</returns>
		public static bool ProcessFilters(ILogger logger, IFilterFactory filterFactory, IExecutionContext executionContext, IExecutableAction action, ExecuteWhen when, IEnumerable<FilterDescriptor> filters)
		{
			if (!filters.Any())
				return true;

			var result = filters
				.Where(desc => !action.ShouldSkipFilter(desc.FilterType))
				.Where(desc => (desc.When & when) != 0)
				.All(desc => ProcessFilter(logger, executionContext, filterFactory, when, desc))
				;
			return result;
		}

		private 
			static bool ProcessFilter(
			ILogger logger, 
			IExecutionContext executionContext, 
			IFilterFactory filterFactory, 
			ExecuteWhen when, 
			FilterDescriptor desc) {
			if (desc.FilterInstance == null) 
			{
				desc.FilterInstance = filterFactory.Create(desc.FilterType);

				var filterAttAware = desc.FilterInstance as IFilterAttributeAware;
				
				if (filterAttAware != null) 
				{
					filterAttAware.Filter = desc.Attribute;
				}
			}

			try
			{
				if (logger.IsDebugEnabled)
				{
					logger.DebugFormat("Running filter {0}/{1}", when, desc.FilterType.FullName);
				}

				return ExecuteFilterDescriptor(desc, when, executionContext);

			}
			catch (Exception ex) {
				if (logger.IsErrorEnabled) {
					logger.Error("Error processing filter " + desc.FilterType.FullName, ex);
				}

				throw;
			}
			}

		private static bool ExecuteFilterDescriptor(FilterDescriptor desc, ExecuteWhen when, IExecutionContext executionContext)
		{
			return desc.FilterInstance.Perform(when, executionContext.EngineContext, executionContext.Controller, executionContext.ControllerContext);
		}
	}
}