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
import { MessageGeneralComponent } from './components/message/message-general/message-general.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {path: 'profile' , component: ProfileComponent},
  {path: 'message' , component: MessageGeneralComponent},
  {path: 'post/:id' , component: PostComponent},
  {path: 'search/:query' , component: SearchComponent},
  {path: 'user/:id' , component: UserComponent},
  {path:'**',component:NotfoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
