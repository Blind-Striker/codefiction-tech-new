import { NgModule, Inject } from '@angular/core';
import { CommonModule, APP_BASE_HREF } from '@angular/common';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { TransferHttpCacheModule } from '@nguniversal/common';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { NavMobileMenuComponent } from './components/navmobilemenu/navmobilemenu.component';
import { BreadcrumbsComponent } from './components/breadcrumbs/breadcrumbs.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { PaginationComponent } from './components/pagination/pagination.component';

import { HomeComponent } from './containers/home/home.component';
import { AboutComponent } from './containers/about/about.component';
import { BlogComponent } from './containers/blog/blog.component';
import { PodcastsComponent } from './containers/podcasts/podcasts.component';
import { PodcastsDetailsComponent } from './containers/podcasts.details/podcasts.details.component';
import { NotFoundComponent } from './containers/not-found/not-found.component';

import { LinkService } from './shared/link.service';
import { PodcastService } from './shared/podcasts.service';
import { P2psService } from './shared/p2ps.service'
import { ORIGIN_URL } from '@nguniversal/aspnetcore-engine/tokens';
import { P2psComponent } from './containers/p2ps/p2ps.component';
import { P2pDetailsComponent } from './containers/p2p-details/p2p-details.component';
import { PersonComponent } from './containers/person/person.component';
import { PersonService } from './shared/person.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        NavMobileMenuComponent,
        BreadcrumbsComponent,
        HeaderComponent,
        FooterComponent,
        PaginationComponent,

        AboutComponent,
        HomeComponent,
        BlogComponent,
        PodcastsComponent,
        PodcastsDetailsComponent,
        NotFoundComponent,
        P2psComponent,
        P2pDetailsComponent,
        PersonComponent
    ],
    imports: [
        AppRoutingModule,
        CommonModule,
        BrowserModule.withServerTransition({ appId: 'my-app-id' }),
        HttpClientModule,
        TransferHttpCacheModule,
        BrowserTransferStateModule,
        FormsModule
    ],
    providers: [
        LinkService,
        PodcastService,
        P2psService,
        PersonService
    ],
    bootstrap: [AppComponent]
})

export class AppModuleShared {
}
