import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AutoCompleteModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { CompanyService } from './company.service';
import { NgModule } from '@angular/core';
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