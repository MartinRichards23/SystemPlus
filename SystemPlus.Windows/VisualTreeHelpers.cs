using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SystemPlus.Windows
{
    public static class VisualTreeHelpers
    {
        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        public static T FindVisualChild<T>(Visual visual) //where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child == null)
                    continue;

                if (child is T)
                    return (T)(object)child;

                T descendent = FindVisualChild<T>(child);

                if (descendent != null)
                    return descendent;
            }

            return default;
        }

        public static T FindVisualParent<T>(DependencyObject child) where T : Visual
        {
            while ((child != null) && !(child is T))
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return (T)child;
        }

        /// <summary>
        /// Returns the first ancester of specified type
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Returns datacontext from first ancstor of that type
        /// </summary>
        public static T FindAncestorDataContext<T>(DependencyObject current)
        {
            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                T data = GetDataContext<T>(current);
                if (data != null)
                    return data;

                current = VisualTreeHelper.GetParent(current);
            }

            return default;
        }

        /// <summary>
        /// Returns a specific ancester of an object
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current, T lookupItem) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T && ReferenceEquals(current, lookupItem))
                    return (T)current;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Finds an ancestor object by name and type
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current, string parentName) where T : DependencyObject
        {
            while (current != null)
            {
                if (!string.IsNullOrEmpty(parentName))
                {
                    FrameworkElement? frameworkElement = current as FrameworkElement;
                    if (current is T && frameworkElement != null && frameworkElement.Name == parentName)
                    {
                        return (T)current;
                    }
                }
                else if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Looks for a child control within a parent by name
        /// </summary>
        public static T? FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null)
                return null;

            T? foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T? childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    FrameworkElement? frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }

                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                        break;
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Looks for a child control within a parent by type
        /// </summary>
        public static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            // Confirm parent is valid.
            if (parent == null)
                return null;

            T? foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T? childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null)
                        break;
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        /// <summary>
        /// Returns typed datacontext from "sender" object
        /// </summary>
        public static T GetDataContext<T>(object sender)
        {
            FrameworkElement? fe = sender as FrameworkElement;

            if (fe != null && fe.DataContext is T)
                return (T)fe.DataContext;

            return default;
        }

        /// <summary>
        /// Returns typed tag from "sender" object
        /// </summary>
        public static T GetTag<T>(object sender)
        {
            FrameworkElement? fe = sender as FrameworkElement;

            if (fe != null && fe.Tag is T)
                return (T)fe.Tag;

            return default;
        }

        /// <summary>
        /// Returns the contextmenu placement target from sender object
        /// </summary>
        public static T GetContextPlacement<T>(object sender) where T : FrameworkElement
        {
            if (!(sender is MenuItem mnu))
                return null;

            if (!(mnu.Parent is ContextMenu context))
                return null;

            return context.PlacementTarget as T;
        }

        /// <summary>
        /// Gets the inner child of type i.e. for a listbox get the inner VirtualizingStackPanel
        /// </summary>
        public static T GetInnerChildOfType<T>(FrameworkElement element) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                FrameworkElement? child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child == null)
                    continue;

                if (child is T)
                    return (T)child;

                T panel = GetInnerChildOfType<T>(child);

                if (panel != null)
                    return panel;
            }

            return null;
        }

        /// <summary>
        /// Gets the visible elements, i.e. from a listbox
        /// </summary>
        public static IList<FrameworkElement> GetVisibleItems(FrameworkElement element)
        {
            VirtualizingStackPanel theStackPanel = GetInnerChildOfType<VirtualizingStackPanel>(element);

            IList<FrameworkElement> visibleElements = new List<FrameworkElement>();

            for (int i = 0; i < theStackPanel.Children.Count; i++)
            {
                if (theStackPanel.Children[i] is FrameworkElement fe)
                    visibleElements.Add(fe);
            }

            return visibleElements;
        }
    }
}