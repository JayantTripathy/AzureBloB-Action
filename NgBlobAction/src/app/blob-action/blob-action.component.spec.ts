import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlobActionComponent } from './blob-action.component';

describe('BlobActionComponent', () => {
  let component: BlobActionComponent;
  let fixture: ComponentFixture<BlobActionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BlobActionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlobActionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
