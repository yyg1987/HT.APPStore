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
    public class NewsService : INewsService
    {
        private int pageSize = 10;
        public NewsService()
        {
        }
        public async Task<ResponseMessage> GetNewsAsync(int position, int pageIndex = 1)
        {
            var url = "";
            switch (position)
            {
                case 1:
                    url = string.Format(Apis.NewsRecommend, pageIndex, pageSize);
                    break;
                case 2:
                    url = string.Format(Apis.NewsWorkHot, pageIndex, pageSize);
                    break;
                default:
                    url = string.Format(Apis.NewsHome, pageIndex, pageSize);
                    break;
            }
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
