using Aurochses.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;

namespace Aurochses.IdentityServer.Database.Data.Mappings
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUser>()
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmailConfirmed, opts => opts.MapFrom(src => src.EmailConfirmed))
                .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
}