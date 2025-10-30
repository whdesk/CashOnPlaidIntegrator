import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey = 'auth_token';
  private apiUrl = 'https://coisdev.initiumsoft.es';
//  private apiUrl = 'https://localhost:7184';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  
  constructor(private http: HttpClient) {
    // Check if token exists in localStorage on service initialization
    this.isAuthenticatedSubject.next(!!this.getToken());
  }

  login(username: string, password: string): Observable<any> {
    const body = new URLSearchParams();
    body.set('client_id', 'frontoffice');
    body.set('client_secret', 'frontoffice_secret');
    body.set('grant_type', 'password');
    body.set('scope', 'openid fo_api profile offline_access');
    body.set('username', username);
    body.set('password', password);

    const headers = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded'
    });

    return this.http.post(`${this.apiUrl}/connect/token`, body.toString(), { headers }).pipe(
      tap((response: any) => {
        if (response.access_token) {
          this.setToken(response.access_token);
          this.isAuthenticatedSubject.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isAuthenticatedSubject.next(false);
  }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }
}
