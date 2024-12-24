export interface User {
  id: string;
  firstName: string;
  lastName: string;
  name: string;
  login: string;
  email: string;
  avatarUrl: string;
  online: boolean;
  createdOn: Date;
  isCurrentUser: boolean;
}
