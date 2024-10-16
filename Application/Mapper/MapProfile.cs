using Application.DTO.User;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Foods;
using Application.Response.Food;

namespace Application.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //CreateMap<RoleDto, Role>().ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            //.ForMember(dest => dest.Id, opt => opt.Ignore()); 
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<string, Role>()
                .ConstructUsing(name => new Role { Name = name, Id = Guid.NewGuid().ToString() });

            CreateMap<UserRegisterRequest, ApplicationUser>();
            CreateMap<FoodModel, GetProductResponse>();
            CreateMap<GetProductResponse,FoodModel>();

        }
    }
}
