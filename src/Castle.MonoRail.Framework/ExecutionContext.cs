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

namespace Castle.MonoRail.Framework
{
	/// <summary>
	/// Plain implementation of <see cref="IExecutionContext"/>
	/// </summary>
	public class ExecutionContext : IExecutionContext
	{
		/// <summary>
		/// Instance of <see cref="IEngineContext"/>
		/// </summary>
		public IEngineContext EngineContext { get; set; }
		/// <summary>
		/// Instance of <see cref="IController"/>
		/// </summary>
		public IController Controller { get; set; }
		/// <summary>
		/// Instance of <see cref="ControllerContext"/> 
		/// </summary>
		public IControllerContext ControllerContext { get; set; }
	}
}