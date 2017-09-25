import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AutoCompleteModule } from 'primeng/primeng';

import { CompanyService } from './company.service';
import { SwitchCompanyComponent } from './switchcompany.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ReactiveFormsModule,
        FormsModule,
        AutoCompleteModule
    ],

    exports: [
        SwitchCompanyComponent
    ],

    declarations: [
        SwitchCompanyComponent
    ],

    providers: [
        CompanyService
    ]
})
export class SwitchCompanyModule {}