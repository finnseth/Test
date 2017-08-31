/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuarantineComponent } from './quarantine.component';

describe('QuarantineComponent', () => {
  let component: QuarantineComponent;
  let fixture: ComponentFixture<QuarantineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuarantineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuarantineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});