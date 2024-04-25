﻿using AutoMapper;
using Talabat.APIs.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {

			CreateMap<AddressDto, Address>();
			CreateMap<Address, AddressDto>();

			CreateMap<UserDto, AppUser>();
			CreateMap<AppUser, UserDto>();

		}
    }
}
