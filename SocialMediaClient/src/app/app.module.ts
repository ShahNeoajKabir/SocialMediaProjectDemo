import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import{TabsModule} from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { FileUploadModule } from 'ng2-file-upload';
import { NavBarComponent } from './Module/Nav-bar/nav-bar/nav-bar.component';
import { RegistrationComponent } from './Module/Nav-bar/registration/registration.component';
import { JwtInterceptor } from './Common/jwt.interceptor';
import { LoadingInterceptor } from './Common/LoadingInterceptor';
import { MemberListComponent } from './Module/Member/member-list/member-list.component';
import { MemberCardComponent } from './Module/Member/member-card/member-card.component';
import { EditMemberComponent } from './Module/Member/edit-member/edit-member.component';
import { MemberDetailsComponent } from './Module/Member/member-details/member-details.component';
import { HomeComponent } from './Module/home/home.component';
import { PhotoEditComponent } from './Module/Member/photo-edit/photo-edit.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AdminPanelComponent } from './Module/Admin/admin-panel/admin-panel.component';
@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    RegistrationComponent,
    MemberListComponent,
    MemberCardComponent,
    EditMemberComponent,
    MemberDetailsComponent,
    HomeComponent,
    PhotoEditComponent,
    AdminPanelComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgxGalleryModule ,
    FormsModule,
    NgxSpinnerModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-top-right'
    }),
    TabsModule.forRoot(),
    FileUploadModule,PaginationModule.forRoot()
  ],
  providers: [

    {provide: HTTP_INTERCEPTORS, useClass:JwtInterceptor , multi:true},
    {provide:HTTP_INTERCEPTORS,useClass:LoadingInterceptor,multi:true}

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

