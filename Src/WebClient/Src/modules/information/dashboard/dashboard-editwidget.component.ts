import { ActivatedRoute, Params, Router } from '@angular/router';
import { Component, DoCheck, Input, KeyValueDiffer, OnInit } from '@angular/core';
import { DashboardService, IWidgetInformation } from './dashboard.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Message, SelectItem } from 'primeng/primeng';

import { Dashboard } from './dashboard';
import { Observable } from 'rxjs/Rx';
import { PatchGraphDocument } from "infrastructure/services/patchGraphDocument";
import { SchemaFormBuilder } from 'infrastructure/services/schema';

@Component({
    // moduleId: module.id,
    templateUrl: './dashboard-editwidget.component.html',
    styleUrls: ['./dashboard-editwidget.component.css'],
    providers: [ DashboardService ],
})

/**
 * Handles Widget edit
 */
export class DashboardEditWidgetComponent implements OnInit {

    widgetForm: Observable<FormGroup>;
    id: number;
    widgetTypes: SelectItem[];
    choises: SelectItem[];
    msgs: Message[] = [];

    /**
     * Initializes a new instance of the DashboardEditWidgetComponent class.
     * @param dashboardService The dashboard service
     */
    constructor( private route: ActivatedRoute,
                 private router: Router,
                 private fb: SchemaFormBuilder,
                 private dashboardService: DashboardService ) {

        this.widgetTypes = [];
        this.widgetTypes.push({label: 'Bar', value: 'Bar'});
        this.widgetTypes.push({label: 'Pie', value: 'Pie'});
        this.widgetTypes.push({label: 'Table', value: 'Table'});


        this.choises = [];
        this.choises.push({label: 'Choise 1', value: 'Choise 1'});
        this.choises.push({label: 'Choise 2', value: 'Choise 2'});
        this.choises.push({label: 'Choise 3', value: 'Choise 3'});
    }


    /********************************/
    submitForm(fg: FormGroup): void {

        if (fg.valid === false) {
            console.log( 'The form is invalid.' );
            this.msgs.push({severity: 'error', summary: 'Error', detail: 'The form data is invalid.'})
            return;
        }

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument( fg );

        if (jsonPatch) {

        }

        // if (jsonPatch !== undefined && jsonPatch.length > 0) {

        //     console.log(JSON.stringify(jsonPatch));
        //     fg.markAsPristine();
        //     this.dashboardService.PatchWidgetById( this.id, JSON.stringify(jsonPatch) ).subscribe( result => {

        //         fg.markAsPristine();
        //     } );
        // }
    }


    /********************************/
    ngOnInit(): void {

        this.widgetForm = this.route.params.switchMap( (params) => {

            this.id = params['id'];
            const data = this.dashboardService.GetWidgetById( this.id  );
            const schema = this.dashboardService.GetWidgetSchema();

            return this.fb.ReactiveBuild( schema, data );
        }).share();
    }
}
