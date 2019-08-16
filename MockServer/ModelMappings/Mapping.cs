using AutoMapper;
using MockServer.Entities;
using MockServer.Models;
using Niusys.Extensions.ComponentModels;
using Niusys.Extensions.Storage.Mongo;

namespace MockServer.ModelMappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap(typeof(Page<>), typeof(Page<>));

            CreateMap<ApiInterface, ApiInterfaceListItem>()
                .ForMember(d => d.InterfaceId, mo => mo.MapFrom(s => s.Sysid.ToString()));

            CreateMap<ApiInterfaceCreateModel, ApiInterface>()
                .ForMember(d => d.RequestPath, mo => mo.MapFrom(s => s.RequestPath.Trim()));

            CreateMap<ApiInterfaceModifyModel, ApiInterface>()
                .ForMember(d => d.Sysid, mo => mo.MapFrom(s => s.InterfaceId.SafeToObjectId()))
                .ForMember(d => d.RequestPath, mo => mo.MapFrom(s => s.RequestPath.Trim()));

            CreateMap<ApiInterface, ApiInterfaceModifyModel>()
                .ForMember(d => d.InterfaceId, mo => mo.MapFrom(s => s.Sysid.ToString()));

            CreateMap<ApiInterface, ApiInterfaceTestModel>()
                .ForMember(d => d.InterfaceId, mo => mo.MapFrom(s => s.Sysid.ToString()))
                .ForMember(d => d.RequestPath, mo => mo.MapFrom(s => $"/{s.Category}{s.RequestPath}"))
                .ForMember(d => d.ResponseResult, mo => mo.MapFrom<EnvelopMessageWrapperResolver, string>(x => x.ResponseResult));
        }
    }
}
