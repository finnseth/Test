import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subscription } from 'rxjs/Rx';

import { MenuItem } from 'primeng/primeng';

import { SessionService } from './../../services/session.service';

import { Company, CompanyService } from './company.service';

@Component({
    selector: 'dua-switchcompany',
    templateUrl: './switchcompany.component.html',
    styleUrls: ['./switchcompany.component.scss']
})
export class SwitchCompanyComponent implements OnInit, OnDestroy {

    private isDualogAdmin = false;
    private companies: Company[];
    acSelectedCompany: Company;
    selectedCompany: Company;
    resultCompanies: Company[];
    placeholder = 'Search for a company';

    private configItems: MenuItem[] = [];

    constructor(
        private companyService: CompanyService,
        private sessionService: SessionService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit(): void {

        this.isDualogAdmin = this.sessionService.IsDualogAdmin;
        if (this.isDualogAdmin) {
            this.companyService.getCompanies().subscribe( companies => {
                this.companies = companies.sort( (c, r) =>  c.name.localeCompare(r.name) );
            });

            if (this.sessionService.GetSelectedCompany() !== undefined) {
                this.acSelectedCompany = this.selectedCompany = this.sessionService.GetSelectedCompany();
                this.placeholder = 'Change company (' + this.acSelectedCompany.name + ')';
            }
        }
    }

    public searchCompany(event) {
        if (this.sessionService.IsDualogAdmin) {
            this.resultCompanies = [];
            for (let i = 0; i < this.companies.length; i++) {
                const com = this.companies[i];
                if (com.name.toLowerCase().indexOf(event.query.toLowerCase()) > -1) {
                    this.resultCompanies.push(com);
                }
            }
        }
    }

    public selectCompany(company: Company) {
        if (company !== null || company !== undefined) {
            this.acSelectedCompany = this.selectedCompany = company;
            this.sessionService.SetSelectedCompany(company);
            this.sessionService.SetSelectedShip(undefined);
            window.location.reload();
        }
    }

    public ngOnDestroy(): void {
    }
}
