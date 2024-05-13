import { Comment } from './comment.model';
import { Likes } from './like';


export interface Post {
    id: number;
    content: string;
    datePosted: Date;
    likeCount: number;
    likes:Likes[];
    authorId: string;
    authorName:string;
    comments?: Comment[];
}
