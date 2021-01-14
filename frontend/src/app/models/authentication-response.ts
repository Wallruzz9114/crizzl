import { IUser } from './user';

export interface IAuthenticationResponse {
  token: string;
  user: IUser;
}
