import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './Common/auth.guard';
import { PreventUnsavedChangesGuard } from './Common/PreventUnsavedChangesGuard';
import { HomeComponent } from './Module/home/home.component';
import { EditMemberComponent } from './Module/Member/edit-member/edit-member.component';
import { MemberDetailsComponent } from './Module/Member/member-details/member-details.component';
import { MemberListComponent } from './Module/Member/member-list/member-list.component';
import { NavBarComponent } from './Module/Nav-bar/nav-bar/nav-bar.component';

const routes: Routes = [
  {path:'', component:HomeComponent},
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate:[AuthGuard],
    children:[
      {path:'Member', component:MemberListComponent},
      {path:'Member/:userName', component:MemberDetailsComponent},
      {path:'Members/edit', component:EditMemberComponent , canDeactivate:[PreventUnsavedChangesGuard]},
      {path:'List', component:MemberListComponent},
      // {path:'Messeges', component:MessegesComponent},
    ]
  },
  


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
