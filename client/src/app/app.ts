import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal, Signal } from '@angular/core';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';


@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http= inject(HttpClient);
  protected title = 'Dating app';
  protected members= signal<any>([])
 
  async ngOnInit(){
    this.members.set( await this.getmembers());
  }

  async getmembers(){
    try{
      return lastValueFrom(this.http.get('https://localhost:5001/api/members')); 
    }
    catch (error) {
      return console.log(error);
    }
    
  }
}