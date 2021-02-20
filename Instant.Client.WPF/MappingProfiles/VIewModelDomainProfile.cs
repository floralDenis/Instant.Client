using AutoMapper;
using Instant.Client.Core.Implementation.InstantServer.ChatService;
using Instant.Client.Core.Models;
using Instant.Client.WPF.Models;
using Instant.Client.WPF.ViewModels;

namespace Instant.Client.WPF.MappingProfiles
{
    public class ViewModelDomainProfile : Profile
    {
        public ViewModelDomainProfile()
        {
            CreateModelsMapping();
        }

        private void CreateModelsMapping()
        {
            //CreateMap<User, Author>()
            //    .ConstructUsing(u => new Author(u.Name))
            //    .ForMember(dst => dst.Data, options => options.MapFrom(src => new UserAddiotionalInfo(src.UserId, src.Bio)));

            //CreateMap<ClientUserData, Author>()
            //    .ConstructUsing(cud => new Author(cud.Name))
            //    .ForMember(dst => dst.Data, options => options.MapFrom(src => new UserAddiotionalInfo(src.UserId, src.Bio)));

            CreateMap<Chat, ChatViewModel>().ReverseMap();
            CreateMap<Core.Models.ChatMessage, ChatMessageModel>();
            CreateMap<Core.Models.User, ChatMemberModel>()
                .ConstructUsing(user => new ChatMemberModel(user.Login));
            CreateMap<Core.Models.UserCredentials, ChatMemberModel>()
                .ConstructUsing(user => new ChatMemberModel(user.Login));
        }
    }
}
