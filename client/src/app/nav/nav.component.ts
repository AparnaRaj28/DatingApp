import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';

import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  //creating a class property to store the form data
  model : any ={}
  // loggedIn = false;
  // currentUser$ : Observable<User | null> =of(null) //assigning a observable of null

  constructor(public accountService : AccountService) { 
    console.log("inside nav.component constructor()")
  }

  ngOnInit(): void {
    console.log("inside nav.component ngOnInit()")
    // this.getCurrentUser();
    // this.currentUser$ = this.accountService.currentUser$;
  }

  // getCurrentUser(){
  //   console.log("inside nav.component getCurrentUser()")
  //   this.accountService.currentUser$.subscribe({
  //     next : user => this.loggedIn = !!user,
  //     error : error => console.log(error)
  //   })
  // }

  //method that is called when the form is submitted
  login()
  {
    // console.log(this.model);
    console.log("inside nav.component login()")
    this.accountService.login(this.model).subscribe({
      next: response =>{
        console.log(response);
        // this.loggedIn = true;
      },
      error: error => console.log(error)
    })
  }
  logout(){
    console.log("logout() from nav.component")
    this.accountService.logout(); //removes the item from local storage
    // this.loggedIn=false;
  }

}
