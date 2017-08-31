import {} from 'jasmine';
import { CsInputComponent } from './';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { TestBed } from '@angular/core/testing';

describe('Component: CsInputComponent', () => {
    let component: CsInputComponent;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [CsInputComponent],
            imports: [ReactiveFormsModule]
        });

        const fixture = TestBed.createComponent(CsInputComponent);
        component = fixture.componentInstance;
    } );

    it('Should have defined component', () => {
        expect(component).toBeDefined();
    } );
} );
