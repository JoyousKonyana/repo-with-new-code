import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageAchievementTypesComponent } from './manage-achievement-types.component';

describe('ManageAchievementTypesComponent', () => {
  let component: ManageAchievementTypesComponent;
  let fixture: ComponentFixture<ManageAchievementTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageAchievementTypesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageAchievementTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
