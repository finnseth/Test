import { FormGroup } from '@angular/forms';
import { SchemaFormArray } from './schema-form-array';
import { SchemaFormControl } from './schema-form-control';

export class SchemaFormGroup extends FormGroup {
    keyProperty: string;

    public Parse( obj: any ) {

        for (const property in obj) {
            const value = obj[property];

            const control = this.controls[ property ];
            if (!control) {
                continue;
            }

            if ( control instanceof SchemaFormArray  ) {
                (<SchemaFormArray> control).Parse( value );
            } else {
                control.setValue( value );
            }
        }
    }

    public afterInit(): void {

        for (const property in this.controls) {

            const c = this.controls[property];
            if (c instanceof SchemaFormArray) {
                (<SchemaFormArray> c).afterInit();
            } else if (c instanceof SchemaFormGroup) {
                (<SchemaFormGroup> c).afterInit();
            } else if (c instanceof SchemaFormControl) {
                (<SchemaFormControl> c).afterInit();
            }
        }
    }

    public static GetKeyValue<T>( control: SchemaFormGroup ): T {
        const val = control.controls[control.keyProperty].value;
        return <T> val;
    }
}
