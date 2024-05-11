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
    PostComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
