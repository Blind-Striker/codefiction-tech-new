import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { P2pDetailsComponent } from './p2p-details.component';

describe('P2pDetailsComponent', () => {
  let component: P2pDetailsComponent;
  let fixture: ComponentFixture<P2pDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ P2pDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(P2pDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
