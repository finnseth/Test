import { CsFormsModule } from '../forms.module';
import { Component, QueryList } from '@angular/core';
import { FormGroupName, NgControl } from '@angular/forms/src/directives';
import { CsSelectComponent, CsSelectModule } from './';
import { ControlContainer, FormGroup, FormsModule } from '@angular/forms';
import { async, TestBed } from '@angular/core/testing';


const ngControlMock =  {
    name: 'test',
    _parent: {
        control: {
            controls: {
                'test': {
                  '__title': 'title',
                  '__description': 'description'
                }
            }
        }
    }
};


@Component({
    template: `<cs-select>
                    <cs-option caption="Foo" value="Foo"></cs-option>
                    <cs-option caption="Bar" value="Bar"></cs-option>
                    <cs-option caption="Baz" value="Baz"></cs-option>
               </cs-select>`,
})
class TestWrapperComponent {
}

describe('Component: CsSelectComponent', () => {
    let component: CsSelectComponent;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [TestWrapperComponent],
            imports: [FormsModule, CsFormsModule],
            providers: [{provide: NgControl, useValue: ngControlMock  }]
        }).compileComponents();

        const fixture = TestBed.createComponent(TestWrapperComponent);
        fixture.detectChanges();
        component = fixture.debugElement.children[0].componentInstance;
        component.ngAfterContentInit();
    } ));


    it('Should create a select box with options.', () => {
        expect(component.options.length).toBe(3, 'because there are options' );
    });


    it('should change value when option changed.', () => {
        component.options.last.selected = true;
        expect( component.value ).toBe('Baz', 'because the the last options should be selected.');
    });


    it('Should disable all other options than the selected one.', () => {
        const arr = component.options.toArray();

        arr[0].selected = true;
        expect( arr[0].selected ).toBeTruthy( 'because 0 is selected.' );
        expect( arr[1].selected ).toBeFalsy( 'because 1 is not selected.' );
        expect( arr[2].selected ).toBeFalsy( 'because 2 is not selected.' );

        arr[1].selected = true;
        expect( arr[0].selected ).toBeFalsy( 'because 0 is not selected.' );
        expect( arr[1].selected ).toBeTruthy( 'because 1 is selected.' );
        expect( arr[2].selected ).toBeFalsy( 'because 2 is not selected.' );

        arr[2].selected = true;
        expect( arr[0].selected ).toBeFalsy( 'because 0 is not selected.' );
        expect( arr[1].selected ).toBeFalsy( 'because 1 is not selected.' );
        expect( arr[2].selected ).toBeTruthy( 'because 2 is selected.' );
    });
} );
