import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as signalR from '@microsoft/signalr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  hubConnection!: signalR.HubConnection;
  roomName = '';
  userName = '';
  players: any[] = [];
  isGameStarted = false;
  roomJoined = false;
  errorMessage = '';
  fastestPlayer: { name: string; time: number } | null = null;
  playerTimes: any[] = [];
  isGameEnded = false;

  ngOnInit(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7025/gamehub')
      .build();

    this.hubConnection.start().catch(err => console.error(err));

    this.hubConnection.on('UpdateRoom', players => {
      this.players = players;
    });

    this.hubConnection.on('FastestPlayer', (playerName, time) => {
      this.fastestPlayer = { name: playerName, time };
    });

    this.hubConnection.on('GameStarted', () => {
      this.isGameStarted = true;
    });

    this.hubConnection.on('PlayerClicked', (playerName, time) => {
      const player = this.players.find(p => p.name === playerName);
      if (player) {
        player.totalTime = time;
      }
    });

    this.hubConnection.on('GameEnded', (winnerName, playerTimes) => {
      this.playerTimes = playerTimes;

      Swal.fire({
        title: `${winnerName} é o vencedor!`,
        icon: 'success',
        confirmButtonText: 'OK',
        confirmButtonColor: '#28a745',
        position: 'top',
        showConfirmButton: true,
        backdrop: true,
        allowOutsideClick: false,
        customClass: {
          popup: 'swal-popup-center',
        },
        width: '400px',
        padding: '1.5em',
        heightAuto: true,
      }).then(() => {
        location.reload();
      });
    });    
  }

  createRoom(): void {
    this.errorMessage = '';
  
    this.hubConnection.invoke('CreateRoom', this.roomName, this.userName)
    .then((result: string) => {
      if (result === 'Success') {
        this.roomJoined = true;
      } else {
        this.errorMessage = result;
      }
    })
    .catch(err => {
      this.roomJoined = false;
      this.errorMessage = err ? err : 'Aconteceu um erro inesperado. Por favor, tente novamente.';
    });
  }

  joinRoom(): void {
    this.errorMessage = '';
  
    this.hubConnection.invoke('JoinRoom', this.roomName, this.userName)
      .then((result: string) => {
        if (result === 'Success') {
          this.roomJoined = true;
        } else {
          this.errorMessage = result;
        }
      })
      .catch(err => {
        this.roomJoined = false;
        this.errorMessage = err ? err : 'Aconteceu um erro inesperado. Por favor, tente novamente.';
      });
  }  

  startGame(): void {
    this.hubConnection.invoke('StartGame', this.roomName)
      .then(() => {
        this.isGameStarted = true;
      })
      .catch(err => {
        this.errorMessage = err ? err : 'Não foi possível iniciar o jogo.';
      });
  }

  playerClick(): void {
    this.hubConnection.invoke('PlayerClick', this.roomName).catch(err => console.error(err));
  }
}
