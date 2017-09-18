import { observable } from 'rxjs/symbol/observable';
import { strictEqual } from 'assert';
import { PatchGraphDocument } from '../../dualog-common/services/patchGraphDocument';
import { Ship } from '../../connection-suite/components/ship/interfaces';
import { CurrentShipService } from '../../connection-suite-shore/services/currentship.service';
import { SchemaFormBuilder } from '../../dualog-common';
import { JsonSchema } from '../../dualog-common';
import { FormGroup } from '@angular/forms';
import { Component, HostListener, OnInit } from '@angular/core';

import { ComponentCanDeactivate } from '../../connection-suite-shore/services/pending_changes.service';
import { Observable } from 'rxjs/Rx';

export enum cachetype {
    No = 1,
    All = 2
}

export enum cardtype {
    Company = 1,
    Ship = 2,
    Compare = 3
}

export interface comparefield {
    key: string,
    prettyname: string
}

export interface dataSet {
    name: string,
    form: FormGroup,
    schemafunc: any,
    retrievefunc: any,
    intialvalues?: JsonSchema,
    cachestrategy?: cachetype,
    card: cardtype;
    schema?: JsonSchema,
    patchfunc?: any,
    compareform?: string,
    copyfield?: comparefield[]
}


export abstract class DualogController implements ComponentCanDeactivate {

    cardForm: dataSet[] = [];
    selectedShip: Ship;
    isCompareModeEnabled: boolean = false;
    selectedCompareShip: Ship;

    constructor(
        private fb2: SchemaFormBuilder,
        private currentShip: CurrentShipService) {
        this.selectedShip = currentShip.getSelectedShip();
    }


    @HostListener('window:beforeunload', ['$event'])
    canDeactivate(): Observable<boolean> | boolean {
        // insert logic to check if there are pending changes here;
        // returning true will navigate without confirmation
        // returning false will show a confirm dialog before navigating away
        return !this.pendingChanges();
    }


    public registerCardForm(name: string, schemafunc: () => Observable<JsonSchema>,
        datafunc: (shipid?: number) => Observable<any>,
        cache: cachetype = cachetype.No,
        card: cardtype = cardtype.Ship,
        patchfunc?: (patchid: number, obj?: PatchGraphDocument) => Observable<any>,
    ): void {
        let obj = {
            name: name,
            form: undefined,
            schemafunc: schemafunc,
            retrievefunc: datafunc,
            cachestrategy: cache,
            card: card,
            patchfunc: patchfunc,
        };

        this.cardForm.push(obj);
    }


    public registerCompare(name: string, compareformname: string, copyfield: comparefield[]) {
        let currentdataset = this.getDataSet(name);
        if (currentdataset) {
            currentdataset.compareform = compareformname;
            currentdataset.copyfield = copyfield;
        }

    }

    public init(): void {
        this.buildCompanyCard();
        this.buildShipCard();

    }

