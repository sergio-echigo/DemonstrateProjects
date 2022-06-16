import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './views/nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './views/home/home.component';
import { SignUpComponent } from './views/sign-up/sign-up.component';
import { MatDialogModule } from '@angular/material/dialog';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { SignInComponent } from './views/sign-in/sign-in.component';
import { HttpClientModule } from '@angular/common/http';
import { ProjectComponent } from './views/project/project.component';
import { PersonalKeyComponent } from './views/personal-key/personal-key.component';
import { AccountComponent } from './views/account/account.component';
import { ProjectsComponent } from './views/projects/projects.component';
import { ProjectDialogComponent } from './views/projects/project-dialog/project-dialog.component';
import { ImgBasePipe } from './pipes/img-base.pipe';
import { ProjDescriptionPipe } from './pipes/proj-description.pipe';
import { LocalDateTimePipe } from './pipes/local-date-time.pipe';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    SignUpComponent,
    SignInComponent,
    ProjectComponent,
    PersonalKeyComponent,
    AccountComponent,
    ProjectsComponent,
    ProjectDialogComponent,
    ImgBasePipe,
    ProjDescriptionPipe,
    LocalDateTimePipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatMenuModule,
    MatIconModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }