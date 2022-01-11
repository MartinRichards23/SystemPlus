using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SystemPlus.Xml
{
    /// <summary>
    /// Extension methods to help with navigating xml documents
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// Checks is a string is a valid xpath expression
        /// </summary>
        public static bool VerifyXPath(string xpath, out string? error)
        {
            try
            {
                XPathExpression.Compile(xpath);

                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Returns attribute value, or null if not present
        /// </summary>
        public static string GetAttributeValue(this XElement element, string name)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            XAttribute? attribute = element.Attribute(name);

            if (attribute == null)
                return string.Empty;

            return attribute.Value;
        }

        public static T? GetAttributeValue<T>(this XElement element, string name) where T : struct
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            XAttribute? attribute = element.Attribute(name);

            if (attribute == null)
                return default;

            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            return (T?)tc.ConvertFrom(attribute.Value);
        }
    }
}