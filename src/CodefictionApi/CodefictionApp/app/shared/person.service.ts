import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ORIGIN_URL } from '@nguniversal/aspnetcore-engine/tokens';
import { Person } from '../models/Person';

@Injectable()
export class PersonService {

  private baseUrl: string;

    constructor(
        private http: HttpClient,
        private injector: Injector
    ) {
        this.baseUrl = this.injector.get(ORIGIN_URL);
    }

    getPerson(name:string) {
        return this.http.get<Person>(`${this.baseUrl}/api/people/${name}`);
    }

}
