import { Component, HostListener, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Observable } from 'rxjs/Rx';

import { PatchGraphDocument } from './../../../infrastructure/services/patchGraphDocument';
import { JsonSchema, SchemaFormBuilder } from './../../../infrastructure/services/schema';

import { Ship } from './../../../common/domain/ship/interfaces';

import { ComponentCanDeactivate } from './../../services/pending_changes.service';
import { CurrentShipService } from './../../services/currentship.service';


export enum CacheType {
    No = 1,
    All = 2
}

export enum CardType {
    Company = 1,
    Ship = 2,
    Compare = 3
}

export enum FormType {
    SingleRow = 1,
    MultipleRow = 2
}

export interface ICompareField {
    key: string,
    prettyname: string
}

export interface IDataSet {
    name: string,
    form: FormGroup,
    schemafunc: any,
    retrievefunc: any,
    intialvalues?: JsonSchema,
    cachestrategy?: CacheType,
    card: CardType;
    formtype: FormType;
    schema?: JsonSchema,
    patchfunc?: any,
    compareform?: string,
    copyfield?: ICompareField[]
}


export abstract class DualogController implements ComponentCanDeactivate {

    cardForm: IDataSet[] = [];
    selectedShip: Ship;
    isCompareModeEnabled = false;
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

    /**
     * Function to register a form for a given card.
     *
     * @param {string} name
     * @param {() => Observable<JsonSchema>} schemafunc
     * @param {(shipid?: number) => Observable<any>} datafunc
     * @param {CacheType} [cache=CacheType.No]
     * @param {CardType} [card=CardType.Ship]
     * @param {(patchid: number, obj?: PatchGraphDocument) => Observable<any>} [patchfunc]
     * @memberof DualogController
     */
    public registerCardForm(name: string, formtype: FormType,
        schemafunc: () => Observable<JsonSchema>,
        datafunc: (shipid?: number) => Observable<any>,
        cache: CacheType = CacheType.No,
        card: CardType = CardType.Ship,
        patchfunc?: (patchid: number, obj?: PatchGraphDocument) => Observable<any>,
    ): void {
        this.cardForm.push({
            name: name,
            form: undefined,
            formtype: formtype,
            schemafunc: schemafunc,
            retrievefunc: datafunc,
            cachestrategy: cache,
            card: card,
            patchfunc: patchfunc,
        });
    }

    /**
     * Function to register forms to compare and fields in the form.
     *
     * @param {string} name
     * @param {string} compareformname
     * @param {ICompareField[]} copyfield
     * @memberof DualogController
     */
    public registerCopy(name: string, compareformname: string, copyfield: ICompareField[]) {
        const currentdataset = this.getDataSet(name);
        if (currentdataset) {
            currentdataset.compareform = compareformname;
            currentdataset.copyfield = copyfield;
        }

    }

    /**
     * Function to initalize the cards
     *
     * @memberof DualogController
     */
    public init(): void {
        this.buildCompanyCard();
        this.buildShipCard();
    }

    /**
     * Loop throug all forms on the company card and retreive the content
     *
     * @memberof DualogController
     */
    public buildCompanyCard(): void {
        for (const singleset of this.cardForm) {
            if (singleset.card === CardType.Company) {
                singleset.schemafunc().subscribe(s => {
                    singleset.schema = s;
                    singleset.retrievefunc().share().subscribe(m => {
                        this.buildForm(singleset.name, m);
                        singleset.intialvalues = singleset.form.value;
                    })
                })
            }
        }
    }


    private buildForm(setname: string, m: any){
        let set = this.getDataSet(setname);
        if (set){
            if (set.formtype === FormType.SingleRow) {
                if (m instanceof Array ) {
                    set.form = this.fb2.Build(set.schema, m[0]);
                } else {
                    set.form = this.fb2.Build(set.schema, m);
                }
            } else {
                if (m instanceof Array ) {
                    set.form = this.fb2.Build(set.schema, m);
                } else {
                    set.form = this.fb2.Build(set.schema, [m]);
                }
            }
        }
    }

    /**
     * Loop through all the forms, and if any ship is selected it retrieve
     * it's information
     *
     * @memberof DualogController
     */
    public buildShipCard(): void {
        for (const singleset of this.cardForm) {
            if (singleset.card === CardType.Ship) {
                singleset.schemafunc().subscribe(s => {
                    singleset.schema = s;
                    this.createForm(singleset, this.selectedShip).subscribe(res => { })
                })
            }
        }
    }

    /**
     * Get the dataset for a given form
     *
     * @param {string} name
     * @returns {IDataSet}
     * @memberof DualogController
     */
    public getDataSet(name: string): IDataSet {
        for (const singleset of this.cardForm) {
            if (singleset.name === name) { return singleset };
        }
        return null;
    }

    /**
     * Get the FormGroup based on a name
     *
     * @param {string} name
     * @returns {FormGroup}
     * @memberof DualogController
     */
    public getFormGroup(name: string): FormGroup {
        const ds = this.getDataSet(name);
        if (ds) { return ds.form; }
        return null;
    }

