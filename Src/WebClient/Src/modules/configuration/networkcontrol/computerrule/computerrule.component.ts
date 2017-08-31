import {
    AccessRights,
    Availability,
    PermissionMap,
} from 'connection-suite-shore/services/permission.service';
import { Component, OnInit } from '@angular/core';
import { ComputerRule, NetworkControlService } from '../networkcontrol.service';
import { JsonSchema, SchemaFormBuilder } from 'dualog-common';

import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { Ship } from 'connection-suite/components/ship/interfaces';

@Component({
  selector: 'dua-nc-computerrule',
  templateUrl: './computerrule.component.html',
  styleUrls: [ './computerrule.component.scss' ]
})
export class ComputerRuleComponent implements OnInit {

    schema: JsonSchema;
    fleetForm: Observable<FormGroup>;
    shipForm: Observable<FormGroup>;
    leftrules: ComputerRule[];
    fleetrules: ComputerRule[];
    shiprules: ComputerRule[];
    companyInfo: ComputerRule[] = [];
    selectedShip: Ship;
    selectedCompareShip: Ship;
    gotCompareShip = false;
    priorities;
    configureFleet = true;

    constructor( private ncService: NetworkControlService, private fb: SchemaFormBuilder ) {
    }

    ngOnInit() {
        this.priorities = [
            {
                label: 'High',
                value: 0
            },
            {
                label: 'Medium',
                value: 50
            },
            {
                label: 'Low',
                value: 99
            },
            {
                label: 'Block',
                value: -1
            },
        ]

        this.ncService.GetComputerRuleSchema().subscribe( s => {
            this.schema = s;

            this.retrieveFleetRules();
        });
    }

    private retrieveFleetRules(): void {
        this.ncService.getFleetRules().subscribe( r => {
            this.fleetrules = r;
            this.leftrules = this.fleetrules;
            this.fleetForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftrules ) );
        });
    }

    handleShipChange(ship: Ship) {
        if ( ship !== null ) {
            this.selectedShip = ship;
            this.ncService.getShipRules(ship.id).subscribe( r => {
                this.shiprules = r;
                this.shipForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.shiprules ) );
            });
        }
    }

    handleCompareChange(ship: Ship) {
        if ( ship !== null ) {
            this.ncService.getShipRules(ship.id).subscribe(r => {
                this.leftrules = r;
                this.gotCompareShip = true;
                this.fleetForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftrules ) );
            });
        }
    }


    showFleet(event: boolean) {
        this.gotCompareShip = false;
        this.leftrules = this.fleetrules;
        this.fleetForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftrules ) );
    }

    getRowStyleClass(rowData: ComputerRule): string {
        return rowData.isCompanyRule ? 'fleetrule' : 'shiprule';
    }

    submitForm(fg: FormGroup): void {
    }

    newShipRule(event: Event ) {
        if (this.selectedShip !== undefined || this.selectedShip !== null) {
            const shiprules = [...this.shiprules];
            const newShipRule: ComputerRule = {
                isCompanyRule: false,
                priority: 99
            }
            shiprules.push(newShipRule);
            this.shiprules = shiprules;
        }
    }

    newFleetRule(event: Event ) {
        if (!this.gotCompareShip) {
            const leftrules = [...this.leftrules];
            const newFleetRule: ComputerRule = {
                 isCompanyRule: true,
                 priority: 99,
            };
            leftrules.push(newFleetRule);
            this.leftrules = leftrules;
        }
    }

    onApplyFleetCard(event: Event ) {
        console.log('@todo: apply fleet rule');
    }

    onCancelFleetCard(event: Event ) {
        this.retrieveFleetRules();
    }

    onDeleteFleetRule(event: Event, rule: ComputerRule) {
        const leftruleIndex = this.leftrules.indexOf(rule);
        this.leftrules = this.leftrules.filter( (val, i) => i !== leftruleIndex);
        // this.onDeleteShipRule(new Event('rmFromShipRules'), rule);
        console.log('@todo: clean up in the ship rules as well, (not possible for now because of the id and objects are unrelated..)');
    }

    onApplyShipCard(event: Event ) {
        console.log('@todo: apply rule');
    }

    onCancelShipCard(event: Event ) {
        this.handleShipChange(this.selectedShip);
    }

    onDeleteShipRule(event: Event, rule: ComputerRule) {
        const shipruleIndex = this.shiprules.indexOf(rule);
        this.shiprules = this.shiprules.filter( (val, i) => i !== shipruleIndex);
    }
}
