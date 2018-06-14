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
    public class BlogsService : IBlogsService
    {
        public BlogsService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(string blogApp, int pageIndex = 1, int pageSize = 20)
        {
            var url = string.Format(Apis.BlogPosts, blogApp, pageIndex, pageSize);
            return await UserHttpClient.Current.GetAsyn(url);
        }
    }
}
