using System.Reflection;

namespace PTP.Infrastructure
{
	public static class AssemblyReference
	{
		public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
	}
}
