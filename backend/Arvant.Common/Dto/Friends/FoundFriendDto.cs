namespace Arvant.Common.Dto.Friends;

public record FoundFriendDto (
    Guid Id,
    string Name,
    string AvatarUrl,
    int MutualFriendsCount
);
