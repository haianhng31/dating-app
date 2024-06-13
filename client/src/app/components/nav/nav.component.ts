import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { User } from '../../models/users';
import { Observable, of } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  
  model: any = {};
  currentUser$: Observable<User | null> = of(null);

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {};
  // In Angular, the constructor with the private keyword and a service as its parameter is used for dependency injection

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: () => this.router.navigateByUrl("/members"),
      // error: error => this.toastr.error(error.error) // this is handled inside interceptor now 
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");
  }
}
