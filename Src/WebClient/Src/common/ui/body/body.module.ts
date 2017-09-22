import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ContentModule } from './../../../infrastructure/ui/content/content.module';
import { DuaStatusBarModule } from './../../../infrastructure/ui/dua-status-bar/dua-status-bar.module';

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
        DuaStatusBarModule
    ],

    declarations: [
        BodyComponent
    ],

    exports: [
        BodyComponent
    ]

})
export class BodyModule {}