    /**
     * Get the fields to copy for a given form
     *
     * @param {string} name
     * @returns {ICompareField[]}
     * @memberof DualogController
     */
    public getFieldToCopy(name: string): ICompareField[] {
        const currentds = this.getDataSet(name);
        if (currentds) { return currentds.copyfield; }
        return null;
    }

    public createForm(dt: IDataSet, ship: Ship): Observable<boolean> {
        return Observable.create(s => {

            if (ship !== undefined) {
                if (dt.cachestrategy === CacheType.No) {
                    if (dt.schema) {
                        dt.retrievefunc(ship.id).share().subscribe(m => {
                            this.buildForm(dt.name, m);
                            dt.intialvalues = dt.form.value;
                            s.next(true);
                        },
                        err => { s.next(false) } )
                    } else { s.next(false); }
                } else { s.next(false); }
            } else { s.next(false); }
        })
    }

    /**
     * Function is called when current ship is changed in the cards header
     * 
     * @param {Ship} ship
     * @returns {void}
     * @memberof DualogController
     */
    shipChanged(ship: Ship): void {
        if (ship !== undefined && this.selectedShip !== undefined && ship.id !== this.selectedShip.id) {
            if (this.pendingCardChanges(CardType.Ship)) {
                if (!confirm('WARNING: You have unsaved changes. ' +
                    'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                    return;
                }
            }
        }
        let localCompareMode: boolean = false;
        if (this.isCompareModeEnabled){
            localCompareMode = this.isCompareModeEnabled;
            this.isCompareModeEnabled = false;
        }

        if (ship !== undefined) {
            for (const singleset of this.cardForm) {
                if (singleset.card === CardType.Ship) {
                    this.createForm(singleset, ship).subscribe(res => { 
                        if (localCompareMode) {
                            this.isCompareModeEnabled = true;
                        }
                    })
                }
            }
            this.currentShip.setSelectedShip(ship);
            this.selectedShip = ship;
        }
    }


    /**
     * Function is called when a compare ship is selected in the cards header
     *
     * @param {Ship} ship
     * @returns {void}
     * @memberof DualogController
     */
    onCompare(ship: Ship): void {
        if (ship !== undefined && !this.isCompareModeEnabled) {
            if (this.pendingCardChanges(CardType.Company)) {
                if (!confirm('WARNING: You have unsaved changes. ' +
                    'Press Cancel to go back and save these changes, or OK to lose these changes.')) {
                    return;
                }
            }
        }

        if (ship !== undefined) {
            this.isCompareModeEnabled = false;
            for (const singleset of this.cardForm) {
                if (singleset.card === CardType.Compare) {
                    if (singleset.schema) {
                        this.createForm(singleset, ship).subscribe(res => {
                            if (res) {
                                singleset.form.disable();
                                this.selectedCompareShip = ship;
                                this.isCompareModeEnabled = true;
                            }
                        });
                    } else {
                        singleset.schemafunc().subscribe(s => {
                            singleset.schema = s;
                            this.createForm(singleset, ship).subscribe(res => {
                                if (res) {
                                    singleset.form.disable();
                                    this.selectedCompareShip = ship;
                                    this.isCompareModeEnabled = true;
                                }
                            });
                        })
                    }
                }
            }
        }
    }

    onFleet(event: boolean) {
        if (event) {
            this.selectedCompareShip = null;
            this.isCompareModeEnabled = false;
        }
    }

    pendingChanges(): boolean {
        if (this.pendingCardChanges(CardType.Company)) {
            return true;
        };
        if (this.pendingCardChanges(CardType.Ship)) {
            return true;
        };
        return false;
    }

    pendingCardChanges(card: CardType): boolean {
        for (const singleset of this.cardForm) {
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
        for (const singleset of this.cardForm) {
            if (singleset.card === CardType.Company) {
                singleset.form.setValue(singleset.intialvalues);
                singleset.form.markAsPristine();
            }
        }
    }


    onCancelShipCard() {
        for (const singleset of this.cardForm) {
            if (singleset.card === CardType.Ship) {
                singleset.form.setValue(singleset.intialvalues);
                singleset.form.markAsPristine();
            }
        }
    }


    public isCardValid(cardtype: CardType): boolean {
        for (const singleset of this.cardForm) {
            if (singleset.card === cardtype) {
                if (singleset.form.valid === false) {
                    return false;
                }
            }
        }
        return true;
    }




    applyCard(cardtype: CardType) {
        if (!this.isCardValid(cardtype)) {
            console.log('The form is invalid.');
            return;
        }

        for (const singleset of this.cardForm) {
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
        this.applyCard(CardType.Company);
    }

    public onApplyShipCard() {
        this.applyCard(CardType.Ship);
    }

    public onCopy(fields: any[], formToUpdate: FormGroup) {
        for (const field of fields) {
            if (field.value !== formToUpdate.value[field.key]) {
                var obj = {};
                obj[field.key] = field.value;
                formToUpdate.controls[field.key].setValue(obj);
                formToUpdate.controls[field.key].markAsDirty();
            }
        }
    }
}
