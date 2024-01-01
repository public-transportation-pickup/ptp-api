using System.Reflection;

namespace PTP.WebAPI;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}