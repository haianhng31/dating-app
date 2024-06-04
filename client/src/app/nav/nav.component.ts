import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { User } from '../models/users';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  
  model: any = {};
  currentUser$: Observable<User | null> = of(null);

  constructor(public accountService: AccountService) {};
  // In Angular, the constructor with the private keyword and a service as its parameter is used for dependency injection

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model) // this returns an Observable => use subscribe to get an Observer 
      .subscribe({
        next: response => {
          console.log(response);
        },
        // what we want to do next with the observerble 
        error: error => console.log(error)
      })
  }

  logout() {
    this.accountService.logout();
  }
}
