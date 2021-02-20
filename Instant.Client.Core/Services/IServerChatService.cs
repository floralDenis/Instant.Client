using Instant.Client.Core.Models;
using Instant.Client.Core.Models.Enums;
using System.Threading.Tasks;

namespace Instant.Client.Core.Services
{
    public interface IServerChatService
    {
        Task<UserCredentials> SignIn(string login, string password);
        Task SignUp(string login, string password, string confirmedPassword);
        Task SignOut(string login, string password);
        Task CreateOrUpdateChat(Chat chat);
        Task DeleteChat(int chatId, string initiatorLogin);
        Task SendMessage(ChatMessage message);
        Task<User> GetUserData(string userLogin);
        Task PromoteChatMember(string userLogin, int chatId, ChatPermissionTypes chatPermission, string initiatorLogin);
    }
}