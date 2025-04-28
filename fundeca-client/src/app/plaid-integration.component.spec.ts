import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaidIntegrationComponent } from './plaid-integration.component';

describe('PlaidIntegrationComponent', () => {
  let component: PlaidIntegrationComponent;
  let fixture: ComponentFixture<PlaidIntegrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlaidIntegrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaidIntegrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
