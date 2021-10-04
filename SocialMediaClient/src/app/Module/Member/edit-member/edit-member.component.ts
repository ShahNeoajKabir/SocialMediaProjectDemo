import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/Model/Member';
import { User } from 'src/app/Model/Token';
import { LoginService } from 'src/app/Service/Login/login.service';
import { MemberService } from 'src/app/Service/Member/member.service';

@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css']
})
export class EditMemberComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member:Member;
  user:User;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private _loginService:LoginService, private _userService:MemberService,
    private _toaster:ToastrService
    ) {
    this._loginService.CurrentUser$.pipe(take(1)).subscribe(user=>this.user=user);
   }

  ngOnInit(): void {
    this.loadMember()
  }



  loadMember(){
    this._userService.GetByUserName(this.user.userName).subscribe(member=>{
      this.member=member;
    })
  }

  UpdateMember(){
    this._userService.updateMember(this.member).subscribe(()=>{
      this._toaster.success("Update Succesfull");

      this.editForm.reset(this.member);
    })
  }

}
