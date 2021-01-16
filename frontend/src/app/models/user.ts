import { IPhoto } from './photo';

export interface IUser {
  username: string;
  email: string;
  gender: string;
  age: number;
  dateOfBirth: Date;
  alias: string;
  createdAt: Date;
  lastActive: Date;
  city: string;
  country: string;
  mainPhotoURL: string;
  photos?: IPhoto[];
}
