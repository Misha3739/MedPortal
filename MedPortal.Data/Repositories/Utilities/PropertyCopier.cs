using System.Linq;

namespace MedPortal.Data.Repositories.Utilities
{
    public static class ReflectionUtils {
	    private static readonly string[] IgnoreFiels = { "Id" };

        public static TTarget Copy<TSource,TTarget>(TSource source) where TTarget : class, new()
        { 
            TTarget target = new TTarget();
            var parentProperties = source.GetType().GetProperties();
            var childProperties = target.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(target, parentProperty.GetValue(source));
                    }
                }
            }

            return target;
        }
        
        public static void Copy<TSource,TTarget>(TSource source, TTarget target)
        { 
            var parentProperties = source.GetType().GetProperties();
            var childProperties = target.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType && !IgnoreFiels.Contains(childProperty.Name))
                    {
                        childProperty.SetValue(target, parentProperty.GetValue(source));
                    }
                }
            }
        }
    }
}