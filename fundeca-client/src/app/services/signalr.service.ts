import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService implements OnDestroy {
  private hubConnection!: HubConnection;
  // private readonly hubUrl = 'https://localhost:7160/notificationHub';
  private readonly hubUrl = 'https://cofodev-02.initiumsoft.es/notificationHub';

  private connectionStatus = new BehaviorSubject<boolean>(false);
  private connectionStatus$ = this.connectionStatus.asObservable();
  private notificationSubject = new Subject<any>();

  constructor(private authService: AuthService) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => this.authService.getToken() || ''
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Trace)
      .build();

    this.registerEvents();
  }

  private registerEvents(): void {
    this.hubConnection.on('PaymentNotification', (message: any) => {
        console.log(message);
      this.notificationSubject.next({ type: 'notification', message });
    });

    this.hubConnection.onclose(() => {
      this.connectionStatus.next(false);
      console.log('SignalR connection closed');
    });
  }

  async startConnection(): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      return;
    }

    try {
      await this.hubConnection.start();
      this.connectionStatus.next(true);
      console.log('SignalR connection started');
    } catch (err) {
      console.error('Error while starting SignalR connection:', err);
      this.connectionStatus.next(false);
    }
  }

  async stopConnection(): Promise<void> {
    if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected) {
      try {
        await this.hubConnection.stop();
        this.connectionStatus.next(false);
      } catch (err) {
        console.error('Error while stopping SignalR connection:', err);
      }
    }
  }

  getConnectionStatus(): Observable<boolean> {
    return this.connectionStatus$;
  }

  getNotifications(): Observable<any> {
    return this.notificationSubject.asObservable();
  }

  registerPaymentNotificationHandler(handler: (message: string) => void) {
    this.hubConnection.on('PaymentNotification', handler);
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
