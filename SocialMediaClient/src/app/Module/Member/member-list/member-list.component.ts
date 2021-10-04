import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Member } from 'src/app/Model/Member';
import { MemberService } from 'src/app/Service/Member/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  member$:Observable<Member[]>;

  constructor(private _userService:MemberService, private _route:Router, private _toaster:ToastrService) { }

  ngOnInit(): void {
    this.member$=this._userService.GetAll();
  }


}
