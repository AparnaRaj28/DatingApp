import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
//  @Input() usersFromHomeComponent: any; //input decorator to fetch data from home component(parent)
 @Output() cancelRegister = new EventEmitter(); //emits an event from child to parent component
  model:any={}

  constructor(private accountService:AccountService,private toastr: ToastrService) { }

  ngOnInit(): void {
  }

   register(){
    // console.log(this.model)
    this.accountService.register(this.model).subscribe({ //passing the model
      next:response=>{
        console.log(response);
        this.cancel();
      },
      error:error=>{
        console.log(error);
        this.toastr.error(error.error)
            }
    })
   }

   cancel(){
    this.cancelRegister.emit(false);
    console.log("cancelled")
   }
}
