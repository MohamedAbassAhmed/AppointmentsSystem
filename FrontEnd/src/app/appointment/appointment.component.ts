import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { catchError, map, throwError } from 'rxjs';
import swal from 'sweetalert';
import { Router } from '@angular/router';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent implements OnInit {
  private http: HttpClient;
  router: Router;
  form= new FormGroup({
  startDate: new FormControl(Date.now),
  endDate: new FormControl(Date.now),
  patintName: new FormControl(''),

})

  constructor(http: HttpClient, router: Router) {
    this.http = http;
    this.router = router;  }

  ngOnInit(): void {
  }

  OnSubmit() {
    var model: appointRequest = {
      start: this.form.controls['startDate'].value,
      end: this.form.controls['endDate'].value,
      PatintName: this.form.controls['patintName'].value
    }
    this.http.post('weatherforecast/api/Appointments/CreateAppointment', model)
      .pipe(map((data: any) => {
        return data;
      }),catchError(e => { return this.handleError(e); }))
      .subscribe(resp => {
        if (resp.status == true) {
          console.log('Success:' + resp.data)
          swal("Done!", "Your Appointment is on :" + resp.data, "success");
           
            
        }
        else {
          swal("Sorry!", "We Couldn't Get You An Appointment", "warning");
          console.log('fail');
        }
    });
  }
  
  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(
      'Something bad happened; please try again later.');
  }

  
}
class appointRequest {
  start: Date;
  end: Date;
  PatintName: string;
}
export class BaseRespose {
  status: boolean;
  message: string;
  data: string;
}
