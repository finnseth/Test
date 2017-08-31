
import { FormGroup, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { SchemaFormArray, SchemaFormGroup } from 'dualog-common/services/schema-form-builder.service';

export class PatchGraphDocument {


    public CreatePatchDocument( fg: FormGroup ): {} {

        const patchData = {};
        this.FormGroupPatch( fg, patchData);
        return patchData;
    }

    private FormGroupPatch( formGroup: FormGroup, obj: {}): void {

        for (const property in formGroup.controls) {
            const c = formGroup.controls[property];

            // Handle if this is a FormGroup
            if (c instanceof FormGroup) {

                const nc = {};
                this.FormGroupPatch(c, nc);
                obj[property] = nc;

            // Handle if this is a FormArray
            } else if (c instanceof FormArray) {

                const nc = this.FormArrayPatch( c );
                if (nc) {
                    obj[property] = nc;
                }

            // Handle if this is a FormControl
            } else if (c instanceof FormControl && c.dirty) {

                obj[property] = c.value;
            }
        }
    }

    private FormArrayPatch( formArray: FormArray ): {} {

        if (formArray.dirty === false) {
            return null;
        }

        const obj = {};

        if (formArray instanceof SchemaFormArray) {

            // Detect wheter there are additions on the current node,
            // and create a create description if there are
            const sfa = <SchemaFormArray> formArray;
            if (sfa._added.length > 0) {
                obj['create'] = sfa._added.map( a => {
                });
            }

            // Detect wheter there are deletions on the current node,
            // and create a delete description if there are
            if (sfa._deleted.length > 0) {
                obj['delete'] = sfa._deleted.map( d => {
                    return { id: d };
                });
            }

            // Detect whether there are changes on the current node,
            // and create a change description if there are
            const chItems = sfa.controls.filter( c => c.dirty === true );
            if (chItems.length > 0) {

                obj['update'] = {};
                chItems.forEach( c => {

                    const schemaFormGroup = <SchemaFormGroup> c;
                    const change = {};
                    this.FormGroupPatch( <FormGroup> c, change );


                    // Get the identifier; either specified by keyProperty, or id
                    const keyProperty = schemaFormGroup['keyProperty'];
                    const id = keyProperty ? schemaFormGroup.controls[keyProperty] : schemaFormGroup.controls['id'];


                    // Set the object to cbe changed
                    obj['update'][id.value] = change;
                });
            }
        }

        return obj;
    }
}
