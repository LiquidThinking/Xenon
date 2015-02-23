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
			private readonly string[] _formatArguments;

			public abstract bool CanHandleSearchingAll { get; }

			public BaseXpathCriteria(string format, params string[] formatArguments )
			{
				_format = format;
				_formatArguments = formatArguments;
			}

			public virtual string CreateCriteria( )
			{
				return String.Format( _format, _formatArguments );
			}

			public virtual string CreateCriteriaForcingSearchAll()
			{
				if ( CanHandleSearchingAll )
					return CreateCriteria();

				return "//*" + CreateCriteria();
			}

		}

		private class FindByTextXpathCriteria : BaseXpathCriteria
		{
			private const string Format = "(//input[@value='{0}' and (@type='submit' or @type='button' or @type= 'reset' ) ] | //*[text() = '{0}'])";

			public FindByTextXpathCriteria( string text ) : base( Format, text ) {}


			public override bool CanHandleSearchingAll
			{
				get { return true; }
			}
		}


		private class FindByAttributeXPathCriteria : BaseXpathCriteria
		{
			public FindByAttributeXPathCriteria( string attributeName, string attributeValue ) : base( "[@{0}='{1}']", attributeName, attributeValue ) {}

			public override bool CanHandleSearchingAll
			{
				get { return false; }
			}
		}

		private class XPathCriteriaBuilder
		{
			private readonly List<BaseXpathCriteria> _criteria;

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
				string result = String.Join( String.Empty, criteriaWithCorrectPositions.Select( x => x.CreateCriteria() ) );
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

		internal IEnumerable<IXenonElement> FindElements()
		{
			string criteria = _criteriaBuilder.GenerateCriteria();
			var result = _browser.FindElementsByXPath( criteria );

			return result;
		}

		public string CriteriaDetails()
		{
			return _criteriaBuilder.CriteriaDetails();
		}

	}
}