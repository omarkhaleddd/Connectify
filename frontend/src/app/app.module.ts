import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AppRoutingModule } from './app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from "@angular/common/http";
import { ProfileComponent } from './profile/profile.component';
import { PostsDisplayComponent } from './components/post/posts-display/posts-display.component';
import { PostGetComponent } from './components/post/post-get/post-get.component';
import { PostCreateComponent } from './components/post/post-create/post-create.component';
import { CommentsDisplayComponent } from './components/comment/comments-display/comments-display.component';
import { CommentsCreateComponent } from './components/comment/comments-create/comments-create.component';
import { CommentGetComponent } from './components/comment/comment-get/comment-get.component';
import { PostComponent } from './post/post.component';
import { PostsProfileDisplayComponent } from './components/post/posts-profile-display/posts-profile-display.component';
import { SearchComponent } from './components/search/search.component';
import { SearchUserComponent } from './components/search-user/search-user.component';
import { UserComponent } from './user/user.component';
import { MessageGeneralComponent } from './components/message/message-general/message-general.component';
import { MessageChatComponent } from './components/message/message-chat/message-chat.component';
import { MessageFriendListSidebarComponent } from './components/message/message-friend-list-sidebar/message-friend-list-sidebar.component';
import { MessageFriendComponent } from './components/message/message-friend/message-friend.component';
import { ModalComponent } from './components/global/modal/modal.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationComponent } from './components/notifications/notification/notification.component';
import { RepostComponent } from './repost/repost.component';
import { RepostCreateComponent } from './components/repost/repost-create/repost-create.component';
import { RepostGetComponent } from './components/repost/repost-get/repost-get.component';
import { VideoCallComponent } from './components/video-call/video-call.component';
import { StreamComponent } from './stream/stream.component';
import { FriendListComponent } from './components/global/friend-list/friend-list.component';
import { DonateModalComponent } from './components/global/donate-modal/donate-modal.component';
import { DonationComponent } from './donation/donation.component';
import { NgxStripeModule } from 'ngx-stripe';
import { environment } from './enviroment/enviroment';
import { MessageGrpComponent } from './components/message/message-grp/message-grp.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotfoundComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    ProfileComponent,
    PostsDisplayComponent,
    PostGetComponent,
    PostCreateComponent,
    CommentsDisplayComponent,
    CommentsCreateComponent,
    CommentGetComponent,
    PostComponent,
    PostsProfileDisplayComponent,
    SearchComponent,
    SearchUserComponent,
    UserComponent,
    ModalComponent,
    NotificationsComponent,
    NotificationComponent,
    MessageGeneralComponent,
    MessageChatComponent,
    MessageFriendListSidebarComponent,
    MessageFriendComponent,
    RepostComponent,
    RepostCreateComponent,
    RepostGetComponent,
    VideoCallComponent,
    StreamComponent,
    FriendListComponent,
    DonateModalComponent,
    DonationComponent,
    MessageGrpComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgxStripeModule.forRoot(environment.stripePublicKey)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
