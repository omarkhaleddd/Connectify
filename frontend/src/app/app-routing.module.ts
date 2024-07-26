import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Import components for routing
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import {RegisterComponent } from './register/register.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { ProfileComponent } from './profile/profile.component';
import { PostComponent } from './post/post.component';
import { SearchComponent } from './components/search/search.component';
import { UserComponent } from './user/user.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { MessageGeneralComponent } from './components/message/message-general/message-general.component';
import { RepostComponent } from './repost/repost.component';
import { StreamComponent } from './stream/stream.component';
import { DonationComponent } from './donation/donation.component';
import { ChatComponent } from './chat/chat.component';
import { ChatBodyComponent } from './components/chat-body/chat-body.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {path: 'profile' , component: ProfileComponent},
  {path: 'message',
    component: ChatComponent,
    children: [
      {
        path: '',
        component: ChatBodyComponent
      },
      {
        path: 'stream',
        component: StreamComponent
      }
  ]},
  {path: 'post/:id' , component: PostComponent},
  {path: 'search/:query' , component: SearchComponent},
  {path: 'user/:id' , component: UserComponent},
  {path: 'notifications' , component: NotificationsComponent},
  {path: 'repost' , component: RepostComponent},
  {path: 'stream' , component: StreamComponent},
  {path: 'donation/:id' , component: DonationComponent},
  {path:'**',component:NotfoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
