import { Company } from 'connection-suite-shore/components/company/company.service';
import { Injectable } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { Observable } from 'rxjs/Rx';
import { Routes } from '@angular/router';
import { Ship } from 'connection-suite/components/ship/interfaces';

@Injectable()
export class SessionService {

    public IsDualogAdmin: boolean;

    constructor() {
    }

    public GetReturnUrl(): string {
        return this.retrieve('ReturnUrl');
    }

    public SetReturnUrl(url: string): void {
        this.store('ReturnUrl', url);
    }

    public GetSelectedShip(): Ship {

        return this.retrieve('SelectedShip');
    }

    public SetSelectedShip(ship: Ship): void {
        this.store('SelectedShip', ship);
    }

    public GetSelectedCompany(): Company {
        return this.retrieve('SelectedCompany');
    }

    public SetSelectedCompany(company: Company): void {
       this.store('SelectedCompany', company);
    }

    /**
     * Check if the key is stored in the session storage.
     * If so the value is returned.
     *
     * @private
     * @param {string} key
     * @returns {*}
     *
     * @memberof AuthenticationService
     */
    private retrieve(key: string): any {
        const item = sessionStorage.getItem(key);

        if (item && item !== 'undefined') {
            return JSON.parse(sessionStorage.getItem(key));
        }

        return;
    }

    /**
     * Store a key/value to the session storage.
     *
     * @private
     * @param {string} key
     * @param {*} value
     *
     * @memberof AuthenticationService
     */
    private store(key: string, value: any) {
        sessionStorage.setItem(key, JSON.stringify(value));
    }
}
