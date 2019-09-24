using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SystemPlus.Xml
{
    /// <summary>
    /// Extension methods to help with navigating xml documents
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks is a string is a valid xpath expression
        /// </summary>
        public static bool VerifyXPath(string xpath, out string error)
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
        /// <returns></returns>
        public static string GetAttributeValue(this XElement element, string name)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return null;

            return attribute.Value;
        }

        public static double GetAttributeDouble(this XElement element, string name, double defaultVal = 0)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return defaultVal;

            if (double.TryParse(attribute.Value, out double result))
                return result;

            return defaultVal;
        }

        public static T GetAttributeValue<T>(this XElement element, string name)
        {
            XAttribute attribute = element.Attribute(name);

            if (attribute == null)
                return default(T);

            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            return (T)tc.ConvertFrom(attribute.Value);
        }
    }
}