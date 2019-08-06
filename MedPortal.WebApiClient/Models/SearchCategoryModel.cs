using System.Collections.Generic;

namespace MedPortal.WebApiClient.Models
{
    public class SearchCategoryModel
    {
        public SearchCategoryModel(SearchCategoryEnum type)
        {
            Type = type;
            Items = new List<ISearchItemModel>();
        }

        public SearchCategoryEnum Type { get; }
        public List<ISearchItemModel> Items { get;  }
    }
}
