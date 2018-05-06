import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ORIGIN_URL } from '@nguniversal/aspnetcore-engine/tokens';
import { Podcast } from '../models/Podcasts';
import { P2P } from '../models/P2P';

@Injectable()
export class P2psService {

    baseUrl: string;
    constructor(
        private http: HttpClient,
        private injector: Injector
    ) {
        this.baseUrl = this.injector.get(ORIGIN_URL);
    }

    getP2Ps() {
        return this.http.get<P2P[]>(`${this.baseUrl}/api/P2Ps`);
    }

    getP2PbySlug(slug: string) {
        return this.http.get<P2P>(`${this.baseUrl}/api/P2Ps/` + slug);
    }
}