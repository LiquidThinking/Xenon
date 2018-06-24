using System;
using System.Collections.Generic;
using System.Linq;

namespace Xenon
{
	public class XenonElementsSearchResult
	{
		private readonly string _searchCriteria;
		public List<IXenonElement> Elements { get; }

		internal XenonElementsSearchResult( List<IXenonElement> elements, IEnumerable<string> searchCriteria )
		{
			_searchCriteria = string.Join( ", ", searchCriteria );
			Elements = elements;
		}

		internal XenonElementsSearchResult( List<IXenonElement> elements, string searchCriteria, params string[] additionalSearchCriteria )
		{
			_searchCriteria = string.Join( ", ", new List<string>( additionalSearchCriteria )
			{
				searchCriteria
			} );
			Elements = elements;
		}

		internal IXenonElement LocateFirstVisibleElement()
		{
			ValidateResults();
			return Elements.First( x => x.IsVisible ).ScrollToElement();
		}

		internal IXenonElement LocateSingleVisibleElement()
		{
			ValidateResults();

			var foundElements = Elements
				.Where( x => x.IsVisible )
				.ToList();

			if ( foundElements.Count == 1 )
				return foundElements.First().ScrollToElement();

			if ( foundElements.Count > 1 )
				throw new Exception( "More than one element was found" );

			throw new Exception( "No element was found" );
		}

		private void ValidateResults()
		{
			if ( !Elements.Any() )
				throw new NoElementsFoundException( _searchCriteria );
		}
	}
}