using System.Linq;
using System.Reflection;

namespace ems.Helpers
{
    public static class ReflectionHelpers
    {
        public static void SetProperty(string property, object target, object value)
        {
            // compoundProperty
            if (property.Contains('.'))
            {
                string[] bits = property.Split('.');
                for (int i = 0; i < bits.Length - 1; i++)
                {
                    PropertyInfo propertyToGet = target.GetType().GetProperty(bits[i]);
                    target = propertyToGet.GetValue(target, null);
                }
                PropertyInfo propertyToSet = target.GetType().GetProperty(bits.Last());
                propertyToSet.SetValue(target, value, null);
            }
            // Simple property
            else
            {
                PropertyInfo propertyToSet = target.GetType().GetProperty(property);
                propertyToSet.SetValue(target, value, null);
            }
        }


    }
}