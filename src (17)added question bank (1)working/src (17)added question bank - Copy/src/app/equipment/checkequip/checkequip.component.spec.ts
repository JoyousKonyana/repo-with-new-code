import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckequipComponent } from './checkequip.component';

describe('CheckequipComponent', () => {
  let component: CheckequipComponent;
  let fixture: ComponentFixture<CheckequipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckequipComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckequipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
