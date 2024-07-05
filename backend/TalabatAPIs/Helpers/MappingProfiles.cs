using AutoMapper;
using Connetify.APIs.DTO;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Core;
using Talabat.Core.Entities.Identity;
using Talabat.APIs.DTO;
using Talabat.Core.Entities.Core.Donation;

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

            CreateMap<GroupMessageDto, Message>();
            CreateMap<Message, GroupMessageDto>();

            CreateMap<RepostDto, Repost>();
			CreateMap<Repost, RepostDto>();

			CreateMap<NotificationDto, Notification>();
			CreateMap<Notification, NotificationDto>();

			CreateMap<FriendRequestDto, FriendRequest>();
			CreateMap<FriendRequest, FriendRequestDto>();

			CreateMap<DonationDto, Donation>();
			CreateMap<Donation, DonationDto>();

            CreateMap<ReportedPostDto, ReportedPost>();
            CreateMap<ReportedPost, ReportedPostDto>();

            CreateMap<FileNameDto, FileNames>();
            CreateMap<FileNames, FileNameDto>();
        }
    }
}
