export default interface User {
  userId: string;
  userName: string | null;
  email: string | null;
  roles: string[];
}
