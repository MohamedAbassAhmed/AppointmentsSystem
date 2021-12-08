import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError ,map} from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import swal from 'sweetalert';



@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.css']
})
export class SettingComponent implements OnInit {

  public http: HttpClient;

  profileForm = new FormGroup({
    sessionPeriod: new FormControl(0),
    waitPeriod: new FormControl(0),
    restPeriod: new FormControl(0)
  });

  constructor(http: HttpClient) {

    this.http = http;
  }
  options: {
    headers?: HttpHeaders | { [header: string]: string | string[] },
    observe?: 'body' | 'events' | 'response',
    params?: HttpParams | { [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean> },
    reportProgress?: boolean,
    responseType?: 'arraybuffer' | 'blob' | 'json' | 'text',
    withCredentials?: boolean,
  }
  ngOnInit(): void {
    this.http.get<BaseRespose>('weatherforecast/api/Setting/GetSettings').subscribe(response => {
      this.profileForm.patchValue({
        sessionPeriod: response.data.sessionPeriod,

        waitPeriod: response.data.waitPeriod,
        restPeriod: response.data.restPeriod,

      });
    })
  }
  onSubmit() {
    var model: Settings = {
      sessionPeriod: this.profileForm.controls['sessionPeriod'].value,
      waitPeriod: this.profileForm.controls['waitPeriod'].value,
      restPeriod: this.profileForm.controls['restPeriod'].value,
    }

    this.http.post('weatherforecast/api/Setting/SaveSettings', model)
      .pipe(map((data: any) => {
        return data;
      }),catchError(r => { return this.handleError(r); }))
      .subscribe(resp => {
        if (resp.status == true) {
          console.log('Success:' + resp.data)
          swal("Done!", "Saved", "success");
        }
        else {
          swal("Sorry!", "Not Saved Please Try again", "warning");
          console.log('fail');
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
  data: Settings;
}
class Settings {
  sessionPeriod: number;
  waitPeriod: number;
  restPeriod: number;
}

