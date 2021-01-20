import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { IUser } from './../../../models/user';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
})
export class UserDetailsComponent implements OnInit {
  public user: IUser;
  public galleryOptions: NgxGalleryOptions[];
  public galleryImages: NgxGalleryImage[];

  constructor(private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((data) => {
      this.user = data['user'];
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imagePercent: 100,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
        imageArrowsAutoHide: true,
        imageSwipe: true,
        imageInfinityMove: true,
        thumbnailsAutoHide: true,
      },
    ];

    this.galleryImages = this.getImages();
  }

  // public getUser(): void {
  //   this.userService.get(+this.activatedRoute.snapshot.params['id']).subscribe(
  //     (user: IUser) => (this.user = user),
  //     (error) => {
  //       this.alertifyService.error('Problem while getting user information');
  //       console.log(error);
  //     }
  //   );
  // }

  public getImages(): NgxGalleryImage[] {
    const imgUrls: NgxGalleryImage[] = [];

    for (const photo of this.user.photos) {
      imgUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description,
      });
    }

    return imgUrls;
  }
}
