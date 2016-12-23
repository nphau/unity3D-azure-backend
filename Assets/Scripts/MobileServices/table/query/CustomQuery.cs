namespace Unity3dAzure.MobileServices
{
    public class CustomQuery
    {
        private string _filter;
        private string _orderBy;
		private string _top= null;

        public CustomQuery(string filter, string orderBy=null)
        {
            _filter = filter;
            _orderBy = orderBy;
        }

		public CustomQuery(string filter, string orderBy=null, string top = null)
		{
			_filter = filter;
			_orderBy = orderBy;
			_top = top;
		}

        public override string ToString()
        {
            string queryString = "";
            string q = "?";
            if (!string.IsNullOrEmpty(_filter)){
                queryString += string.Format("{0}$filter=({1})", q, escape(_filter));
                q = "&";
            }
            if (!string.IsNullOrEmpty(_orderBy)){
                queryString += string.Format("{0}$orderby={1}", q, escape(_orderBy));
                q = "&";
            }
			if (!string.IsNullOrEmpty(_top)){
				queryString += string.Format("{0}$top={1}", q, escape(_top));
				q = "&";
			}
            return queryString;
        }
        
        public string escape(string s)
        {
            return s.Replace(" ","%20");
        }
        
    }
}