using System.ComponentModel;
using System.Reflection;

namespace NotificationCenter.Core.Enums
{
    /// <summary>
    /// A class that contains extention methods.
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// An extension method that gets the <see cref="DescriptionAttribute"/> of an Object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="source">The instance of the object of which the <see cref="DescriptionAttribute"/> should be returned.</param>
        /// <returns>The value of the <see cref="DescriptionAttribute"/>, if any.</returns>
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
