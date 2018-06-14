using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Https;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Services
{
    public class SearchService : ISearchService
    {
        public SearchService()
        {
        }
        public async Task<ResponseMessage> GetSearchAsync(int position, string keyWords, int pageIndex = 1, int pageSize = 20)
        {
            var url = "";
            switch (position)
            {
                case 0:
                    url = string.Format(Apis.Search, "bolgs", keyWords, pageIndex, pageSize);
                    break;
                case 1:
                    url = string.Format(Apis.Search, "news", keyWords, pageIndex, pageSize);
                    break;
                case 2:
                    url = string.Format(Apis.Search, "kb", keyWords, pageIndex, pageSize);
                    break;
                case 3:
                    url = string.Format(Apis.Search, "question", keyWords, pageIndex, pageSize);
                    break;
            }
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
