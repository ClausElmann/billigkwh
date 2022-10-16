import { ComponentFixture, TestBed } from "@angular/core/testing";

import { ElLoginComponent } from "./forgot-password.component";

describe("LoginComponent", () => {
  let component: ElLoginComponent;
  let fixture: ComponentFixture<ElLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ElLoginComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ElLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
