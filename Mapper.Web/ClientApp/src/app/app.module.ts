import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './material.module';
import { ServicesModule } from './services.module';

import { AppComponent } from './app.component';

import { Routes, RouteComponents } from './routes';

@NgModule({
  declarations: [
    AppComponent,
    [...RouteComponents]
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    ServicesModule,
    RouterModule.forRoot(Routes)
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
