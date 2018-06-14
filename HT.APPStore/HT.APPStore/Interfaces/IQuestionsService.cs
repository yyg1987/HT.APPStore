using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IQuestionsService
    {
        Task<ResponseMessage> GetQuestionsAsync(int position, int pageIndex = 1, int pageSize = 20);
        Task<ResponseMessage> EditQuestionsAsync(Questions questions);
    }
}
