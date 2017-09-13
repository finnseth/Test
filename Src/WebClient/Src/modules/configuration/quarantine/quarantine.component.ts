import * as assert from 'assert';

import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from '../../../dualog-common';
import { QuarantineCompanyConfig, QuarantineService, QuarantineVesselConfig } from './quarantine.service';

import { CopyField } from './../../../connection-suite-shore/components/copy/copy.component';
import { CurrentShipService } from '../../../connection-suite-shore/services/currentship.service';
import { DualogController } from '../dualog.controller';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { PatchGraphDocument } from '../../../dualog-common/services/patchGraphDocument';
import { SelectItem } from 'primeng/primeng';
import { Ship } from '../../../connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.scss']
})
export class QuarantineComponent extends DualogController implements OnInit {

    companyInfo: QuarantineCompanyConfig;
    selectedShip: Ship;
    selectedCompareShip: Ship;
    quarantinevessels: QuarantineVesselConfig[];
    schema: JsonSchema;
    copyForm: FormGroup;
    vqForm: FormGroup;
    cqForm: FormGroup;
    vesselquarantinecols: any[];
    isCompareModeEnabled = false;

    copyFields: CopyField[] = [];

    cqInitialValues: JsonSchema;
    vqInitialValues: JsonSchema;

    constructor(
        private quarantineService: QuarantineService,
        private fb: SchemaFormBuilder,
        private currentShip: CurrentShipService ) {
            super();
            this.selectedShip = currentShip.getSelectedShip();
    }

    ngOnInit() {

        this.schema = this.quarantineService.getVesselConfigSchema().subscribe( s => {
            this.schema = s;

            this.quarantineService.getCompanyConfigList().share().subscribe( m => {
                this.companyInfo = m[0];

                this.cqForm = this.fb.Build(this.schema , this.companyInfo);

                this.cqInitialValues = this.cqForm.value;
            });

            this.quarantineService.getVesselConfigList().subscribe( m => {
                this.quarantinevessels = m;
                this.shipChanged(this.selectedShip);
            });
        });
    }

    pendingChanges(): boolean {
        if (this.pendingChangesCompany()) {
            return true;
        };
        if (this.pendingChangesShip()) {
            return true;
        };

        return false;
    }

    private pendingChangesShip(): boolean {
        if (JSON.stringify(this.vqForm.value) !== JSON.stringify(this.vqInitialValues)) {
            return true;
        };
        return false;
    }

    private pendingChangesCompany(): boolean {
        if (JSON.stringify(this.cqForm.value) !== JSON.stringify(this.cqInitialValues)) {
            return true;
        };
        return false;
    }

    private findVessel(shipId: number): QuarantineVesselConfig {
        if ( this.quarantinevessels !== undefined ) {
            for (const shipSettings of this.quarantinevessels) {
                if (shipSettings.vesselId === shipId) {
                    return shipSettings;
                }
            }
        }
        return null;
    }

