import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { IPhoto } from './../../../models/photo';
import { AlertifyService } from './../../../services/alertify.service';
import { AuthenticationService } from './../../../services/authentication.service';
import { UserService } from './../../../services/user.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: IPhoto[];

  public uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  public currentMainPhoto: IPhoto;

  constructor(
    private userService: UserService,
    private alertifyService: AlertifyService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {}

  public fileOverBase(isOver: boolean) {
    this.hasBaseDropZoneOver = isOver;
  }

  public initializeUploader(): void {
    this.uploader = new FileUploader({
      url: environment.apiURL + 'photos/upload',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (_item, response) => {
      if (response) {
        const result: IPhoto = JSON.parse(response);
        this.photos.push(result);

        if (result.isMain) {
          this.authenticationService.changeUserPhoto(result.url);
          this.authenticationService.currentUser.mainPhotoURL = result.url;
          localStorage.setItem('user', JSON.stringify(this.authenticationService.currentUser));
        }
      }
    };
  }

  public setMainPhoto(photo: IPhoto): void {
    this.userService.setMainPhoto(photo.id).subscribe(
      () => {
        this.currentMainPhoto = this.photos.filter((p) => p.isMain === true)[0];
        this.currentMainPhoto.isMain = false;
        photo.isMain = true;
        this.authenticationService.changeUserPhoto(photo.url);
        this.authenticationService.currentUser.mainPhotoURL = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authenticationService.currentUser));
      },
      (error) => this.alertifyService.error(error)
    );
  }

  public deletePhoto(id: number): void {
    this.alertifyService.confirm('Are you sutre you want to delete this photo?', () => {
      this.userService.deletePhoto(id).subscribe(
        () => {
          this.photos.splice(this.photos.findIndex((p) => p.id === id));
          this.alertifyService.success('Photo has been deleted');
        },
        (error) => {
          this.alertifyService.error('Failed to delete the photo');
          console.log(error);
        }
      );
    });
  }
}
