using AutoMapper;
using Instant.Client.Core.Services;
using Instant.Client.WPF.Commands;
using Instant.Client.WPF.Enums;
using Instant.Client.WPF.Services.Abstract;
using Instant.Client.WPF.ViewModels;
using Instant.Client.WPF.ViewModels.ChatSettingsDialog;
using Instant.Client.WPF.Windows;
using Ninject;
using System;
using System.Windows;

namespace Instant.Client.WPF.Services.Implementation
{
    public class ViewModelFactoryService : IViewModelFactoryService
    {
        private readonly IKernel ninjectKernel;
        private readonly ICommandFactoryService commandFactoryService;

        public ViewModelFactoryService(
            IKernel ninjectKernel,
            ICommandFactoryService commandFactoryService)
        {
            this.ninjectKernel = ninjectKernel;
            this.commandFactoryService = commandFactoryService;
        }

        public object CreateViewModel(ViewTypes viewType)
        {
            object viewModel;
            switch (viewType)
            {
                case ViewTypes.SignIn:
                    viewModel = CreateSignInViewModel();
                    break;
                case ViewTypes.SignUp:
                    viewModel = CreateSignUpViewModel();
                    break;
                case ViewTypes.Main:
                    viewModel = CreateMainViewModel();
                    break;
                default:
                    throw new ArgumentException($"Unknown view type requested: {viewType}");
            }

            return viewModel;
        }

        public object CreateDialogViewModel(ChatSettingsDialogViewTypes viewType, Window dialogWindow)
        {
            object dialogViewModel;
            switch (viewType)
            {
                case ChatSettingsDialogViewTypes.CreateChat:
                    dialogViewModel = CreateChatCreationViewModel(dialogWindow);
                    break;
                case ChatSettingsDialogViewTypes.UpdateChat:
                    dialogViewModel = CreateUpdateChatViewModel(dialogWindow);
                    break;
                default:
                    throw new ArgumentException($"Unknown dialog view type requested: {viewType}");
            }

            return dialogViewModel;
        }

        private AuthorizationViewModel CreateSignInViewModel()
        {
            var signInCommand = commandFactoryService.CreateCommand(CommandTypes.SignInCommand);
            var switchViewCommand = commandFactoryService.CreateCommand(CommandTypes.SwitchViewCommand);
            ((SwitchViewCommand)switchViewCommand).TargetViewType = ViewTypes.SignUp;

            var viewModel = new AuthorizationViewModel(
                AuthorizationTypes.SignIn,
                signInCommand,
                switchViewCommand);

            return viewModel;
        }

        private AuthorizationViewModel CreateSignUpViewModel()
        {
            var signInCommand = commandFactoryService.CreateCommand(CommandTypes.SignUpCommand);
            var switchViewCommand = commandFactoryService.CreateCommand(CommandTypes.SwitchViewCommand);
            ((SwitchViewCommand)switchViewCommand).TargetViewType = ViewTypes.SignIn;

            var viewModel = new AuthorizationViewModel(
                AuthorizationTypes.SignUp,
                signInCommand,
                switchViewCommand);

            return viewModel;
        }

        private MainViewModel CreateMainViewModel()
        {
            var sendMessageCommand = commandFactoryService.CreateCommand(CommandTypes.SendMessageCommand);
            var showChatCreationDialogCommand = commandFactoryService.CreateCommand(CommandTypes.ShowChatCreationDialogCommand);
            ((ShowChatCreationDialogCommand) showChatCreationDialogCommand).ViewModelFactoryService = this;
            var switchChatCommand = commandFactoryService.CreateCommand(CommandTypes.SwitchChatCommand);
            var signOutCommand = commandFactoryService.CreateCommand(CommandTypes.SignOutCommand);

            var viewModel = new MainViewModel(
                sendMessageCommand,
                showChatCreationDialogCommand,
                switchChatCommand,
                signOutCommand,
                this.ninjectKernel.Get<IChatService>(),
                this.ninjectKernel.Get<IUserService>(),
                this.ninjectKernel.Get<IMapper>());

            return viewModel;
        }

        private CreateChatViewModel CreateChatCreationViewModel(Window dialogWindow)
        {
            var cancelChatCreationCommand = commandFactoryService.CreateCommand(CommandTypes.CloseWindowCommand);
            ((CloseWindowCommand) cancelChatCreationCommand).Window = dialogWindow;

            var createChatCommand = commandFactoryService.CreateCommand(CommandTypes.CreateChatCommand);
            ((CreateChatCommand) createChatCommand).Window = (ChatSettingsDialogWindow) dialogWindow;

            var addChatMemberToGroupListCommand = commandFactoryService.CreateCommand(CommandTypes.AddChatMemberToGroupListCommand);

            var removeChatMemberFromGroupListCommand = commandFactoryService.CreateCommand(CommandTypes.RemoveChatMemberFromGroupListCommand);

            var viewModel = new CreateChatViewModel(
                cancelChatCreationCommand,
                addChatMemberToGroupListCommand,
                createChatCommand,
                removeChatMemberFromGroupListCommand);

            return viewModel;
        }

        private UpdateChatViewModel CreateUpdateChatViewModel(Window dialogWindow)
        {
            var cancelChatCreationCommand = commandFactoryService.CreateCommand(CommandTypes.CloseWindowCommand);
            ((CloseWindowCommand)cancelChatCreationCommand).Window = dialogWindow;

            var addChatMemberToGroupListCommand = commandFactoryService.CreateCommand(CommandTypes.AddChatMemberToGroupListCommand);

            var updateChatCommand = commandFactoryService.CreateCommand(CommandTypes.UpdateChatComamnd);
            ((UpdateChatCommand)updateChatCommand).Window = dialogWindow;

            var promoteChatMemberCommand = commandFactoryService.CreateCommand(CommandTypes.PromoteChatMemberCommand);

            var deleteChatCommand = commandFactoryService.CreateCommand(CommandTypes.DeleteChatCommand);
            ((DeleteChatCommand)deleteChatCommand).Window = dialogWindow;

            var viewModel = new UpdateChatViewModel(
                cancelChatCreationCommand,
                addChatMemberToGroupListCommand,
                updateChatCommand,
                promoteChatMemberCommand,
                deleteChatCommand);

            return viewModel;
        }
    }
}
