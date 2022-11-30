import { Component, OnInit } from '@angular/core';
import { User } from './models/user';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users:any;

  constructor(private accountService : AccountService)
  {
       console.log("Inside app.component constructor()");
  }
  ngOnInit(){
    console.log("Inside appcomponent ngOnInit()");
    // this.getUsers();
    this.setCurrentUSer(); //this sets the current user object if we have something on the local storage
  }



 setCurrentUSer(){ //method to set the current user 
  console.log("inside app.component setCurrentUser()");
  const userString = localStorage.getItem('user');
  if(!userString) return;
  const user:User = JSON.parse(userString);
  this.accountService.setCurrentUser(user);
  
 }

}
