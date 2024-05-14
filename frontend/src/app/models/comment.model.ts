export interface Comment {
    id: number;
    postId: number;
    content: string;
    likeCount: number
    datePosted: Date;
    authorId: string;
    authorName:string;
}
