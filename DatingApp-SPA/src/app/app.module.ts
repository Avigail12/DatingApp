import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import {TimeagoModule} from 'ngx-timeago';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

export function tokenGetter(){
   return localStorage.getItem('token');
}

import { AppComponent } from './app.component';
import { from } from 'rxjs';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { AuthService } from './_services/auth.service';
import { MemberCardComponent } from './member/member-list/member-card/member-card.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './member/member-list/member-detail/member-detail.component';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberDetailResolver } from './_resolves/member-detail.resolver';
import { MemberListResolver } from './_resolves/member-list.resolver';
import { MemberEditComponent } from './member/member-list/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolves/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './member/member-list/photo-editor/photo-editor.component';
import { ListsResolver } from './_resolves/lists.resolver';
import { MessagesResolver } from './_resolves/messages.resolver';
import { MemberMessagesComponent } from './member/member-list/member-messages/member-messages.component';



export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}

// @Pipe({
//    name: 'timeAgo',
//    pure: false
// })
// export class TimeAgoExtendsPipe extends TimeAgoPipe {}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      MessagesComponent,
      MemberCardComponent,
      ListsComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      MemberMessagesComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule, 
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      PaginationModule.forRoot(),
      TabsModule.forRoot(),
      ButtonsModule.forRoot(),
      BsDatepickerModule.forRoot(),
      BsDropdownModule.forRoot(),
      TimeagoModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      NgxGalleryModule,
      FileUploadModule,
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth'],
         }
      })
   ],
   providers: [
      ErrorInterceptorProvider,
      // AuthService,
      // AlertifyService,
       AuthGuard,
      // UserService,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PreventUnsavedChanges,
      ListsResolver,
      MessagesResolver,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig } 
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
