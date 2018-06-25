using System;
using System.Collections.Generic;
using System.Linq;

namespace Xenon
{
	public class XenonElementsSearchResult : List<IXenonElement>
	{
		private readonly string _searchCriteria;

		internal XenonElementsSearchResult( IEnumerable<IXenonElement> elements, params string[] searchCriteria )
			: base( elements )
		{
			_searchCriteria = string.Join( ", ", new List<string>( searchCriteria ) );
		}

		internal IXenonElement LocateFirstVisibleElement()
		{
			ValidateResults();
			return this.First( x => x.IsVisible ).ScrollToElement();
		}

		internal IXenonElement LocateSingleVisibleElement()
		{
			ValidateResults();

			var foundElements = this
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
			if ( !this.Any() )
				throw new NoElementsFoundException( _searchCriteria );
		}

		public override string ToString() => _searchCriteria;
	}
}