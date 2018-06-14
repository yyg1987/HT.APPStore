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
    public class KbArticlesDetailsService : IKbArticlesDetailsService
    {
        public KbArticlesDetailsService()
        {
        }
        public async Task<ResponseMessage> GetKbArticlesAsync(int id)
        {
            var url = string.Format(Apis.KbArticlesBody, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
