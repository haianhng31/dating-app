import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:5001/api/';
  validationErrors: string[] = [];
  constructor(private http: HttpClient) {}

  get404Error() 
  {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.error('Error: ', error),
      complete: () => console.log('complete')
    })
  }

  get400Error() 
  {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.error('Error: ', error),
      complete: () => console.log('complete')
    })
  }

  get500Error() 
  {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.error('Error: ', error),
      complete: () => console.log('complete')
    })
  }

  get401Error() 
  {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: response => console.log(response),
      error: error => console.error('Error: ', error),
      complete: () => console.log('complete')
    })
  }

  get400ValidationError() 
  {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.error('Error: ', error),
        this.validationErrors = error;
      },
      complete: () => console.log('complete')
    })
  }

}
