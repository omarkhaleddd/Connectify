import { Comment } from './comment.model';

export interface Post {
    id: number;
    content: string;
    datePosted: Date;
    likeCount: number;
    authorId: string;
    comments?: Comment[];
}