    shipChanged(ship: Ship) {

        if (ship !== undefined && this.selectedShip !== undefined && ship !== this.selectedShip) {
            if (this.pendingChangesShip()){
               if (!confirm('WARNING: You have unsaved changes. ' +
                'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                return;
               }
            }
        }

        if ( ship !== undefined ) {
            const shipSettings = this.findVessel(ship.id);
            if (shipSettings !== null) {
                this.currentShip.setSelectedShip(ship);
                this.selectedShip = ship;
                this.vqForm = this.fb.Build( this.schema , shipSettings  );
                this.vqInitialValues = this.vqForm.value;
            }
        }
    }

    compareShipChanged(ship: Ship) {

        if (ship !== undefined && !this.isCompareModeEnabled){
            if (this.pendingChangesCompany()){
               if (!confirm('WARNING: You have unsaved changes. ' +
                'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                return;
               }
            }
        }

        if ( ship !== undefined ) {
            const shipSettings = this.findVessel(ship.id);
            if (shipSettings !== null) {

                this.selectedCompareShip = ship;
                this.isCompareModeEnabled = true;
                this.cqForm = this.fb.Build( this.schema ,  shipSettings );
                this.cqInitialValues = this.cqForm.value;
                this.createCopyFields();
            }
        }
    }

    showFleet(event: boolean) {
        if (event) {
            this.selectedCompareShip = null;
            this.isCompareModeEnabled = false;
            this.cqForm = this.fb.Build( this.schema , this.companyInfo );
        }
    }

    onApplyShipCard(fg: FormGroup ) {

        if (fg.valid === false) {
            console.log( 'The form is invalid.' );
            return;
        }

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument(  fg );
        if (jsonPatch ) {
            this.quarantineService.PatchVesselQuarantine( fg.value.quarantineId, jsonPatch ).subscribe( result => {
                fg.markAsPristine();
            }, error => {
                const obj = JSON.parse(error._body);
                alert(obj.message);
            } );
        }

    }

    onCancelShipCard(fg: FormGroup) {
        this.vqForm.setValue(this.vqInitialValues);
    }

    onApplyFleetCard(fg: FormGroup) {

        if (fg.valid === false) {
            console.log( 'The form is invalid.' );
            return;
        }

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument(  fg );
        if (jsonPatch ) {
            this.quarantineService.PatchCompanyQuarantine( fg.value.quarantineId, jsonPatch ).subscribe( result => {
                fg.markAsPristine();
            }, error => {
                const obj = JSON.parse(error._body);
                alert(obj.message);
            } );
        }

    }

    onCancelFleetCard(event: Event) {
        this.cqForm.setValue(this.cqInitialValues);
    }

    getShipFieldSetting(ship: Ship, field: string) {
        const shipSettings = this.findVessel(ship.id);
        if (shipSettings !== null) {
            if (shipSettings[field] !== undefined) {
                return shipSettings[field];
            }
        }
        return null;
    }

    private createCopyFields () {
        this.copyForm = this.cqForm;
        this.copyFields = [];
        for (const key in this.cqInitialValues) {
            switch (key) {
                case 'useThisLevel':
                    this.addCopyField(key, 'Override fleet configuration', this.cqInitialValues[key]);
                break;
                case 'onHoldStationaryUser':
                    this.addCopyField(key, 'Stationary users', this.cqInitialValues[key]);
                break;
                case 'onHoldCrew':
                    this.addCopyField(key, 'Crew (personal) users', this.cqInitialValues[key]);
                break;
                case 'notificationOnHoldOriginal':
                    this.addCopyField(key, 'Message originator', this.cqInitialValues[key]);
                break;
                case 'notificationOnHoldPostmaster':
                    this.addCopyField(key, 'Ship administrator', this.cqInitialValues[key]);
                break;
                case 'notificationOnHoldRecipient':
                    this.addCopyField(key, 'Message recipient(s)', this.cqInitialValues[key]);
                break;
                case 'notificationOnHoldAdmins':
                    this.addCopyField(key, 'Shore administrator(s)', this.cqInitialValues[key]);
                break;
                case 'maxBodyLength':
                    this.addCopyField(key, 'Max body length', this.cqInitialValues[key]);
                break;
                case 'onHoldDuration':
                    this.addCopyField(key, 'Hold for duration', this.cqInitialValues[key]);
                break;
                case 'notificationSender':
                    this.addCopyField(key, 'Custom notification sender', this.cqInitialValues[key]);
                break;
            }
        }
    }

    // move to super
    private addCopyField(key: string, name: string, value: any): void {
        this.copyFields.push({
            name: name,
            key: key,
            value: value
        });
    }

    // move to super
    private copySettings(fields: any[], formToUpdate: FormGroup) {
        for (const field of fields) {
            if (field.value !== formToUpdate.value[field.key]) {
                formToUpdate.controls[field.key].setValue(field.value);
                formToUpdate.controls[field.key].markAsDirty();
            }
        }
    }
}
