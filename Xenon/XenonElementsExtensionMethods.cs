using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenon
{
    internal static class XenonElementsExtensionMethods
    {
        internal static IXenonElement LocateFirstVisibleElement( this IEnumerable<IXenonElement> elements )
        {
            return elements.First( x => x.IsVisible ).ScrollToElement();
        }

        internal static IXenonElement LocateSingleVisibleElement( this IEnumerable<IXenonElement> elements )
        {
            var foundElements = elements.Where( x => x.IsVisible ).ToList();

            if ( foundElements.Count == 1 )
                return foundElements.First().ScrollToElement();

            if ( foundElements.Count > 1 )
                throw new Exception( "More than one element was found" );

            throw new Exception( "No element was found" );
        }
    }
}