import {
    AccessRights,
    Availability,
    PermissionMap,
} from 'connection-suite-shore/services/permission.service';
import { Component, OnInit } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from 'dualog-common';
import { QuarantineCompanyConfig, QuarantineService, QuarantineVesselConfig } from './quarantine.service';

import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { SelectItem } from 'primeng/primeng';
import { Ship } from 'connection-suite/components/ship/interfaces'; // todo

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.scss']
})
export class QuarantineComponent implements OnInit {

    permission: string = PermissionMap.Config.Email.Quarantine.name;

    status: SelectItem[];
    companyInfo: QuarantineCompanyConfig[];
    selectedShip: Ship;
    selectedCompareShip: Ship;
    quarantinevessels: QuarantineVesselConfig[];
    schema: JsonSchema;
    vqForm: Observable<FormGroup>;
    cqForm: Observable<FormGroup>;
    vesselquarantinecols: any[];
    isCompareModeEnabled = false;

    constructor( private quarantineService: QuarantineService, private fb: SchemaFormBuilder ) {
    }

    ngOnInit() {
        this.status = [
            {
                label: 'Yes',
                value: true
            },
            {
                label: 'No',
                value: false
            },
        ];

        this.schema = this.quarantineService.getVesselConfigSchema().subscribe( s => {
            this.schema = s;

            this.quarantineService.getCompanyConfigList().subscribe( m => {
                this.companyInfo = m;
                this.cqForm = this.fb.ReactiveBuild(Observable.of( this.schema ), Observable.of(m));
            });

            this.quarantineService.getVesselConfigList().subscribe( m => {
                this.quarantinevessels = m;
                this.shipChanged(this.selectedShip);
            });
        });
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
        if ( ship !== undefined ) {
            this.selectedShip = ship;

            const shipSettings = this.findVessel(ship.id);
            if (shipSettings !== null) {
                this.selectedShip = ship;
                this.vqForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( shipSettings ) );
            }
        }
    }

    compareShipChanged(ship: Ship) {
        const shipSettings = this.findVessel(ship.id);
        if (shipSettings !== null) {
            this.selectedCompareShip = ship;
            this.isCompareModeEnabled = true;
            this.cqForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( shipSettings ) );
        }
    }

    showFleet(event: boolean) {
        if (event) {
            this.isCompareModeEnabled = false;
            this.cqForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.companyInfo ) );
        }
    }

    onApplyShipCard(event: Event) {
        console.log('@todo save ship card');
    }

    onCancelShipCard(event: Event) {
        console.log('@todo cancel ship card');
    }

    onApplyFleetCard(event: Event) {
        console.log('@todo save fleet card');
    }

    onCancelFleetCard(event: Event) {
        console.log('@todo cancel fleet card');
    }
}
