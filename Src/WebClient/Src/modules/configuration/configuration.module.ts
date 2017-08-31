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

import { CardModule } from 'connection-suite/components/card/card.module';
import { CommonModule } from '@angular/common';
import { ConfigurationComponent } from './configuration.component';
import { CoreModule } from './core/core.module';
import { EmailModule } from './email/email.module';
import { HorizontalSubMenuModule } from 'connection-suite';
import { InternetModule } from './internet/internet.module';
import { NgModule } from '@angular/core';
import { SwitchCompanyModule } from 'connection-suite-shore/components/company/switchcompany.module';
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
        HorizontalSubMenuModule,
        MegaMenuModule,
        PanelModule,
        ButtonModule,
        InternetModule,
        EmailModule,
        OverlayPanelModule,
        SelectButtonModule,
        CardModule,
        SwitchCompanyModule
    ],

    declarations: [
        ConfigurationComponent
    ],

    providers: [
    ]
})
export class ConfigurationModule {}
