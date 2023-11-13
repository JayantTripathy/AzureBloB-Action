import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BlobActionComponent } from './blob-action/blob-action.component';
import { HttpClientModule } from '@angular/common/http';   
import { FormsModule } from '@angular/forms'; 
@NgModule({
  declarations: [
    AppComponent,
    BlobActionComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,  
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
