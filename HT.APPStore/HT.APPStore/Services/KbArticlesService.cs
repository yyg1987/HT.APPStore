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
    public class KbArticlesService : IKbArticlesService
    {
        private int pageSize = 10;
        public KbArticlesService()
        {
        }
        public async Task<ResponseMessage> GetKbArticlesAsync(int pageIndex = 1)
        {
            var url = string.Format(Apis.KbArticles, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
