using System.Threading.Tasks;

namespace InvestmentManager.QueueMessage
{
    public interface IQueueMessage
    {
        Task<bool> SendMessage(string message);
    }
}
