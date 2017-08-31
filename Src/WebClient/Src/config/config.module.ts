import { MenuItem, MenubarModule, TabMenuModule } from 'primeng/primeng';

import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ConfigComponent } from './config.component';
import { CsFormsModule } from '../connection-suite/forms/index';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        CsFormsModule,
        TabMenuModule,
        MenubarModule
    ],

    declarations: [
        ConfigComponent,
    ],

    providers: [
    ]
})

export class ConfigModule { }
