import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabdemoComponent } from './tabdemo.component';

describe('TabdemoComponent', () => {
  let component: TabdemoComponent;
  let fixture: ComponentFixture<TabdemoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabdemoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TabdemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
