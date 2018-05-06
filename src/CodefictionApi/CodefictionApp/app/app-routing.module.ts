import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { HomeComponent } from './containers/home/home.component';
import { AboutComponent } from './containers/about/about.component';
import { BlogComponent } from './containers/blog/blog.component';
import { PodcastsComponent } from './containers/podcasts/podcasts.component';
import { PodcastsDetailsComponent } from './containers/podcasts.details/podcasts.details.component';
import { NotFoundComponent } from './containers/not-found/not-found.component';
import { P2pDetailsComponent } from './containers/p2p-details/p2p-details.component';
import { PersonComponent } from './containers/person/person.component';

const appRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent,

        // *** SEO Magic ***
        // We're using "data" in our Routes to pass in our <title> <meta> <link> tag information
        // Note: This is only happening for ROOT level Routes, you'd have to add some additional logic if you wanted this for Child level routing
        // When you change Routes it will automatically append these to your document for you on the Server-side
        //  - check out app.component.ts to see how it's doing this
        data: {
            title: 'Homepage',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'about',
        component: AboutComponent,
        data: {
            title: 'About',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'blog',
        component: BlogComponent,
        data: {
            title: 'Blog',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'podcasts',
        component: PodcastsComponent,
        data: {
            title: 'Podcasts',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'podcast/:slug',
        component: PodcastsDetailsComponent,
        data: {
            title: 'Podcasts',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'p2p/:slug',
        component: P2pDetailsComponent,
        data: {
            title: 'P2P Podcastler',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: 'person/:name',
        component: PersonComponent,
        data: {
            title: 'Konuşmacılar',
            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
            ]
        }
    },
    {
        path: '**',
        component: NotFoundComponent,
        data: {
            title: '404 - Not found',
            meta: [{ name: 'description', content: '404 - Error' }],
            links: [
                { rel: 'canonical', href: 'http://blogs.example.com/bootstrap/something' },
                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/bootstrap-demo' }
            ]
        }
    }
];


@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes,
            {
                useHash: false,
                preloadingStrategy: PreloadAllModules,
                initialNavigation: 'enabled'
            }
        )
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }
