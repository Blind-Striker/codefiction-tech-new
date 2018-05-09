import { Component, OnInit } from '@angular/core';
import { P2psService } from '../../shared/p2ps.service';
import { P2P } from '../../models/P2P';

@Component({
  selector: 'app-p2ps',
  templateUrl: './p2ps.component.html',
  styleUrls: ['./p2ps.component.css']
})
export class P2psComponent implements OnInit {

  p2ps: P2P[];
  constructor(private p2pService:P2psService) { }

  ngOnInit() {
    
    this.p2pService.getP2Ps().subscribe(result => {
      console.log('HttpClient [GET] /api/p2ps', result);
      this.p2ps =  result;
  });
  }

}
