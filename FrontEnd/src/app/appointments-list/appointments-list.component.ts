import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, throwError } from 'rxjs';
import swal from 'sweetalert';

@Component({
  selector: 'app-appointments-list',
  templateUrl: './appointments-list.component.html',
  styleUrls: ['./appointments-list.component.css']
})
export class AppointmentsListComponent implements OnInit {
  http: HttpClient;
 
  public appointments: Appointments[];

  constructor(http: HttpClient) {
    this.http = http;
    
  }
  //GetAppointments
  ngOnInit(): void {
    this.http.get<BaseRespose>('weatherforecast/api/Appointments/GetAppointments')
      .pipe(map((data: any) => {
        return data;
      }), catchError(r => { return this.handleError(r); }))
      .subscribe(resp => {
        this.appointments = resp.data;
        if (resp.status != true) {
          swal("Sorry!", "Fialed to get data", "warning");

        }
        else {
          this.appointments = resp.data;

        }
      })
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
class BaseRespose {
  status: boolean;
  message: string;
  data: Appointments[];
}
class Appointments {
  start: string;
  end: string;
  patintName: string
}
