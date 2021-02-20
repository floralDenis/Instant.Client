using AutoMapper;
using Instant.Client.Core.Services;
using Instant.Client.WPF.Commands;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.Services.Abstract;
using Instant.Client.WPF.ViewModels;
using Ninject;
using System;
using System.Windows.Input;

namespace Instant.Client.WPF.Services.Implementation
{
    public class CommandFactoryService : ICommandFactoryService
    {
        private readonly IKernel ninjectKernel;

        private readonly MainWindowViewModel mainWindowViewModel;

        public CommandFactoryService(
            IKernel ninjectKernel,
            MainWindowViewModel mainWindowViewModel)
        {
            this.ninjectKernel = ninjectKernel;
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public ICommand CreateCommand(CommandTypes commandType)
        {
            ICommand command;
            switch (commandType)
            {
                case CommandTypes.SignInCommand:
                    command = CreateSignInCommand();
                    break;
                case CommandTypes.SignUpCommand:
                    command = CreateSignUpCommand();
                    break;
                case CommandTypes.SwitchViewCommand:
                    command = CreateSwitchViewCommand();
                    break;
                case CommandTypes.SendMessageCommand:
                    command = CreateSendMessageCommand();
                    break;
                case CommandTypes.CloseWindowCommand:
                    command = CreateCloseWindowCommand();
                    break;
                case CommandTypes.ShowChatCreationDialogCommand:
                    command = CreateShowChatCreationDialogCommand();
                    break;
                case CommandTypes.CreateChatCommand:
                    command = CreateChatCreationCommand();
                    break;
                case CommandTypes.AddChatMemberToGroupListCommand:
                    command = CreateAddChatMemberToGroupListCommand();
                    break;
                case CommandTypes.RemoveChatMemberFromGroupListCommand:
                    command = CreateRemoveChatMemberFromGroupListCommand();
                    break;
                case CommandTypes.SwitchChatCommand:
                    command = CreateSwitchChatCommand();
                    break;
                case CommandTypes.UpdateLastOnlineCommand:
                    command = CreateUpdateLastOnlineCommand();
                    break;
                case CommandTypes.SignOutCommand:
                    command = CreateSignOutCommand();
                    break;
                case CommandTypes.UpdateChatComamnd:
                    command = CreateUpdateChatCommand();
                    break;
                case CommandTypes.PromoteChatMemberCommand:
                    command = CreatePromoteChatMemberCommand();
                    break;
                case CommandTypes.DeleteChatCommand:
                    command = CreateDeleteChatCommand(); 
                    break;
                default:
                    throw new ArgumentException($"Unknown command type requested: {commandType}");
            }

            return command;
        }

        private ICommand CreateSignInCommand()
        {
            var command = new SignInCommand(
                this.mainWindowViewModel,
                this.ninjectKernel.Get<IUserService>(),
                this.ninjectKernel.Get<IServerChatService>());

            return command;
        }

        private ICommand CreateSignUpCommand()
        {
            var command = new SignUpCommand(
                this.mainWindowViewModel,
                this.ninjectKernel.Get<IServerChatService>());

            return command;
        }

        private ICommand CreateSwitchViewCommand()
        {
            var command = new SwitchViewCommand(
                this.mainWindowViewModel);

            return command;
        }

        private ICommand CreateSendMessageCommand()
        {
            var command = new SendMessageCommand(
                this.ninjectKernel.Get<IServerChatService>());

            return command;
        }

        private ICommand CreateShowChatCreationDialogCommand()
        {
            var command = new ShowChatCreationDialogCommand();

            return command;
        }

        private ICommand CreateChatCreationCommand()
        {
            var command = new CreateChatCommand(
                this.ninjectKernel.Get<IServerChatService>());

            return command;
        }

        private ICommand CreateCloseWindowCommand()
        {
            var command = new CloseWindowCommand();
            return command;
        }

        private ICommand CreateAddChatMemberToGroupListCommand()
        {
            var command = new AddChatMemberToGroupListCommand();
            return command;
        }

        private ICommand CreateRemoveChatMemberFromGroupListCommand()
        {
            var command = new RemoveChatMemberFromGroupListCommand();
            return command;
        }

        private ICommand CreateSwitchChatCommand()
        {
            var command = new SwitchChatCommand(
                this.ninjectKernel.Get<IChatService>(),
                this.ninjectKernel.Get<IUserService>(),
                this.ninjectKernel.Get<IServerChatService>(),
                this.ninjectKernel.Get<IMapper>());
            return command;
        }

        private ICommand CreateUpdateLastOnlineCommand()
        {
            var command = new UpdateLastOnlineCommand(
                this.ninjectKernel.Get<IUserService>());
            return command;
        }

        private ICommand CreateSignOutCommand()
        {
            var command = new SignOutCommand(
                this.ninjectKernel.Get<IUserService>(),
                this.ninjectKernel.Get<IServerChatService>(),
                this.mainWindowViewModel);
            return command;
        }

        private ICommand CreateUpdateChatCommand()
        {
            var command = new UpdateChatCommand(this.ninjectKernel.Get<IServerChatService>());
            return command;
        }

        private ICommand CreatePromoteChatMemberCommand()
        {
            var command = new PromoteChatMemberCommand(this.ninjectKernel.Get<IServerChatService>());
            return command;
        }

        private ICommand CreateDeleteChatCommand()
        {
            var command = new DeleteChatCommand(this.ninjectKernel.Get<IServerChatService>());
            return command;
        }
    }
}
