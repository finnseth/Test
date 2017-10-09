import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { InfrastructureModule } from './../../../infrastructure/infrastructure.module';
import { ContentModule } from './../../../infrastructure/ui/content/content.module';
import { DuaStatusBarModule } from './../../../infrastructure/ui/dua-status-bar/dua-status-bar.module';
import { MenuModule } from './../../../infrastructure/ui/menus/menu.module';

import { HeaderModule } from './../header/header.module';
import { BreadcrumbModule } from './../breadcrumb/breadcrumb.module';
import { BodyComponent } from './body.component';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        HeaderModule,
        BreadcrumbModule,
        ContentModule,
        DuaStatusBarModule,
        MenuModule,
        InfrastructureModule
    ],

    declarations: [
        BodyComponent
    ],

    exports: [
        BodyComponent
    ]

})
export class BodyModule {}
