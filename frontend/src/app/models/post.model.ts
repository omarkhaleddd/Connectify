import { Comment } from './comment.model';
import { Likes } from './like';


export interface Post {
    id: number;
    content: string;
    datePosted: Date;
    LikeCount: number;
    Likes:Likes[];
    authorId: string;
    authorName:string;
    comments?: Comment[];
    post:Post;
    mentions: string[];
}
