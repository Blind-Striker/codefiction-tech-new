import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../shared/person.service';
import { ActivatedRoute } from '@angular/router';
import { Person } from '../../models/Person';

@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {

  person: Person;
  constructor(
    private personService: PersonService,
    private route: ActivatedRoute) { }

  ngOnInit() {

    let name = this.route.snapshot.paramMap.get('name');

    this.personService.getPerson(name).subscribe(result => {
      this.person = result;
    })
  }

}