    public buildCompanyCard(): void {
        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype.Company) {
                singleset.schemafunc().subscribe(s => {
                    singleset.schema = s;
                    singleset.retrievefunc().share().subscribe(m => {
                        singleset.form = this.fb2.Build(s, m[0]);
                        singleset.intialvalues = singleset.form.value;
                    })
                })
            }
        }
    }


    public buildShipCard(): void {
        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype.Ship) {
                singleset.schemafunc().subscribe(s => {
                    singleset.schema = s;
                    this.createForm(singleset, this.selectedShip).subscribe(res => {})
                })
            }
        }
    }


    public getDataSet(name: string): dataSet {
        for (let singleset of this.cardForm) {
            if (singleset.name === name) return singleset;
        }
        return null;
    }

    public getFormGroup(name: string): FormGroup {
        let ds = this.getDataSet(name);
        if (ds) return ds.form;
        return null;
    }

    public getFieldToCopy(name: string): comparefield[] {
        let currentds = this.getDataSet(name);
        if (currentds) return currentds.copyfield;
        return null;
    }

    public createForm(dt: dataSet, ship: Ship): Observable<boolean> {

         return Observable.create( s => {

            if (ship !== undefined) {
                if (dt.cachestrategy === cachetype.No) {
                    if (dt.schema) {
                        dt.retrievefunc(ship.id).share().subscribe(m => {
                            dt.form = this.fb2.Build(dt.schema, m);
                            dt.intialvalues = dt.form.value;
                            s.next(true);
                        })
                    }
                }
            }

            s.next(false);
        })
}


    shipChanged(ship: Ship) {

        if (ship !== undefined && this.selectedShip !== undefined && ship.id !== this.selectedShip.id) {
            if (this.pendingCardChanges(cardtype.Ship)) {
                if (!confirm('WARNING: You have unsaved changes. ' +
                    'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                    return;
                }
            }
        }
        if (ship !== undefined) {
            for (let singleset of this.cardForm) {
                if (singleset.card === cardtype.Ship) {
                    this.createForm(singleset, ship).subscribe(res => {})
                }
            }
            this.currentShip.setSelectedShip(ship);
            this.selectedShip = ship;
        }
    }



    onCompare(ship: Ship) {

        if (ship !== undefined && !this.isCompareModeEnabled) {
            if (this.pendingCardChanges(cardtype.Company)) {
                if (!confirm('WARNING: You have unsaved changes. ' +
                    'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                    return;
                }
            }
        }

        if (ship !== undefined) {
            for (let singleset of this.cardForm) {
                if (singleset.card === cardtype.Compare) {
                    if (singleset.schema) {
                        this.createForm(singleset, ship).subscribe(res => {
                            if (res){
                                this.selectedCompareShip = ship;
                                this.isCompareModeEnabled = true;
                            }
                        });
                    } else {
                        singleset.schemafunc().subscribe(s => {
                            singleset.schema = s;
                            this.createForm(singleset, ship).subscribe(res => {
                                if (res){
                                    this.selectedCompareShip = ship;
                                    this.isCompareModeEnabled = true;
                                }
                            });
                        })
                    }
                }
            }


            //                this.createCopyFields();
        }
    }

    onFleet(event: boolean) {
        if (event) {
            this.selectedCompareShip = null;
            this.isCompareModeEnabled = false;
        }
    }


    pendingChanges(): boolean {
        if (this.pendingCardChanges(cardtype.Company)) {
            return true;
        };
        if (this.pendingCardChanges(cardtype.Ship)) {
            return true;
        };

        return false;
    }

    pendingCardChanges(card: cardtype): boolean {
        for (let singleset of this.cardForm) {
            if (singleset.card === card) {
                if (singleset.form && singleset.intialvalues) {
                    if (JSON.stringify(singleset.form.value) !== JSON.stringify(singleset.intialvalues)) {
                        return true;
                    };
                }
            }
        }
        return false;
    }


    onCancelFleetCard() {
        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype.Company) {
                singleset.form.setValue(singleset.intialvalues);
                singleset.form.markAsPristine();
            }
        }
    }


    onCancelShipCard() {
        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype.Ship) {
                singleset.form.setValue(singleset.intialvalues);
                singleset.form.markAsPristine();
            }
        }
    }


    public isCardValid(cardtype: cardtype): boolean {
        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype) {
                if (singleset.form.valid === false) {
                    return false;
                }
            }
        }
        return true;
    }




    applyCard(cardtype: cardtype) {
        if (!this.isCardValid(cardtype)) {
            console.log('The form is invalid.');
            return;
        }

        for (let singleset of this.cardForm) {
            if (singleset.card === cardtype) {
                if (singleset.patchfunc) {
                    const pgd = new PatchGraphDocument();
                    const jsonPatch = pgd.CreatePatchDocument(singleset.form);
                    if (jsonPatch) {
                        singleset.patchfunc(singleset.form.value.quarantineId, jsonPatch).subscribe(result => {
                            singleset.form.markAsPristine();
                        }, error => {
                            const obj = JSON.parse(error._body);
                            alert(obj.message);
                        });
                    }
                }
            }
        }
    }


    onApplyFleetCard() {
        this.applyCard(cardtype.Company);
    }


    public onApplyShipCard() {
        this.applyCard(cardtype.Ship);
    }

    // move to super
    public onCopy(fields: any[], formToUpdate: FormGroup) {
        for (const field of fields) {
            if (field.value !== formToUpdate.value[field.key]) {
                formToUpdate.controls[field.key].setValue(field.value);
                formToUpdate.controls[field.key].markAsDirty();
            }
        }
    }

}
