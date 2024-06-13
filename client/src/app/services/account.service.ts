import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../models/users';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

// when the service is provided in the root module, such as the app module 
// when our app is initialized, the the service is also initialized at the same time
// and the service is not destroyed until the user is finished with our application 
// (instead of being destroyed everytime user moves between components)

// this is a good place to store state that we want our application to remember 
// no matter where that user is in the app 

export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null); // Union type 
  currentUser$ = this.currentUserSource.asObservable();
  // The asObservable() method converts the BehaviorSubject into a plain observable.
  // By calling asObservable(), you create a read-only view of the BehaviorSubject, 
  // preventing external code from directly calling methods like next() to emit new values.
  // The $ suffix indicates that currentUser$ is an observable.

  constructor(private http: HttpClient) { 
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + "account/register", model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        // return user; // needs this if we want to console.log user 
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
