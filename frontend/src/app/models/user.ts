import { IPhoto } from './photo';

export interface IUser {
  id: number;
  username: string;
  email: string;
  gender: string;
  age: number;
  bio: string;
  datingTarget: string;
  interests: string;
  dateOfBirth: Date;
  alias: string;
  createdAt: Date;
  lastActive: Date;
  city: string;
  country: string;
  mainPhotoURL: string;
  photos?: IPhoto[];
}
