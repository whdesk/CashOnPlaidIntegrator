import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { SignalRService } from '../../services/signalr.service';
import { Subscription } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-signalr-login',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    DatePipe
  ],
  templateUrl: './signalr-login.component.html',
  styleUrls: ['./signalr-login.component.scss']
})
export class SignalrLoginComponent implements OnInit, OnDestroy {
  loginForm: FormGroup;
  isConnected = false;
  notifications: any[] = [];
  private subscriptions = new Subscription();

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private signalrService: SignalRService
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Check connection status
    this.subscriptions.add(
      this.signalrService.getConnectionStatus().subscribe(connected => {
        this.isConnected = connected;
      })
    );

    // Listen for notifications
    this.subscriptions.add(
      this.signalrService.getNotifications().subscribe(notification => {
        if (notification) {
          this.notifications.unshift({
            ...notification,
            timestamp: new Date()
          });
          
          // Keep only the last 50 notifications
          if (this.notifications.length > 50) {
            this.notifications.pop();
          }
        }
      })
    );

    this.setupSignalRListeners();
  }

  private setupSignalRListeners() {
    this.signalrService.registerPaymentNotificationHandler((message: string) => {
      const logMessage = `[SignalR] ${new Date().toISOString()}: ${message}`;
      console.log(logMessage);
      this.notifications.unshift({
        message: logMessage,
        timestamp: new Date()
      });
      
      if (this.notifications.length > 50) {
        this.notifications.pop();
      }
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const { username, password } = this.loginForm.value;
    
    this.authService.login(username, password).subscribe({
      next: () => {
        // Start SignalR connection after successful login
        this.signalrService.startConnection().then(() => {
          console.log('SignalR connection started successfully');
        }).catch(err => {
          console.error('Failed to start SignalR connection:', err);
        });
      },
      error: (err) => {
        console.error('Login failed:', err);
        alert('Login failed. Please check your credentials.');
      }
    });
  }

  disconnect(): void {
    this.signalrService.stopConnection().then(() => {
      this.authService.logout();
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
