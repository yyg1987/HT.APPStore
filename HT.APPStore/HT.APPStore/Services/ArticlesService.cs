using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Https;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HT.APPStore.Services
{
    public class ArticlesService : IArticlesService
    {
        public ArticlesService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(int position, int pageIndex = 1, int pageSize = 20)
        {
            var url = "";
            switch (position)
            {
                case 1:
                    url = string.Format(Apis.ArticleHot, pageIndex, pageSize);
                    break;
                default:
                    url = string.Format(Apis.ArticleHome, pageIndex, pageSize);
                    break;
            }
            try
            {
                return await TokenHttpClient.Current.GetAsyn(url);
            }
            catch (System.Exception ex)
            {
                var result = new ResponseMessage();
                result.Success = false;
                result.Message = ex.Message;
                DependencyService.Get<ILog>().SendLog("ArticlesService.GetArticlesAsync:" + ex.Message);
                return result;
            }
        }
    }
}
