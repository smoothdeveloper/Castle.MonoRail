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

namespace Castle.MonoRail.Framework.Internal
{
	using System.Collections;
	using System.Collections.Generic;
	using Castle.MonoRail.Framework.Descriptors;

	/// <summary>
	/// This implementation (of <see cref="IComparer"/> and <see cref="IComparer{T}"/>)
	/// is used to sort the filters based on their Execution Order.
	/// </summary>
	/// <remarks>was moved from DefaultControllerDescriptorProvider</remarks>
	internal class FilterDescriptorComparer : IComparer, IComparer<FilterDescriptor> {
		private static readonly FilterDescriptorComparer instance = new FilterDescriptorComparer();

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterDescriptorComparer"/> class.
		/// </summary>
		private FilterDescriptorComparer() {
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static FilterDescriptorComparer Instance {
			get { return instance; }
		}

		/// <summary>
		/// Compares the specified left.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>left execution order - right</returns>
		public int Compare(object left, object right) {
			return ((FilterDescriptor)left).ExecutionOrder - ((FilterDescriptor)right).ExecutionOrder;
		}
		/// <summary>
		/// Compares the specified left.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>left execution order - right</returns>
		public int Compare(FilterDescriptor left, FilterDescriptor right) {
			return Compare((object)left, right);
		}
	}
}