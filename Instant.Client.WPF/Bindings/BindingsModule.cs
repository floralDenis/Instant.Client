using AutoMapper;
using Instant.Client.Core.Implementation.InstantServer.ChatService;
using Instant.Client.Core.Implementation.MappingProfiles;
using Instant.Client.Core.Implementation.Services;
using Instant.Client.Core.Implementation.Services.Callbacks;
using Instant.Client.Core.Services;
using Instant.Client.WPF.MappingProfiles;
using Ninject;
using Ninject.Modules;

namespace Instant.Client.WPF.Bindings
{
    public class BindingsModule : NinjectModule
    {
        public override void Load()
        {
            ConfigureServiceBindings();
            ConfigureBindingForAutoMapper();
        }

        private void ConfigureServiceBindings()
        {
            Bind<Core.Services.IChatService>().To<Core.Implementation.Services.ChatService>();
            Bind<IUserService>().To<UserService>();
            Bind<IStorageService>().To<StorageService>();
            Bind<IChatMessageService>().To<ChatMessageService>();
            Bind<IServerChatService>().To<ServerChatService>().InSingletonScope();
            Bind<IChatServiceCallback>().To<ChatServiceCallback>().InSingletonScope();
            Bind<DataContractObjectsFactory>().ToSelf().InSingletonScope();
        }

        private void ConfigureBindingForAutoMapper()
        {
            var mapperConfiguration = GetAutoMapperConfiguration();
            Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();

            Bind<IMapper>().ToMethod(ctx =>
                new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));
        }

        private MapperConfiguration GetAutoMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ViewModelDomainProfile>();
                cfg.AddProfile<CommunicationDomanProfile>();
            });

            return config;
        }
    }
}
