using System;
using System.ServiceModel;
using System.Threading.Tasks;
using AutoMapper;
using Instant.Client.Core.Implementation.InstantServer.ChatService;
using DomainModels = Instant.Client.Core.Models;
using Instant.Client.Core.Services;
using DataContracts = Instant.Client.Core.Implementation.InstantServer.ChatService;

namespace Instant.Client.Core.Implementation.Services
{
    public class ServerChatService : IServerChatService
    {
        private ChatServiceClient chatServiceClient;

        private DataContractObjectsFactory dataContractObjectsFactory;
        private readonly IChatServiceCallback chatServiceCallback;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public bool IsConnected 
        {
            get => this.chatServiceClient != null;
            private set
            {
                this.chatServiceClient = null;
            }
        }

        public ServerChatService(
            DataContractObjectsFactory dataContractObjectsFactory,
            IChatServiceCallback chatServiceCallback,
            IUserService userService,
            IMapper mapper)
        {
            this.dataContractObjectsFactory = dataContractObjectsFactory;
            this.chatServiceCallback = chatServiceCallback;
            this.userService = userService;
            this.mapper = mapper;
        }
        
        private void Connect()
        {
            if (!this.IsConnected)
            {
                var context = new InstanceContext(this.chatServiceCallback);
                this.chatServiceClient = new ChatServiceClient(context);
            }
        }

        public async Task<DomainModels.UserCredentials> SignIn(string login, string password)
        {
            if (!this.IsConnected)
            {
                Connect();
            }

            var clientUserData = this.userService.GetCurrentClientUserData();
            DateTime lastOnline = clientUserData?.LastOnline ?? default;

            var options = this.dataContractObjectsFactory.CreateAuthorizeUserOptions(login, password, lastOnline);
            var response = await this.chatServiceClient.SignInAsync(options);

            if (response.OperationResultType != OperationResultTypes.Success)
            {
                throw new OperationCanceledException($"Sign In operation failed with status {response.OperationResultType}. " +
                    $"Message: {response.Message}");
            }

            var userData = (DataContracts.User) response.ExtraData;
            clientUserData = mapper.Map<DomainModels.UserCredentials>(userData);

            return clientUserData;
        }

        public async Task SignUp(string login, string password, string confirmedPassword)
        {
            if (!this.IsConnected)
            {
                Connect();
            }

            var options = this.dataContractObjectsFactory.CreateAuthorizeUserOptions(login, password);
            var response = await this.chatServiceClient.SignUpAsync(options);

            if (response.OperationResultType != OperationResultTypes.Success)
            {
                throw new OperationCanceledException($"Sign Up operation failed with status {response.OperationResultType}. " +
                    $"Message: {response.Message}");
            }
        }

        public async Task SignOut(string login, string password)
        {
            if (this.IsConnected)
            {
                var options = this.dataContractObjectsFactory.CreateAuthorizeUserOptions(login, password);
                await this.chatServiceClient.DisconnectAsync(options);
                this.IsConnected = false;
            }
        }

        public async Task SendMessage(DomainModels.ChatMessage message)
        {
            var options = this.mapper.Map<SendMessageOptions>(message);
            var response = await this.chatServiceClient.SendMessageAsync(options);

            if (response.OperationResultType != OperationResultTypes.Success)
            {
                throw new OperationCanceledException($"Send Message operation failed with status {response.OperationResultType}. " +
                    $"Message: {response.Message}");
            }
        }

        public async Task CreateOrUpdateChat(DomainModels.Chat chat)
        {
            try
            {
                var options = this.mapper.Map<CreateOrUpdateChatOptions>(chat);
                var clientUserData = this.userService.GetCurrentClientUserData();
                options.InitiatorLogin = clientUserData.Login;

                var response = await this.chatServiceClient.CreateOrUpdateChatAsync(options);

                if (response.OperationResultType != OperationResultTypes.Success)
                {
                    throw new OperationCanceledException($"Create or update chat operation failed with status {response.OperationResultType}. " +
                        $"Message: {response.Message}");
                }
            }
            catch (Exception exc)
            {

            }
        }

        public Task DeleteChat(int chatId, string initiatorLogin)
        {
            var options = this.dataContractObjectsFactory.CreateDeleteChatOptions(chatId, initiatorLogin);
            return this.chatServiceClient.DeleteChatAsync(options);
        }

        public async Task<DomainModels.User> GetUserData(string userLogin)
        {
            var operationResult = await this.chatServiceClient.GetUserDataAsync(userLogin);
            var domainUser = this.mapper.Map<DomainModels.User>((DataContracts.User) operationResult.ExtraData);
            return domainUser;
        }

        public Task PromoteChatMember(
            string userLogin, 
            int chatId, 
            DomainModels.Enums.ChatPermissionTypes chatPermission, 
            string initiatorLogin)
        {
            var options = this.dataContractObjectsFactory.CreateAddOrUpdateChatPermissionOptions(
                userLogin,
                chatId,
                (ChatPermissionTypes) chatPermission,
                initiatorLogin);

            return this.chatServiceClient.UpdateUserPermissionInChatAsync(options);
        }
    }
}