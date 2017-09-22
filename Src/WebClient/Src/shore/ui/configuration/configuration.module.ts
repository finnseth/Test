import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    ButtonModule,
    DataTableModule,
    MegaMenuModule,
    MenubarModule,
    MultiSelectModule,
    OverlayPanelModule,
    PanelModule,
    SelectButtonModule,
} from 'primeng/primeng';

import { SwitchCompanyModule } from './../../../common/ui/company/switchcompany.module';

import { ConfigurationComponent } from './configuration.component';
import { OrganizationModule } from './organization/organization.module';
import { EmailModule } from './email/email.module';
import { InternetModule } from './internet/internet.module';
import { configurationRouting } from './configuration.routing';

@NgModule({
    imports: [
        configurationRouting,
        CommonModule,
        OrganizationModule,
        ReactiveFormsModule,
        FormsModule,
        MenubarModule,
        DataTableModule,
        MultiSelectModule,
        MegaMenuModule,
        PanelModule,
        ButtonModule,
        InternetModule,
        EmailModule,
        OverlayPanelModule,
        SelectButtonModule,
        SwitchCompanyModule
    ],

    declarations: [
        ConfigurationComponent
    ],

    providers: [
    ]
})
export class ConfigurationModule {}
