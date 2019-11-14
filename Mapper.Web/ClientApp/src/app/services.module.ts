import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Services } from './services';

@NgModule({
  providers: [
    [...Services]
  ],
  imports: [
    HttpClientModule
  ],
  exports: [
    HttpClientModule
  ]
})
export class ServicesModule { }
