using AutoMapper;
using Instant.Client.Core.Implementation.InstantServer.ChatService;
using DomainModels = Instant.Client.Core.Models;
using DataContracts = Instant.Client.Core.Implementation.InstantServer.ChatService;
using Instant.Client.Core.Models;

namespace Instant.Client.Core.Implementation.MappingProfiles
{
    public class CommunicationDomanProfile : Profile
    {
        public CommunicationDomanProfile()
        {
            CreateModelsMapping();
        }

        private void CreateModelsMapping()
        {
            CreateMap<DomainModels.ChatMessage, SendMessageOptions>().ReverseMap();
            CreateMap<DataContracts.User, DomainModels.User>().ReverseMap();
            CreateMap<DataContracts.AddOrUpdateChatPermissionOptions, DomainModels.ChatPermission>().ReverseMap();
            CreateMap<DomainModels.Chat, DataContracts.CreateOrUpdateChatOptions>().ReverseMap();
            CreateMap<DataContracts.User, UserCredentials>().ReverseMap();
            CreateMap<DomainModels.User, UserCredentials>().ReverseMap();
        }
    }
}
