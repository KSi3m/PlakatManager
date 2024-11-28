using AutoMapper;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;

namespace ElectionMaterialManager.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateElectionItemCommand, LED>()
                 .ForMember(x => x.Tags, opt => opt.Ignore());
            CreateMap<CreateElectionItemCommand, Poster>()
                 .ForMember(x => x.Tags, opt => opt.Ignore());
            CreateMap<CreateElectionItemCommand, Billboard>()
                 .ForMember(x => x.Tags, opt => opt.Ignore());

            CreateMap<CreateBillboardCommand, Billboard>()
                .ForMember(x => x.Tags, opt => opt.Ignore());
                //.ForMember(x => x.Author, opt => opt.Ignore());
            CreateMap<CreatePosterCommand, Poster>()
                 .ForMember(x => x.Tags, opt => opt.Ignore());
            CreateMap<CreateLEDCommand, LED>()
                 .ForMember(x => x.Tags, opt => opt.Ignore());

            CreateMap<ElectionItem, ElectionItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
                
            CreateMap<ElectionItem, ElectionItemDetailDto>()
                    .IncludeBase<ElectionItem, ElectionItemDto>();

            CreateMap<Billboard, ElectionItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                //.ForSourceMember(src => src.Author, opt => opt.DoNotValidate());
            ;
             
            CreateMap<Billboard, ElectionItemDetailDto>()
                .IncludeBase<Billboard, ElectionItemDto>();

            CreateMap<Poster, ElectionItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<Poster, ElectionItemDetailDto>()
                .IncludeBase<Poster, ElectionItemDto>();

            CreateMap<LED, ElectionItemDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<LED, ElectionItemDetailDto>()
                .IncludeBase<LED, ElectionItemDto>();


          
       


            CreateMap<Tag, TagDto>();
            CreateMap<User, AuthorDto>();
            CreateMap<Comment, CommentDto>();

            MapsForEditElectionItemCommand();

        }


        public void MapsForEditElectionItemCommand()
        {
            CreateMap<EditElectionItemCommand, ElectionItem>()
           /*.ForAllMembers(opts => {
             * opts.Condition((src, dest, srcMember) => srcMember != null);
            });// nie działa z jakiegos powodu */
           .ForMember(dest => dest.Area, opt => opt.Condition(src => src.Area != null))
           .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority != null))
           .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size != null))
           .ForMember(dest => dest.Cost, opt => opt.Condition(src => src.Cost != null))
           .ForMember(dest => dest.StatusId, opt => opt.Condition(src => src.StatusId != null))
    
           .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
           .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Longitude != null));


            CreateMap<EditElectionItemCommand, Poster>()
                .ForMember(dest => dest.Area, opt => opt.Condition(src => src.Area != null))
                .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority != null))
                .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size != null))
                .ForMember(dest => dest.Cost, opt => opt.Condition(src => src.Cost != null))
                .ForMember(dest => dest.StatusId, opt => opt.Condition(src => src.StatusId != null))
              
                .ForMember(dest => dest.PaperType, opt => opt.Condition(src => src.PaperType != null))
                .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
                .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Longitude != null));

            CreateMap<EditElectionItemCommand, LED>()
             .ForMember(dest => dest.Area, opt => opt.Condition(src => src.Area != null))
            .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority != null))
            .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size != null))
            .ForMember(dest => dest.Cost, opt => opt.Condition(src => src.Cost != null))
            .ForMember(dest => dest.StatusId, opt => opt.Condition(src => src.StatusId != null))
            .ForMember(dest => dest.RefreshRate, opt => opt.Condition(src => src.RefreshRate != null))
            .ForMember(dest => dest.Resolution, opt => opt.Condition(src => src.Area != null))
            .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
            .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Longitude != null));

            CreateMap<EditElectionItemCommand, Billboard>()
              .ForMember(dest => dest.Area, opt => opt.Condition(src => src.Area != null))
            .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority != null))
            .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size != null))
            .ForMember(dest => dest.Cost, opt => opt.Condition(src => src.Cost != null))
            .ForMember(dest => dest.StatusId, opt => opt.Condition(src => src.StatusId != null))
            .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.Area != null))
            .ForMember(dest => dest.EndDate, opt => opt.Condition(src => src.Area != null))
            .ForMember(dest => dest.Longitude, opt => opt.Condition(src => src.Longitude != null))
            .ForMember(dest => dest.Latitude, opt => opt.Condition(src => src.Longitude != null));
        }
    }
}
