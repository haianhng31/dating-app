import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/account.service';
import { User } from './models/users';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  // OnInit: happens after our component has been constructed 
  
  title: string = 'hello';
  users: any;

  constructor(private http: HttpClient, private accountService: AccountService) {}
  // The constructor is normally considered too early to go and fetch data from the API 
  // -> Implement a lifecycle event inside this component

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.error('Error: ', error),
      complete: () => console.log('Done')
    })
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
