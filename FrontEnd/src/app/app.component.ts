import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public forecasts?: WeatherForecast;

  constructor(http: HttpClient) {
    //http.get<WeatherForecast>('/weatherforecast/api/setting/GetSessionPeriod').subscribe(result => {
    //  this.forecasts = result;
    //}, error => console.error(error));
  }

  title = 'FrontEnd';
}

interface WeatherForecast {
  data: object;
  status: boolean;
  message: string;
}
