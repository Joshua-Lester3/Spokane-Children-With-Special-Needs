export default class TokenService {
    private tokenKey: string = "token";
    private guidKey: string = "guid";
  
    public setToken(token: string | undefined) {
      if (token === undefined) {
        localStorage.removeItem(this.tokenKey);
      } else {
        localStorage.setItem(this.tokenKey, token);
      }
    }
  
    public getToken(): string {
      return localStorage.getItem(this.tokenKey) ?? "";
    }

    public isNotExpired(): boolean {
      const token = this.getToken();
      if (token === "" || token === undefined || token.split(".").length !== 3) {
        return false;
      }
      let expiration = JSON.parse(atob(token.split(".")[1]))['exp'];
      return Math.floor(Date.now() / 1000) < expiration;
    }
  
    public isLoggedIn(): boolean {
      const token = this.getToken();
      
      return token !== "" && !(token.localeCompare("undefined") === 0) && this.isNotExpired();
    }
  
    public setGuid(guid: string | undefined | null) {
      if (guid === undefined) {
        const token = this.getToken();
        const guid = JSON.parse(atob(token.split(".")[1])).userId;
        localStorage.setItem(this.guidKey, guid);
      } else if (guid === null) {
        localStorage.removeItem(this.guidKey);
      } else {
        localStorage.setItem(this.guidKey, guid);
      }
    }
  
    public getGuid(): string {
      return localStorage.getItem(this.guidKey) ?? "";
    }
  
    public getUserName() {
      const token = this.getToken();
      if (token === "" || token === "undefined") {
        return "";
      }
      return JSON.parse(atob(token.split(".")[1])).userName;
    }
  
    public isAdmin(): boolean {
      const token = this.getToken();
      if (token === "" || token === undefined || token.split(".").length !== 3) {
        return false;
      }
      console.log(JSON.parse(atob(token.split(".")[1])));
      let result = JSON.parse(atob(token.split(".")[1]))['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return result?.toLowerCase() === "admin" && this.isNotExpired();
    }
  
    public generateTokenHeader() {
      return { Authorization: `Bearer ${this.getToken()}` };
    }
  }
  