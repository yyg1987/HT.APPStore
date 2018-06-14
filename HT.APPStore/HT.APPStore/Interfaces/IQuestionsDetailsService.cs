using HT.APPStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HT.APPStore.Interfaces
{
    public interface IQuestionsDetailsService
    {
        Task<ResponseMessage> GetQuestionsAsync(int id);
        Task<ResponseMessage> GetAnswersAsync(int id, int pageIndex, int pageSize);
        Task<ResponseMessage> PostAnswerAsync(int id, string content);
        Task<ResponseMessage> EditAnswerAsync(int questionId, int answerId, int userId, string content);
        Task<ResponseMessage> DeleteAnswerAsync(int questionId, int answerId);
    }
}
