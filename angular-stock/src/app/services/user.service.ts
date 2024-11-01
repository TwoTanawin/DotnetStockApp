import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';
import { UserModelLogin } from '../Models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiURL = environment.dotnet_api_url;

  // Header
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    })
  }

  constructor(private http: HttpClient) { }

  // Method for Login
  Login(data: UserModelLogin): Observable<UserModelLogin> {
    return this.http.post<UserModelLogin>(
      this.apiURL + 'Authenticate/login', 
      data, 
      this.httpOptions
    )
  }

  // Method for Register
  // Register(data: UserModelRegister): Observable<UserModelRegister> {
  //   return this.http.post<UserModelRegister>(
  //     this.apiURL + 'Authenticate/register-user', 
  //     data, 
  //     this.httpOptions
  //   )
  // }

  // Method for Logout
  Logout() {
    // Remove token from local storage
    localStorage.removeItem('token')
    return this.http.post(this.apiURL + 'Authenticate/logout', this.httpOptions)
  }
}
