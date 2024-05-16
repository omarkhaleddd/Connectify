using AutoMapper;
using Connetify.APIs.DTO;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Core;
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

            CreateMap<AppUserDto, AppUser>();
            CreateMap<AppUser, AppUserDto>();

            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>();

            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();

            CreateMap<PostLikesDto, PostLikes>();
            CreateMap<PostLikes, PostLikesDto>();

            CreateMap<BlockListDto, BlockList>();
            CreateMap<BlockList, BlockListDto>();

            CreateMap<FriendDto, AppUserFriend>();
            CreateMap<AppUserFriend, FriendDto>();

            CreateMap<MessageDto, Message>();
            CreateMap<Message, MessageDto>();

            CreateMap<FriendRequestDto, FriendRequest>();
			CreateMap<FriendRequest, FriendRequestDto>();
		}
    }
}
