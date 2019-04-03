import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { TelerikReportingModule } from '@progress/telerik-angular-report-viewer';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    TelerikReportingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
