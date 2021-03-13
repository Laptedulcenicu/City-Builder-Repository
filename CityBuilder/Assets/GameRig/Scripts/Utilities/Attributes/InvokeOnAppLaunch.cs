using System;
using JetBrains.Annotations;

namespace GameRig.Scripts.Utilities.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[MeansImplicitUse]
	public class InvokeOnAppLaunch : Attribute
	{
		public Type[] Dependencies { get; }

		public InvokeOnAppLaunch(params Type[] dependencies)
		{
			Dependencies = dependencies;
		}
	}
}