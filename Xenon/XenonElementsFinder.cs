using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenon
{
	public class XenonElementsFinder
	{
		private abstract class BaseXpathCriteria
		{
			private readonly string _format;
			protected readonly string[] _formatArguments;

			public abstract bool CanHandleSearchingAll { get; }

			protected BaseXpathCriteria(string format, params string[] formatArguments )
			{
				_format = format;
				_formatArguments = formatArguments;
			}

			internal virtual string CreateCriteria()
			{
				return string.Format( _format, _formatArguments );
			}

			public virtual string CreateCriteriaForcingSearchAll()
			{
				if ( CanHandleSearchingAll )
					return CreateCriteria();

				return "//*" + CreateCriteria();
			}

			/// <summary>
			/// A summary of what this object was searching for for logging in case of failure
			/// </summary>
			/// <returns></returns>
			public abstract string GetVerboseMessage();
		}

		private class FindByTextXpathCriteria : BaseXpathCriteria
		{
			private const string Format = "(//input[@value='{0}' and (@type='submit' or @type='button' or @type= 'reset' ) ] | //*[normalize-space(text()) = normalize-space('{0}')])";

			public FindByTextXpathCriteria( string text ) : base( Format, text )
			{
			}

			public override bool CanHandleSearchingAll => true;

			public override string GetVerboseMessage() => $"Searching for element(s) with text '{_formatArguments.Single()}'";
		}

		private class FindByContainsTextXpathCriteria : BaseXpathCriteria
		{
			private const string Format = "//*[contains(@value,'{0}')] | //*[contains(normalize-space(text()), normalize-space('{0}'))]";
			public FindByContainsTextXpathCriteria( string text ) : base( Format, text )
			{
			}

			public override bool CanHandleSearchingAll => true;

			public override string GetVerboseMessage() => $"Searching for element(s) containing text {_formatArguments.Single()}";
		}


		private class FindByAttributeXPathCriteria : BaseXpathCriteria
		{
			public FindByAttributeXPathCriteria( string attributeName, string attributeValue ) : base( "[@{0}='{1}']", attributeName, attributeValue )
			{
			}

			public override bool CanHandleSearchingAll => false;

			public override string GetVerboseMessage() => $"Searching for element(s) with attribute '{_formatArguments[0]}' with value '{_formatArguments[1]}'";
		}

		private class FindByCssClassXPathCriteria : BaseXpathCriteria
		{
			public FindByCssClassXPathCriteria( string className) : base( "[contains(@class, '{0}')]", className ) {}

			public override bool CanHandleSearchingAll => false;

			public override string GetVerboseMessage() => $"Searching for element(s) with css class '{_formatArguments.Single()}'";
		}

		private class XPathCriteriaBuilder
		{
			private readonly List<BaseXpathCriteria> _criteria;

			public List<string> SearchCriteria => _criteria.Select( x => x.GetVerboseMessage() ).ToList();

			public XPathCriteriaBuilder()
			{
				_criteria = new List<BaseXpathCriteria>();
			}


			public void AddCriteria<TCriteria>( TCriteria criterion ) where TCriteria : BaseXpathCriteria
			{
				_criteria.Add( criterion );
			}

			public string GenerateCriteria()
			{
				string result = String.Empty;

				if ( _criteria.Count == 1 )
					result = _criteria.First().CreateCriteriaForcingSearchAll();
				else if ( _criteria.Any( x => x.CanHandleSearchingAll ) )
					result = GenerateCriteriaWhereCriterionCanHandleSearchingAll();
				else if ( _criteria.Count( x => x.CanHandleSearchingAll ) == 0 )
					result = CriteriaWhereForcingFirstCriterionToSearchAll();

				return result;
			}

			private string CriteriaWhereForcingFirstCriterionToSearchAll( )
			{
				var criteriaWithCorrectPositions = _criteria.Skip( 1 ).Select( x => x.CreateCriteria() ).ToList();
				criteriaWithCorrectPositions.Insert( 0, _criteria.First().CreateCriteriaForcingSearchAll() );
				var result = String.Join( String.Empty, criteriaWithCorrectPositions );
				return result;
			}

			private string GenerateCriteriaWhereCriterionCanHandleSearchingAll( )
			{
				var criteriaWhichShouldBeFirst = _criteria.First( x => x.CanHandleSearchingAll );
				var criteriaWithCorrectPositions = _criteria.Except( new List<BaseXpathCriteria>
				{
					criteriaWhichShouldBeFirst
				} ).ToList();

				criteriaWithCorrectPositions.Insert( 0, criteriaWhichShouldBeFirst );
				var result = string.Join( string.Empty, criteriaWithCorrectPositions.Select( x => x.CreateCriteria() ) );
				return result;
			}

			public string CriteriaDetails()
			{
				var builder = new StringBuilder();
				foreach ( var criterion in _criteria )
					builder.Append( criterion.CreateCriteria() );
				return builder.ToString();
			}
		}

		private readonly IXenonBrowser _browser;
		private readonly XPathCriteriaBuilder _criteriaBuilder;

		public XenonElementsFinder( IXenonBrowser browser )
		{
			_browser = browser;
			_criteriaBuilder = new XPathCriteriaBuilder();
		}

		public XenonElementsFinder TextIs( string text )
		{
			_criteriaBuilder.AddCriteria( new FindByTextXpathCriteria( text ) );
			return this;
		}

		public XenonElementsFinder AttributeIs( string attributeName, string attributeValue )
		{
			_criteriaBuilder.AddCriteria( new FindByAttributeXPathCriteria( attributeName, attributeValue ) );
			return this;
		}

		public XenonElementsFinder CssClassIs( string className )
		{
			_criteriaBuilder.AddCriteria( new FindByCssClassXPathCriteria( className ) );
			return this;
		}

		public XenonElementsFinder ContainsText( string text )
		{
			_criteriaBuilder.AddCriteria( new FindByContainsTextXpathCriteria( text ) );
			return this;
		}

		internal XenonElementsSearchResult FindElements()
		{	
			var criteria = _criteriaBuilder.GenerateCriteria();
			var elements = _browser.FindElementsByXPath( criteria ).Elements;
			return new XenonElementsSearchResult( elements, _criteriaBuilder.SearchCriteria );
		}

		public string CriteriaDetails() => _criteriaBuilder.CriteriaDetails();
	}
}