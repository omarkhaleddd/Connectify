import { Likes } from "./like";
import { Post } from "./post.model";

export interface Repost {
    id: number;
    content: string;
    likes: Likes[];
    likeCount: number;
    datePosted: Date;
    comments?: Comment[];
    authorId: string;
    authorName: string;
    postId: number;
    post: Post;
}