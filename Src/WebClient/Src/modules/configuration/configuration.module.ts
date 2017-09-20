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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { ConfigurationComponent } from './configuration.component';
import { CoreModule } from './core/core.module';
import { EmailModule } from './email/email.module';
import { InternetModule } from './internet/internet.module';
import { NgModule } from '@angular/core';
import { SwitchCompanyModule } from 'common/ui/company/switchcompany.module';
import { configurationRouting } from './configuration.routing';

@NgModule({
    imports: [
        configurationRouting,
        CommonModule,
        ReactiveFormsModule,
        CoreModule,
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
