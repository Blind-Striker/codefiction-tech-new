import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { P2psComponent } from './p2ps.component';

describe('P2psComponent', () => {
  let component: P2psComponent;
  let fixture: ComponentFixture<P2psComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ P2psComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(P2psComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
