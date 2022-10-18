import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DragDropComponent } from './door-edit-configuration.component';

describe('DragDropComponent', () => {
  let component: DragDropComponent;
  let fixture: ComponentFixture<DragDropComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DragDropComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DragDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});