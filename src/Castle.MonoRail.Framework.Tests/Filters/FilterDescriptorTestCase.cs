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

namespace Castle.MonoRail.Framework.Tests.Filters
{
	using Castle.MonoRail.Framework.Descriptors;
	using Castle.MonoRail.Framework.Tests.Actions;
	using NUnit.Framework;

	[TestFixture]
	public class FilterDescriptorTestCase
	{
		[Test]		
		public void FilterDescriptor_can_be_constructed_from_FilterAttribute_instance()
		{
			var filterattribute = new	FilterAttribute(ExecuteWhen.Always, typeof(ActionMethodExecutorTestCase.DummyFilter)); 
			var descriptor = new FilterDescriptor(filterattribute);
			Assert.AreEqual(filterattribute.ExecutionOrder, descriptor.ExecutionOrder);
			Assert.AreEqual(filterattribute.When, descriptor.When);
			Assert.AreEqual(filterattribute.FilterType, descriptor.FilterType);
		}
	}
}