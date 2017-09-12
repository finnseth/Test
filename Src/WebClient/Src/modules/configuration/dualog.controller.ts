import { FormGroup } from '@angular/forms';

export class DualogController {

    private compareFields;

    protected CompareDiff(shipForm: FormGroup, compareForm: FormGroup, fields) {

        this.compareFields = fields;

        console.log(shipForm);
        console.log(compareForm);

        shipForm.valueChanges.subscribe( formElements => {
            for (const key in this.compareFields) {
                if (compareForm !== undefined) {
                    if (compareForm.value[key] !== formElements[key]) {
                        this.compareFields[key] = true;
                    } else {
                        this.compareFields[key] = false;
                    }
                }
            }
        });

        if (compareForm !== undefined) {
            compareForm.valueChanges.subscribe( formElements => {
                for (const key in this.compareFields) {
                    console.log('shipform: ' + shipForm);
                    if (shipForm.value[key] !== formElements[key]) {
                        this.compareFields[key] = true;
                    } else {
                        this.compareFields[key] = false;
                    }
                }
            });
        }
    }
}

