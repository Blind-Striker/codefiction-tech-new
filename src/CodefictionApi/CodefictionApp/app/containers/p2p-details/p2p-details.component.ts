import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { Router, ActivatedRoute } from '@angular/router';
import { P2psService } from '../../shared/p2ps.service';
import { IPodcast } from '../../models/contracts/IPodcast';

@Component({
  selector: 'app-p2p-details',
  templateUrl: './p2p-details.component.html',
  styleUrls: ['./p2p-details.component.css']
})
export class P2pDetailsComponent implements OnInit {

  p2p: IPodcast;
  soundCloudUrl: SafeResourceUrl;
  youtubeUrl: SafeResourceUrl;

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object,
    private p2pService: P2psService) {

  }

  ngOnInit() {
    let slug = this.route.snapshot.paramMap.get('slug');

    this.p2pService.getP2PbySlug(slug).subscribe(result => {
      this.p2p = result;

      let soundCloudUrl: string = 'https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/' +
        this.p2p.soundcloudId + '&color=%23ff5500&auto_play=false&hide_related=false&show_comments=true&show_user=true&show_reposts=false&show_teaser=true';

      this.soundCloudUrl = this.sanitizer.bypassSecurityTrustResourceUrl(soundCloudUrl);
      this.youtubeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.p2p.youtubeUrl);
    });
  }

}
