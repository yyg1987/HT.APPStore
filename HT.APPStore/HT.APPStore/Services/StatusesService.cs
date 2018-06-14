using HT.APPStore.Helpers;
using HT.APPStore.Helpers.Https;
using HT.APPStore.Interfaces;
using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Services
{
    public class StatusesService : IStatusesService
    {
        public StatusesService()
        {
        }
        public async Task<ResponseMessage> GetStatusesAsync(int position, int pageIndex = 1, int pageSize = 20)
        {
            string statusType = "all";
            switch (position)
            {
                case 0:
                    statusType = "all";
                    break;
                case 1:
                    statusType = "following";
                    break;
                case 2:
                    statusType = "my";
                    break;
                case 3:
                    statusType = "mycomment";
                    break;
                case 4:
                    statusType = "recentcomment";
                    break;
                case 5:
                    statusType = "mention";
                    break;
                case 6:
                    statusType = "comment";
                    break;
                default:
                    statusType = "all";
                    break;
            }
            var url = string.Format(Apis.Status, statusType, pageIndex, pageSize);
            if (position > 0)
            {
                return await UserHttpClient.Current.GetAsyn(url);
            }
            else
            {
                return await TokenHttpClient.Current.GetAsyn(url);
            }
        }
        public async Task<ResponseMessage> EditStatusesAsync(Statuses statuses)
        {
            var url = string.Format(Apis.StatusADD);
            var parameters = new Dictionary<string, string>();
            parameters.Add("IsPrivate", "false");
            parameters.Add("Content", statuses.Content);

            return await UserHttpClient.Current.PostAsync(url, new FormUrlEncodedContent(parameters));
        }
        public async Task<ResponseMessage> DeleteStatusesAsync(int id)
        {
            var url = string.Format(Apis.StatusDelete, id);
            return await UserHttpClient.Current.DeleteAsync(url);
        }

    }
}
