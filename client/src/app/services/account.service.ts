import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new BehaviorSubject<User>(null);
  currentUser$ = this.currentUserSource.asObservable(); //$ is used to represent a variable as observable

  constructor(private http:HttpClient) { }

  login(model:any)
  {
     return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
     map((response:User)=>{
           const user= response;
           if(user){
            localStorage.setItem('user',JSON.stringify(user)) //only stores key value pairs in strings 
            this.currentUserSource.next(user);
           }
     })
     )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl+ 'account/register',model).pipe(
      map(user =>{
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user; //to get the response in console from register component
      })
    )
  }

  setCurrentUser(user : User){
    this.currentUserSource.next(user);
  }

  logout(){
    console.log("Log out from account.service")
    localStorage.removeItem('user'); //key of user
    this.currentUserSource.next(null);
  }
}
