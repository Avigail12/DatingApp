import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';

// פיתרון כדי שלא נצטרך לשים ? ליד המשתמש בקומפוננטת פרטי חבר user?.knowAs
@Injectable()
export class MessagesResolver implements Resolve<Message[]>{
    pageNumber = 1;
    pageSize = 12;
    messageContainer = 'Unread';

    constructor(private userService: UserService, private router: Router,
                private alertify: AlertifyService, private authService: AuthService){}


    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        // do subscribe itself
        return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber,
             this.pageSize, this.messageContainer).pipe(
            catchError( error => {
                this.alertify.error('Problem retriving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
