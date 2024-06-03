import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  // OnInit: happens after our component has been constructed 
  
  title: string = 'hello';
  users: any;

  constructor(private http: HttpClient) {}
  // The constructor is normally considered too early to go and fetch data from the API 
  // -> Implement a lifecycle event inside this component

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.error('Error: ', error),
      complete: () => console.log('Done')
    })
    // this returns an Observable
  }
}
