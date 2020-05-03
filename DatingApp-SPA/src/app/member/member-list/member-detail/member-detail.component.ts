import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { error } from 'protractor';
import { ActivatedRoute } from '@angular/router';
import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {NgxGalleryImage} from '@kolkov/ngx-gallery';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
   user: User;
   galleryOptions: NgxGalleryOptions[];
   galleryImages: NgxGalleryImage[];
   constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        // open img on full screen
        preview: false
      }
    ];
    this.galleryImages = this.getImages();
  }

  getImages(){
    const imageUrl = [];
    for (const photo of this.user.photos) {
      imageUrl.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description
      });
    }
    return imageUrl;
  }

}
// כתוב במקום בדף RESOLVE

//  loadUser(){
//    // שמים + כדי להמיר מסטרינג לנאמבר כי בהתחלה זה כתובת יואראל
//   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => {
//     this.user = user;
//   }, error =>{
//     this.alertify.error(error);
//   });
//  }